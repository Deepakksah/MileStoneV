using System;
using System.Globalization;
using VideoOS.Common.Integration;

namespace VideoOS.Server;

public static class LogDataAccessor
{
	public static void Initialize()
	{
		LogDataComponentBase.Initialize();
	}

	public static void Close()
	{
		LogDataComponentBase.Close();
	}

	public static void CreateLogEntry(LogEntry logEntry)
	{
		CreateLogEntryInternal(logEntry);
	}

	public static void CreateLogEntries(LogEntry[] logEntries)
	{
		CreateLogEntriesInternal(logEntries);
	}

	private static void CreateLogEntryInternal(LogEntry logEntry)
	{
		using LogDataComponent logDataComponent = new LogDataComponent();
		if (logEntry is LogEntrySystem)
		{
			logDataComponent.CreateLogEntry(logEntry as LogEntrySystem);
		}
		else if (logEntry is LogEntryAudit)
		{
			logDataComponent.CreateLogEntry(logEntry as LogEntryAudit);
		}
		else if (logEntry is LogEntryRule)
		{
			logDataComponent.CreateLogEntry(logEntry as LogEntryRule);
		}
	}

	private static void CreateLogEntriesInternal(LogEntry[] logEntries)
	{
		using LogDataComponent logDataComponent = new LogDataComponent();
		logDataComponent.CreateLogEntries(logEntries);
	}

	public static LogQueryResult QueryLog(LogQuery logQuery)
	{
		using LogDataComponent logDataComponent = new LogDataComponent(logQuery.LanguageName);
		LogQueryResult logQueryResult = new LogQueryResult();
		logQueryResult.LogType = logQuery.LogType;
		logQueryResult.PageSize = logQuery.PageSize;
		string filter = BuildFilter(logQuery.LogFilters);
		switch (logQuery.LogType)
		{
		case LogType.SystemLog:
		{
			LogEntrySystem[] array2;
			if (logQuery.QueryFormat == LogQueryFormat.PageSize)
			{
				array2 = logDataComponent.QueryLogSystem(logQuery.Page, logQuery.PageSize, filter);
			}
			else if (logQuery.QueryFormat == LogQueryFormat.DateInterval)
			{
				array2 = logDataComponent.QueryLogSystem(logQuery.StartDate, logQuery.EndDate, filter);
			}
			else
			{
				if (logQuery.QueryFormat != LogQueryFormat.DateEnd)
				{
					throw new InvalidOperationException("Invalid LogQueryFormat");
				}
				array2 = logDataComponent.QueryLogSystem(logQuery.EndDate, logQuery.Page, logQuery.PageSize, filter);
			}
			LogEntry[] logEntries = array2;
			logQueryResult.LogEntries = logEntries;
			logQueryResult.ActualPageSize = array2.GetLength(0);
			logQueryResult.CurrentPage = logQuery.Page;
			logQueryResult.TotalPages = int.MaxValue;
			if (logQueryResult.TotalPages == 0)
			{
				logQueryResult.TotalPages = 1;
			}
			return logQueryResult;
		}
		case LogType.AuditLog:
		{
			LogEntryAudit[] array3;
			if (logQuery.QueryFormat == LogQueryFormat.PageSize)
			{
				array3 = logDataComponent.QueryLogAudit(logQuery.Page, logQuery.PageSize, filter);
			}
			else if (logQuery.QueryFormat == LogQueryFormat.DateInterval)
			{
				array3 = logDataComponent.QueryLogAudit(logQuery.StartDate, logQuery.EndDate, filter);
			}
			else
			{
				if (logQuery.QueryFormat != LogQueryFormat.DateEnd)
				{
					throw new InvalidOperationException("Invalid LogQueryFormat");
				}
				array3 = logDataComponent.QueryLogAudit(logQuery.EndDate, logQuery.Page, logQuery.PageSize, filter);
			}
			LogEntry[] logEntries = array3;
			logQueryResult.LogEntries = logEntries;
			logQueryResult.ActualPageSize = array3.GetLength(0);
			logQueryResult.CurrentPage = logQuery.Page;
			logQueryResult.TotalPages = int.MaxValue;
			if (logQueryResult.TotalPages == 0)
			{
				logQueryResult.TotalPages = 1;
			}
			return logQueryResult;
		}
		case LogType.RuleLog:
		{
			LogEntryRule[] array;
			if (logQuery.QueryFormat == LogQueryFormat.PageSize)
			{
				array = logDataComponent.QueryLogRule(logQuery.Page, logQuery.PageSize, filter);
			}
			else if (logQuery.QueryFormat == LogQueryFormat.DateInterval)
			{
				array = logDataComponent.QueryLogRule(logQuery.StartDate, logQuery.EndDate, filter);
			}
			else
			{
				if (logQuery.QueryFormat != LogQueryFormat.DateEnd)
				{
					throw new InvalidOperationException("Invalid LogQueryFormat");
				}
				array = logDataComponent.QueryLogRule(logQuery.EndDate, logQuery.Page, logQuery.PageSize, filter);
			}
			LogEntry[] logEntries = array;
			logQueryResult.LogEntries = logEntries;
			logQueryResult.ActualPageSize = array.GetLength(0);
			logQueryResult.CurrentPage = logQuery.Page;
			logQueryResult.TotalPages = int.MaxValue;
			if (logQueryResult.TotalPages == 0)
			{
				logQueryResult.TotalPages = 1;
			}
			return logQueryResult;
		}
		default:
			throw new ArgumentOutOfRangeException("logQuery", "Unknown log type");
		}
	}

	public static LogEntryField[] QueryLogFilter(LogFilter logFilter)
	{
		return logFilter.LogFilterEntry switch
		{
			LogFilterEntry.Level => QueryFilterLevelList(logFilter), 
			LogFilterEntry.SourceName => QueryFilterSourceList(logFilter), 
			LogFilterEntry.ResourceName => QueryFilterResourceNameList(logFilter), 
			LogFilterEntry.ResourceType => QueryFilterResourceTypeList(logFilter), 
			LogFilterEntry.ResourceHost => QueryFilterResourceHostList(logFilter), 
			LogFilterEntry.EventName => QueryFilterEventNameList(logFilter), 
			LogFilterEntry.ServiceName => QueryFilterServiceNameList(logFilter), 
			LogFilterEntry.UserName => QueryFilterUserNameList(logFilter), 
			LogFilterEntry.AuditId => QueryFilterAuditIdList(logFilter), 
			LogFilterEntry.AuditCategory => QueryFilterAuditTypeList(logFilter), 
			LogFilterEntry.Permission => QueryFilterPermissionList(logFilter), 
			LogFilterEntry.Location => QueryFilterLocationList(logFilter), 
			LogFilterEntry.RuleName => QueryFilterRuleName(logFilter), 
			_ => null, 
		};
	}

	private static LogEntryField[] QueryFilterLevelList(LogFilter logFilter)
	{
		LogEntryField[] result = null;
		using (LogDataComponent logDataComponent = new LogDataComponent(logFilter.CultureName))
		{
			switch (logFilter.LogType)
			{
			case LogType.SystemLog:
				result = logDataComponent.QueryLogSystemSeverityList();
				break;
			case LogType.AuditLog:
				result = logDataComponent.QueryLogAuditLevelList();
				break;
			}
		}
		return result;
	}

	private static LogEntryField[] QueryFilterSourceList(LogFilter logFilter)
	{
		LogEntryField[] result = null;
		using (LogDataComponent logDataComponent = new LogDataComponent(logFilter.CultureName))
		{
			switch (logFilter.LogType)
			{
			case LogType.SystemLog:
				result = logDataComponent.QueryLogSystemSourceList();
				break;
			case LogType.AuditLog:
				result = logDataComponent.QueryLogAuditSourceList();
				break;
			case LogType.RuleLog:
				result = logDataComponent.QueryLogRuleSourceList();
				break;
			}
		}
		return result;
	}

	private static LogEntryField[] QueryFilterResourceNameList(LogFilter logFilter)
	{
		LogEntryField[] result = null;
		using (LogDataComponent logDataComponent = new LogDataComponent(logFilter.CultureName))
		{
			if (logFilter.LogType == LogType.AuditLog)
			{
				result = logDataComponent.QueryLogAuditResourceNameList();
			}
		}
		return result;
	}

	private static LogEntryField[] QueryFilterResourceTypeList(LogFilter logFilter)
	{
		LogEntryField[] result = null;
		using (LogDataComponent logDataComponent = new LogDataComponent(logFilter.CultureName))
		{
			if (logFilter.LogType == LogType.AuditLog)
			{
				result = logDataComponent.QueryLogAuditResourceTypeList();
			}
		}
		return result;
	}

	private static LogEntryField[] QueryFilterResourceHostList(LogFilter logFilter)
	{
		LogEntryField[] result = null;
		using (LogDataComponent logDataComponent = new LogDataComponent(logFilter.CultureName))
		{
			if (logFilter.LogType == LogType.AuditLog)
			{
				result = logDataComponent.QueryLogAuditResourceHostList();
			}
		}
		return result;
	}

	private static LogEntryField[] QueryFilterEventNameList(LogFilter logFilter)
	{
		LogEntryField[] result = null;
		using (LogDataComponent logDataComponent = new LogDataComponent(logFilter.CultureName))
		{
			switch (logFilter.LogType)
			{
			case LogType.SystemLog:
				result = logDataComponent.QueryLogSystemEventTypeList();
				break;
			case LogType.RuleLog:
				result = logDataComponent.QueryLogRuleEventNameList();
				break;
			}
		}
		return result;
	}

	private static LogEntryField[] QueryFilterServiceNameList(LogFilter logFilter)
	{
		LogEntryField[] result = null;
		using (LogDataComponent logDataComponent = new LogDataComponent(logFilter.CultureName))
		{
			LogType logType = logFilter.LogType;
			if (logType == LogType.RuleLog)
			{
				result = logDataComponent.QueryLogRuleServiceNameList();
			}
		}
		return result;
	}

	private static LogEntryField[] QueryFilterUserNameList(LogFilter logFilter)
	{
		LogEntryField[] result = null;
		using (LogDataComponent logDataComponent = new LogDataComponent(logFilter.CultureName))
		{
			if (logFilter.LogType == LogType.AuditLog)
			{
				result = logDataComponent.QueryUserNameList();
			}
		}
		return result;
	}

	private static LogEntryField[] QueryFilterAuditIdList(LogFilter logFilter)
	{
		LogEntryField[] result = null;
		using (LogDataComponent logDataComponent = new LogDataComponent(logFilter.CultureName))
		{
			if (logFilter.LogType == LogType.AuditLog)
			{
				result = logDataComponent.QueryAuditIdList();
			}
		}
		return result;
	}

	private static LogEntryField[] QueryFilterAuditTypeList(LogFilter logFilter)
	{
		LogEntryField[] result = null;
		using (LogDataComponent logDataComponent = new LogDataComponent(logFilter.CultureName))
		{
			if (logFilter.LogType == LogType.AuditLog)
			{
				result = logDataComponent.QueryLogAuditTypeList();
			}
		}
		return result;
	}

	private static LogEntryField[] QueryFilterPermissionList(LogFilter logFilter)
	{
		LogEntryField[] result = null;
		using (LogDataComponent logDataComponent = new LogDataComponent(logFilter.CultureName))
		{
			if (logFilter.LogType == LogType.AuditLog)
			{
				result = logDataComponent.QueryLogAuditPermissionList();
			}
		}
		return result;
	}

	private static LogEntryField[] QueryFilterLocationList(LogFilter logFilter)
	{
		LogEntryField[] result = null;
		using (LogDataComponent logDataComponent = new LogDataComponent(logFilter.CultureName))
		{
			if (logFilter.LogType == LogType.AuditLog)
			{
				result = logDataComponent.QueryLocationList();
			}
		}
		return result;
	}

	private static LogEntryField[] QueryFilterRuleName(LogFilter logFilter)
	{
		LogEntryField[] result = null;
		using (LogDataComponent logDataComponent = new LogDataComponent(logFilter.CultureName))
		{
			LogType logType = logFilter.LogType;
			if (logType == LogType.RuleLog)
			{
				result = logDataComponent.QueryRuleNameList();
			}
		}
		return result;
	}

	private static string BuildFilter(LogFilter[] filters)
	{
		string text = string.Empty;
		if (filters != null && filters.GetLength(0) > 0)
		{
			int num = 1;
			foreach (LogFilter logFilter in filters)
			{
				if (num > 1)
				{
					text += " AND ";
				}
				text += string.Format(CultureInfo.InvariantCulture, "{0} {1} '{2}'", LogDataComponent.GetColumnNameForWhereClause(logFilter.LogFilterEntry).Replace("'", "''"), logFilter.ConditionAsString, logFilter.Value.Replace("'", "''"));
				num++;
			}
		}
		return text;
	}
}
