using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Services;
using VideoOS.Log;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Server;

public class LogDataComponentBase : IDisposable
{
	private const string ApplicationName = "Corporate";

	private const string ComponentName = "ManagementServer";

	private const string ModuleName = null;

	public static string LogDefFolder;

	public static string UnsentLogsFolder;

	public static string MsgsVersion;

	public const string DefaultCultureName = "en-US";

	public const string StandardClientCacheName = "StandardClientCache";

	private static object _logClientLock = new object();

	private static LogClient _logClient = null;

	private static bool _refreshLogClientRequest = true;

	protected string _culture = "en-US";

	private static LogClient LogClientStandard
	{
		get
		{
			if (_logClient == null)
			{
				RefreshLogClientSettings();
				_refreshLogClientRequest = false;
			}
			return _logClient;
		}
	}

	public LogDataComponentBase(string culture)
	{
		_culture = culture;
	}

	private static void RefreshLogClientSettings()
	{
		LogDefFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogServiceMsgs"));
		UnsentLogsFolder = Path.GetFullPath(Path.Combine(OemResources.Instance.Server.CommonApplicationDataFolder, "LogServiceFileQueue"));
		MsgsVersion = InitializeMessageVersion();
		_logClient?.Dispose();
		_logClient = new LogClient();
		_logClient.SetServer(GetLogServerURL(), "StandardClientCache");
		_logClient.SetFileQueuePath(UnsentLogsFolder);
		UploadDefinitions(_logClient);
	}

	private static string InitializeMessageVersion()
	{
		string text = Path.Combine(LogDefFolder, "LogDefinitions_en-US.lmsg");
		string message = "";
		if (File.Exists(text))
		{
			XPathDocument xPathDocument = new XPathDocument(text);
			XPathNavigator xPathNavigator = xPathDocument.CreateNavigator();
			XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode("LogMessages/version");
			if (xPathNavigator2 != null)
			{
				if (!string.IsNullOrEmpty(xPathNavigator2.Value))
				{
					return xPathNavigator2.Value;
				}
				message = "Initialization of message version failed. Empty version node in file. File: " + text;
				Server.Instance.Logger.LogError("Initialization of message version failed. Empty version node in file. File: {LogFile}", text);
			}
		}
		else
		{
			message = "Initialization of message version failed. Log version was not found. File: " + text;
			Server.Instance.Logger.LogError("Initialization of message version failed. Log version was not found. File: {LogFile}", text);
		}
		throw new LogDefinitionFileInvalidException(message);
	}

	private static void RefreshLogClientSettingsIfRequested()
	{
		if (_refreshLogClientRequest)
		{
			RefreshLogClientSettings();
			_refreshLogClientRequest = false;
		}
	}

	public static void Initialize()
	{
		lock (_logClientLock)
		{
			_refreshLogClientRequest = true;
		}
	}

	public static void Close()
	{
		lock (_logClientLock)
		{
			_logClient?.Dispose();
			_logClient = null;
		}
	}

	private static void UploadDefinitions(LogClient logClient)
	{
		Server.Instance.Logger.LogInformation("Uploading log client definitions from {LogDefFolder}", LogDefFolder);
		bool flag = false;
		if (!logClient.UploadCultureFiles(LogDefFolder, recursive: false, forceupload: false, asynchronous: true))
		{
			Server.Instance.Logger.LogWarning("LogClient.UploadCultureFiles failed. LogDefFolder: '{LogDefFolder}'", LogDefFolder);
		}
		LogRetention logRetention = new LogRetention();
		using (ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(Server.Instance.Data)))
		{
			logRetention.SystemLogRetention.DaysToKeepLogEntries = configurationManager.SystemSettings[SettingList.LogSystemTimeToKeepLogEntries].ValueAsInt64 / 60 / 60 / 24;
			logRetention.SystemLogRetention.MaxNumberOfLogEntries = configurationManager.SystemSettings[SettingList.LogSystemMaxLogEntries].ValueAsInt64;
			logRetention.AuditLogRetention.DaysToKeepLogEntries = configurationManager.SystemSettings[SettingList.LogAuditTimeToKeepLogEntries].ValueAsInt64 / 60 / 60 / 24;
			logRetention.AuditLogRetention.MaxNumberOfLogEntries = configurationManager.SystemSettings[SettingList.LogAuditMaxLogEntries].ValueAsInt64;
			logRetention.RuleLogRetention.DaysToKeepLogEntries = configurationManager.SystemSettings[SettingList.LogRuleTimeToKeepLogEntries].ValueAsInt64 / 60 / 60 / 24;
			logRetention.RuleLogRetention.MaxNumberOfLogEntries = configurationManager.SystemSettings[SettingList.LogRuleMaxLogEntries].ValueAsInt64;
		}
		string logRetentionAsXml = logRetention.GetLogRetentionAsXml();
		if (!logClient.SetRetentionTime(logRetentionAsXml))
		{
			Server.Instance.Logger.LogError("LogClient.SetRetentionTime failed. retentionXml: '{RetentionXml}'", logRetentionAsXml);
		}
		else
		{
			Server.Instance.Logger.LogDebug("LogClient.SetRetentionTime succeeded. retentionXml: '{RetentionXml}'", logRetentionAsXml);
		}
	}

	public static string GetLogServerURL()
	{
		using (ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(Server.Instance.Data)))
		{
			foreach (RegisteredServer value in configurationManager.RegisteredServers.Values)
			{
				if (value.Type == ServiceTypes.ServiceType_LogService)
				{
					return value.Addresses[0].URL;
				}
			}
		}
		if (ServerConfig.Instance.LogServiceUrlDevOnly != null)
		{
			return ServerConfig.Instance.LogServiceUrlDevOnly.AbsoluteUri;
		}
		return string.Empty;
	}

	protected LogEntryField[] GetDistinctColumnValues(string columnName, string groupName, bool getName)
	{
		List<LogEntryField> list = new List<LogEntryField>();
		lock (_logClientLock)
		{
			RefreshLogClientSettingsIfRequested();
			LogClientStandard.GetColumnValuesEx(columnName, _culture, groupName, "", 1, 100, getName, out var result);
			foreach (ArrayList item in result)
			{
				if (getName && item.Count > 1)
				{
					list.Add(new LogEntryField((string)item[1], (string)item[0]));
				}
				else
				{
					list.Add(new LogEntryField(null, (string)item[0]));
				}
			}
			return list.ToArray();
		}
	}

	private static void WriteToLogServer(string objectId, string objectName, string objectLocationName, string category, string userId, string userLocation, DateTime timestamp, string groupName, Severity severity, string messageId, string messageCulture, Hashtable variables, List<LogMessageUserField> userFields)
	{
		lock (_logClientLock)
		{
			RefreshLogClientSettingsIfRequested();
			if (!string.IsNullOrEmpty(userId))
			{
				LogClientStandard.SetUserId(userId);
			}
			if (!string.IsNullOrEmpty(userLocation))
			{
				LogClientStandard.SetUserLocation(userLocation);
			}
			LogClientStandard.LogWrite(null, null, objectId, objectName, null, objectLocationName, timestamp, MsgsVersion, "Corporate", "ManagementServer", null, groupName, category, Status.StatusQuo, severity, messageId, null, messageCulture, variables, userFields);
		}
	}

	protected static void WriteToLogServer(string sourceName, DateTime timestamp, string groupName, Severity severity, string messageId, string messageCulture, Hashtable variables, List<LogMessageUserField> userFields)
	{
		WriteToLogServer(null, sourceName, null, null, null, null, timestamp, groupName, severity, messageId, messageCulture, variables, userFields);
	}

	protected static void WriteAuditToLogServer(string objectId, string sourceName, string objectLocationName, string category, string userId, string userLocation, DateTime timestamp, string groupName, Severity severity, string messageId, string messageCulture, Hashtable variables, List<LogMessageUserField> userFields)
	{
		WriteToLogServer(objectId, sourceName, objectLocationName, category, userId, userLocation, timestamp, groupName, severity, messageId, messageCulture, variables, userFields);
	}

	public void Dispose()
	{
	}
}
