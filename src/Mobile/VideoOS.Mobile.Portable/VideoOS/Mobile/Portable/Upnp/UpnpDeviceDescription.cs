using System.Xml.Serialization;

namespace VideoOS.Mobile.Portable.Upnp;

[XmlRoot("root", Namespace = "urn:schemas-upnp-org:device-1-0")]
public class UpnpDeviceDescription
{
	public const string DeviceTypeFormat = "urn:{0}:device:{1}:1";

	public const string DeviceFriendlyNameFormat = "{0} Server {1}";

	public const string UdnFormat = "uuid:{0}";

	public const string UsnFormatRootDevice = "{0}::upnp:rootdevice";

	[XmlElement("specVersion")]
	public SpecVersion SpecVersion { get; set; }

	[XmlElement("device")]
	public Device Device { get; set; }

	public UpnpDeviceDescription()
	{
		SpecVersion = new SpecVersion();
	}
}
