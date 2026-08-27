using System;

namespace VideoOS.Mobile.Portable.MetaChannel;

public class ByteBuffer : Buffer<byte>
{
	public ByteBuffer(int buffSize)
		: base(buffSize)
	{
	}

	public ByteBuffer(byte[] Outer, int Offset, int Len)
		: base(Outer, Offset, Len)
	{
	}

	public void Compact()
	{
		if (offset > 0)
		{
			if (lenWritten > 0)
			{
				Buffer.BlockCopy(data, offset, data, 0, lenWritten);
			}
			offset = 0;
		}
	}

	public void AddData(byte[] Outer, int Offset, int Len)
	{
		Buffer.BlockCopy(Outer, Offset, data, base.end, Len);
		lenWritten += Len;
	}

	public void AddData(ByteBuffer buffer)
	{
		AddData(buffer.data, buffer.offset, buffer.lenWritten);
	}

	public void Resize(int newSize)
	{
		if (newSize != base.size)
		{
			if (newSize < base.end)
			{
				throw new Exception("Impossible to resize the buffer to smaller that current written size");
			}
			byte[] dst = new byte[newSize];
			Buffer.BlockCopy(data, 0, dst, 0, base.end);
			data = dst;
		}
	}
}
