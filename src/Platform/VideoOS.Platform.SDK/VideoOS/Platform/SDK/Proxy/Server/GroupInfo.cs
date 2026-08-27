using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[XmlInclude(typeof(InputGroupInfo))]
[XmlInclude(typeof(OutputGroupInfo))]
[XmlInclude(typeof(MicrophoneGroupInfo))]
[XmlInclude(typeof(SpeakerGroupInfo))]
[XmlInclude(typeof(EventTypeGroupInfo))]
[XmlInclude(typeof(AlertTypeGroupInfo))]
[XmlInclude(typeof(CameraGroupInfo))]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public abstract class GroupInfo : IGroupInfo
{
	private Guid groupIdField;

	private string nameField;

	private string descriptionField;

	public Guid GroupId
	{
		get
		{
			return groupIdField;
		}
		set
		{
			groupIdField = value;
		}
	}

	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	public string Description
	{
		get
		{
			return descriptionField;
		}
		set
		{
			descriptionField = value;
		}
	}
}
