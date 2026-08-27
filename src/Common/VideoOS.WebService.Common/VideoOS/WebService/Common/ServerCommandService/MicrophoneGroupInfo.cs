using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class MicrophoneGroupInfo : GroupInfo
{
	[DataMember]
	public MicrophoneGroupInfo[] MicrophoneGroups { get; set; }

	[DataMember]
	public Guid[] Microphones { get; set; }
}
