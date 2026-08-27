using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class SchemeParameter
{
	[XmlElement("ParameterName")]
	public string Name { get; set; }

	[XmlElement("ParameterValue")]
	public string Value { get; set; }

	public SchemeParameter()
	{
	}

	public SchemeParameter(string name, string value)
	{
		Name = name;
		Value = value;
	}
}
