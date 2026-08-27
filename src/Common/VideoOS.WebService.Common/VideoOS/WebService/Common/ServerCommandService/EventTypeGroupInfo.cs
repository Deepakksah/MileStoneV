using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class EventTypeGroupInfo : GroupInfo
{
	[DataMember]
	public EventTypeGroupInfo[] EventTypeGroups { get; set; }

	[DataMember]
	public Guid[] EventTypes { get; set; }
}
