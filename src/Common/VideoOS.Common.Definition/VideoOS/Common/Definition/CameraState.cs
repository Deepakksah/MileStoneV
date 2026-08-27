namespace VideoOS.Common.Definition;

public enum CameraState
{
	Unknown,
	Started,
	Stopped,
	DbRepair,
	Overflow,
	WriteDbError,
	LicenseError,
	ConnectionError
}
