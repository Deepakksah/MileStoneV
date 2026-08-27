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
public class SpeakerGroupInfo : GroupInfo, ISpeakerGroupInfo, IGroupInfo
{
	private SpeakerGroupInfo[] speakerGroupsField;

	private Guid[] speakersField;

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

	public SpeakerGroupInfo[] SpeakerGroups
	{
		get
		{
			return speakerGroupsField;
		}
		set
		{
			speakerGroupsField = value;
		}
	}

	public Guid[] Speakers
	{
		get
		{
			return speakersField;
		}
		set
		{
			speakersField = value;
		}
	}
}
