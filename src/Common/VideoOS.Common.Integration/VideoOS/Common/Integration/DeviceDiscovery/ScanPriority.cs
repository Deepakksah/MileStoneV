using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class ScanPriority
{
	public const int PriorityExcludeDriver = -1;

	public const int DefaultPriority = 0;

	[XmlElement("DeviceId")]
	public int DriverNumber { get; set; }

	[XmlElement("Priority")]
	public int Priority { get; set; }

	public ScanPriority()
	{
	}

	public ScanPriority(int driverNumber, int priority)
	{
		DriverNumber = driverNumber;
		Priority = priority;
	}
}
