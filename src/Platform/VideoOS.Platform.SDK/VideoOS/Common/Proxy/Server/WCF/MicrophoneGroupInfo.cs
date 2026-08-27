using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MicrophoneGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class MicrophoneGroupInfo : GroupInfo, IMicrophoneGroupInfo, IGroupInfo
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

	IMicrophoneGroupInfo[] IMicrophoneGroupInfo.MicrophoneGroups
	{
		get
		{
			return MicrophoneGroups;
		}
		set
		{
			MicrophoneGroups = (MicrophoneGroupInfo[])value;
		}
	}
}
