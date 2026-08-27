using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("EventTypeInfo: {Name} ({EventTypeId})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class EventTypeInfo
{
	[DataMember]
	public Guid EventTypeId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Description { get; set; }
}
