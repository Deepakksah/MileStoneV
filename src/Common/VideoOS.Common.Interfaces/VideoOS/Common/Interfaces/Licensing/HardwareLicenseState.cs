namespace VideoOS.Common.Interfaces.Licensing;

public enum HardwareLicenseState
{
	Unknown,
	Valid,
	Temporary,
	Expired,
	ChangesWithoutActivation,
	Missing,
	Free,
	MissingDuplicate,
	NoLicenseRequired
}
