using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public static class VmoResourcesLookup
{
	public static string GetErrorMessage(string key, CultureInfo culture)
	{
		string result = string.Empty;
		if (!string.IsNullOrEmpty(key))
		{
			result = VmoResources.ResourceManager.GetString(key, culture);
		}
		return result;
	}

	public static string GetMessageParameterString(string key, CultureInfo culture)
	{
		string result = string.Empty;
		if (!string.IsNullOrEmpty(key))
		{
			result = VmoMessageParamsResources.ResourceManager.GetString(key, culture);
		}
		return result;
	}
}
