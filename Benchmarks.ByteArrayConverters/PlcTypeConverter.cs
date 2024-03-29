﻿using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Benchmarks.ByteArrayConverters;

public static class PlcTypeConverter
{
    private static readonly Encoding StringEncoding = Encoding.ASCII;

    private static readonly int LengthOfArrayLength = sizeof(uint);
    private static readonly int LengthOfChar = StringEncoding.GetByteCount("a");
    
    private const int LengthOfDateTimeString = 8;

    /// <summary>
    ///     Converts a byte array (prefixed with the content length) to a string.
    /// </summary>
    /// <param name="plcBytes">The byte array to convert.</param>
    /// <returns>A string representing the content of the byte array.</returns>
    /// <exception cref="System.ArgumentNullException">value</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">
    ///     The specified byte array is too short - it should be at least 5
    ///     elements long
    /// </exception>
    /// <exception cref="System.ArgumentException">The specified byte array contains more data than expected</exception>
    public static string StringWithLength(byte[] plcBytes)
    {
        if (plcBytes == null)
            throw new ArgumentNullException(nameof(plcBytes));

        if (plcBytes.Length < sizeof(int) + 1)
            throw new ArgumentOutOfRangeException(nameof(plcBytes), plcBytes,
                $"The specified byte array is too short - it should be at least {LengthOfArrayLength + 1} elements long");

        var length = BitConverter.ToUInt32(plcBytes[..LengthOfArrayLength]);
        var content = StringEncoding.GetString(plcBytes[LengthOfArrayLength..]);

        if (length != content.Length)
            throw new ArgumentException(
                $"The specified byte array contains more data than expected (length prefix dictates {length} bytes of content, but found {content.Length})");

        return content;
    }

    /// <summary>
    ///     Converts an array of PLC-bytes, prefixed with the length of the intended array, to an array of primitive C# types.
    /// </summary>
    /// <param name="typeOfElement">The element type of the destination array.</param>
    /// <param name="plcBytes">The PLC-bytes to convert from.</param>
    /// <returns>An array of PLC-bytes, prefixed with the length of the intended array, to an array of primitive C# types.</returns>
    /// <remarks>Cast to T[] for ease of use.</remarks>
    public static Array ArrayWithLength(Type typeOfElement, byte[] plcBytes)
    {
        var typeCodeOfElement = Type.GetTypeCode(typeOfElement);
        var lengthOfElement = SizeOf(typeCodeOfElement);
        var lengthOfArray = BitConverter.ToUInt32(plcBytes[..LengthOfArrayLength]);
        var offset = LengthOfArrayLength;

        var valueOfArray = Array.CreateInstance(typeOfElement, lengthOfArray);
        for (var i = 0; i < lengthOfArray; i++)
        {
            var valueOfElement = Convert(typeOfElement, plcBytes[offset..(offset + lengthOfElement)]);
            valueOfArray.SetValue(valueOfElement, i);
            offset += lengthOfElement;
        }

        return valueOfArray;
    }

    /// <summary>
    ///     Converts a PLC-byte array to an ordered sequence of primitive types or array of primitive types.
    /// </summary>
    /// <param name="plcBytes">The PLC-bytes to convert.</param>
    /// <param name="destinationTypes">An ordered sequence of destination types.</param>
    /// <returns>
    ///     An array of tuples containing a description of the type and its value.
    /// </returns>
    /// <remarks>The type code is set to <see cref="TypeCode.Object" /> for array types.</remarks>
    /// <example>
    ///     This is a struct that describes the output types from the PLC:
    ///     <code>
    /// public struct PlcFields
    /// {
    ///     public bool xBool;
    ///     public byte usiByte;
    ///     public sbyte siByte;
    ///     public ushort uiShort;
    ///     public short iShort;
    ///     public uint udiInt;
    ///     public int diInt;
    ///     public ulong uliLong;
    ///     public long liLong;
    ///     public string acString;
    ///     public int[] adiInt;
    /// }
    /// </code>
    ///     This demonstrates how to use the method:
    ///     <code>
    /// var orderedValues = PlcTypeConverter
    ///     .FromOrderedPlcByteArray(
    ///             bytes,
    ///             typeof(bool),
    ///             typeof(byte),
    ///             typeof(sbyte),
    ///             typeof(ushort),
    ///             typeof(short),
    ///             typeof(uint),
    ///             typeof(int),
    ///             typeof(ulong),
    ///             typeof(long),
    ///             typeof(string),
    ///             typeof(int[]),
    ///             typeof(DateTime)
    ///         );
    /// </code>
    /// </example>
    public static (TypeCode Type, object Value)[] FromOrderedPlcByteArray(byte[] plcBytes, params Type[] destinationTypes)
    {
        var offset = 0;
        var converted = destinationTypes
            .Select(t =>
            {
                if (t == typeof(string))
                {
                    var lengthOfString = BitConverter.ToInt32(plcBytes[offset..(offset + LengthOfArrayLength)]);
                    var fieldLength = LengthOfArrayLength + lengthOfString * LengthOfChar;
                    var valueOfString = StringWithLength(plcBytes[offset..(offset + fieldLength)]);
                    offset += fieldLength;
                    return (TypeCode.String, valueOfString);
                }

                if (t == typeof(DateTime))
                {
                    var lengthOfDateTimeString = LengthOfDateTimeString;
                    var valueOfString = DateTime.FromFileTimeUtc(BitConverter.ToInt64(plcBytes[offset..(offset + lengthOfDateTimeString)]));
                    offset += lengthOfDateTimeString;
                    return (TypeCode.String, valueOfString);
                }

                if (typeof(IEnumerable).IsAssignableFrom(t))
                {
                    var lengthOfArray = BitConverter.ToInt32(plcBytes[offset..(offset + LengthOfArrayLength)]);
                    var typeOfElement = t.GetElementType();
                    var typeCodeOfElement = Type.GetTypeCode(typeOfElement);
                    var lengthOfElement = SizeOf(typeCodeOfElement);
                    var fieldLength = LengthOfArrayLength + lengthOfArray * lengthOfElement;
                    if (typeOfElement != null)
                    {
                        var valueOfArray = ArrayWithLength(typeOfElement, plcBytes[offset..(offset + fieldLength)]);
                        offset += fieldLength;
                        return (TypeCode.Object, valueOfArray);
                    }
                }

                var typeCode = Type.GetTypeCode(t);
                var length = SizeOf(typeCode);
                var data = plcBytes[offset..(offset + length)];
                var value = Convert(t, data);
                offset += length;
                return (typeCode, value);
            })
            .ToArray();
        return converted;
    }

    /// <summary>
    ///     Converts the array of PLC-bytes to its corresponding C# type.
    /// </summary>
    /// <param name="type">The type to convert to.</param>
    /// <param name="plcBytes">The PLC-byte array to convert from.</param>
    /// <returns>The C# representation of the PLC-bytes.</returns>
    private static object Convert(Type type, byte[] plcBytes)
    {
        if (type == typeof(string))
            return StringEncoding.GetString(plcBytes);

        if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            var typeOfElement = type.GetElementType();
            var typeCodeOfElement = Type.GetTypeCode(typeOfElement);
            var lengthOfElement = SizeOf(typeCodeOfElement);
            var numberOfElements = plcBytes.Length / lengthOfElement;

            if (typeOfElement == null)
                throw new TypeLoadException($"Unable to get element type from {type}");

            var converted = Array.CreateInstance(typeOfElement, numberOfElements);
            for (var i = 0; i < numberOfElements; i++)
            {
                var element = ConvertSingle(typeOfElement, plcBytes[(i * lengthOfElement)..((i + 1) * lengthOfElement)]);
                converted.SetValue(element, i);
            }

            return converted;
        }

        return ConvertSingle(type, plcBytes);

        static object ConvertSingle(Type elementType, byte[] data)
        {
            return Type.GetTypeCode(elementType) switch
            {
                TypeCode.Boolean => data[0].FromPlcBOOL(),
                TypeCode.Byte => data[0].FromPlcUSINT(),
                TypeCode.SByte => data[0].FromPlcSINT(),
                TypeCode.UInt16 => data.FromPlcUINT(),
                TypeCode.Int16 => data.FromPlcINT(),
                TypeCode.UInt32 => data.FromPlcUDINT(),
                TypeCode.Int32 => data.FromPlcDINT(),
                TypeCode.UInt64 => data.FromPlcULINT(),
                TypeCode.Int64 => data.FromPlcLINT(),
                TypeCode.Char => (char)data[0].FromPlcUSINT(),
                _ => throw new InvalidOperationException($"Type {Type.GetTypeCode(elementType)} cannot be converted to or from PLC")
            };
        }
    }

    /// <summary>
    ///     Converts the array of PLC-bytes using properties from a User Defined Type.
    ///
    ///     Important: This method uses Reflection and should reconsider usage if the purpose is for many calls.
    ///     Some performance issues are detected if it's compared to manual conversion.
    ///
    ///     BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1766 (20H2/October2020Update)
    ///     Intel Core i9-9880H CPU 2.30GHz, 1 CPU, 16 logical and 8 physical cores
    ///     .NET SDK=6.0.302
    ///       [Host]     : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT
    ///       DefaultJob : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT
    ///
    ///
    ///     |                            Method |       Mean |    Error |   StdDev | Rank |  Gen 0 | Allocated |
    ///     |---------------------------------- |-----------:|---------:|---------:|-----:|-------:|----------:|
    ///     | ConvertFromBytesToUserDefinedType | 1,828.9 ns | 33.16 ns | 48.61 ns |    2 | 0.2213 |   1,880 B |
    ///     |          ConvertByteArrayManually |   250.2 ns |  5.03 ns | 10.73 ns |    1 | 0.0429 |     360 B |
    ///
    ///      * Hints *
    ///       Outliers
    ///       ByteArrayValueSetBenchmark.ConvertByteArrayUsingReflection: Default -> 1 outlier  was  removed (2.01 us)
    ///
    ///      * Legends *
    ///       Mean      : Arithmetic mean of all measurements
    ///       Error     : Half of 99.9% confidence interval
    ///       StdDev    : Standard deviation of all measurements
    ///       Rank      : Relative position of current benchmark mean among all benchmarks (Arabic style)
    ///       Gen 0     : GC Generation 0 collects per 1000 operations
    ///       Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
    ///       1 ns      : 1 Nanosecond (0.000000001 sec)
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <param name="userDefinedType">OPC UA Connectivity Package User defined type to convert to.</param>
    /// <param name="plcBytes">The PLC-byte array to convert from.</param>
    /// <returns>The C# representation of the PLC-bytes.</returns>
    public static void ConvertFromBytesToUserDefinedType<T>(T userDefinedType, byte[] plcBytes)
    {
        var userDefinedPropInfo = typeof(T)
            .GetProperties()
            .Where(a => a.PropertyType != typeof(NodeId));

        var definedPropInfo = userDefinedPropInfo as PropertyInfo[] ?? userDefinedPropInfo.ToArray();
        var propInfoTypesArray = definedPropInfo.Select(p => p.PropertyType).ToArray();

        var tupleValues = FromOrderedPlcByteArray(plcBytes, propInfoTypesArray);

        var typePropertiesArray = definedPropInfo.ToArray();

        for (var i = 0; i < tupleValues.Length; i++)
            typePropertiesArray[i].SetValue(userDefinedType, tupleValues[i].Value);
    }

    private static int SizeOf(TypeCode typeCode) => typeCode switch
    {
        TypeCode.Boolean => sizeof(bool),
        TypeCode.Byte => sizeof(byte),
        TypeCode.SByte => sizeof(sbyte),
        TypeCode.UInt16 => sizeof(ushort),
        TypeCode.Int16 => sizeof(short),
        TypeCode.UInt32 => sizeof(uint),
        TypeCode.Int32 => sizeof(int),
        TypeCode.UInt64 => sizeof(ulong),
        TypeCode.Int64 => sizeof(long),
        TypeCode.Char => LengthOfChar,
        _ => throw new InvalidOperationException($"Type {typeCode} cannot be converted to or from PLC")
    };
}
