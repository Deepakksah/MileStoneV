using System;
using System.Globalization;
using VideoOS.Platform.Resources;

namespace VideoOS.Platform.AccessControl.Internal.Util;

public static class DisplayTextHelper
{
	public static string GetBuiltInDisplayName(string key, string fallbackValue = "")
	{
		string text = null;
		try
		{
			text = Strings.ResourceManager.GetString("AccessControl" + key.Replace('.', '_'));
		}
		catch (Exception)
		{
		}
		if (text == null)
		{
			text = fallbackValue;
		}
		return text;
	}

	public static string GetBuiltInDisplayName(string key, CultureInfo culture, string fallbackValue = "")
	{
		string text = null;
		try
		{
			text = Strings.ResourceManager.GetString("AccessControl" + key.Replace('.', '_'), culture);
		}
		catch (Exception)
		{
		}
		if (text == null)
		{
			text = fallbackValue;
		}
		return text;
	}
}
