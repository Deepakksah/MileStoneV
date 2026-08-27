using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class ConnectionQuality
{
	public const string FPSXmlName = "FpsTousends";

	public const string BandwidthXmlName = "BandWidthKb";

	public const string BandwidthDecimalXmlName = "BandWidthDecimalKb";

	[JsonProperty("FpsTousends")]
	[XmlAttribute(AttributeName = "FpsTousends")]
	public uint FPSInTausents { get; set; }

	[JsonProperty("BandWidthKb")]
	[XmlAttribute(AttributeName = "BandWidthKb")]
	public uint BandwidthInKB { get; set; }

	[JsonProperty("BandWidthDecimalKb")]
	[XmlAttribute(AttributeName = "BandWidthDecimalKb")]
	public double BandwidthDecimalInKB { get; set; }

	public ConnectionQuality()
	{
	}

	public ConnectionQuality(ConnectionQuality other)
	{
		FPSInTausents = other.FPSInTausents;
		BandwidthInKB = other.BandwidthInKB;
		BandwidthDecimalInKB = other.BandwidthDecimalInKB;
	}
}
