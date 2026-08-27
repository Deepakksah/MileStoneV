using System;
using System.Linq;

namespace VideoOS.Platform.Util;

public static class ByteArrayHelper
{
	public static byte[] Combine(params byte[][] arrays)
	{
		byte[] array = new byte[arrays.Sum((byte[] x) => x.Length)];
		int num = 0;
		foreach (byte[] array2 in arrays)
		{
			Buffer.BlockCopy(array2, 0, array, num, array2.Length);
			num += array2.Length;
		}
		return array;
	}

	public static void ByteArrayToIntArray(byte[] byteArray, int startIndexByteArray, int[] intArray, int startIndexIntArray, int intCounts, int bytesPerInt)
	{
		CheckByteToIntInputParameters(byteArray, startIndexByteArray, intArray, startIndexIntArray, intCounts, bytesPerInt);
		if (1 == bytesPerInt)
		{
			for (int i = 0; i < intCounts; i++)
			{
				intArray[startIndexIntArray + i] = byteArray[startIndexByteArray + i];
			}
			return;
		}
		if (2 == bytesPerInt)
		{
			for (int j = 0; j < intCounts; j++)
			{
				intArray[startIndexIntArray + j] = BitConverter.ToInt16(byteArray, startIndexByteArray + (j << 1));
			}
			return;
		}
		throw new NotImplementedException();
	}

	private static void CheckByteToIntInputParameters(byte[] byteArray, int startIndexByteArray, int[] intArray, int startIndexIntArray, int intCounts, int bytesPerInt)
	{
		int num = intCounts * bytesPerInt;
		int num2 = byteArray.Length - startIndexByteArray;
		if (num > num2)
		{
			throw new InvalidParameterMIPException("Expected byte array size is less than required");
		}
		int num3 = intArray.Length - startIndexIntArray;
		if (intCounts > num3)
		{
			throw new InvalidParameterMIPException("Expected int array size is less than required");
		}
	}

	public static int[] ByteArrayToIntArray(byte[] byteArray, int startIndex, int intCounts, int bytesPerInt)
	{
		int[] array = new int[intCounts];
		ByteArrayToIntArray(byteArray, startIndex, array, 0, intCounts, bytesPerInt);
		return array;
	}

	public static void IntArrayToByteArray(int[] intArray, int startIndexIntArray, byte[] byteArray, int startIndexByteArray, int intCounts, int bytesPerInt)
	{
		CheckByteToIntInputParameters(byteArray, startIndexByteArray, intArray, startIndexIntArray, intCounts, bytesPerInt);
		if (1 == bytesPerInt)
		{
			for (int i = 0; i < intCounts; i++)
			{
				byteArray[startIndexByteArray + i] = (byte)intArray[startIndexIntArray + i];
			}
			return;
		}
		if (2 == bytesPerInt)
		{
			for (int j = 0; j < intCounts; j++)
			{
				byte[] bytes = BitConverter.GetBytes((short)intArray[startIndexIntArray + j]);
				byteArray[startIndexByteArray + (j << 1)] = bytes[0];
				byteArray[startIndexByteArray + (j << 1) + 1] = bytes[1];
			}
			return;
		}
		throw new NotImplementedException();
	}

	public static byte[] IntArrayToByteArray(int[] intArray, int startIndexIntArray, int intCounts, int bytesPerInt)
	{
		byte[] array = new byte[intCounts * bytesPerInt];
		IntArrayToByteArray(intArray, startIndexIntArray, array, 0, intCounts, bytesPerInt);
		return array;
	}
}
