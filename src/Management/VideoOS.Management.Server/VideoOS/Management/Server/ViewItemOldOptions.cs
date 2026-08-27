using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "ViewItemOptions", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class ViewItemOldOptions
{
	[DataMember]
	public string UserDefinedId { get; set; }

	[DataMember]
	public CustomProperty[] CustomProperties { get; set; }
}
