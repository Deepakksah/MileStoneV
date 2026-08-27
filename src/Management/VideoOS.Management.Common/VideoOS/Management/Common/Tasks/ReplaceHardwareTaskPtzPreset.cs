using System;
using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

public class ReplaceHardwareTaskPtzPreset
{
	[XmlAttribute("Id", DataType = "string")]
	public string InternalId { get; set; }

	[XmlIgnore]
	public Guid Id
	{
		get
		{
			return new Guid(InternalId);
		}
		set
		{
			InternalId = value.ToString();
		}
	}

	[XmlAttribute("Name", DataType = "string")]
	public string Name { get; set; }

	public ReplaceHardwareTaskPtzPreset()
	{
	}

	public ReplaceHardwareTaskPtzPreset(Guid id, string name)
	{
		Id = id;
		Name = name;
	}
}
