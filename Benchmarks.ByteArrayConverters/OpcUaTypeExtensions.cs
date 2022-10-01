using System;

namespace Benchmarks.ByteArrayConverters;

public static class OpcUaTypeExtensions
{
    #region BOOL

    /// <summary>
    ///     Converts a <see cref="byte" /> to a <see cref="bool" />.
    /// </summary>
    public static bool FromPlcBOOL(this byte value) => BitConverter.ToBoolean(new[]
    {
        value
    });
    
    #endregion BOOL

    #region USINT

    /// <summary>
    ///     Converts a <see cref="byte" /> to a <see cref="byte" />.
    /// </summary>
    public static byte FromPlcUSINT(this byte value) => value;
    #endregion USINT

    #region SINT

    /// <summary>
    ///     Converts a <see cref="byte" /> to a <see cref="sbyte" />.
    /// </summary>
    public static sbyte FromPlcSINT(this byte value) => (sbyte)value;
    
    #endregion SINT
    
    /// <summary>
    ///     Converts an array of <see cref="byte" /> to a <see cref="ushort" />.
    /// </summary>
    public static ushort FromPlcUINT(this byte[] value) => BitConverter.ToUInt16(value);

    /// <summary>
    ///     Converts an array of <see cref="byte" /> to a <see cref="short" />.
    /// </summary>
    public static short FromPlcINT(this byte[] value) => BitConverter.ToInt16(value);

    /// <summary>
    ///     Converts an array of <see cref="byte" /> to a <see cref="uint" />.
    /// </summary>
    public static uint FromPlcUDINT(this byte[] value) => BitConverter.ToUInt32(value);

    /// <summary>
    ///     Converts an array of <see cref="byte" /> to a <see cref="int" />.
    /// </summary>
    public static int FromPlcDINT(this byte[] value) => BitConverter.ToInt32(value);
 
    /// <summary>
    ///     Converts an array of <see cref="byte" /> to a <see cref="ulong" />.
    /// </summary>
    public static ulong FromPlcULINT(this byte[] value) => BitConverter.ToUInt64(value);
    /// <summary>
    ///     Converts an array of <see cref="byte" /> to a <see cref="long" />.
    /// </summary>
    public static long FromPlcLINT(this byte[] value) => BitConverter.ToInt64(value);
}
