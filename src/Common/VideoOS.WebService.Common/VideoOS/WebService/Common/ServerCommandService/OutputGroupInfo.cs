using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class OutputGroupInfo : GroupInfo
{
	[DataMember]
	public OutputGroupInfo[] OutputGroups { get; set; }

	[DataMember]
	public Guid[] Outputs { get; set; }
}
