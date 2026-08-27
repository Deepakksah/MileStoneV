using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MetadataDeviceGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MetadataDeviceGroupInfo : GroupInfo
{
	private MetadataDeviceGroupInfo[] MetadataDeviceGroupsField;

	private Guid[] MetadataDevicesField;

	[DataMember]
	public MetadataDeviceGroupInfo[] MetadataDeviceGroups
	{
		get
		{
			return MetadataDeviceGroupsField;
		}
		set
		{
			MetadataDeviceGroupsField = value;
		}
	}

	[DataMember]
	public Guid[] MetadataDevices
	{
		get
		{
			return MetadataDevicesField;
		}
		set
		{
			MetadataDevicesField = value;
		}
	}
}
