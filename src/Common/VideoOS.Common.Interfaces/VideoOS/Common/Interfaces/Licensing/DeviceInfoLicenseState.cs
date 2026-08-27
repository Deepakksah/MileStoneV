namespace VideoOS.Common.Interfaces.Licensing;

public enum DeviceInfoLicenseState
{
	Unknown,
	ChangeWithoutActivation,
	Permanent,
	Grace,
	Expired,
	Unlicensed,
	NoLicenseRequired,
	LicensedOnOtherRecordingServer,
	UnlicensedDuplicate
}
