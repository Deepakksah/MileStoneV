using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MicrophoneGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MicrophoneGroupInfo : GroupInfo
{
	[OptionalField]
	private MicrophoneGroupInfo[] MicrophoneGroupsField;

	[OptionalField]
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
			if (MicrophoneGroupsField != value)
			{
				MicrophoneGroupsField = value;
				RaisePropertyChanged("MicrophoneGroups");
			}
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
			if (MicrophonesField != value)
			{
				MicrophonesField = value;
				RaisePropertyChanged("Microphones");
			}
		}
	}
}
