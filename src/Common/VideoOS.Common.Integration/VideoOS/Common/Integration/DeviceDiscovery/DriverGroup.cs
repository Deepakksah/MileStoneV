using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class DriverGroup
{
	[XmlElement("Name")]
	public string Name { get; set; }

	[XmlElement("Expression")]
	public string Expression { get; set; }

	public DriverGroup()
	{
		Name = string.Empty;
		Expression = string.Empty;
	}
}
