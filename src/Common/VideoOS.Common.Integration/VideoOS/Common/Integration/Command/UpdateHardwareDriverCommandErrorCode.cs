namespace VideoOS.Common.Integration.Command;

public enum UpdateHardwareDriverCommandErrorCode
{
	NotOneHardwareRow,
	NotOneHardwareDefinitionRow,
	HardwareDefinitionNotFound,
	DeviceNotRelatingToHardware,
	DatabaseCommunicationError
}
