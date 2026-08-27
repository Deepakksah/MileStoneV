using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SpeakerGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SpeakerGroupInfo : GroupInfo
{
	[OptionalField]
	private SpeakerGroupInfo[] SpeakerGroupsField;

	[OptionalField]
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
			if (SpeakerGroupsField != value)
			{
				SpeakerGroupsField = value;
				RaisePropertyChanged("SpeakerGroups");
			}
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
			if (SpeakersField != value)
			{
				SpeakersField = value;
				RaisePropertyChanged("Speakers");
			}
		}
	}
}
