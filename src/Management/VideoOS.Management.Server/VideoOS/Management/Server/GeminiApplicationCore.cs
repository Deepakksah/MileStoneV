using System;
using System.Diagnostics;
using VideoOS.Common.Integration;

namespace VideoOS.Management.Server;

public class GeminiApplicationCore
{
	private static Lazy<GeminiEventLog> EventLog = new Lazy<GeminiEventLog>(() => new GeminiEventLog(OemResources.Instance.Server.ServiceName));

	public static GeminiWebApplicationHost RuntimeServiceHost { get; private set; }

	public static MarkedDataSynchronizationManager MarkedDataSynchronizationManager { get; } = new MarkedDataSynchronizationManager();

	public static SiteManager SiteManager { get; } = new SiteManager();

	public static IFeatureCacheManager FeatureCacheManager { get; } = new FeatureCacheManager();

	public static ClientProfileSpecificationCacheManager ClientProfileSpecificationCacheManager { get; } = new ClientProfileSpecificationCacheManager();

	public static SystemMonitorStateManager SystemMonitorStateManager { get; } = new SystemMonitorStateManager();

	public static RetentionTimeRule RetentionTimeRule { get; } = new RetentionTimeRule(new ScheduleLogger());

	public static ServiceChannelTimers ServiceChannelTimers { get; } = new ServiceChannelTimers();

	public static bool IsExecutingModuleTest => Config.Instance.GetAppSetting("ModuleTest", "False").Equals("True", StringComparison.OrdinalIgnoreCase);

	public static void StartGemini()
	{
		GeminiTrace.Info("Starting application");
		GeminiWebApplicationHost obj = (RuntimeServiceHost = new GeminiWebApplicationHost(new GeminiServiceHostProperties()));
		EntityNameAttributeHandlerWrapper.EnsureExecutingAssemblyScanned();
		SystemMonitorStateManager.Start();
		RetentionTimeRule.Update();
		using (GeminiRequestContext geminiRequestContext = obj.CreateSystemContext())
		{
			geminiRequestContext.GetService<EncryptionService>();
			geminiRequestContext.GetService<TokenCacheService>();
			((IGeminiServerService)geminiRequestContext.GetService<TaskMonitorService>()).StartService((IGeminiRequestContext)geminiRequestContext);
			((IGeminiServerService)geminiRequestContext.GetService<HardwareBlockMonitor>()).StartService((IGeminiRequestContext)geminiRequestContext);
			((IGeminiServerService)geminiRequestContext.GetService<GeminiIdentityService>()).StartService((IGeminiRequestContext)geminiRequestContext);
			((IGeminiServerService)geminiRequestContext.GetService<IdentityCleanupService>()).StartService((IGeminiRequestContext)geminiRequestContext);
			geminiRequestContext.GetService<SystemMonitorEventQueueService>().StartService(geminiRequestContext);
			geminiRequestContext.GetService<NameLookupService>().StartService(geminiRequestContext);
			geminiRequestContext.GetService<PermissionAuditLogService>().StartService(geminiRequestContext);
		}
		ServiceChannelTimers.Initialize();
		GeminiTrace.Info("Application initialized");
	}

	public static void StopGemini()
	{
		SiteManager.Stop();
		MarkedDataSynchronizationManager.Stop();
		SystemMonitorStateManager.Stop();
		RetentionTimeRule.Dispose();
		ServiceChannelTimers.Dispose();
	}

	public static Version ServerVersion()
	{
		Version version = Version.Parse(BuildInfo.ProductVersion);
		if (CheckDevelopmentRegistry.FakeHigherServerVersion)
		{
			version = new Version(version.Major + 1, version.Minor, version.Build, version.Revision);
		}
		return new Version(version.Major, version.Minor, 0, version.Revision);
	}

	public static void Log(string logMessage, int eventId, EventLogEntryType level)
	{
		EventLog.Value.Log(null, logMessage, eventId, level);
	}

	public static void Log(string messageBrief, string logMessage, int eventId, EventLogEntryType level)
	{
		EventLog.Value.Log(messageBrief, logMessage, eventId, level);
	}

	public static void LogException(string logMessage, Exception ex)
	{
		EventLog.Value.LogException(null, logMessage, ex);
	}

	public static void LogException(string logMessage, Exception ex, int eventId, EventLogEntryType level)
	{
		EventLog.Value.LogException(null, logMessage, ex, eventId, level);
	}

	public static void LogException(IGeminiRequestContext requestContext, string logMessage, Exception ex)
	{
		EventLog.Value.LogException(requestContext, logMessage, ex);
	}

	public static void LogException(IGeminiRequestContext requestContext, string logMessage, Exception ex, int eventId, EventLogEntryType level)
	{
		EventLog.Value.LogException(requestContext, logMessage, ex, eventId, level);
	}
}
