using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SpeakerGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SpeakerGroupInfo : GroupInfo
{
	private SpeakerGroupInfo[] SpeakerGroupsField;

	private Guid[] SpeakersField;

	[DataMember]
	public SpeakerGroupInfo[] SpeakerGroups
	{
		get
		{
			return SpeakerGroupsField;
		}
		set
		{
			SpeakerGroupsField = value;
		}
	}

	[DataMember]
	public Guid[] Speakers
	{
		get
		{
			return SpeakersField;
		}
		set
		{
			SpeakersField = value;
		}
	}
}
