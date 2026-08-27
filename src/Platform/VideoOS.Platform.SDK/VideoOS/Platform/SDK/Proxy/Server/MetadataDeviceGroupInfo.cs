using System;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

public class MetadataDeviceGroupInfo : IMetadataDeviceGroupInfo, IGroupInfo
{
	public MetadataDeviceGroupInfo[] MetadataDeviceGroups { get; set; }

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

	public Guid[] MetadataDevices { get; set; }

	public Guid GroupId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }
}
