using System;
using System.Globalization;
using System.Xml;

namespace VideoOS.Common.SharedTimeProfile;

public class TimeProfileSunClockDefinition
{
	private int sunsetOffset;

	private int sunriseOffset;

	private GPSCoordinate gpsCoordinate;

	private TimeZoneInfo timezoneInfo;

	public int SunSetOffset
	{
		get
		{
			return sunsetOffset;
		}
		set
		{
			ValidateOffset(value);
			sunsetOffset = value;
		}
	}

	public int SunRiseOffset
	{
		get
		{
			return sunriseOffset;
		}
		set
		{
			ValidateOffset(value);
			sunriseOffset = value;
		}
	}

	public GPSCoordinate GPSCoordinate
	{
		get
		{
			return gpsCoordinate;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			gpsCoordinate = value;
		}
	}

	public TimeZoneInfo TimeZone
	{
		get
		{
			return timezoneInfo;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			timezoneInfo = value;
		}
	}

	public TimeProfileSunClockDefinition()
	{
		Init(0, 0, new GPSCoordinate(0.0, 0.0), TimeZoneInfo.Local.Id);
	}

	public TimeProfileSunClockDefinition(int sunRiseOffSet, int sunSetOffSet, GPSCoordinate gpsCoordinate, string timeZoneId)
	{
		Init(sunRiseOffSet, sunSetOffSet, gpsCoordinate, timeZoneId);
	}

	public TimeProfileSunClockDefinition(int sunRiseOffSet, int sunSetOffSet, string gpsCoordinateDisplayName, string timeZoneId)
	{
		GPSCoordinate gPSCoordinate = null;
		try
		{
			gPSCoordinate = new GPSCoordinate(gpsCoordinateDisplayName);
		}
		catch (FormatException innerException)
		{
			throw new ArgumentException("Invalid GPS parsing of gpsCoordinateDisplayName", innerException);
		}
		catch (ArgumentOutOfRangeException innerException2)
		{
			throw new ArgumentException("GPS values in gpsCoordinateDisplayName is out of range", innerException2);
		}
		Init(sunRiseOffSet, sunSetOffSet, gPSCoordinate, timeZoneId);
	}

	public void Init(int sunRiseOffSet, int sunSetOffSet, GPSCoordinate gpsCoordinate, string timeZoneId)
	{
		SunSetOffset = sunSetOffSet;
		SunRiseOffset = sunRiseOffSet;
		if (gpsCoordinate == null)
		{
			throw new ArgumentNullException("gpsCoordinate");
		}
		if (timeZoneId == null)
		{
			throw new ArgumentNullException("timeZoneId");
		}
		GPSCoordinate = gpsCoordinate;
		TimeZoneInfo timeZoneInfo = null;
		foreach (TimeZoneInfo systemTimeZone in TimeZoneInfo.GetSystemTimeZones())
		{
			if (systemTimeZone.Id == timeZoneId)
			{
				timeZoneInfo = systemTimeZone;
				break;
			}
		}
		if (timeZoneInfo == null)
		{
			throw new ArgumentException("timeZoneId contains unknown time zone id");
		}
		TimeZone = timeZoneInfo;
	}

	private static void ValidateOffset(int offSet)
	{
		if (offSet > 120 || offSet < -120)
		{
			throw new ArgumentOutOfRangeException("offSet", "SunRiseOffset must be between -120 and 120");
		}
	}

	public string ToXml()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml("<SunClockTimeProfile></SunClockTimeProfile>");
		XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "GPSCoordinate", null);
		xmlNode.InnerText = GPSCoordinate.DisplayName;
		XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Element, "SunRiseOffset", null);
		xmlNode2.InnerText = SunRiseOffset.ToString(CultureInfo.InvariantCulture);
		XmlNode xmlNode3 = xmlDocument.CreateNode(XmlNodeType.Element, "SunSetOffset", null);
		xmlNode3.InnerText = SunSetOffset.ToString(CultureInfo.InvariantCulture);
		XmlNode xmlNode4 = xmlDocument.CreateNode(XmlNodeType.Element, "TimeZone", null);
		xmlNode4.InnerText = TimeZone.Id;
		xmlDocument.DocumentElement.AppendChild(xmlNode);
		xmlDocument.DocumentElement.AppendChild(xmlNode2);
		xmlDocument.DocumentElement.AppendChild(xmlNode3);
		xmlDocument.DocumentElement.AppendChild(xmlNode4);
		return xmlDocument.OuterXml;
	}

	public static TimeProfileSunClockDefinition FromXml(string definitionXml)
	{
		if (definitionXml == null)
		{
			throw new ArgumentNullException("definitionXml");
		}
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.LoadXml(definitionXml);
		}
		catch (XmlException)
		{
			throw new ArgumentException("definitionXml can't be loaded as xml");
		}
		GPSCoordinate gPSCoordinate = null;
		int result = int.MinValue;
		int result2 = int.MinValue;
		TimeZoneInfo timeZoneInfo = null;
		XmlNode xmlNode = xmlDocument.SelectSingleNode("SunClockTimeProfile/GPSCoordinate");
		if (xmlNode == null)
		{
			throw new ArgumentException("definitionXml doesn't contain gps node");
		}
		if (!GPSCoordinate.Validate(xmlNode.InnerText))
		{
			throw new ArgumentException("gps node doesn't contain a valid gps coordinate");
		}
		try
		{
			gPSCoordinate = new GPSCoordinate(xmlNode.InnerText);
		}
		catch (FormatException innerException)
		{
			throw new ArgumentException("Invalid GPS parsing", innerException);
		}
		catch (ArgumentOutOfRangeException innerException2)
		{
			throw new ArgumentException("GPS values is out of range", innerException2);
		}
		if (!int.TryParse((xmlDocument.SelectSingleNode("SunClockTimeProfile/SunRiseOffset") ?? throw new ArgumentException("definitionXml doesn't contain sunrise offset node")).InnerText, out result))
		{
			throw new ArgumentException("sunrise offset isn't specified as a integer");
		}
		try
		{
			ValidateOffset(result);
		}
		catch (ArgumentOutOfRangeException innerException3)
		{
			throw new ArgumentException("Sunrise offset is out of range", innerException3);
		}
		if (!int.TryParse((xmlDocument.SelectSingleNode("SunClockTimeProfile/SunSetOffset") ?? throw new ArgumentException("definitionXml doesn't contain sunset offset node")).InnerText, out result2))
		{
			throw new ArgumentException("sunset offset isn't specified as a integer");
		}
		XmlNode xmlNode2 = xmlDocument.SelectSingleNode("SunClockTimeProfile/TimeZone");
		if (xmlNode2 == null)
		{
			throw new ArgumentException("definitionXml doesn't contain timezone node");
		}
		try
		{
			ValidateOffset(result2);
		}
		catch (ArgumentOutOfRangeException innerException4)
		{
			throw new ArgumentException("Sunset offset is out of range", innerException4);
		}
		TimeZoneInfo timeZoneInfo2 = null;
		foreach (TimeZoneInfo systemTimeZone in TimeZoneInfo.GetSystemTimeZones())
		{
			if (timeZoneInfo2 == null)
			{
				timeZoneInfo2 = systemTimeZone;
			}
			if (CompareTimeZone(systemTimeZone, "UTC"))
			{
				timeZoneInfo2 = systemTimeZone;
			}
			if (CompareTimeZone(systemTimeZone, xmlNode2.InnerText))
			{
				timeZoneInfo = systemTimeZone;
				break;
			}
		}
		if (timeZoneInfo == null)
		{
			timeZoneInfo = timeZoneInfo2;
		}
		return new TimeProfileSunClockDefinition(result, result2, gPSCoordinate, timeZoneInfo.Id);
	}

	public static bool CompareTimeZone(TimeZoneInfo timezoneInfo, string timezoneId)
	{
		return string.Equals(timezoneInfo.Id, timezoneId, StringComparison.OrdinalIgnoreCase);
	}
}
