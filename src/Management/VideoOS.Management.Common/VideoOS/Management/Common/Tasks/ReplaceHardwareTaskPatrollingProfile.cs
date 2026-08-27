using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

public class ReplaceHardwareTaskPatrollingProfile
{
	[XmlAttribute("Name", DataType = "string")]
	public string Name { get; set; }

	public ReplaceHardwareTaskPatrollingProfile()
	{
	}

	public ReplaceHardwareTaskPatrollingProfile(string name)
	{
		Name = name;
	}
}
