using System;
using System.Collections.Generic;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Management.Server;

public interface IFeatureCacheManager
{
	string GetFeatureParameterValue(IGeminiRequestContext requestContext, string featureName, string parameterName);

	void CheckFeatureLicense(IGeminiRequestContext requestContext, string featureName);

	bool IsFeatureLicensed(IGeminiRequestContext requestContext, string featureName);

	bool IsTrialLicense(IGeminiRequestContext requestContext);

	int GetRecorderLicenseCount(IGeminiRequestContext requestContext);

	bool HasLicense(IGeminiRequestContext requestContext);

	string GetLicenseProductName(IGeminiRequestContext requestContext);

	string GetSLC(IGeminiRequestContext systemRequestContext);

	string GetSLCEnvironment(IGeminiRequestContext systemRequestContext);

	string GetProductSku(IGeminiRequestContext requestContext);

	int GetProductCode(IGeminiRequestContext requestContext);

	DateTime GetLicenseExpiryDate(IGeminiRequestContext systemRequestContext);

	bool LicenseExpires(IGeminiRequestContext systemRequestContext);

	IEnumerable<string> QueryAllFeatureNames(IGeminiRequestContext systemRequestContext);

	bool TryGetFeatureParameterValue(IGeminiRequestContext requestContext, string featureName, string parameterName, out string value);

	int FeatureParameterCount(IGeminiRequestContext requestContext, string featureName);

	void Update(IGeminiRequestContext requestContext);

	bool IsZeroLicensed(IGeminiRequestContext requestContext, string driverName);

	IEnumerable<ILicenseProperty> QueryAllLicenseProperties(IGeminiRequestContext requestContext);
}
