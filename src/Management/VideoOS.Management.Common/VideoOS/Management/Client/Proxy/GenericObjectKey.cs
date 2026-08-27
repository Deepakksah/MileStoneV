using System.Runtime.Serialization;

namespace VideoOS.Management.Client.Proxy;

[DataContract(Name = "GenericObjectKey", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class GenericObjectKey
{
	[DataMember]
	public string Id { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string ParentId { get; set; }

	[DataMember]
	public int Position { get; set; }

	[DataMember]
	public string Type { get; set; }
}
