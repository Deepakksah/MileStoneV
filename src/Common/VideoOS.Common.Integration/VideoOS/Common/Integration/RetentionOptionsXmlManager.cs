using System;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using VideoOS.Common.Integration.Properties;

namespace VideoOS.Common.Integration;

public static class RetentionOptionsXmlManager
{
	public static string Serialize(RetentionOptionsXmlFormatter formatter)
	{
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(RetentionOptionsXmlFormatter));
			using StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
			xmlSerializer.Serialize(stringWriter, formatter);
			return stringWriter.ToString();
		}
		catch (InvalidOperationException)
		{
			throw;
		}
	}

	public static RetentionOptionsXmlFormatter Deserialize(string xmlText)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(RetentionOptionsXmlFormatter));
		using StringReader textReader = new StringReader(xmlText);
		try
		{
			return (RetentionOptionsXmlFormatter)xmlSerializer.Deserialize(textReader);
		}
		catch (InvalidOperationException)
		{
			throw;
		}
	}

	public static string GetOptionTimeDisplayName(OptionType optionType, int unit)
	{
		bool flag = unit == 1;
		return optionType switch
		{
			OptionType.Days => string.Format(CultureInfo.CurrentCulture, "{0} {1}", unit, flag ? EvidenceLock.EvidenceLock_TimeDay : EvidenceLock.EvidenceLock_TimeDays), 
			OptionType.Weeks => string.Format(CultureInfo.CurrentCulture, "{0} {1}", unit, flag ? EvidenceLock.EvidenceLock_TimeWeek : EvidenceLock.EvidenceLock_TimeWeeks), 
			OptionType.Month => string.Format(CultureInfo.CurrentCulture, "{0} {1}", unit, flag ? EvidenceLock.EvidenceLock_TimeMonth : EvidenceLock.EvidenceLock_TimeMonths), 
			OptionType.Years => string.Format(CultureInfo.CurrentCulture, "{0} {1}", unit, flag ? EvidenceLock.EvidenceLock_TimeYear : EvidenceLock.EvidenceLock_TimeYears), 
			OptionType.Indefinite => EvidenceLock.EvidenceLock_TimeIndefinite, 
			OptionType.UserDefined => EvidenceLock.EvidenceLock_TimeUserDefined, 
			_ => string.Empty, 
		};
	}
}
