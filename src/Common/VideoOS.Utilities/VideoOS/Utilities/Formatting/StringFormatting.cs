using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Utilities.Formatting;

public static class StringFormatting
{
	private delegate string FormatByteStringDelegate(decimal result, string unit, IFormatProvider formatProvider);

	public static string FormatByteSizeStringExact(long bytes, IFormatProvider formatProvider)
	{
		return FormatByteSizeString(bytes, changeUnitBeforeLimit: false, formatProvider, GetByteSizeResultStringExact);
	}

	public static string FormatByteSizeStringExact(ulong bytes, IFormatProvider formatProvider)
	{
		return FormatByteSizeString(bytes, changeUnitBeforeLimit: false, formatProvider, GetByteSizeResultStringExact);
	}

	private static string GetByteSizeResultStringExact(decimal result, string unit, IFormatProvider formatProvider)
	{
		decimal num = System.Math.Round(result, 2);
		if (num % 1m == 0m)
		{
			return string.Format(formatProvider, FormattingResources.BytesFormatZeroDigits, num, unit);
		}
		return string.Format(formatProvider, FormattingResources.BytesFormatTwoDigits, result, unit);
	}

	public static string FormatByteSizeString(long bytes)
	{
		CultureInfo formatProvider = (CultureInfo.CurrentUICulture.IsNeutralCulture ? CultureInfo.CurrentCulture : CultureInfo.CurrentUICulture);
		return FormatByteSizeString(bytes, changeUnitBeforeLimit: true, formatProvider, GetByteSizeResultString);
	}

	public static string FormatByteSizeString(ulong bytes)
	{
		CultureInfo formatProvider = (CultureInfo.CurrentUICulture.IsNeutralCulture ? CultureInfo.CurrentCulture : CultureInfo.CurrentUICulture);
		return FormatByteSizeString(bytes, changeUnitBeforeLimit: true, formatProvider, GetByteSizeResultString);
	}

	private static string GetByteSizeResultString(decimal result, string unit, IFormatProvider formatProvider)
	{
		if (result > 99m)
		{
			return string.Format(formatProvider, FormattingResources.BytesFormatZeroDigits, result, unit);
		}
		if (result > 9m)
		{
			return string.Format(formatProvider, FormattingResources.BytesFormatOneDigit, result, unit);
		}
		return string.Format(formatProvider, FormattingResources.BytesFormatTwoDigits, result, unit);
	}

	private static string FormatByteSizeString(ulong bytes, bool changeUnitBeforeLimit, IFormatProvider formatProvider, FormatByteStringDelegate formatByteString)
	{
		decimal num = (changeUnitBeforeLimit ? 11 : 0);
		if ((decimal)bytes >= 1125899906842624m * (1024m - num))
		{
			decimal result = (decimal)bytes / (decimal)System.Math.Pow(1024.0, 6.0);
			return formatByteString(result, FormattingResources.UnitExabyte, formatProvider);
		}
		if ((decimal)bytes >= 1099511627776m * (1024m - num))
		{
			decimal result2 = (decimal)bytes / (decimal)System.Math.Pow(1024.0, 5.0);
			return formatByteString(result2, FormattingResources.UnitPetabyte, formatProvider);
		}
		if ((decimal)bytes >= 1073741824m * (1024m - num))
		{
			decimal result3 = (decimal)bytes / (decimal)System.Math.Pow(1024.0, 4.0);
			return formatByteString(result3, FormattingResources.UnitTerabyte, formatProvider);
		}
		if ((decimal)bytes >= 1048576m * (1024m - num))
		{
			decimal result4 = (decimal)bytes / (decimal)System.Math.Pow(1024.0, 3.0);
			return formatByteString(result4, FormattingResources.UnitGigabyte, formatProvider);
		}
		if ((decimal)bytes >= 1024m * (1024m - num))
		{
			decimal result5 = (decimal)bytes / (decimal)System.Math.Pow(1024.0, 2.0);
			return formatByteString(result5, FormattingResources.UnitMegabyte, formatProvider);
		}
		if ((decimal)bytes >= 1024m - num)
		{
			decimal result6 = (decimal)bytes / 1024m;
			return formatByteString(result6, FormattingResources.UnitKilobyte, formatProvider);
		}
		if (bytes == 1)
		{
			return FormattingResources.BytesFormatOneByte;
		}
		return string.Format(formatProvider, FormattingResources.BytesFormatBytes, bytes);
	}

	private static string FormatByteSizeString(long bytes, bool changeUnitBeforeLimit, IFormatProvider formatProvider, FormatByteStringDelegate formatByteString)
	{
		return FormatByteSizeString((ulong)bytes, changeUnitBeforeLimit, formatProvider, formatByteString);
	}

	public static string FormatTimeSpanString(long units, TimeUnit inputTimeUnit)
	{
		if (inputTimeUnit == TimeUnit.Minutes)
		{
			if (units > 0 && units % 1440 == 0L)
			{
				long num = units / 1440;
				if (num == 1)
				{
					return FormattingResources.TimeFormatOneDay;
				}
				return string.Format(CultureInfo.CurrentCulture, FormattingResources.TimeFormatDays, num);
			}
			if (units > 0 && units % 60 == 0L)
			{
				long num2 = units / 60;
				if (num2 == 1)
				{
					return FormattingResources.TimeFormatOneHour;
				}
				return string.Format(CultureInfo.CurrentCulture, FormattingResources.TimeFormatHours, num2);
			}
			if (units == 1)
			{
				return FormattingResources.TimeFormatOneMinute;
			}
			return string.Format(CultureInfo.CurrentCulture, FormattingResources.TimeFormatMinutes, units);
		}
		throw new NotSupportedException("Unsupported TimeUnit value received");
	}

	public static string PrettifyInlineXmlString(string inlineXml)
	{
		if (inlineXml == null)
		{
			throw new ArgumentNullException("inlineXml");
		}
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(inlineXml);
			using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = System.Xml.Formatting.Indented;
			xmlDocument.WriteTo(xmlTextWriter);
			xmlTextWriter.Flush();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(stringWriter.ToString());
			return stringBuilder.ToString();
		}
		catch (XmlException ex)
		{
			if (!string.IsNullOrEmpty(inlineXml))
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", ex.Message, inlineXml);
			}
			throw new ArgumentException("Configuration xml is empty");
		}
	}
}
