using System.Xml.Serialization;

namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

[XmlRoot("GetGenericPortMappingEntryResponse", Namespace = "urn:schemas-upnp-org:service:WANIPConnection:1")]
public class GetGenericPortMappingEntryResponse : PortMapping
{
}
