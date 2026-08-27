using System;
using System.Collections.Generic;

namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseFeatureProvider
{
	bool IsFeatureLicensed(string featureName);

	IEnumerable<IFeature> QueryVisibleFeatures();

	bool TryGetFeatureExpiryDate(string featureName, out DateTime expiryDate);
}
