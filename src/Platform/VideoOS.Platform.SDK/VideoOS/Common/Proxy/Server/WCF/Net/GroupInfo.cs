using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "GroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[KnownType(typeof(CameraGroupInfo))]
[KnownType(typeof(EventTypeGroupInfo))]
[KnownType(typeof(InputGroupInfo))]
[KnownType(typeof(MetadataDeviceGroupInfo))]
[KnownType(typeof(MicrophoneGroupInfo))]
[KnownType(typeof(OutputGroupInfo))]
[KnownType(typeof(SpeakerGroupInfo))]
[KnownType(typeof(AlertTypeGroupInfo))]
public class GroupInfo : IGroupInfo
{
	private string DescriptionField;

	private Guid GroupIdField;

	private string NameField;

	[DataMember]
	public string Description
	{
		get
		{
			return DescriptionField;
		}
		set
		{
			DescriptionField = value;
		}
	}

	[DataMember]
	public Guid GroupId
	{
		get
		{
			return GroupIdField;
		}
		set
		{
			GroupIdField = value;
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}
}
