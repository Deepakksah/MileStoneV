using System;

namespace VideoOS.Mobile.MP4Muxing.VideoParser;

public class RbspParser
{
	private readonly byte[] _bytes;

	private uint _currentBit;

	public RbspParser(byte[] data, int startIndex, Func<byte[], byte[]> convertor)
	{
		_bytes = data;
		_currentBit = (uint)(startIndex * 8);
		if (convertor != null)
		{
			_bytes = convertor(data);
		}
	}

	public uint ReadBit()
	{
		uint num = _currentBit / 8;
		uint num2 = _currentBit % 8 + 1;
		_currentBit++;
		return (uint)((_bytes[num] >> (int)(8 - num2)) & 1);
	}

	public uint ReadBits(byte nBits)
	{
		uint num = 0u;
		for (int i = 0; i < nBits; i++)
		{
			num |= ReadBit() << nBits - i - 1;
		}
		return num;
	}

	public uint ReadUe()
	{
		return (uint)ReadExponentialGolombCode();
	}

	public int ReadSe()
	{
		int num = ReadExponentialGolombCode();
		if ((num & 1) != 0)
		{
			return (num + 1) / 2;
		}
		return -(num / 2);
	}

	public int ReadExponentialGolombCode()
	{
		int num = 0;
		while (ReadBit() == 0)
		{
			num++;
		}
		int num2 = 1 << num;
		for (int num3 = num - 1; num3 >= 0; num3--)
		{
			num2 |= (int)(ReadBit() << num3);
		}
		return num2 - 1;
	}
}
