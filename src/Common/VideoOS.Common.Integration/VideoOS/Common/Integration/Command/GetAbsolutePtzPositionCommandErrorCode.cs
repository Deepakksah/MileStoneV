namespace VideoOS.Common.Integration.Command;

public enum GetAbsolutePtzPositionCommandErrorCode
{
	RecorderNotFoundByDeviceId,
	DeviceNotFound,
	DeviceNotImplementIPresetInterface,
	DeviceNotSupportPresets,
	DatabaseCommunicationError,
	UnableToContactDevice
}
