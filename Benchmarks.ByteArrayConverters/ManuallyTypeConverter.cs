using System.Linq;
using System.Text;
using System;

namespace Benchmarks.ByteArrayConverters;
public class ManuallyTypeConverter
{
    public static void ConvertFromBytesToTypesManually(TypeExample typeToConvert, byte[] inputArrayBytes)
    {
        var currentIndex = 0;

        var imModeLength = BitConverter.ToInt32(inputArrayBytes, 0);
        typeToConvert.FirstString = Encoding.UTF8.GetString(inputArrayBytes, currentIndex += 4, imModeLength);
        currentIndex += imModeLength;
        typeToConvert.FirstDateTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(inputArrayBytes.Skip(currentIndex).Take(8).ToArray(), 0));

        typeToConvert.FirstBoolean = BitConverter.ToBoolean(inputArrayBytes, currentIndex += 8);

        var qsvLength = BitConverter.ToInt32(inputArrayBytes, ++currentIndex);
        typeToConvert.SecondString = Encoding.UTF8.GetString(inputArrayBytes, currentIndex += 4, qsvLength);

        var pmdLength = BitConverter.ToInt32(inputArrayBytes, currentIndex += qsvLength);
        typeToConvert.ThirdString = Encoding.UTF8.GetString(inputArrayBytes, currentIndex += 4, pmdLength);

        var pmdDenominationLength = BitConverter.ToInt32(inputArrayBytes, currentIndex += pmdLength);
        typeToConvert.FourthString = Encoding.UTF8.GetString(inputArrayBytes, currentIndex += 4, pmdDenominationLength);

        var cmdAlarmLength = BitConverter.ToInt32(inputArrayBytes, currentIndex += pmdDenominationLength);
        typeToConvert.FifthString = Encoding.UTF8.GetString(inputArrayBytes, currentIndex + 4, cmdAlarmLength);
    }
}
