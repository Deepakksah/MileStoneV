namespace VideoOS.Common.Integration.Command;

public enum TaskType
{
	AddHardware,
	DeleteStorage,
	MoveStorage,
	DeleteArchive,
	MoveArchive,
	MountArchive,
	UnmountArchive,
	TruncateDeviceStorage,
	MoveDevice,
	SetEncryptionMethod,
	SetEncryptionPassword,
	Archiving,
	Repairing,
	MoveHardware,
	ActivateLicense,
	HardwareScan,
	HardwareScanExpress,
	DeleteHardware,
	ChangeHardwarePassword,
	UpdateHardware,
	ReplaceHardware,
	FirmwareUpgrade,
	AddIdpAdmin,
	DeleteIdpAdmin,
	DeleteIdpUser,
	GetDevicePresets,
	DeleteRecorder
}
