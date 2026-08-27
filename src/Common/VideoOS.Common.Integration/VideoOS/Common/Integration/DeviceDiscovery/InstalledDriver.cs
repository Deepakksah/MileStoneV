using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[DebuggerDisplay("Driver: {Name} ({Number})")]
public class InstalledDriver
{
	[XmlElement("DeviceId")]
	public int Number { get; set; }

	[XmlElement("DeviceTypeString")]
	public string Name { get; set; }

	[XmlElement("DeviceDllName")]
	public string DllName { get; set; }

	[XmlElement("DriverGroupName")]
	public string GroupName { get; set; }

	[XmlElement("CachedXml")]
	public string CachedXml { get; set; }
}
