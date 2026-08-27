using System;

namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseManager : ILicenseFeatureProvider, ILicenseRequestFactory, IDisposable
{
	string SystemInformationXml { get; }

	int RecorderLicenseCount { get; }

	int HardwareLicenseCount { get; }

	int ChannelLicenseCount { get; }

	string SystemSLC { get; }

	ITotalLicense TotalActivatedLicenses { get; }

	string ProductName { get; }

	string EncryptedSlc { get; }

	IRecordersRequiringLicenseUpdateDictionary RecordersRequiringUpdate { get; }

	ILicenseCountManager CountManager { get; }

	int TotalReplacementLimit { get; }

	bool IsLicenseExpired { get; }

	bool LicenseExpires { get; }

	DateTime LicenseExpiryDate { get; }

	string LicenseOwnerXml { get; set; }

	string SitesInformationXml { get; set; }

	Exception LastAutomaticActivationError { get; }

	DateTime LicenseActivatedDate { get; }

	IActivationManager ActivationManager { get; set; }

	IHardwareLicenseCountEvaluator HardwareLicenseCountEvaluator { get; }

	ILicenseCalculator LicenseCalculator { get; }

	int TotalAllowedChangesWithoutActivation { get; }

	event EventHandler AutomaticActivationFailed;

	event EventHandler RecordersNeedLicenseUpdate;

	void ActivateLicense(string userName, string password);

	string GetFeatureParameterValue(string featureName, string featureParameterName);

	byte[] GetRecorderLicense(Guid recorderId);

	byte[] GetRecorderLicense(Guid recorderId, ILicenseSnapshot licenseSnapshot, bool acquireLicenseForCurrentlyUnlicensedItems);

	byte[] CreateServerLicenseRequest(bool includeHardwareIds);

	bool IsHardwareInChangesWithoutActivation(string mac);

	bool IsHardwareInCurrentYearChangesWithoutActivation(string mac);

	bool IsHardwareActivated(IHardwareInfo hardwareInfo);

	bool NearExpiryAutomaticActivationNeeded(DateTime utcNow, DateTime expiryDate);

	void Initialize();

	bool HasLicense();

	void VerifyLicense();

	void QueueAutomaticActivation();

	void UpdateLicense(ILicenseDocument newLicense, LicenseUpdateOption updateOption);

	bool IsActivationRequired();

	bool QueryAutomaticLicenseActivationEnabled();

	int GetICPFeedLicenses(string feedType);

	int GetFeedLicenses(string feedType);

	ILicenseSnapshot LoadLicenseSnapshot();

	void UpdateLicenseSnapshot(ILicenseSnapshot licenseSnapshot);
}
