namespace VideoOS.Mobile.Portable.MetaChannel;

public class MessageSeparatorRN : MessageSeparator
{
	private static readonly int _separatorLen = 2;

	private static readonly byte _separatorR = 13;

	private static readonly byte _separatorN = 10;

	private static bool CheckSeparator(byte[] data, int offset)
	{
		if (data[offset] == _separatorR)
		{
			return data[offset + 1] == _separatorN;
		}
		return false;
	}

	public MessageSeparatorRN()
		: base(CheckSeparator, _separatorLen)
	{
	}
}
