using System;
using System.Linq;

namespace VideoOS.Mobile.Portable.Utilities;

public class JpegHelper
{
	public static readonly byte[] JpegStartCode = new byte[2] { 255, 216 };

	public static readonly byte[] JpegEndCode = new byte[2] { 255, 217 };

	public static bool IsJpeg(byte[] data)
	{
		bool result = false;
		if (data != null && data.Length > JpegStartCode.Length + JpegEndCode.Length)
		{
			result = AreTheSame(data, 0, JpegStartCode, 0, JpegStartCode.Length) && AreTheSame(data, data.Length - JpegEndCode.Length, JpegEndCode, 0, JpegEndCode.Length);
		}
		return result;
	}

	public static bool AreTheSame(byte[] first, int firstStart, byte[] second, int secondStart, int length)
	{
		bool result = true;
		for (int i = 0; i < length; i++)
		{
			if (first[firstStart + i] != second[secondStart + i])
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public static Tuple<int, int> GetJpegDimensions(byte[] bytes)
	{
		if (bytes.Length < 4 || !Enumerable.SequenceEqual(bytes.SubArray(0, 4), new byte[4] { 255, 216, 255, 224 }))
		{
			return null;
		}
		int num = 0;
		while (num < bytes.Length)
		{
			if (bytes[num] != byte.MaxValue)
			{
				return null;
			}
			byte b = bytes[++num];
			switch (b)
			{
			case 1:
			case 209:
			case 210:
			case 211:
			case 212:
			case 213:
			case 214:
			case 215:
			case 216:
			case 217:
				num++;
				continue;
			}
			if (b == 192 || b == 194)
			{
				break;
			}
			int num2 = (bytes[++num] << 8) + bytes[num + 1];
			num += num2;
		}
		num += 3;
		int item = (bytes[++num] << 8) + bytes[++num];
		return Tuple.Create((bytes[++num] << 8) + bytes[++num], item);
	}
}
