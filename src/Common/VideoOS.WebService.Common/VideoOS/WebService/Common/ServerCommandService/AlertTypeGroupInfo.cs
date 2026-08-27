using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class AlertTypeGroupInfo : GroupInfo
{
	[DataMember]
	public AlertTypeGroupInfo[] AlertTypeGroups { get; set; }

	[DataMember]
	public Guid[] AlertTypes { get; set; }
}
