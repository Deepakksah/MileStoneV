namespace VideoOS.Platform.SDK.Interface;

public interface IMetadataDeviceInfo : IMediaDeviceInfo, IDeviceInfo
{
	IMetadataDeviceSecurityInfo MetadataDeviceSecurity { get; set; }

	IMetadataTypeInfo[] MetadataTypes { get; set; }
}
