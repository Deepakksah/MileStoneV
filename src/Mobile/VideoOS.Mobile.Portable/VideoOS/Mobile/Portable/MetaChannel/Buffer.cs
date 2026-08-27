namespace VideoOS.Mobile.Portable.MetaChannel;

public class Buffer<T>
{
	public T[] data;

	public int offset;

	public int lenWritten;

	public int begin => offset;

	public int end => offset + lenWritten;

	public int rest => data.Length - end;

	public int size => data.Length;

	public Buffer(int buffSize)
	{
		data = new T[buffSize];
		Clear();
	}

	public Buffer(T[] Outer, int Offset, int Len)
	{
		data = Outer;
		offset = Offset;
		lenWritten = Len;
	}

	public void Clear()
	{
		offset = (lenWritten = 0);
	}

	public int MoveOffset(int delta)
	{
		if (delta <= lenWritten)
		{
			offset += delta;
			lenWritten -= delta;
		}
		return offset;
	}
}
