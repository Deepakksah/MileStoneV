using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public class GenericDataBox : Box
{
	public override bool CanHaveData => true;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Parse<T>(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, Func<byte[], uint, T> convertor, uint bytesInType, ref T parameter) where T : struct
	{
		if (restInBoxSize - readBytes < bytesInType)
		{
			throw new Exception("Not enough data in box");
		}
		parameter = convertor(buffer, startIndex + readBytes);
		readBytes += bytesInType;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ParseByteArray(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, byte[] parameter)
	{
		int num = parameter.Length;
		if (restInBoxSize - readBytes < num)
		{
			throw new Exception("Not enough data in box");
		}
		Array.Copy(buffer, startIndex + readBytes, parameter, 0L, num);
		readBytes += (uint)num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ParseUInt8(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, ref byte parameter)
	{
		Parse(buffer, startIndex, restInBoxSize, ref readBytes, BoxHelper.ToUInt8, 1u, ref parameter);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ParseUInt16(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, ref ushort parameter)
	{
		Parse(buffer, startIndex, restInBoxSize, ref readBytes, BoxHelper.ToUInt16, 2u, ref parameter);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ParseUInt32(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, ref uint parameter)
	{
		Parse(buffer, startIndex, restInBoxSize, ref readBytes, BoxHelper.ToUInt32, 4u, ref parameter);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ParseUInt64(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, ref ulong parameter)
	{
		Parse(buffer, startIndex, restInBoxSize, ref readBytes, BoxHelper.ToUInt64, 8u, ref parameter);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ParseDateAsUInt64(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, ref DateTime dateTime)
	{
		ulong parameter = 0uL;
		ParseUInt64(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter);
		dateTime = BoxTimeConvertor.FromBoxTime(parameter);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ParseDateAsUInt32(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, ref DateTime dateTime)
	{
		uint parameter = 0u;
		ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter);
		dateTime = BoxTimeConvertor.FromBoxTime(parameter);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ParseUtf8String(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, ref string parameter)
	{
		uint num;
		for (num = 0u; restInBoxSize - readBytes - num != 0 && buffer[startIndex + readBytes + num] != 0; num++)
		{
		}
		parameter = Encoding.UTF8.GetString(buffer, (int)(startIndex + readBytes), (int)num);
		readBytes += num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint GetUtf8StringLength(string text)
	{
		return (uint)(Encoding.UTF8.GetBytes(text).Length + 1);
	}
}
