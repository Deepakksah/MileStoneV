using System;
using System.Drawing;
using System.Text.RegularExpressions;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Platform;

namespace VideoOS.Mobile.Oem;

public class MipSdkWrapperOem
{
	public const string LogoLargeName = "LogoLarge";

	public const string LogoSmallName = "LogoSmall";

	public const string LogoWebName = "LogoWeb";

	public const string CobrandLinkName = "CobrandLink";

	public const string ServerDescriptionName = "ServerDescription";

	public static object GetOemResource(string key)
	{
		object result = null;
		try
		{
			result = EnvironmentManager.Instance.LicenseManager.OemResourceManager.GetOemResource(key);
		}
		catch (Exception ex)
		{
			Log.Error("MipAskWrapperOem.GetOemResource()", ex.Message);
		}
		return result;
	}

	public static Image GetOemImage(string key)
	{
		return GetOemResource(key) as Image;
	}

	public static string GetOemString(string key)
	{
		return GetOemResource(key) as string;
	}

	public static string GetOemStringSingleLine(string key)
	{
		string text = GetOemString(key);
		if (text != null)
		{
			text = Regex.Replace(text, "\\r\\n?|\\n", "");
		}
		return text;
	}
}
