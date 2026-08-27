using System;
using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

[XmlRoot("Result")]
public class AddHardwareTaskResult
{
	[XmlElement("HardwareId", DataType = "string")]
	public string InternalHardwareId { get; set; }

	[XmlIgnore]
	public Guid HardwareId
	{
		get
		{
			return new Guid(InternalHardwareId);
		}
		set
		{
			InternalHardwareId = value.ToString();
		}
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static AddHardwareTaskResult FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<AddHardwareTaskResult>(xml);
	}
}
