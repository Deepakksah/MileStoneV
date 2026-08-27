using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration.Properties;

namespace VideoOS.Common.Integration;

public abstract class LogManagerBase : ILogAuditLogger, ILogManager
{
	private static Dictionary<int, ILogEntryDefinition> _logList;

	private static Dictionary<Guid, Dictionary<int, ILogRecurrence>> _activeList;

	private ILogReceiver _logReceiver;

	public ILogReceiver LogReceiver => _logReceiver;

	static LogManagerBase()
	{
		_logList = new Dictionary<int, ILogEntryDefinition>();
		_activeList = new Dictionary<Guid, Dictionary<int, ILogRecurrence>>();
		InitializeLogMessages();
	}

	private static void InitializeLogMessages()
	{
		LogEntryId[] specialLogEntryIds = new LogEntryId[6]
		{
			LogEntryId.LicenseValid,
			LogEntryId.LicenseExpired,
			LogEntryId.LicenseInvalid,
			LogEntryId.DatabaseConnectionLost,
			LogEntryId.DatabaseConnectionRestored,
			LogEntryId.MediaDatabaseMinimumDiskSpaceReached
		};
		foreach (LogEntryDefinition item in LogEntryDefinitions.AllDefinitions.Where((LogEntryDefinition e) => !specialLogEntryIds.Any((LogEntryId s) => s == (LogEntryId)e.LogId)))
		{
			AddLogEntryDefinition(item);
		}
		List<LogEntryDefinition> source = LogEntryDefinitions.AllDefinitions.Where((LogEntryDefinition e) => specialLogEntryIds.Any((LogEntryId s) => s == (LogEntryId)e.LogId)).ToList();
		LogEntryDefinition logEntryDefinition = source.First((LogEntryDefinition e) => e.LogId == 1106);
		LogEntryDefinition logEntryDefinition2 = source.First((LogEntryDefinition e) => e.LogId == 1107);
		LogEntryDefinition logEntryDefinition3 = source.First((LogEntryDefinition e) => e.LogId == 1108);
		AddLogEntryDefinition(new LogEntryDefinitionChangeState(new LogEntryDefinition[3] { logEntryDefinition, logEntryDefinition2, logEntryDefinition3 }, new LogChangeStateRecurrenceHandler()));
		LogEntryDefinition logDefinitionSetState = source.First((LogEntryDefinition e) => e.LogId == 1210);
		LogEntryDefinition logDefinitionClearState = source.First((LogEntryDefinition e) => e.LogId == 1211);
		AddLogEntryDefinition(new LogEntryDefinitionState(logDefinitionSetState, logDefinitionClearState, new LogIgnoreRecurrenceHandler()));
		AddLogEntryDefinition(new LogEntryDefinitionState(source.First((LogEntryDefinition e) => e.LogId == 2073), new LogTimeRecurrenceHandler(TimeSpan.FromMinutes(1.0))));
	}

	public LogManagerBase(ILogReceiver logReceiver)
	{
		if (logReceiver == null)
		{
			throw new ArgumentNullException("logReceiver");
		}
		_logReceiver = logReceiver;
		_logReceiver.Initialize();
	}

	private static void AddLogEntryDefinition(LogEntryDefinition logEntryDefinition)
	{
		_logList.Add(logEntryDefinition.LogId, logEntryDefinition);
	}

	private static void AddLogEntryDefinition(LogEntryDefinitionState logEntryDefinitionState)
	{
		_logList.Add(logEntryDefinitionState.LogDefinitionSetState.LogId, logEntryDefinitionState);
		LogEntryDefinition[] logDefinitionsClearStates = logEntryDefinitionState.LogDefinitionsClearStates;
		foreach (LogEntryDefinition logEntryDefinition in logDefinitionsClearStates)
		{
			_logList.Add(logEntryDefinition.LogId, logEntryDefinitionState);
		}
	}

	private static void AddLogEntryDefinition(LogEntryDefinitionChangeState logEntryDefinitionChangeState)
	{
		LogEntryDefinition[] logEntryDefinitions = logEntryDefinitionChangeState.LogEntryDefinitions;
		foreach (LogEntryDefinition logEntryDefinition in logEntryDefinitions)
		{
			_logList.Add(logEntryDefinition.LogId, logEntryDefinitionChangeState);
		}
	}

	public static LogEntry GetLogEntry(LogEntryId logEntryId, params string[] arguments)
	{
		try
		{
			LogEntryDefinition logEntryDefinition = GetLogEntryDefinition(logEntryId);
			if (logEntryDefinition.LogEntryType == LogType.SystemLog)
			{
				LogEntrySystem obj = new LogEntrySystem((LogEntryId)logEntryDefinition.LogId)
				{
					LogLevel = logEntryDefinition.LogLevel,
					Arguments = arguments
				};
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				string description = logEntryDefinition.Description;
				object[] args = arguments;
				obj.Description = string.Format(invariantCulture, description, args);
				return obj;
			}
			if (logEntryDefinition.LogEntryType == LogType.AuditLog)
			{
				LogEntryAudit obj2 = new LogEntryAudit((LogEntryId)logEntryDefinition.LogId)
				{
					LogLevel = logEntryDefinition.LogLevel,
					Arguments = arguments
				};
				CultureInfo invariantCulture2 = CultureInfo.InvariantCulture;
				string description2 = logEntryDefinition.Description;
				object[] args = arguments;
				obj2.Description = string.Format(invariantCulture2, description2, args);
			}
			else if (logEntryDefinition.LogEntryType == LogType.RuleLog)
			{
				LogEntryRule obj3 = new LogEntryRule((LogEntryId)logEntryDefinition.LogId)
				{
					LogLevel = logEntryDefinition.LogLevel,
					Arguments = arguments
				};
				CultureInfo invariantCulture3 = CultureInfo.InvariantCulture;
				string description3 = logEntryDefinition.Description;
				object[] args = arguments;
				obj3.Description = string.Format(invariantCulture3, description3, args);
			}
		}
		catch (KeyNotFoundException)
		{
			return null;
		}
		return null;
	}

	public static LogEntryDefinition GetLogEntryDefinition(LogEntryId logEntryId)
	{
		LogEntryDefinition result = null;
		if (_logList.TryGetValue((int)logEntryId, out var value))
		{
			result = ((value is LogEntryDefinitionChangeState) ? ((LogEntryDefinitionChangeState)value).GetLogEntryDefinition((int)logEntryId) : ((!(value is LogEntryDefinitionState)) ? (value as LogEntryDefinition) : ((LogEntryDefinitionState)value).GetLogEntryDefinition((int)logEntryId)));
		}
		return result;
	}

	private void HandleLogResult(LogCheckResult checkResult, LogEntry logEntry, bool skipQueue)
	{
		switch (checkResult)
		{
		case LogCheckResult.Log:
			if (logEntry.LogEntryDefinition == null)
			{
				LogEntryDefinition logEntryDefinition = GetLogEntryDefinition((LogEntryId)logEntry.LogId);
				logEntry.LogLevel = logEntryDefinition.LogLevel;
				if (!logEntry.HasDescription)
				{
					CultureInfo invariantCulture2 = CultureInfo.InvariantCulture;
					string description2 = logEntryDefinition.Description;
					object[] arguments = logEntry.Arguments;
					logEntry.Description = string.Format(invariantCulture2, description2, arguments);
				}
			}
			_logReceiver.AddLogEntry(logEntry, skipQueue);
			break;
		case LogCheckResult.LogAndTerminate:
			if (logEntry.Description == null)
			{
				LogEntryDefinition logEntryDefinition = GetLogEntryDefinition((LogEntryId)logEntry.LogId);
				logEntry.LogLevel = logEntryDefinition.LogLevel;
				if (!logEntry.HasDescription)
				{
					CultureInfo invariantCulture = CultureInfo.InvariantCulture;
					string description = logEntryDefinition.Description;
					object[] arguments = logEntry.Arguments;
					logEntry.Description = string.Format(invariantCulture, description, arguments);
				}
			}
			_logReceiver.AddLogEntry(logEntry, skipQueue);
			RemoveLogState(logEntry);
			break;
		case LogCheckResult.Terminate:
			RemoveLogState(logEntry);
			break;
		case LogCheckResult.Nothing:
			break;
		}
	}

	private void RemoveLogState(LogEntry logEntry)
	{
		if (!(_logList[logEntry.LogId] is LogEntryDefinitionState logEntryDefinitionState))
		{
			return;
		}
		lock (_activeList)
		{
			Dictionary<int, ILogRecurrence> value = null;
			if (_activeList.TryGetValue(logEntry.SourceId, out value))
			{
				value.Remove(logEntryDefinitionState.LogDefinitionSetState.LogId);
			}
		}
	}

	public static int GetNumberOfParameters(LogEntryId logEntryId)
	{
		return GetLogEntryDefinition(logEntryId)?.NumberOfParameters ?? 0;
	}

	public static string GetLogDescription(LogEntryId logEntryId, params string[] arguments)
	{
		LogEntryDefinition logEntryDefinition = GetLogEntryDefinition(logEntryId);
		if (logEntryDefinition != null)
		{
			return string.Format(CultureInfo.InvariantCulture, logEntryDefinition.Description, arguments);
		}
		return string.Empty;
	}

	public static void SetLogDefinitionState(LogEntryId logEntryId, ILogRecurrence recurrenceHandler)
	{
		try
		{
			if (_logList[(int)logEntryId] is LogEntryDefinitionState logEntryDefinitionState)
			{
				logEntryDefinitionState.RecurrenceHandler = recurrenceHandler;
			}
		}
		catch (KeyNotFoundException)
		{
		}
	}

	public virtual void AddLogEntry(LogEntry logEntry)
	{
		AddLogEntry(logEntry, skipQueue: false);
	}

	public virtual void AddLogEntry(LogEntry logEntry, bool skipQueue)
	{
		if (logEntry == null)
		{
			throw new ArgumentNullException("logEntry");
		}
		ILogEntryDefinition logEntryDefinition = ((logEntry.LogEntryDefinition != null) ? logEntry.LogEntryDefinition : _logList[logEntry.LogId]);
		if (logEntryDefinition is LogEntryDefinitionChangeState)
		{
			LogEntryDefinitionChangeState logEntryDefinitionChangeState = (LogEntryDefinitionChangeState)logEntryDefinition;
			LogCheckResult logCheckResult = LogCheckResult.Undefined;
			lock (logEntryDefinitionChangeState.RecurrenceHandler)
			{
				logCheckResult = logEntryDefinitionChangeState.RecurrenceHandler.CheckLog(logEntry, setState: true);
			}
			if (logCheckResult != LogCheckResult.Undefined)
			{
				HandleLogResult(logCheckResult, logEntry, skipQueue);
			}
		}
		else if (logEntryDefinition is LogEntryDefinitionState)
		{
			LogEntryDefinitionState logEntryDefinitionState = (LogEntryDefinitionState)logEntryDefinition;
			LogCheckResult logCheckResult2 = LogCheckResult.Undefined;
			lock (_activeList)
			{
				if (logEntryDefinitionState.IsClearState(logEntry.LogId))
				{
					Dictionary<int, ILogRecurrence> value = null;
					if (_activeList.TryGetValue(logEntry.SourceId, out value))
					{
						ILogRecurrence value2 = null;
						if (value.TryGetValue(logEntryDefinitionState.LogDefinitionSetState.LogId, out value2))
						{
							logCheckResult2 = value2.CheckLog(logEntry, setState: false);
						}
					}
				}
				else
				{
					ILogRecurrence value3 = null;
					Dictionary<int, ILogRecurrence> value4 = null;
					if (!_activeList.TryGetValue(logEntry.SourceId, out value4))
					{
						value4 = new Dictionary<int, ILogRecurrence>();
						value3 = logEntryDefinitionState.RecurrenceHandler.Clone();
						value4.Add(logEntryDefinitionState.LogDefinitionSetState.LogId, value3);
						_activeList.Add(logEntry.SourceId, value4);
					}
					else if (!value4.TryGetValue(logEntryDefinitionState.LogDefinitionSetState.LogId, out value3))
					{
						value3 = logEntryDefinitionState.RecurrenceHandler.Clone();
						value4.Add(logEntryDefinitionState.LogDefinitionSetState.LogId, value3);
					}
					logCheckResult2 = value3.CheckLog(logEntry, setState: true);
				}
			}
			if (logCheckResult2 != LogCheckResult.Undefined)
			{
				HandleLogResult(logCheckResult2, logEntry, skipQueue);
			}
		}
		else
		{
			HandleLogResult(LogCheckResult.Log, logEntry, skipQueue);
		}
	}

	public virtual void AddLogEntry(LogEntryId logEntryId, params string[] arguments)
	{
		AddLogEntry(logEntryId, skipQueue: false, arguments);
	}

	public virtual void AddLogEntry(LogEntryId logEntryId, bool skipQueue, params string[] arguments)
	{
		LogEntry logEntry = GetLogEntry(logEntryId, arguments);
		AddLogEntry(logEntry, skipQueue);
	}

	public virtual void AddAuditLogEntry(LogEntryAudit logEntryAudit, bool skipQueue)
	{
		AddLogEntry(logEntryAudit, skipQueue);
	}

	public virtual void AddAuditLogEntry(LogEntryAudit logEntryAudit)
	{
		AddLogEntry(logEntryAudit, skipQueue: false);
	}

	public static ResourceType RegisteredDeviceTypeToResourceType(string deviceType)
	{
		return deviceType switch
		{
			"Camera" => ResourceType.Camera, 
			"AudioInput" => ResourceType.Microphone, 
			"AudioOutput" => ResourceType.Speaker, 
			"Metadata" => ResourceType.Metadata, 
			"Input" => ResourceType.Input, 
			"Output" => ResourceType.Output, 
			_ => throw new InvalidOperationException("Unknown device type: " + deviceType), 
		};
	}

	public static string CreatePtzMoveDirectionResourceString(MoveDirection direction)
	{
		return direction switch
		{
			MoveDirection.Down => AuditPtzResources.DirectionDown, 
			MoveDirection.Up => AuditPtzResources.DirectionUp, 
			MoveDirection.Left => AuditPtzResources.DirectionLeft, 
			MoveDirection.Right => AuditPtzResources.DirectionRight, 
			MoveDirection.LeftUp => AuditPtzResources.DirectionLeftUp, 
			MoveDirection.LeftDown => AuditPtzResources.DirectionLeftDown, 
			MoveDirection.RightUp => AuditPtzResources.DirectionRightUp, 
			MoveDirection.RightDown => AuditPtzResources.DirectionRightDown, 
			MoveDirection.ZoomIn => AuditPtzResources.DirectionZoomIn, 
			MoveDirection.ZoomOut => AuditPtzResources.DirectionZoomOut, 
			_ => string.Empty, 
		};
	}
}
