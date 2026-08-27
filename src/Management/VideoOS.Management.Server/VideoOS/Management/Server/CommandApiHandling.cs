using System;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public static class CommandApiHandling
{
	public static ProductInfo GetProductInfo()
	{
		ProductInfo productInfo = new ProductInfo();
		using (GeminiRequestContext geminiRequestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext())
		{
			productInfo.ProductName = GeminiApplicationCore.FeatureCacheManager.GetLicenseProductName(geminiRequestContext);
			productInfo.ProductCode = GeminiApplicationCore.FeatureCacheManager.GetProductCode(geminiRequestContext);
			productInfo.SLC = GeminiApplicationCore.FeatureCacheManager.GetSLC(geminiRequestContext);
		}
		productInfo.ProductLine = new Guid("83A8680E-30B1-46d7-8FD2-CB3D0BF7DB9E");
		productInfo.VendorId = OemResources.Instance.App.VendorId;
		productInfo.SubProduct = (string.IsNullOrEmpty("") ? Guid.Empty : new Guid(""));
		productInfo.MajorVersion = BuildInfo.Major;
		productInfo.MinorVersion = BuildInfo.Minor;
		productInfo.ServiceVersion = BuildInfo.GetRevisionLetter();
		productInfo.BuildConfiguration = BuildInfo.Configuration;
		productInfo.BuildNumber = BuildInfo.Build;
		if (CheckDevelopmentRegistry.FakeHigherServerVersion)
		{
			productInfo.MajorVersion = BuildInfo.Major + "1";
		}
		if (DateTime.TryParse(BuildInfo.BuildDate, out var result))
		{
			productInfo.BuildDate = result;
		}
		return productInfo;
	}

	public static SystemLicenseInfo GetSystemLicenseInfo()
	{
		SystemLicenseInfo systemLicenseInfo = new SystemLicenseInfo();
		using (GeminiRequestContext geminiRequestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext())
		{
			systemLicenseInfo.HasLicense = GeminiApplicationCore.FeatureCacheManager.HasLicense(geminiRequestContext);
			if (systemLicenseInfo.HasLicense)
			{
				systemLicenseInfo.SLC = GeminiApplicationCore.FeatureCacheManager.GetSLC(geminiRequestContext);
				systemLicenseInfo.SLCEnvironment = GeminiApplicationCore.FeatureCacheManager.GetSLCEnvironment(geminiRequestContext);
				systemLicenseInfo.ProductSku = GeminiApplicationCore.FeatureCacheManager.GetProductSku(geminiRequestContext);
				systemLicenseInfo.IsTrialLicense = GeminiApplicationCore.FeatureCacheManager.IsTrialLicense(geminiRequestContext);
				systemLicenseInfo.Expires = GeminiApplicationCore.FeatureCacheManager.LicenseExpires(geminiRequestContext);
				systemLicenseInfo.ExpiryDate = (systemLicenseInfo.Expires ? GeminiApplicationCore.FeatureCacheManager.GetLicenseExpiryDate(geminiRequestContext) : DateTime.MaxValue);
				systemLicenseInfo.Features = GeminiApplicationCore.FeatureCacheManager.QueryAllFeatureNames(geminiRequestContext).ToArray();
			}
		}
		return systemLicenseInfo;
	}

	public static bool CheckSecurityEventTriggerWithMetadata(IGeminiRequestContext requestContext, Guid eventId)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		UserDefinedEvent userDefinedEvent = null;
		using (IUserDefinedEventComponent userDefinedEventComponent = requestContext.CreateUserDefinedEventComponent())
		{
			userDefinedEvent = userDefinedEventComponent.QueryUserDefinedEvent(eventId.ToString());
		}
		bool flag = userDefinedEvent != null;
		try
		{
			if (flag)
			{
				requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "TRIGGER", eventId.ToString());
				if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "TimeControlledAccessRights"))
				{
					try
					{
						requestContext.GetSecurityManagerService().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "TRIGGER", eventId.ToString());
					}
					catch (UnauthorizedAccessException)
					{
						throw new TimeBasedUnauthorizedAccessException();
					}
				}
			}
		}
		catch (SecurityObjectDoesNotExistException)
		{
			return false;
		}
		return flag;
	}
}
