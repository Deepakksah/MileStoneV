using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Server.Properties;
using VideoOS.Utilities.Telemetry;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Server;

public sealed class ServerLogManager : LogManagerBase, IServerLogManager
{
	private static ServerLogManager _serverLogManager;

	private static object _serverLogManagerLock = new object();

	private static IReadOnlyDictionary<LogType, LogDefinition> _logDefinitions;

	private static HashSet<int> _allowedLogsWithCurrentLogLevel = new HashSet<int>();

	private static ILocalizedLogger _auditAndSystemLogger;

	private LogEntryAuditLogger _ServerLogEntryAuditLogger;

	public static ServerLogManager Instance
	{
		get
		{
			if (_serverLogManager == null)
			{
				throw new InvalidOperationException("Log manager not initialized");
			}
			return _serverLogManager;
		}
	}

	public LogEntryAuditLogger ServerLogEntryAuditLogger
	{
		get
		{
			if (_ServerLogEntryAuditLogger == null)
			{
				throw new Exception("Log manager not initialized");
			}
			return _ServerLogEntryAuditLogger;
		}
	}

	public IAuditLogger AuditLogger => ServerLogEntryAuditLogger;

	public ServerLogManager(ILogReceiver serverLogReceiver, ILocalizedLogger auditAndSystemLogger)
		: base(serverLogReceiver)
	{
		_ServerLogEntryAuditLogger = new LogEntryAuditLogger(auditAndSystemLogger, this, new ServerInfoCollector());
	}

	public static void Initialize(ILogReceiver logReceiver, IReadOnlyDictionary<LogType, LogDefinition> logDefinitions, ILocalizedLogger auditAndSystemLogger)
	{
		if (_serverLogManager == null)
		{
			_logDefinitions = logDefinitions;
			SetAllowedLogsForLogLevel();
			_serverLogManager = new ServerLogManager(logReceiver, auditAndSystemLogger);
		}
		_auditAndSystemLogger = auditAndSystemLogger;
	}

	private static void SetAllowedLogsForLogLevel()
	{
		lock (_serverLogManagerLock)
		{
			_allowedLogsWithCurrentLogLevel.Clear();
			foreach (LogEntryDefinition allDefinition in LogEntryDefinitions.AllDefinitions)
			{
				LogDefinition logDefinition = _logDefinitions[allDefinition.LogEntryType];
				if (logDefinition.CanUseLogLevels)
				{
					if (logDefinition.LogLevel <= allDefinition.LogLevel)
					{
						_allowedLogsWithCurrentLogLevel.Add(allDefinition.LogId);
					}
				}
				else
				{
					_allowedLogsWithCurrentLogLevel.Add(allDefinition.LogId);
				}
			}
		}
	}

	public static bool IsLogEnabled(LogType logType)
	{
		foreach (LogDefinition value in ServerConfig.Instance.LogConfiguration.Logs.Values)
		{
			if (logType == value.LogType)
			{
				return value.Enabled;
			}
		}
		throw new ArgumentException("LogType does not exists");
	}

	public void Close()
	{
		base.LogReceiver.Close();
	}

	public void UpdateSystemSettings(ConfigurationDataSet.SystemSettingsDataTable table)
	{
		if (table == null)
		{
			throw new ArgumentNullException("table");
		}
		foreach (LogDefinition value in _logDefinitions.Values)
		{
			foreach (ConfigurationDataSet.SystemSettingsRow row in table.Rows)
			{
				if (row.Name == string.Format(CultureInfo.InvariantCulture, "Log{0}TimeToKeepLogEntries", value.Name))
				{
					lock (value)
					{
						int num = Convert.ToInt32(row.Setting, CultureInfo.InvariantCulture);
						value.TimeToKeepLogEntries = TimeSpan.FromSeconds(num);
					}
				}
				if (row.Name == string.Format(CultureInfo.InvariantCulture, "Log{0}MaxLogEntries", value.Name))
				{
					lock (value)
					{
						value.MaxLogEntries = Convert.ToInt32(row.Setting, CultureInfo.InvariantCulture);
					}
				}
				if (row.Name == string.Format(CultureInfo.InvariantCulture, "Log{0}Enabled", value.Name))
				{
					lock (value)
					{
						value.Enabled = Convert.ToBoolean(row.Setting, CultureInfo.InvariantCulture);
					}
				}
				if (row.Name == string.Format(CultureInfo.InvariantCulture, "Log{0}CanUseLevels", value.Name))
				{
					lock (value)
					{
						value.CanUseLogLevels = Convert.ToBoolean(row.Setting, CultureInfo.InvariantCulture);
					}
				}
				if (value.CanUseLogLevels && row.Name == string.Format(CultureInfo.InvariantCulture, "Log{0}Level", value.Name))
				{
					lock (value)
					{
						value.LogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), row.Setting);
					}
				}
				if (row.Name == Enum.GetName(typeof(SettingList), SettingList.LogAuditPlaybackFramesBeforeLogging))
				{
					lock (value)
					{
						value.AuditLogPlaybackFramesBeforeLogging = int.Parse(row.Setting, CultureInfo.InvariantCulture);
					}
				}
				if (row.Name == Enum.GetName(typeof(SettingList), SettingList.LogAuditPlaybackWindowSizeInSec))
				{
					lock (value)
					{
						value.AuditLogPlaybackWindowSizeInSec = int.Parse(row.Setting, CultureInfo.InvariantCulture);
					}
				}
				if (row.Name == Enum.GetName(typeof(SettingList), SettingList.LogAuditEnableUserAccessLogging))
				{
					lock (value)
					{
						value.ExtendedUserLogging = Convert.ToBoolean(row.Setting, CultureInfo.InvariantCulture);
					}
				}
			}
		}
		SetAllowedLogsForLogLevel();
	}

	public void UpdateSystemSettings(ConfigurationManager configurationManager)
	{
		if (configurationManager == null)
		{
			throw new ArgumentNullException("configurationManager");
		}
		foreach (LogDefinition value in _logDefinitions.Values)
		{
			lock (value)
			{
				string name = string.Format(CultureInfo.InvariantCulture, "Log{0}TimeToKeepLogEntries", value.Name);
				if (configurationManager.SystemSettings.TryGetSetting(name, out var setting))
				{
					int num = Convert.ToInt32(setting.Value, CultureInfo.InvariantCulture);
					value.TimeToKeepLogEntries = TimeSpan.FromSeconds(num);
				}
				name = string.Format(CultureInfo.InvariantCulture, "Log{0}MaxLogEntries", value.Name);
				if (configurationManager.SystemSettings.TryGetSetting(name, out setting))
				{
					value.MaxLogEntries = Convert.ToInt32(setting.Value, CultureInfo.InvariantCulture);
				}
				name = string.Format(CultureInfo.InvariantCulture, "Log{0}Enabled", value.Name);
				if (configurationManager.SystemSettings.TryGetSetting(name, out setting))
				{
					value.Enabled = Convert.ToBoolean(setting.Value, CultureInfo.InvariantCulture);
				}
				name = string.Format(CultureInfo.InvariantCulture, "Log{0}CanUseLevels", value.Name);
				if (configurationManager.SystemSettings.TryGetSetting(name, out setting))
				{
					value.CanUseLogLevels = Convert.ToBoolean(setting.Value, CultureInfo.InvariantCulture);
				}
				if (value.CanUseLogLevels)
				{
					name = string.Format(CultureInfo.InvariantCulture, "Log{0}Level", value.Name);
					if (configurationManager.SystemSettings.TryGetSetting(name, out setting))
					{
						value.LogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), setting.Value, ignoreCase: true);
					}
				}
				name = string.Format(CultureInfo.InvariantCulture, "Log{0}PlaybackFramesBeforeLogging", value.Name);
				if (configurationManager.SystemSettings.TryGetSetting(name, out setting))
				{
					value.AuditLogPlaybackFramesBeforeLogging = int.Parse(setting.Value, CultureInfo.InvariantCulture);
				}
				name = string.Format(CultureInfo.InvariantCulture, "Log{0}PlaybackWindowSizeInSec", value.Name);
				if (configurationManager.SystemSettings.TryGetSetting(name, out setting))
				{
					value.AuditLogPlaybackWindowSizeInSec = int.Parse(setting.Value, CultureInfo.InvariantCulture);
				}
				name = string.Format(CultureInfo.InvariantCulture, "Log{0}EnableUserAccessLogging", value.Name);
				if (configurationManager.SystemSettings.TryGetSetting(name, out setting))
				{
					value.ExtendedUserLogging = Convert.ToBoolean(setting.Value, CultureInfo.InvariantCulture);
				}
			}
		}
		SetAllowedLogsForLogLevel();
	}

	public static LogQueryResult QueryLog(LogQuery logQuery)
	{
		return LogDataAccessor.QueryLog(logQuery);
	}

	public static byte[] QueryLogExport(LogQuery logQuery, int codePage, CultureInfo culture)
	{
		ThreadPriority priority = Thread.CurrentThread.Priority;
		try
		{
			Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
			MemoryStream memoryStream = new MemoryStream();
			byte[] array;
			using (StreamWriter streamWriter = new StreamWriter(memoryStream))
			{
				LogQueryResult logQueryResult = LogDataAccessor.QueryLog(logQuery);
				StringBuilder stringBuilder = new StringBuilder();
				LogEntry[] logEntries = logQueryResult.LogEntries;
				foreach (LogEntry logEntry in logEntries)
				{
					stringBuilder.Clear();
					switch (logQueryResult.LogType)
					{
					case LogType.SystemLog:
						if (logEntry is LogEntrySystem logEntrySystem)
						{
							stringBuilder.Append(logEntrySystem.LogLevelValue).Append("\t");
							stringBuilder.Append(logEntrySystem.TimeStampUtc.ToString("s", culture)).Append("\t");
							stringBuilder.Append(logEntrySystem.TimeStampLocal.ToString("s", culture)).Append("\t");
							stringBuilder.Append(logEntrySystem.Description).Append("\t");
							stringBuilder.Append(logEntrySystem.LogId.ToString(CultureInfo.InvariantCulture)).Append("\t");
							stringBuilder.Append(logEntrySystem.SourceTypeValue).Append("\t");
							stringBuilder.Append(logEntrySystem.SourceName).Append("\t");
							stringBuilder.Append(logEntrySystem.EventType);
							streamWriter.WriteLine(stringBuilder);
						}
						break;
					case LogType.AuditLog:
						if (logEntry is LogEntryAudit logEntryAudit)
						{
							stringBuilder.Append(logEntryAudit.LogLevelValue).Append("\t");
							stringBuilder.Append(logEntryAudit.TimeStampUtc.ToString("s", culture)).Append("\t");
							stringBuilder.Append(logEntryAudit.TimeStampLocal.ToString("s", culture)).Append("\t");
							stringBuilder.Append(logEntryAudit.Description).Append("\t");
							stringBuilder.Append(logEntryAudit.AuditTypeValue).Append("\t");
							stringBuilder.Append(logEntryAudit.AccessGrantedValue).Append("\t");
							stringBuilder.Append(logEntryAudit.LogId.ToString(CultureInfo.InvariantCulture)).Append("\t");
							stringBuilder.Append(logEntryAudit.UserCredentials).Append("\t");
							stringBuilder.Append(logEntryAudit.Location).Append("\t");
							stringBuilder.Append(logEntryAudit.ResourceTypeValue).Append("\t");
							stringBuilder.Append(logEntryAudit.ResourceName).Append("\t");
							stringBuilder.Append(logEntryAudit.ResourceLocation);
							streamWriter.WriteLine(stringBuilder);
						}
						break;
					case LogType.RuleLog:
						if (logEntry is LogEntryRule logEntryRule)
						{
							stringBuilder.Append(logEntryRule.LogLevelValue).Append("\t");
							stringBuilder.Append(logEntryRule.TimeStampUtc.ToString("s", culture)).Append("\t");
							stringBuilder.Append(logEntryRule.TimeStampLocal.ToString("s", culture)).Append("\t");
							stringBuilder.Append(logEntryRule.LogId.ToString(CultureInfo.InvariantCulture)).Append("\t");
							stringBuilder.Append(logEntryRule.ServiceName).Append("\t");
							stringBuilder.Append(logEntryRule.RuleName).Append("\t");
							stringBuilder.Append(logEntryRule.SourceTypeValue).Append("\t");
							stringBuilder.Append(logEntryRule.SourceName).Append("\t");
							stringBuilder.Append(logEntryRule.EventType).Append("\t");
							stringBuilder.Append(logEntryRule.GeneratorTypeValue).Append("\t");
							stringBuilder.Append(logEntryRule.GeneratorName).Append("\t");
							stringBuilder.Append(logEntryRule.Description);
							streamWriter.WriteLine(stringBuilder);
						}
						break;
					}
				}
				streamWriter.Flush();
				array = new byte[memoryStream.Length];
				memoryStream.Seek(0L, SeekOrigin.Begin);
				memoryStream.Read(array, 0, (int)memoryStream.Length);
			}
			using MemoryStream memoryStream2 = new MemoryStream();
			using (GZipStream gZipStream = new GZipStream(memoryStream2, CompressionMode.Compress))
			{
				gZipStream.Write(array, 0, array.Length);
			}
			return memoryStream2.ToArray();
		}
		finally
		{
			Thread.CurrentThread.Priority = priority;
		}
	}

	public static LogEntryField[] QueryLogFilter(LogFilter logFilter)
	{
		return LogDataAccessor.QueryLogFilter(logFilter);
	}

	private static void CreateEventLogSource()
	{
		if (!EventLog.SourceExists(OemResources.Instance.Server.ServiceName))
		{
			EventLog.CreateEventSource(OemResources.Instance.Server.ServiceName, "Application");
		}
	}

	public static void AddEventLogEntry(string message, EventLogEntryType logType, params string[] arguments)
	{
		try
		{
			CreateEventLogSource();
			EventLog.WriteEntry(OemResources.Instance.Server.ServiceName, message, logType);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	public static void AddEventLogEntry(LogEntryId logEntryId, EventLogEntryType logType, params string[] arguments)
	{
		try
		{
			CreateEventLogSource();
			string logDescription = LogManagerBase.GetLogDescription(logEntryId, arguments);
			EventLog.WriteEntry(OemResources.Instance.Server.ServiceName, logDescription, logType);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	public override void AddLogEntry(LogEntryId logEntryId, params string[] arguments)
	{
		AddLogEntry(logEntryId, skipQueue: false, arguments);
	}

	public static bool ShouldEntryBeLogged(LogEntry logEntry)
	{
		if (logEntry == null)
		{
			throw new ArgumentNullException("logEntry");
		}
		return _allowedLogsWithCurrentLogLevel.Contains(logEntry.LogId);
	}

	public override void AddLogEntry(LogEntryId logEntryId, bool skipQueue, params string[] arguments)
	{
		LogEntry logEntry = LogManagerBase.GetLogEntry(logEntryId, arguments);
		if (ShouldEntryBeLogged(logEntry))
		{
			logEntry.SourceId = Guid.Empty;
			logEntry.SourceTypeValue = SourceType.Server.ToString();
			logEntry.SourceName = Resources.EventGeneratorSourceServer;
			base.AddLogEntry(logEntry, skipQueue);
		}
	}

	public void AddLogEntries(LogEntry[] logEntries)
	{
		base.LogReceiver.AddLogEntries(logEntries, skipQueue: false);
	}
}
