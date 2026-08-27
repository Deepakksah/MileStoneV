using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

[XmlRoot("Result")]
public class HardwareScanTaskResult : HardwareScanResult
{
	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static HardwareScanTaskResult FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<HardwareScanTaskResult>(xml);
	}
}
