using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class PortInterval
{
	public const ushort DefaultPort = 80;

	[XmlElement("FirstPort")]
	public ushort FirstPort { get; set; }

	[XmlElement("LastPort")]
	public ushort LastPort { get; set; }

	public PortInterval()
	{
		FirstPort = 80;
		LastPort = 80;
	}

	public PortInterval(ushort port)
	{
		FirstPort = port;
		LastPort = port;
	}

	public PortInterval(ushort firstPort, ushort lastPort)
	{
		FirstPort = firstPort;
		LastPort = lastPort;
	}
}
