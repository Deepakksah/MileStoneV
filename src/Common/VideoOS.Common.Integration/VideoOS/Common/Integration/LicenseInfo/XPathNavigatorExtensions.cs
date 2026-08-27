using System;
using System.Globalization;
using System.Xml.XPath;

namespace VideoOS.Common.Integration.LicenseInfo;

public static class XPathNavigatorExtensions
{
	public static string GetAttributeValueAsString(this XPathNavigator navigator, string attributeName)
	{
		if (navigator == null)
		{
			throw new ArgumentNullException("navigator");
		}
		return navigator.GetAttribute(attributeName, string.Empty);
	}

	public static bool GetAttributeValueAsBoolean(this XPathNavigator navigator, string attributeName)
	{
		if (navigator == null)
		{
			throw new ArgumentNullException("navigator");
		}
		return Convert.ToBoolean(navigator.GetAttribute(attributeName, string.Empty), CultureInfo.InvariantCulture);
	}

	public static short GetAttributeValueAsInt16(this XPathNavigator navigator, string attributeName)
	{
		if (navigator == null)
		{
			throw new ArgumentNullException("navigator");
		}
		return Convert.ToInt16(navigator.GetAttribute(attributeName, string.Empty), CultureInfo.InvariantCulture);
	}

	public static int GetAttributeValueAsInt32(this XPathNavigator navigator, string attributeName)
	{
		if (navigator == null)
		{
			throw new ArgumentNullException("navigator");
		}
		string attribute = navigator.GetAttribute(attributeName, string.Empty);
		if (string.IsNullOrEmpty(attribute))
		{
			return 0;
		}
		return Convert.ToInt32(attribute, CultureInfo.InvariantCulture);
	}

	public static DateTime GetAttributeValueAsDateTime(this XPathNavigator navigator, string attributeName)
	{
		if (navigator == null)
		{
			throw new ArgumentNullException("navigator");
		}
		return DateTime.ParseExact(navigator.GetAttribute(attributeName, string.Empty), "o", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
	}
}
