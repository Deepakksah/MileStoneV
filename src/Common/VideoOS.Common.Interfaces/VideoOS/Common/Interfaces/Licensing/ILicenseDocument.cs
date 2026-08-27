using System;
using System.Collections.ObjectModel;
using System.Xml;

namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseDocument
{
	Collection<ILicenseProperty> AllLicenseProperties { get; }

	string Sites { get; }

	string CustomerData { get; }

	DateTime Issued { get; }

	string SoftwareLicenseCode { get; }

	string SoftwareLicenseCodeEnvironment { get; }

	string CustomerName { get; }

	string EncryptedSlc { get; }

	DateTime ExpiryDate { get; }

	DateTime ActivatedDate { get; }

	bool IsLicenseExpired { get; }

	string ProductName { get; }

	string ProductVersion { get; }

	string ProductSku { get; }

	string SoftwareUpgradePlanExpireDate { get; }

	string MipPluginLicenseXml { get; }

	int RecorderLicenseCount { get; }

	int ChannelLicenseCount { get; }

	int HardwareLicenseCount { get; }

	int HardwareGracePeriod { get; }

	int TotalActivatedHardwareLicenses { get; }

	int TotalActivatedCameraLicenses { get; }

	int TotalActivatedICPCameraLicenses { get; }

	int TotalActivatedArculesCameraLicenses { get; }

	ReadOnlyCollection<string> AllFeatureNames { get; }

	ReadOnlyCollection<string> FeedTypes { get; }

	ReadOnlyCollection<string> ICPFeedTypes { get; }

	IReplacementLimit ReplacementLimit { get; }

	bool IsTrialLicense { get; }

	XmlNode WhitelistAsXmlNode { get; }

	void VerifyLicense();

	bool FeatureExists(string featureName);

	bool IsFeatureLicensed(string featureName);

	string GetFeatureParameterValue(string featureName, string featureParameterName);

	bool TryGetFeatureExpiryDate(string featureName, out DateTime expiryDate);

	int GetHardwareCount(bool isInterconnected);

	int GetHardwareCamerasCount(string macAddress);

	string GetDeviceLicenseKey(string macAddress);

	bool IsMacAddressLicensed(string macAddress);

	bool IsMacAddressLicensedHash(string macAddressHash);

	bool IsDeviceLicensed(IDeviceInfo deviceInfo);

	bool IsIcpDeviceLicensed(IDeviceInfo deviceInfo, int currentlyAllocated);

	int GetICPFeedLicenses(string feedType);

	int GetFeedLicenses(string feedType);

	int GetFeedLicenses(string feedType, bool isICPFeed);

	bool TryGetFeatureParameterValue(string featureName, string parameterName, out string parameterValue);

	Collection<string> GetFeatureParameterNames(string featureName);

	int FeatureParameterCount(string featureName);

	Guid SystemBuilderManufacturerId();
}
