using System.Globalization;

namespace VideoOS.Common.Integration.LicenseInfo;

public static class LicenseDisplayNameKeys
{
	public static string ApplicationNameKey(string featureName)
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}_{1}", featureName, "ApplicationName");
	}

	public static string SLCKey(string featureName)
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}_{1}", featureName, "SLC");
	}
}
