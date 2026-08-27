using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SpeakerGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class SpeakerGroupInfo : GroupInfo, ISpeakerGroupInfo, IGroupInfo
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

	ISpeakerGroupInfo[] ISpeakerGroupInfo.SpeakerGroups
	{
		get
		{
			return SpeakerGroups;
		}
		set
		{
			SpeakerGroups = (SpeakerGroupInfo[])value;
		}
	}
}
