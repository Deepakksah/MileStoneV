using System.Xml.Serialization;

namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

[XmlRoot("AddPortMapping", Namespace = "urn:schemas-upnp-org:service:WANIPConnection:1")]
public class AddPortMapping : PortMapping
{
	public AddPortMapping()
	{
	}

	public AddPortMapping(PortMapping portMapping)
		: base(portMapping)
	{
	}
}
