namespace VideoOS.Installer.Core.Cab;

public enum FDIError
{
	None,
	CabinetNotFound,
	NotACabinet,
	UnknownCabinetVersion,
	CorruptCabinet,
	AllocFail,
	BadCompressionType,
	MdiFail,
	TargetFile,
	ReserveMismatch,
	WrongCabinet,
	UserAbort
}
