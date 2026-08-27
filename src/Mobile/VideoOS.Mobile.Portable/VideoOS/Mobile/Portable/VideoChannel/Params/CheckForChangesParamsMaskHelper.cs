namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public static class CheckForChangesParamsMaskHelper
{
	public static bool ContainsOption(this CheckForChangesParamsMask mask, CheckForChangesParamsMask option)
	{
		return (mask & option) == option;
	}
}
