using System;
using System.Collections.Generic;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration;

public interface IFeatureManager
{
	bool HasLicense { get; }

	bool IsTrialLicense { get; }

	string LicenseProductName { get; }

	string SLC { get; }

	string ProductSku { get; }

	bool LicenseExpires { get; }

	DateTime LicenseExpiryDate { get; }

	IEnumerable<string> AllFeatureNames { get; }

	IEnumerable<ILicenseProperty> AllLicenseProperties { get; }

	void Initialize(ILicenseDocument licenseDocument);

	void Refresh(ILicenseDocument licenseDocument);

	bool IsFeatureLicensed(string featureName);

	string GetFeatureParameterValue(string featureName, string parameterName);

	T GetFeatureParameterValue<T>(string featureName, string parameterName) where T : struct;

	int FeatureParameterCount(string featureName);

	bool TryGetFeatureParameterValue(string featureName, string parameterName, out string value);

	bool TryGetFeatureParameterValue<T>(string featureName, string parameterName, out T value) where T : struct;

	bool TryGetRecorderLicenseCount(out int count);
}
