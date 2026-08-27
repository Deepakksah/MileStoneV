using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "ViewOptions", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class ViewOptionsOld
{
	[DataMember]
	public string UserDefinedId { get; set; }

	[DataMember]
	public ViewItemOld[] ViewItems { get; set; }

	[DataMember]
	public CustomProperty[] CustomProperties { get; set; }
}
