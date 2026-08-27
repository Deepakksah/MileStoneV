using System;
using System.Globalization;
using System.Xml;

namespace VideoOS.Management.Common;

public class SunClockTimeProfileDefinition
{
	private int sunsetOffset;

	private int sunriseOffset;

	private GpsCoordinate gpsCoordinate;

	private string timezoneInfo;

	public int SunsetOffset
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

	public int SunriseOffset
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

	public GpsCoordinate GpsCoordinate
	{
		get
		{
			return gpsCoordinate;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value", "Gps coordinate can't be null");
			}
			gpsCoordinate = value;
		}
	}

	public string TimeZoneId
	{
		get
		{
			return timezoneInfo;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value", "TimeZone can't be null");
			}
			timezoneInfo = value;
		}
	}

	public SunClockTimeProfileDefinition()
	{
		Init(0, 0, new GpsCoordinate(0.0, 0.0), TimeZoneInfo.Local.Id);
	}

	public SunClockTimeProfileDefinition(int sunriseOffset, int sunsetOffset, GpsCoordinate gpsCoordinate, string timeZoneId)
	{
		Init(sunriseOffset, sunsetOffset, gpsCoordinate, timeZoneId);
	}

	public SunClockTimeProfileDefinition(int sunriseOffset, int sunsetOffset, string gpsCoordinateDisplayName, string timeZoneId)
	{
		GpsCoordinate gpsCoordinate = null;
		try
		{
			gpsCoordinate = new GpsCoordinate(gpsCoordinateDisplayName);
		}
		catch (FormatException innerException)
		{
			throw new ArgumentException("Invalid GPS parsing of gpsCoordinateDisplayName", innerException);
		}
		catch (ArgumentOutOfRangeException innerException2)
		{
			throw new ArgumentException("GPS values in gpsCoordinateDisplayName is out of range", innerException2);
		}
		Init(sunriseOffset, sunsetOffset, gpsCoordinate, timeZoneId);
	}

	public void Init(int sunriseOffset, int sunsetOffset, GpsCoordinate gpsCoordinate, string timeZoneId)
	{
		SunsetOffset = sunsetOffset;
		SunriseOffset = sunriseOffset;
		if (gpsCoordinate == null)
		{
			throw new ArgumentNullException("gpsCoordinate", "gpsCoordinate can't be null");
		}
		if (timeZoneId == null)
		{
			throw new ArgumentNullException("timeZoneId", "timeZoneId can't be null");
		}
		GpsCoordinate = gpsCoordinate;
		TimeZoneId = timeZoneId;
	}

	private static void ValidateOffset(int offset)
	{
		if (offset > 120 || offset < -120)
		{
			throw new ArgumentOutOfRangeException("offset", "Sunrise offset must be between -120 and 120");
		}
	}

	public string ToXml()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml("<SunClockTimeProfile></SunClockTimeProfile>");
		XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "GPSCoordinate", null);
		xmlNode.InnerText = GpsCoordinate.DisplayName;
		XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Element, "SunRiseOffset", null);
		xmlNode2.InnerText = SunriseOffset.ToString(CultureInfo.InvariantCulture);
		XmlNode xmlNode3 = xmlDocument.CreateNode(XmlNodeType.Element, "SunSetOffset", null);
		xmlNode3.InnerText = SunsetOffset.ToString(CultureInfo.InvariantCulture);
		XmlNode xmlNode4 = xmlDocument.CreateNode(XmlNodeType.Element, "TimeZone", null);
		xmlNode4.InnerText = TimeZoneId;
		xmlDocument.DocumentElement.AppendChild(xmlNode);
		xmlDocument.DocumentElement.AppendChild(xmlNode2);
		xmlDocument.DocumentElement.AppendChild(xmlNode3);
		xmlDocument.DocumentElement.AppendChild(xmlNode4);
		return xmlDocument.OuterXml;
	}

	public static SunClockTimeProfileDefinition FromXml(string definitionXml)
	{
		if (definitionXml == null)
		{
			throw new ArgumentNullException("definitionXml", "definitionXml can't be null");
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
		GpsCoordinate gpsCoordinate = null;
		int result = int.MinValue;
		int result2 = int.MinValue;
		TimeZoneInfo timeZoneInfo = null;
		XmlNode xmlNode = xmlDocument.SelectSingleNode("SunClockTimeProfile/GPSCoordinate");
		if (xmlNode == null)
		{
			throw new ArgumentException("definitionXml doesn't contain gps node");
		}
		if (!GpsCoordinate.Validate(xmlNode.InnerText))
		{
			throw new ArgumentException("gps node doesn't contain a valid gps coordinate");
		}
		try
		{
			gpsCoordinate = new GpsCoordinate(xmlNode.InnerText);
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
		foreach (TimeZoneInfo systemTimeZone in TimeZoneInfo.GetSystemTimeZones())
		{
			if (systemTimeZone.Id == xmlNode2.InnerText)
			{
				timeZoneInfo = systemTimeZone;
				break;
			}
		}
		if (xmlNode2 == null)
		{
			throw new ArgumentException("No valid timezone have been specified");
		}
		return new SunClockTimeProfileDefinition(result, result2, gpsCoordinate, timeZoneInfo.Id);
	}
}
