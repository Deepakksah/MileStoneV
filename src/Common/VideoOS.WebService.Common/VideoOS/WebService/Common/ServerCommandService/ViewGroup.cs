using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ViewGroup
{
	[DataMember]
	public ViewGroupInfo ViewGroupInfo { get; set; }

	[DataMember]
	public ViewGroupData ViewGroupData { get; set; }
}
