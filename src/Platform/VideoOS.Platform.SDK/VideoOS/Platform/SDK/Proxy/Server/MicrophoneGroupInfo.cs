using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class MicrophoneGroupInfo : GroupInfo, IMicrophoneGroupInfo, IGroupInfo
{
	private MicrophoneGroupInfo[] microphoneGroupsField;

	private Guid[] microphonesField;

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

	public MicrophoneGroupInfo[] MicrophoneGroups
	{
		get
		{
			return microphoneGroupsField;
		}
		set
		{
			microphoneGroupsField = value;
		}
	}

	public Guid[] Microphones
	{
		get
		{
			return microphonesField;
		}
		set
		{
			microphonesField = value;
		}
	}
}
