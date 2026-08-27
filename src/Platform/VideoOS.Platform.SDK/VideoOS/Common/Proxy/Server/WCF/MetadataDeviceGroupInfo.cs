using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MetadataDeviceGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class MetadataDeviceGroupInfo : GroupInfo, IMetadataDeviceGroupInfo, IGroupInfo
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

	IMetadataDeviceGroupInfo[] IMetadataDeviceGroupInfo.MetadataDeviceGroups
	{
		get
		{
			return MetadataDeviceGroups;
		}
		set
		{
			MetadataDeviceGroups = (MetadataDeviceGroupInfo[])value;
		}
	}
}
