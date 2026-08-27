using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public enum VideoSourceError
{
	[EnumMember]
	None,
	[EnumMember]
	ErrorUnspecified,
	[EnumMember]
	WarningUnspecified,
	[EnumMember]
	WarningIntradaProcessWarning,
	[EnumMember]
	ErrorCameraNotAvailable,
	[EnumMember]
	ErrorVideoSourceLicenseInvalid,
	[EnumMember]
	ErrorVideoSourceLicenseExpired,
	[EnumMember]
	ErrorCountryModuleLicenseInvalid,
	[EnumMember]
	ErrorCountryModuleLicenseExpired,
	[EnumMember]
	ErrorFailedToStartIntradaProcess,
	[EnumMember]
	ErrorFailedToStartIntradaProcessNoCountry,
	[EnumMember]
	ErrorIntradaProcessError,
	[EnumMember]
	ErrorVideoFeedLost
}
