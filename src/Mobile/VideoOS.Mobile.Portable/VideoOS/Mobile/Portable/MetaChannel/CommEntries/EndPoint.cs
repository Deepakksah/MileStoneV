using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class EndPoint
{
	public const string IpAddressName = "IpAddress";

	public const string PortName = "Port";

	[JsonProperty("IpAddress")]
	[XmlAttribute(AttributeName = "IpAddress")]
	public string IpAddress { get; set; }

	[JsonProperty("Port")]
	[XmlAttribute(AttributeName = "Port")]
	public string Port { get; set; }
}
