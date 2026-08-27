using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[KnownType(typeof(CameraGroupInfo))]
[KnownType(typeof(EventTypeGroupInfo))]
[KnownType(typeof(InputGroupInfo))]
[KnownType(typeof(MetadataDeviceGroupInfo))]
[KnownType(typeof(MicrophoneGroupInfo))]
[KnownType(typeof(OutputGroupInfo))]
[KnownType(typeof(SpeakerGroupInfo))]
[KnownType(typeof(AlertTypeGroupInfo))]
[ExcludeFromCodeCoverage]
public class GroupInfo : IExtensibleDataObject, IGroupInfo
{
	private ExtensionDataObject extensionDataField;

	private string DescriptionField;

	private Guid GroupIdField;

	private string NameField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

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
