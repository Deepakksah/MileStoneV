using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class InputGroupInfo : GroupInfo
{
	[DataMember]
	public InputGroupInfo[] InputGroups { get; set; }

	[DataMember]
	public Guid[] Inputs { get; set; }
}
