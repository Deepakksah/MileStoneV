using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MicrophoneGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MicrophoneGroupInfo : GroupInfo
{
	private MicrophoneGroupInfo[] MicrophoneGroupsField;

	private Guid[] MicrophonesField;

	[DataMember]
	public MicrophoneGroupInfo[] MicrophoneGroups
	{
		get
		{
			return MicrophoneGroupsField;
		}
		set
		{
			MicrophoneGroupsField = value;
		}
	}

	[DataMember]
	public Guid[] Microphones
	{
		get
		{
			return MicrophonesField;
		}
		set
		{
			MicrophonesField = value;
		}
	}
}
