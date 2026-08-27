using System;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Extensions.Logging;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.User;
using VideoOS.IM.Common.GlobalDefinitions.Utilities;
using VideoOS.IM.Common.Service.Abstractions;
using VideoOS.IM.Server.Logger;
using VideoOS.IM.Server.Service;
using VideoOS.IM.Server.Service.DALImplementation;
using VideoOS.IM.Server.Service.DALImplementation.Repository;
using VideoOS.IM.Server.Service.ExternalAgents;
using VideoOS.IM.Server.Service.Licenses.Providers;
using VideoOS.IM.Server.Service.Licenses.Validators;
using VideoOS.IM.Server.Service.OEM;
using VideoOS.IM.Server.Service.OidcTokens;
using VideoOS.IM.Server.Service.RetentionTimes;
using VideoOS.IM.Server.Service.RetentionTimes.Decorators;
using VideoOS.IM.Server.Service.Time;
using VideoOS.IM.Server.Service.WindowsRegistries;
using VideoOS.IM.Server.WebApi.App_Start;
using VideoOS.IM.Server.WebApi.Utils;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Telemetry;
using VideoOS.Utilities.Telemetry.Configuration;

namespace VideoOS.IM.Server.WebApi;

public class WebApiApplication : HttpApplication
{
	private TokenCacheDelegatingHandler _tokenCacheDelegatingHandler;

	protected void Application_Start()
	{
		S2STokenService.Instance.InitializeTokenCache();
		string settingsFilePath = TelemetrySettingsConfigurator.GetSettingsFilePath(OemResourceService.Instance.CompanyNameShort);
		_tokenCacheDelegatingHandler = new TokenCacheDelegatingHandler((CancellationToken _) => S2STokenService.Instance.GetAccessTokenAsTokenAsync());
		LoggerProvider.Instance.InitLogger(OemResourceService.Instance.CommonApplicationDataFolder, _tokenCacheDelegatingHandler, settingsFilePath);
		if (TryGetLogServerAddress(out var logServerAddress))
		{
			LoggerProvider.Instance.AuditLogger.Initialize(logServerAddress);
		}
		ILicenseProvider licenseProvider = new LicenseProvider(new WinRegistryService(), WebApiClient.Instance);
		LicenseValidationStatusService.Instance.InitVerifyLicenseValidationStatus(licenseProvider, LoggerProvider.Instance, OemResourceService.Instance);
		LicenseValidationStatusService.Instance.StartVerifyLicenseValidationStatusAsync();
		DatabaseConnectionInfoFromStorage databaseConnectionInfoFromStorage = new DatabaseConnectionInfoFromStorage(SqlConnectionServerType.IncidentManager);
		string activeConnectionString = databaseConnectionInfoFromStorage.ConnectionString;
		((IRepositoryService)new RepositoryEntityFrameworkService()).Initialize(activeConnectionString);
		DtoMilestoneUser initUser = new DtoMilestoneUser
		{
			UserName = Constants.WebApiIdentityName,
			MilestoneUserId = Constants.WebApiIdentityName
		};
		GlobalWebApiAppDALInstanceSelector.GetDtoDALImplementation = () => new DtoLocalServiceDAL(activeConnectionString);
		GlobalWebApiAppDALInstanceSelector.GetDtoDALImplementationForBackground = () => new DtoLocalServiceDAL(activeConnectionString, initUser);
		IncidentRetentionTimeCleanupService.Instance.InitApplyIncidentRetentionAfter(new RetentionTimeIncidentDecorator(), new UtcLocalTimeProvider(), ADtoDAL.GlobalSettingsManager);
		IncidentRetentionTimeCleanupService.Instance.StartApplyingRetentionTimeCleanupAsync();
		SwaggerConfig.Register();
		GlobalConfiguration.Configure(WebApiConfig.Register);
		FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
		RouteConfig.RegisterRoutes(RouteTable.Routes);
		LoggerProvider.Instance.Logger.LogInformation(Constants.WebApiIdentityName + " has started.");
	}

	protected void Application_End(object sender, EventArgs e)
	{
		LoggerProvider.Instance.Dispose();
		((HttpMessageHandler)_tokenCacheDelegatingHandler).Dispose();
	}

	private static bool TryGetLogServerAddress(out Uri logServerAddress)
	{
		logServerAddress = null;
		try
		{
			string result = S2STokenService.Instance.GetAccessTokenAsync().GetAwaiter().GetResult();
			logServerAddress = LogServerAddressResolver.GetLogServerAddressAsync(result).GetAwaiter().GetResult();
		}
		catch (Exception arg)
		{
			LoggerProvider.Instance.Logger.LogWarning($"Could not retrieve Log Server address: {arg}");
			return false;
		}
		return true;
	}
}
