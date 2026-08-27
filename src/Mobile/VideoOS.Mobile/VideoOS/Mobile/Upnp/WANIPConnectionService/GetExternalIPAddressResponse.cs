using System.Xml.Serialization;

namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

[XmlRoot("GetExternalIPAddressResponse", Namespace = "urn:schemas-upnp-org:service:WANIPConnection:1")]
public class GetExternalIPAddressResponse
{
	[XmlElement(ElementName = "NewExternalIPAddress", Namespace = "")]
	public string NewExternalIPAddress { get; set; }
}
