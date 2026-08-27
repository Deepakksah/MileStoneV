using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class ViewGroupInternal
{
	[DataMember]
	public ViewGroupInfo ViewGroupInfo { get; set; }

	[DataMember]
	public ViewGroupInternalData ViewGroupData { get; set; }
}
