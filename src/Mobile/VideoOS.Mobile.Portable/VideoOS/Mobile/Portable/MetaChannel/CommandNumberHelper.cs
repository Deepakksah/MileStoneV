namespace VideoOS.Mobile.Portable.MetaChannel;

public static class CommandNumberHelper
{
	private static uint _currCommandNumber;

	public static uint CommandNumber => _currCommandNumber++;
}
