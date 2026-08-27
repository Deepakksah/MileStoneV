using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IMetadataDeviceGroupInfo : IGroupInfo
{
	IMetadataDeviceGroupInfo[] MetadataDeviceGroups { get; set; }

	Guid[] MetadataDevices { get; set; }
}
