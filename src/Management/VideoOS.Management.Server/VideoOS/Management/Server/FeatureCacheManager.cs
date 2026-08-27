using System;
using System.Collections.Generic;
using VideoOS.Common.Integration;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class FeatureCacheManager : IFeatureCacheManager
{
	private FeatureManager featureManager = new FeatureManager();

	private bool _initialized;

	public FeatureCacheManager()
	{
		_initialized = false;
	}

	public string GetFeatureParameterValue(IGeminiRequestContext requestContext, string featureName, string parameterName)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.GetFeatureParameterValue(featureName, parameterName);
	}

	public int FeatureParameterCount(IGeminiRequestContext requestContext, string featureName)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.FeatureParameterCount(featureName);
	}

	public bool TryGetFeatureParameterValue(IGeminiRequestContext requestContext, string featureName, string parameterName, out string value)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.TryGetFeatureParameterValue(featureName, parameterName, out value);
	}

	public void CheckFeatureLicense(IGeminiRequestContext requestContext, string featureName)
	{
		if (!IsFeatureLicensed(requestContext, featureName))
		{
			throw new FeatureNotLicensedException();
		}
	}

	public bool IsFeatureLicensed(IGeminiRequestContext requestContext, string featureName)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.IsFeatureLicensed(featureName);
	}

	public bool IsTrialLicense(IGeminiRequestContext requestContext)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.IsTrialLicense;
	}

	public int GetRecorderLicenseCount(IGeminiRequestContext requestContext)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		if (!featureManager.TryGetRecorderLicenseCount(out var count))
		{
			return 0;
		}
		return count;
	}

	public string GetLicenseProductName(IGeminiRequestContext requestContext)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.LicenseProductName;
	}

	public bool HasLicense(IGeminiRequestContext requestContext)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.HasLicense;
	}

	public string GetSLC(IGeminiRequestContext requestContext)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.SLC;
	}

	public string GetSLCEnvironment(IGeminiRequestContext requestContext)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.SLCEnvironment;
	}

	public string GetProductSku(IGeminiRequestContext requestContext)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.ProductSku;
	}

	public int GetProductCode(IGeminiRequestContext requestContext)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return CorporateProductCodes.GetProductCode(featureManager.ProductSku);
	}

	public DateTime GetLicenseExpiryDate(IGeminiRequestContext requestContext)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.LicenseExpiryDate;
	}

	public bool LicenseExpires(IGeminiRequestContext requestContext)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.LicenseExpires;
	}

	public IEnumerable<string> QueryAllFeatureNames(IGeminiRequestContext requestContext)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.AllFeatureNames;
	}

	public void Update(IGeminiRequestContext requestContext)
	{
		try
		{
			using ILicenseComponent licenseComponent = requestContext.CreateLicenseComponent();
			ILicenseDocument licenseDocument = licenseComponent.QueryLicenseDocument();
			if (licenseDocument != null)
			{
				featureManager.Initialize(licenseDocument);
				_initialized = true;
			}
		}
		catch (LicenseException ex)
		{
			if (!_initialized)
			{
				GeminiTrace.Error("License unreadable, this may happen during an upgrade due to changes in license format, In that case the license belonging to the upgrade is applied immediately after and no action is required", ex);
				return;
			}
			throw;
		}
	}

	public bool IsZeroLicensed(IGeminiRequestContext requestContext, string driverName)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		if (featureManager.TryGetFeatureParameterValue("CustomLicenseType", driverName, out var value))
		{
			return value == "ZeroLicenseType";
		}
		return false;
	}

	public IEnumerable<ILicenseProperty> QueryAllLicenseProperties(IGeminiRequestContext requestContext)
	{
		if (!_initialized)
		{
			Update(requestContext);
		}
		return featureManager.AllLicenseProperties;
	}
}
