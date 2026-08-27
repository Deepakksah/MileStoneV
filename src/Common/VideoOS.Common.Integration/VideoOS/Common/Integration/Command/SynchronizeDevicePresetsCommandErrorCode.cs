namespace VideoOS.Common.Integration.Command;

public enum SynchronizeDevicePresetsCommandErrorCode
{
	DeviceNotFound,
	DeviceNotStarted,
	DeviceNotImplementIPresetInterface,
	DeviceNotSupportPresets,
	DeviceCommunicationError
}
