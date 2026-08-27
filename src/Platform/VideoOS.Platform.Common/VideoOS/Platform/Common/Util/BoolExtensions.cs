namespace VideoOS.Platform.Common.Util;

public static class BoolExtensions
{
	public static string ToYesNo(this bool value)
	{
		return MediaCommonUtility.GetYesNoValue(value);
	}
}
