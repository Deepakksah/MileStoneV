using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public static class BoxHelper
{
	public static uint ParseSizeAndType(byte[] buffer, uint startIndex, uint maxSize, ref uint size, ref string type)
	{
		if (buffer.Length < 8)
		{
			throw new Exception("Parse error - buffer size < 8");
		}
		size = ToUInt32(buffer, startIndex);
		if (size > buffer.Length - startIndex)
		{
			throw new Exception("Parse error (len)");
		}
		if (size > maxSize)
		{
			throw new Exception("Parse error (maxSize)");
		}
		type = Encoding.ASCII.GetString(buffer, (int)(startIndex + 4), 4);
		return 8u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong ToUInt64(byte[] buffer, uint startIndex)
	{
		return ((ulong)buffer[startIndex] << 56) | ((ulong)buffer[startIndex + 1] << 48) | ((ulong)buffer[startIndex + 2] << 40) | ((ulong)buffer[startIndex + 3] << 32) | ((ulong)buffer[startIndex + 4] << 24) | ((ulong)buffer[startIndex + 5] << 16) | ((ulong)buffer[startIndex + 6] << 8) | buffer[startIndex + 7];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint ToUInt32(byte[] buffer, uint startIndex)
	{
		return (uint)((buffer[startIndex] << 24) | (buffer[startIndex + 1] << 16) | (buffer[startIndex + 2] << 8) | buffer[startIndex + 3]);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint ToUInt32(string data)
	{
		return ToUInt32(Encoding.ASCII.GetBytes(data), 0u);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint ToUInt24(byte[] buffer, uint startIndex)
	{
		return (uint)((buffer[startIndex] << 16) | (buffer[startIndex + 1] << 8) | buffer[startIndex + 2]);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ushort ToUInt16(byte[] buffer, uint startIndex)
	{
		return (ushort)((ushort)(buffer[startIndex] << 8) | buffer[startIndex + 1]);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte ToUInt8(byte[] buffer, uint startIndex)
	{
		return buffer[startIndex];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Int16ToBuffer(ushort value, byte[] buffer, uint startIndex)
	{
		buffer[startIndex] = (byte)((value >> 8) & 0xFF);
		buffer[startIndex + 1] = (byte)(value & 0xFF);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte[] Int16ToBuffer(ushort value)
	{
		byte[] array = new byte[2];
		Int16ToBuffer(value, array, 0u);
		return array;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Int24ToBuffer(uint value, byte[] buffer, uint startIndex)
	{
		buffer[startIndex] = (byte)((value >> 16) & 0xFF);
		buffer[startIndex + 1] = (byte)((value >> 8) & 0xFF);
		buffer[startIndex + 2] = (byte)(value & 0xFF);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte[] Int24ToBuffer(uint value)
	{
		byte[] array = new byte[3];
		Int24ToBuffer(value, array, 0u);
		return array;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Int32ToBuffer(uint value, byte[] buffer, uint startIndex)
	{
		buffer[startIndex] = (byte)((value >> 24) & 0xFF);
		buffer[startIndex + 1] = (byte)((value >> 16) & 0xFF);
		buffer[startIndex + 2] = (byte)((value >> 8) & 0xFF);
		buffer[startIndex + 3] = (byte)(value & 0xFF);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte[] Int32ToBuffer(uint value)
	{
		byte[] array = new byte[4];
		Int32ToBuffer(value, array, 0u);
		return array;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Int64ToBuffer(ulong value, byte[] buffer, uint startIndex)
	{
		buffer[startIndex] = (byte)((value >> 56) & 0xFF);
		buffer[startIndex + 1] = (byte)((value >> 48) & 0xFF);
		buffer[startIndex + 2] = (byte)((value >> 40) & 0xFF);
		buffer[startIndex + 3] = (byte)((value >> 32) & 0xFF);
		buffer[startIndex + 4] = (byte)((value >> 24) & 0xFF);
		buffer[startIndex + 5] = (byte)((value >> 16) & 0xFF);
		buffer[startIndex + 6] = (byte)((value >> 8) & 0xFF);
		buffer[startIndex + 7] = (byte)(value & 0xFF);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte[] Int64ToBuffer(ulong value)
	{
		byte[] array = new byte[8];
		Int64ToBuffer(value, array, 0u);
		return array;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte[] BoxTypeToBuffer(string type)
	{
		return Encoding.ASCII.GetBytes(type);
	}
}
