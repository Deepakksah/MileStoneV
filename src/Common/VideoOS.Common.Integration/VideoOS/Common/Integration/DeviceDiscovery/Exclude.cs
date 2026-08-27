using System.ComponentModel;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class Exclude
{
	public const ushort AllPorts = 0;

	[XmlElement("IpAddress")]
	public string IPAddress { get; set; }

	[XmlElement("Port")]
	[DefaultValue(0)]
	public ushort Port { get; set; }

	public Exclude()
	{
	}

	public Exclude(string ipAddress)
	{
		IPAddress = ipAddress;
		Port = 0;
	}

	public Exclude(string ipAddress, ushort port)
	{
		IPAddress = ipAddress;
		Port = port;
	}
}
