using System.Xml.Serialization;

namespace VideoOS.Mobile.Portable.Upnp;

public class UpnpService
{
	[XmlElement("serviceType")]
	public string ServiceType { get; set; }

	[XmlElement("serviceId")]
	public string ServiceId { get; set; }

	[XmlElement("controlURL")]
	public string ControlUrl { get; set; }

	[XmlElement("eventSubURL")]
	public string EventUrl { get; set; }

	[XmlElement("SCPDURL")]
	public string ScpdUrl { get; set; }
}
