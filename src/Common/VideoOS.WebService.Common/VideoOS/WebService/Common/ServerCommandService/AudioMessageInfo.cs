using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("AudioMessageInfo: {Name} ({Id})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class AudioMessageInfo
{
	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Description { get; set; }
}
