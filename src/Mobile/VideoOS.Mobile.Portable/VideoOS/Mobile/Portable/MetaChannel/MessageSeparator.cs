namespace VideoOS.Mobile.Portable.MetaChannel;

public class MessageSeparator
{
	public delegate bool IsSeparator(byte[] data, int offset);

	public delegate ErrorCodes OnMessage(byte[] data, int offset, int length);

	private readonly IsSeparator _isSeparator;

	private readonly int _separatorLen;

	public OnMessage MessageEvent { get; set; }

	public int MessagesCount { get; private set; }

	public MessageSeparator(IsSeparator isSeparatorFunc, int separatorLen)
	{
		_isSeparator = isSeparatorFunc;
		_separatorLen = separatorLen;
	}

	public ErrorCodes ProcessBuffer(byte[] data, ref int offset, ref int length, bool isEndOfStream)
	{
		ErrorCodes result = ErrorCodes.Ok;
		int num = 0;
		if (!isEndOfStream)
		{
			for (int i = 0; i <= length - _separatorLen; i++)
			{
				if (_isSeparator(data, offset + i))
				{
					int num2 = i - num;
					if (num2 > 0)
					{
						result = MessageEvent(data, offset + num, num2);
						MessagesCount++;
					}
					num = i + _separatorLen;
					i += _separatorLen - 1;
				}
			}
		}
		else if (0 < length)
		{
			result = MessageEvent(data, offset + num, length);
			MessagesCount++;
		}
		offset += num;
		length -= num;
		return result;
	}

	public ErrorCodes ProcessBuffer(ByteBuffer tBuffer, bool isEndOfStream)
	{
		return ProcessBuffer(tBuffer.data, ref tBuffer.offset, ref tBuffer.lenWritten, isEndOfStream);
	}

	public void CompactBuffer(ByteBuffer tBuffer)
	{
		tBuffer.Compact();
	}

	public void CompactBuffer(byte[] data, ref int offset, ref int length)
	{
		ByteBuffer byteBuffer = new ByteBuffer(data, offset, length);
		CompactBuffer(byteBuffer);
		offset = byteBuffer.offset;
		length = byteBuffer.lenWritten;
	}
}
