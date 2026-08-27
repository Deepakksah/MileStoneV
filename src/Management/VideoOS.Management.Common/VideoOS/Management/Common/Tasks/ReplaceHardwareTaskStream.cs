using System;
using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

public class ReplaceHardwareTaskStream
{
	[XmlAttribute("ReferenceId", DataType = "string")]
	public string InternalReferenceId { get; set; }

	[XmlIgnore]
	public Guid ReferenceId
	{
		get
		{
			return new Guid(InternalReferenceId);
		}
		set
		{
			InternalReferenceId = value.ToString();
		}
	}

	[XmlAttribute("Name", DataType = "string")]
	public string Name { get; set; }

	public ReplaceHardwareTaskStream()
	{
	}

	public ReplaceHardwareTaskStream(Guid referenceId, string name)
	{
		ReferenceId = referenceId;
		Name = name;
	}
}
