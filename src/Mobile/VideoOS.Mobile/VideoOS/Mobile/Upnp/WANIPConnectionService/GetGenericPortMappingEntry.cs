using System.Xml.Serialization;

namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

[XmlRoot("GetGenericPortMappingEntry", Namespace = "urn:schemas-upnp-org:service:WANIPConnection:1")]
public class GetGenericPortMappingEntry
{
	[XmlElement(Namespace = "", ElementName = "NewPortMappingIndex")]
	public int NewPortMappingIndex { get; set; }

	public GetGenericPortMappingEntry()
	{
		NewPortMappingIndex = 0;
	}
}
