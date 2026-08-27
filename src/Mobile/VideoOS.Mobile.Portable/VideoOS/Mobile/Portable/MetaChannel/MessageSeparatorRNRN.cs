namespace VideoOS.Mobile.Portable.MetaChannel;

public class MessageSeparatorRNRN : MessageSeparator
{
	private static readonly int _separatorLen = 4;

	private static readonly byte _separatorR = 13;

	private static readonly byte _separatorN = 10;

	private static bool CheckSeparator(byte[] data, int offset)
	{
		if (data[offset] == _separatorR && data[offset + 1] == _separatorN && data[offset + 2] == _separatorR)
		{
			return data[offset + 3] == _separatorN;
		}
		return false;
	}

	public MessageSeparatorRNRN()
		: base(CheckSeparator, _separatorLen)
	{
	}
}
