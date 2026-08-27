using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

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
public class GroupInfo : IExtensibleDataObject
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
