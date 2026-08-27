using System.Configuration;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;

namespace VideoOS.SCH.Services.DataAccess;

public class ConfigSettings
{
	public static string CompanyName => OemResources.Instance.CompanyName;

	public static string ApplicationName => OemResources.Instance.ApplicationName;

	public static string InstallDirectory => OemResources.Instance.ServiceChannelInstallDir;

	public static int SCHSessionTimeoutSecs
	{
		get
		{
			if (int.TryParse(ConfigurationManager.AppSettings["VideoOS.SCH.Services.SCHSessionTimeoutSecs"], out var result))
			{
				return result;
			}
			throw new ServiceChannelException("App setting 'VideoOS.SCH.Services.SCHSessionTimeoutSecs' is missing or could not be converted to an Int32.");
		}
	}

	public static int MaxNoofMillisecsToWaitForMessages
	{
		get
		{
			if (int.TryParse(ConfigurationManager.AppSettings["VideoOS.SCH.Services.MaxNoofMillisecsToWaitForMessages"], out var result))
			{
				return result;
			}
			throw new ServiceChannelException("App setting 'VideoOS.SCH.Services.MaxNoofMillisecsToWaitForMessages' is missing or could not be converted to an Int32.");
		}
	}

	public static AppDataStorageType AppDataStorageType
	{
		get
		{
			string text = ConfigurationManager.AppSettings["VideoOS.SCH.Services.AppDataStorageType"];
			if (text == null || text.Length == 0)
			{
				throw new ServiceChannelException("App setting 'VideoOS.SCH.Services.AppDataStorageType' is missing or empty.");
			}
			if (!(text == "InMemory"))
			{
				if (text == "SQLServer")
				{
					return AppDataStorageType.SQLServer;
				}
				throw new ServiceChannelException("Invalid configuration value for the Service Channel data storage type: '" + text + "'.");
			}
			return AppDataStorageType.InMemory;
		}
	}

	public static string SQLServerConnectionString
	{
		get
		{
			string text = ConfigurationManager.AppSettings["VideoOS.SCH.Services.DataAccess.SQLServer.ConnectionString"];
			if (string.IsNullOrEmpty(text))
			{
				throw new ServiceChannelException("App setting 'VideoOS.SCH.Services.DataAccess.SQLServer.ConnectionString' is missing or empty.");
			}
			return text;
		}
	}

	public static bool MessageTypeEnforced
	{
		get
		{
			if (bool.TryParse(ConfigurationManager.AppSettings["VideoOS.SCH.Services.Data.MessageTypeEnforced"], out var result))
			{
				return result;
			}
			throw new ServiceChannelException("App setting 'VideoOS.SCH.Services.Data.MessageTypeEnforced' is missing or could not be converted to a Boolean.");
		}
	}

	public static int MaxResponseDataSize
	{
		get
		{
			if (int.TryParse(ConfigurationManager.AppSettings["VideoOS.SCH.Services.Interfaces.Web.MaxResponseDataSize"], out var result))
			{
				return result;
			}
			throw new ServiceChannelException("App setting 'VideoOS.SCH.Services.Interfaces.Web.MaxResponseDataSize' is missing or could not be converted to an Int32.");
		}
	}

	public static int RecommendedMsgChkIntervalMs
	{
		get
		{
			if (int.TryParse(ConfigurationManager.AppSettings["VideoOS.SCH.Services.Interfaces.Web.RecommendedMsgChkIntervalMs"], out var result))
			{
				return result;
			}
			throw new ServiceChannelException("App setting 'VideoOS.SCH.Services.Interfaces.Web.RecommendedMsgChkIntervalInMs' is missing or could not be converted to an Int32.");
		}
	}

	public static bool SessionMsgIndicatorUsed
	{
		get
		{
			if (bool.TryParse(ConfigurationManager.AppSettings["VideoOS.SCH.Services.SessionMsgIndicatorUsed"], out var result))
			{
				return result;
			}
			throw new ServiceChannelException("App setting 'VideoOS.SCH.Services.SessionMsgIndicatorUsed' is missing or could not be converted to a Boolean.");
		}
	}

	public static int DeleteExpiredSessionsJobIntervalSecs
	{
		get
		{
			if (int.TryParse(ConfigurationManager.AppSettings["VideoOS.SCH.Services.BatchJobs.DeleteExpiredSessionsJobIntervalSecs"], out var result))
			{
				return result;
			}
			throw new ServiceChannelException("App setting 'VideoOS.SCH.Services.BatchJobs.DeleteExpiredSessionsJobIntervalSecs' is missing or could not be converted to an Int32.");
		}
	}

	public static int DeleteNonSessionMessagesIntervalSecs
	{
		get
		{
			if (int.TryParse(ConfigurationManager.AppSettings["VideoOS.SCH.Services.BatchJobs.DeleteNonSessionMessagesIntervalSecs"], out var result))
			{
				return result;
			}
			throw new ServiceChannelException("App setting 'VideoOS.SCH.Services.BatchJobs.DeleteNonSessionMessagesIntervalSecs' is missing or could not be converted to an Int32.");
		}
	}

	public static int ServiceRegistrationIntervalSecs
	{
		get
		{
			if (int.TryParse(ConfigurationManager.AppSettings["VideoOS.SCH.Services.BatchJobs.ServiceRegistrationIntervalSecs"], out var result))
			{
				return result;
			}
			throw new ServiceChannelException("App setting 'VideoOS.SCH.Services.BatchJobs.ServiceRegistrationIntervalSecs' is missing or could not be converted to an Int32.");
		}
	}

	public static int SessionMsgIndicatorUpdateIntervalMs
	{
		get
		{
			if (int.TryParse(ConfigurationManager.AppSettings["VideoOS.SCH.Services.BatchJobs.SessionMsgIndicatorUpdateIntervalMs"], out var result))
			{
				return result;
			}
			throw new ServiceChannelException("App setting 'VideoOS.SCH.Services.BatchJobs.SessionMsgIndicatorUpdateIntervalMs' is missing or could not be converted to an Int32.");
		}
	}

	public static string WinEventLogSourceName
	{
		get
		{
			string text = ConfigurationManager.AppSettings["VideoOS.SCH.WinEventLogSourceName"];
			if (string.IsNullOrEmpty(text))
			{
				throw new ServiceChannelException("App setting 'VideoOS.SCH.WinEventLogSourceName' is missing or empty.");
			}
			return text;
		}
	}

	public static bool MessageLogEnabled
	{
		get
		{
			if (bool.TryParse(ConfigurationManager.AppSettings["VideoOS.SCH.Services.MessageLogEnabled"], out var result))
			{
				return result;
			}
			throw new ServiceChannelException("App setting 'VideoOS.SCH.Services.MessageLogEnabled' is missing or could not be converted to a Boolean.");
		}
	}

	public static string MessageLogFileName => ConfigurationManager.AppSettings["VideoOS.SCH.Services.MessageLogFileName"];

	public static int ServerCommandServiceTimeout
	{
		get
		{
			if (int.TryParse(ConfigurationManager.AppSettings["VideoOS.SCH.ServerCommandServiceTimeout"], out var result))
			{
				return result;
			}
			throw new ServiceChannelException("App setting 'VideoOS.SCH.ServerCommandServiceTimeout' is missing or could not be converted to an Int32.");
		}
	}

	public static int IdentityTokenCacheEntryLifetimeInSeconds => 120;

	public static int IdentityTokenCachePurgeIntervalInSeconds => 300;

	public static int IdentityTokenCacheInactiveTimeLimitInSeconds => 300;
}
