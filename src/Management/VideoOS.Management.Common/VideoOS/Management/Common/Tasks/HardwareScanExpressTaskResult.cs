using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

[XmlRoot("Result")]
public class HardwareScanExpressTaskResult
{
	[XmlArray("HardwareScanResults")]
	public HardwareScanResult[] HardwareScanResults { get; set; }

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static HardwareScanExpressTaskResult FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<HardwareScanExpressTaskResult>(xml);
	}
}
