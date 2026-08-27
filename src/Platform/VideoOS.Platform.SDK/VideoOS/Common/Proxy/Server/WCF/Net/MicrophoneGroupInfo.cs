using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "MicrophoneGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
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
