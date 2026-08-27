using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "MetadataDeviceGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
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
