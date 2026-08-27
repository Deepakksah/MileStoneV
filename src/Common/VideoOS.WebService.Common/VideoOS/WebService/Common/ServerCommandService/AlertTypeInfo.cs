using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("AlertTypeInfo: {Name} ({AlertTypeId})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class AlertTypeInfo
{
	[DataMember]
	public Guid AlertTypeId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Description { get; set; }
}
