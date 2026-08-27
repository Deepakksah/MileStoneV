using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class EndiansInvertor
{
	public static long Invert(long value)
	{
		return (value >> 56) | ((value >> 40) & 0xFF00) | ((value >> 24) & 0xFF0000) | ((value >> 8) & 0xFF000000u) | ((value << 8) & 0xFF00000000L) | ((value << 24) & 0xFF0000000000L) | ((value << 40) & 0xFF000000000000L) | (value << 56);
	}

	public static ulong Invert(ulong value)
	{
		return (value >> 56) | ((value >> 40) & 0xFF00) | ((value >> 24) & 0xFF0000) | ((value >> 8) & 0xFF000000u) | ((value << 8) & 0xFF00000000L) | ((value << 24) & 0xFF0000000000L) | ((value << 40) & 0xFF000000000000L) | (value << 56);
	}

	public static int Invert(int value)
	{
		return (value >> 24) | ((value >> 8) & 0xFF00) | ((value << 8) & 0xFF0000) | (value << 24);
	}

	public static uint Invert(uint value)
	{
		return (value >> 24) | ((value >> 8) & 0xFF00) | ((value << 8) & 0xFF0000) | (value << 24);
	}

	public static short Invert(short value)
	{
		return (short)((value >> 8) | (value << 8));
	}

	public static ushort Invert(ushort value)
	{
		return (ushort)((value >> 8) | (value << 8));
	}

	public static Guid Invert(Guid value)
	{
		byte[] array = value.ToByteArray();
		byte b = array[0];
		array[0] = array[3];
		array[3] = b;
		b = array[1];
		array[1] = array[2];
		array[2] = b;
		b = array[4];
		array[4] = array[5];
		array[5] = b;
		b = array[6];
		array[6] = array[7];
		array[7] = b;
		return new Guid(array);
	}
}
