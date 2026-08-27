using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IConfigurationInfo
{
	Guid ServerId { get; set; }

	string ServerName { get; set; }

	string ServerDescription { get; set; }

	IServerOption[] ServerOptions { get; set; }

	IRecorderInfo[] Recorders { get; set; }

	IApplicationSecurityInfo ApplicationAccess { get; set; }

	int FailoverCheckInterval { get; set; }

	ILicenseInfo[] Licenses { get; set; }

	IDeviceMappingInfo[] DeviceMappings { get; set; }

	ICameraGroupInfo[] CameraGroups { get; set; }

	IInputGroupInfo[] InputGroups { get; set; }

	IOutputGroupInfo[] OutputGroups { get; set; }

	IMicrophoneGroupInfo[] MicrophoneGroups { get; set; }

	ISpeakerGroupInfo[] SpeakerGroups { get; set; }

	IMetadataDeviceGroupInfo[] MetadataDeviceGroups { get; set; }

	IEventTypeGroupInfo[] EventTypeGroups { get; set; }

	IEventTypeInfo[] EventTypes { get; set; }

	ISystemEventTypeInfo[] SystemEventTypes { get; set; }

	IAlertTypeGroupInfo[] AlertTypeGroups { get; set; }

	IAlertTypeInfo[] AlertTypes { get; set; }

	IMatrixMonitorInfo[] MatrixMonitors { get; set; }

	IBookmarkSettingInfo BookmarkSettings { get; set; }

	IRetentionSettingInfo RetentionSettings { get; set; }
}
