using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.DataCollectorService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public class MachineProperty
{
	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Value { get; set; }

	public MachineProperty()
		: this(string.Empty, string.Empty)
	{
	}

	public MachineProperty(string name, string value)
	{
		Name = name;
		Value = value;
	}
}
