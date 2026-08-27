using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "SpeakerGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
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
