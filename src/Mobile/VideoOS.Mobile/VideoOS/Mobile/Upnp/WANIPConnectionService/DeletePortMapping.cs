using System.Xml.Serialization;

namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

[XmlRoot("DeletePortMapping", Namespace = "urn:schemas-upnp-org:service:WANIPConnection:1")]
public class DeletePortMapping
{
	[XmlElement(Namespace = "", ElementName = "NewRemoteHost")]
	public string NewRemoteHost { get; set; }

	[XmlElement(Namespace = "", ElementName = "NewExternalPort")]
	public ushort NewExternalPort { get; set; }

	[XmlElement(Namespace = "", ElementName = "NewProtocol")]
	public string NewProtocol { get; set; }
}
