using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("GroupInfo: {Name} ({GroupId})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public abstract class GroupInfo
{
	[DataMember]
	public Guid GroupId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Description { get; set; }
}
