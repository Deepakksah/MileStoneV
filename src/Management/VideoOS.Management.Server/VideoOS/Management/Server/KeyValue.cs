using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "KeyValueInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class KeyValue
{
	[DataMember]
	public string Key { get; set; }

	[DataMember]
	public string Value { get; set; }

	[DataMember]
	public string Type { get; set; }
}
