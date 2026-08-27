using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class SpeakerGroupInfo : GroupInfo
{
	[DataMember]
	public SpeakerGroupInfo[] SpeakerGroups { get; set; }

	[DataMember]
	public Guid[] Speakers { get; set; }
}
