using System.ComponentModel;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class AddressInterval
{
	[XmlElement("DNS")]
	[DefaultValue("")]
	public string DnsHostName { get; set; }

	[XmlElement("FirstIpAddress")]
	[DefaultValue("")]
	public string FirstIPAddress { get; set; }

	[XmlElement("LastIpAddress")]
	[DefaultValue("")]
	public string LastIPAddress { get; set; }

	[XmlElement("PortInterval")]
	public PortIntervalCollection PortIntervals { get; set; }

	[XmlElement("UseTcpPortScanning")]
	[DefaultValue(true)]
	public bool UseTcpPortScanning { get; set; }

	[XmlElement("UseSecurePort")]
	public bool UseSecurePort { get; set; }

	[XmlElement("InParams")]
	public string InputParametersRaw
	{
		get
		{
			return InputParameters?.ToJson() ?? string.Empty;
		}
		set
		{
			InputParameters = DiscoveryInputParameters.FromJson(value);
		}
	}

	[XmlIgnore]
	public DiscoveryInputParameters InputParameters { get; set; }

	public AddressInterval()
	{
		PortIntervals = new PortIntervalCollection();
		InputParameters = new DiscoveryInputParameters
		{
			DetectFactoryDefault = true
		};
	}

	public AddressInterval(string dnsHostName, bool useSecurePort)
		: this()
	{
		DnsHostName = dnsHostName;
		UseSecurePort = useSecurePort;
	}

	public AddressInterval(string dnsHostName, ushort port, bool useSecurePort)
		: this()
	{
		DnsHostName = dnsHostName;
		PortIntervals.Add(new PortInterval(port));
		UseSecurePort = useSecurePort;
	}

	public AddressInterval(string firstIPAddress, string lastIPAddress, bool useSecurePort)
		: this()
	{
		FirstIPAddress = firstIPAddress;
		LastIPAddress = lastIPAddress;
		UseSecurePort = useSecurePort;
	}

	public AddressInterval(string firstIPAddress, string lastIPAddress, ushort port, bool useSecurePort)
		: this()
	{
		FirstIPAddress = firstIPAddress;
		LastIPAddress = lastIPAddress;
		PortIntervals.Add(new PortInterval(port));
		UseSecurePort = useSecurePort;
	}

	public AddressInterval(string dnsHostName, ushort firstPort, ushort lastPort, bool useSecurePort)
		: this()
	{
		DnsHostName = dnsHostName;
		PortIntervals.Add(new PortInterval(firstPort, lastPort));
		UseSecurePort = useSecurePort;
	}

	public AddressInterval(string firstIPAddress, string lastIPAddress, ushort firstPort, ushort lastPort, bool useSecurePort)
		: this()
	{
		FirstIPAddress = firstIPAddress;
		LastIPAddress = lastIPAddress;
		PortIntervals.Add(new PortInterval(firstPort, lastPort));
		UseSecurePort = useSecurePort;
	}
}
