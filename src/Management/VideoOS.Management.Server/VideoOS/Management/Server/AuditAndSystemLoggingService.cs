using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Management.Server.Telemetry;
using VideoOS.Telemetry;
using VideoOS.Utilities.Telemetry;
using VideoOS.Utilities.Telemetry.Configuration;

namespace VideoOS.Management.Server;

public class AuditAndSystemLoggingService : IGeminiServerService, IDisposable
{
	private ILocalizedLoggerFactory _localizedLoggerFactory;

	private ILocalizedLogger _logger;

	public ILocalizedLogger Logger => _logger;

	public void Dispose()
	{
		_localizedLoggerFactory.Dispose();
	}

	public void StartService(IGeminiRequestContext requestContext)
	{
		GeminiTraceWrappingLogger instance = GeminiTraceWrappingLogger.Instance;
		instance.LogInformation("Starting AuditAndSystemLoggingService");
		TokenCacheService tokenCacheService = requestContext.GetService<TokenCacheService>();
		string settingsFilePath = TelemetrySettingsConfigurator.GetSettingsFilePath(OemResources.Instance.App.CompanyName);
		_localizedLoggerFactory = VideoOS.Utilities.Telemetry.Telemetry.GetILocalizedLoggerFactory("Server", () => new HttpClient((HttpMessageHandler)(object)new TokenCacheDelegatingHandler((CancellationToken cancellationToken) => tokenCacheService.ServerLogsTokenCache.RefreshToken(cancellationToken))), instance, settingsFilePath);
		_logger = _localizedLoggerFactory.CreateLocalizedLogger("Server");
		bool isProductionEnv = string.IsNullOrEmpty(GeminiApplicationCore.FeatureCacheManager.GetSLCEnvironment(requestContext));
		string sLC = GeminiApplicationCore.FeatureCacheManager.GetSLC(requestContext);
		TelemetryEnabler telemetryEnabler = new TelemetryEnabler(requestContext);
		TelemetryKeyProvider telemetryKeyProvider = new TelemetryKeyProvider(isProductionEnv);
		TelemetryManager telemetryManager = new TelemetryManager(instance, telemetryKeyProvider, telemetryEnabler);
		string text = ((_logger.BaseLogger.HighestEnabledLogLevel() == Microsoft.Extensions.Logging.LogLevel.None) ? "disabled" : "enabled");
		telemetryManager.TrackTrace("Audit logging using OpenTelemetry " + text, new Dictionary<string, string>
		{
			{ "environmentID", "ManagementServer" },
			{ "Product SLC", sLC }
		});
	}

	public void StopService(IGeminiRequestContext requestContext)
	{
	}
}
