using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Common.Integration;
using VideoOS.Log;

namespace VideoOS.Server;

public sealed class LogDataComponent : LogDataComponentBase
{
	private class LogServiceResultSystem
	{
		public string Name { get; private set; }

		public string LogTime { get; private set; }

		public string Severity { get; private set; }

		public string SeverityValue { get; private set; }

		public string ObjectName { get; private set; }

		public string Message { get; private set; }

		public string SourceTypeValue { get; private set; }

		public string EventTypeId { get; private set; }

		public string EventType { get; private set; }

		public LogServiceResultSystem(ArrayList list)
		{
			Name = (list[1] as string).Trim();
			LogTime = (list[2] as string).Trim();
			Severity = (list[3] as string).Trim();
			SeverityValue = (list[4] as string).Trim();
			ObjectName = (list[5] as string).Trim();
			Message = (list[6] as string).Trim();
			SourceTypeValue = (list[7] as string).Trim();
			EventTypeId = (list[8] as string).Trim();
			EventType = (list[9] as string).Trim();
		}
	}

	private class LogServiceResultAudit
	{
		public string Name { get; private set; }

		public string LogTime { get; private set; }

		public string Severity { get; private set; }

		public string SeverityValue { get; private set; }

		public string Message { get; private set; }

		public string UserId { get; private set; }

		public string UserLocation { get; private set; }

		public string ResourceID { get; private set; }

		public string ResourceName { get; private set; }

		public string ResourceTypeValue { get; private set; }

		public string ResourceLocation { get; private set; }

		public string AccessGrantedValue { get; private set; }

		public string AuditTypeValue { get; private set; }

		public LogServiceResultAudit(ArrayList list)
		{
			Name = (list[1] as string).Trim();
			LogTime = (list[2] as string).Trim();
			Severity = (list[3] as string).Trim();
			SeverityValue = (list[4] as string).Trim();
			UserId = (list[5] as string).Trim();
			UserLocation = (list[6] as string).Trim();
			Message = (list[7] as string).Trim();
			ResourceID = (list[8] as string).Trim();
			ResourceName = (list[9] as string).Trim();
			ResourceTypeValue = (list[10] as string).Trim();
			ResourceLocation = (list[11] as string).Trim();
			AccessGrantedValue = (list[12] as string).Trim();
			AuditTypeValue = (list[13] as string).Trim();
		}
	}

	private class LogServiceResultRule
	{
		public string Name { get; private set; }

		public string LogTime { get; private set; }

		public string Severity { get; private set; }

		public string SeverityValue { get; private set; }

		public string ObjectName { get; private set; }

		public string Message { get; private set; }

		public string SourceTypeValue { get; private set; }

		public string EventTypeId { get; private set; }

		public string EventType { get; private set; }

		public string ServiceName { get; private set; }

		public string GeneratorId { get; private set; }

		public string GeneratorTypeValue { get; private set; }

		public string GeneratorName { get; private set; }

		public string RuleName { get; private set; }

		public LogServiceResultRule(ArrayList list)
		{
			Name = (list[1] as string).Trim();
			LogTime = (list[2] as string).Trim();
			Severity = (list[3] as string).Trim();
			SeverityValue = (list[4] as string).Trim();
			ObjectName = (list[5] as string).Trim();
			Message = (list[6] as string).Trim();
			SourceTypeValue = (list[7] as string).Trim();
			EventTypeId = (list[8] as string).Trim();
			EventType = (list[9] as string).Trim();
			ServiceName = (list[10] as string).Trim();
			GeneratorId = (list[11] as string).Trim();
			GeneratorTypeValue = (list[12] as string).Trim();
			GeneratorName = (list[13] as string).Trim();
			RuleName = (list[14] as string).Trim();
		}
	}

	private const string SystemFieldsToSelect = "name, time, severity.key, severity, objectname, message, user.sourcetype, user.eventtypeid, user.eventtype";

	private const string AuditFieldsToSelect = "name, time, severity.key, severity, userid, userlocation, message, user.resourceid, user.resourcename, user.resourcetype, user.resourcelocation, user.accessgranted, user.audittype";

	private const string RuleFieldsToSelect = "name, time, severity.key, severity, objectname, message, user.sourcetype, user.eventtypeid, user.eventtype, user.servicename, user.generatorid, user.generatortype, user.generatorname, user.rulename";

	private const string OrderByClause = "";

	public LogDataComponent()
		: base("en-US")
	{
	}

	public LogDataComponent(string culture)
		: base(culture)
	{
	}

	private static void AddCustomField(string fieldName, string fieldType, string fieldValue, List<LogMessageUserField> customFields)
	{
		customFields.Add(new LogMessageUserField
		{
			FieldName = fieldName,
			Type = fieldType,
			Value = fieldValue
		});
	}

	public void CreateLogEntry(LogEntrySystem logEntrySystem)
	{
		List<LogMessageUserField> list = new List<LogMessageUserField>();
		AddCustomField("SourceType", "enum", logEntrySystem.SourceTypeValue, list);
		AddCustomField("EventTypeId", "string", logEntrySystem.EventTypeId.ToString(), list);
		AddCustomField("EventType", "string", logEntrySystem.EventType, list);
		string text = GetLogEntryIdName(logEntrySystem.LogId);
		if (string.IsNullOrEmpty(text))
		{
			text = logEntrySystem.LogId.ToString(CultureInfo.InvariantCulture);
		}
		LogDataComponentBase.WriteToLogServer(logEntrySystem.SourceName, logEntrySystem.TimeStampUtc, "System", GetSeverityByLogLevel(logEntrySystem.LogLevel), text, _culture, GetVariablesTable(logEntrySystem), list);
	}

	public void CreateLogEntry(LogEntryAudit logEntryAudit)
	{
		List<LogMessageUserField> list = new List<LogMessageUserField>();
		AddCustomField("ResourceId", "string", logEntryAudit.ResourceID, list);
		AddCustomField("ResourceName", "string", logEntryAudit.ResourceName, list);
		AddCustomField("ResourceType", "enum", logEntryAudit.ResourceTypeValue, list);
		AddCustomField("ResourceLocation", "string", logEntryAudit.ResourceLocation, list);
		AddCustomField("AccessGranted", "enum", logEntryAudit.AccessGrantedValue, list);
		AddCustomField("AuditType", "enum", logEntryAudit.AuditTypeValue, list);
		string text = GetLogEntryIdName(logEntryAudit.LogId);
		if (string.IsNullOrEmpty(text))
		{
			text = logEntryAudit.LogId.ToString(CultureInfo.InvariantCulture);
		}
		LogDataComponentBase.WriteAuditToLogServer(logEntryAudit.ResourceID, logEntryAudit.ResourceName, logEntryAudit.ResourceLocation, logEntryAudit.AuditTypeValue, logEntryAudit.UserCredentials, logEntryAudit.Location, logEntryAudit.TimeStampUtc, "Audit", GetSeverityByLogLevel(logEntryAudit.LogLevel), text, _culture, GetVariablesTable(logEntryAudit), list);
	}

	public void CreateLogEntry(LogEntryRule logEntryRule)
	{
		List<LogMessageUserField> list = new List<LogMessageUserField>();
		AddCustomField("SourceType", "enum", logEntryRule.SourceTypeValue, list);
		AddCustomField("EventTypeId", "string", logEntryRule.EventTypeId.ToString(), list);
		AddCustomField("EventType", "string", logEntryRule.EventType, list);
		AddCustomField("ServiceName", "string", logEntryRule.ServiceName, list);
		AddCustomField("GeneratorId", "string", logEntryRule.GeneratorId.ToString(), list);
		AddCustomField("GeneratorType", "enum", logEntryRule.GeneratorTypeValue, list);
		AddCustomField("GeneratorName", "string", logEntryRule.GeneratorName, list);
		AddCustomField("RuleName", "string", logEntryRule.RuleName, list);
		string text = GetLogEntryIdName(logEntryRule.LogId);
		if (string.IsNullOrEmpty(text))
		{
			text = logEntryRule.LogId.ToString(CultureInfo.InvariantCulture);
		}
		LogDataComponentBase.WriteToLogServer(logEntryRule.SourceName, logEntryRule.TimeStampUtc, "Rule", GetSeverityByLogLevel(logEntryRule.LogLevel), text, _culture, GetVariablesTable(logEntryRule), list);
	}

	public void CreateLogEntries(LogEntry[] logEntries)
	{
		try
		{
			foreach (LogEntry logEntry in logEntries)
			{
				if (logEntry is LogEntrySystem)
				{
					CreateLogEntry((LogEntrySystem)logEntry);
				}
				else if (logEntry is LogEntryAudit)
				{
					CreateLogEntry((LogEntryAudit)logEntry);
				}
				else if (logEntry is LogEntryRule)
				{
					CreateLogEntry((LogEntryRule)logEntry);
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public LogEntrySystem[] QueryLogSystem(int pageNum, int pageSize, string filter)
	{
		string arg = (string.IsNullOrEmpty(filter) ? string.Empty : string.Format(CultureInfo.InvariantCulture, " WHERE {0} ", filter));
		string selectStatement = string.Format(CultureInfo.InvariantCulture, "SELECT name, time, severity.key, severity, objectname, message, user.sourcetype, user.eventtypeid, user.eventtype FROM System {0} {1}", arg, "");
		return QueryLogSystem(selectStatement, pageNum, pageSize);
	}

	public LogEntrySystem[] QueryLogSystem(DateTime endDate, int pageNum, int pageSize, string filter)
	{
		string text = (string.IsNullOrEmpty(filter) ? string.Empty : string.Format(CultureInfo.InvariantCulture, " WHERE {0} ", filter));
		text += (string.IsNullOrEmpty(text) ? " WHERE " : " AND ");
		text += GetTimeWhereSQLExpr(endDate);
		string selectStatement = string.Format(CultureInfo.InvariantCulture, "SELECT name, time, severity.key, severity, objectname, message, user.sourcetype, user.eventtypeid, user.eventtype FROM System {0} {1}", text, "");
		return QueryLogSystem(selectStatement, pageNum, pageSize);
	}

	public LogEntrySystem[] QueryLogSystem(DateTime startDate, DateTime endDate, string filter)
	{
		string text = (string.IsNullOrEmpty(filter) ? string.Empty : string.Format(CultureInfo.InvariantCulture, " WHERE {0} ", filter));
		text += (string.IsNullOrEmpty(text) ? " WHERE " : " AND ");
		text += GetTimeWhereSQLExpr(startDate, endDate);
		string selectStatement = string.Format(CultureInfo.InvariantCulture, "SELECT name, time, severity.key, severity, objectname, message, user.sourcetype, user.eventtypeid, user.eventtype FROM System {0} {1}", text, "");
		return QueryLogSystem(selectStatement, 1, 1000000);
	}

	private LogEntrySystem[] QueryLogSystem(string selectStatement, int pageNum, int pageSize)
	{
		List<LogEntrySystem> list = new List<LogEntrySystem>();
		try
		{
			ArrayList result;
			using (LogClient logClient = new LogClient())
			{
				logClient.SetServer(LogDataComponentBase.GetLogServerURL(), "StandardClientCache");
				logClient.GetLogMessages(selectStatement, _culture, pageNum, pageSize, out result, out var _);
			}
			for (int i = 0; i < result.Count; i++)
			{
				LogEntrySystem logEntrySystem = GetLogEntrySystem((ArrayList)result[i]);
				if (logEntrySystem != null)
				{
					list.Add(logEntrySystem);
				}
			}
			return list.ToArray();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public LogEntryAudit[] QueryLogAudit(int pageNum, int pageSize, string filter)
	{
		string arg = (string.IsNullOrEmpty(filter) ? string.Empty : string.Format(CultureInfo.InvariantCulture, " WHERE {0} ", filter));
		string selectStatement = string.Format(CultureInfo.InvariantCulture, "SELECT name, time, severity.key, severity, userid, userlocation, message, user.resourceid, user.resourcename, user.resourcetype, user.resourcelocation, user.accessgranted, user.audittype FROM Audit {0} {1}", arg, "");
		return QueryLogAudit(selectStatement, pageNum, pageSize);
	}

	public LogEntryAudit[] QueryLogAudit(DateTime endDate, int pageNum, int pageSize, string filter)
	{
		string text = (string.IsNullOrEmpty(filter) ? string.Empty : string.Format(CultureInfo.InvariantCulture, " WHERE {0} ", filter));
		text += (string.IsNullOrEmpty(text) ? " WHERE " : " AND ");
		text += GetTimeWhereSQLExpr(endDate);
		string selectStatement = string.Format(CultureInfo.InvariantCulture, "SELECT name, time, severity.key, severity, userid, userlocation, message, user.resourceid, user.resourcename, user.resourcetype, user.resourcelocation, user.accessgranted, user.audittype FROM Audit {0} {1}", text, "");
		return QueryLogAudit(selectStatement, pageNum, pageSize);
	}

	public LogEntryAudit[] QueryLogAudit(DateTime startDate, DateTime endDate, string filter)
	{
		string text = (string.IsNullOrEmpty(filter) ? string.Empty : string.Format(CultureInfo.InvariantCulture, " WHERE {0} ", filter));
		text += (string.IsNullOrEmpty(text) ? " WHERE " : " AND ");
		text += GetTimeWhereSQLExpr(startDate, endDate);
		string selectStatement = string.Format(CultureInfo.InvariantCulture, "SELECT name, time, severity.key, severity, userid, userlocation, message, user.resourceid, user.resourcename, user.resourcetype, user.resourcelocation, user.accessgranted, user.audittype FROM Audit {0} {1}", text, "");
		return QueryLogAudit(selectStatement, 1, 1000);
	}

	private LogEntryAudit[] QueryLogAudit(string selectStatement, int pageNum, int pageSize)
	{
		List<LogEntryAudit> list = new List<LogEntryAudit>();
		try
		{
			ArrayList result;
			using (LogClient logClient = new LogClient())
			{
				logClient.SetServer(LogDataComponentBase.GetLogServerURL(), "StandardClientCache");
				logClient.GetLogMessages(selectStatement, _culture, pageNum, pageSize, out result, out var _);
			}
			for (int i = 0; i < result.Count; i++)
			{
				LogEntryAudit logEntryAudit = GetLogEntryAudit((ArrayList)result[i]);
				if (logEntryAudit != null)
				{
					list.Add(logEntryAudit);
				}
			}
			return list.ToArray();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public LogEntryRule[] QueryLogRule(int pageNum, int pageSize, string filter)
	{
		string arg = (string.IsNullOrEmpty(filter) ? string.Empty : string.Format(CultureInfo.InvariantCulture, " WHERE {0} ", filter));
		string selectStatement = string.Format(CultureInfo.InvariantCulture, "SELECT name, time, severity.key, severity, objectname, message, user.sourcetype, user.eventtypeid, user.eventtype, user.servicename, user.generatorid, user.generatortype, user.generatorname, user.rulename FROM Rule {0} {1}", arg, "");
		return QueryLogRule(selectStatement, pageNum, pageSize);
	}

	public LogEntryRule[] QueryLogRule(DateTime endDate, int pageNum, int pageSize, string filter)
	{
		string text = (string.IsNullOrEmpty(filter) ? string.Empty : string.Format(CultureInfo.InvariantCulture, " WHERE {0} ", filter));
		text += (string.IsNullOrEmpty(text) ? " WHERE " : " AND ");
		text += GetTimeWhereSQLExpr(endDate);
		string selectStatement = string.Format(CultureInfo.InvariantCulture, "SELECT name, time, severity.key, severity, objectname, message, user.sourcetype, user.eventtypeid, user.eventtype, user.servicename, user.generatorid, user.generatortype, user.generatorname, user.rulename FROM Rule {0} {1}", text, "");
		return QueryLogRule(selectStatement, pageNum, pageSize);
	}

	public LogEntryRule[] QueryLogRule(DateTime startDate, DateTime endDate, string filter)
	{
		string text = (string.IsNullOrEmpty(filter) ? string.Empty : string.Format(CultureInfo.InvariantCulture, " WHERE {0} ", filter));
		text += (string.IsNullOrEmpty(text) ? " WHERE " : " AND ");
		text += GetTimeWhereSQLExpr(startDate, endDate);
		string selectStatement = string.Format(CultureInfo.InvariantCulture, "SELECT name, time, severity.key, severity, objectname, message, user.sourcetype, user.eventtypeid, user.eventtype, user.servicename, user.generatorid, user.generatortype, user.generatorname, user.rulename FROM Rule {0} {1}", text, "");
		return QueryLogRule(selectStatement, 1, 1000);
	}

	private LogEntryRule[] QueryLogRule(string selectStatement, int pageNum, int pageSize)
	{
		List<LogEntryRule> list = new List<LogEntryRule>();
		try
		{
			ArrayList result;
			using (LogClient logClient = new LogClient())
			{
				logClient.SetServer(LogDataComponentBase.GetLogServerURL(), "StandardClientCache");
				logClient.GetLogMessages(selectStatement, _culture, pageNum, pageSize, out result, out var _);
			}
			for (int i = 0; i < result.Count; i++)
			{
				LogEntryRule logEntryRule = GetLogEntryRule((ArrayList)result[i]);
				if (logEntryRule != null)
				{
					list.Add(logEntryRule);
				}
			}
			return list.ToArray();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public LogEntryField[] QueryLogSystemSourceList()
	{
		return GetDistinctColumnValues("objectname", "System", getName: false);
	}

	public LogEntryField[] QueryLogSystemEventTypeList()
	{
		return GetDistinctColumnValues("user.eventtype", "System", getName: false);
	}

	public LogEntryField[] QueryLogSystemSeverityList()
	{
		return GetDistinctColumnValues("severity", "System", getName: true);
	}

	public LogEntryField[] QueryLogAuditSourceList()
	{
		return GetDistinctColumnValues("user.resourcename", "Audit", getName: false);
	}

	public LogEntryField[] QueryLogAuditResourceNameList()
	{
		return GetDistinctColumnValues("user.resourcename", "Audit", getName: false);
	}

	public LogEntryField[] QueryLogAuditResourceTypeList()
	{
		return GetDistinctColumnValues("user.resourcetype", "Audit", getName: true);
	}

	public LogEntryField[] QueryLogAuditResourceHostList()
	{
		return GetDistinctColumnValues("user.resourcelocation", "Audit", getName: false);
	}

	public LogEntryField[] QueryLogAuditTypeList()
	{
		return GetDistinctColumnValues("user.audittype", "Audit", getName: true);
	}

	public LogEntryField[] QueryLogAuditPermissionList()
	{
		return GetDistinctColumnValues("user.accessgranted", "Audit", getName: true);
	}

	public LogEntryField[] QueryUserNameList()
	{
		return GetDistinctColumnValues("userid", "Audit", getName: false);
	}

	public LogEntryField[] QueryAuditIdList()
	{
		return GetDistinctColumnValues("name", "Audit", getName: false);
	}

	public LogEntryField[] QueryLocationList()
	{
		return GetDistinctColumnValues("userlocation", "Audit", getName: false);
	}

	public LogEntryField[] QueryLogAuditLevelList()
	{
		return GetDistinctColumnValues("severity", "Audit", getName: true);
	}

	public LogEntryField[] QueryLogRuleSourceList()
	{
		return GetDistinctColumnValues("objectname", "Rule", getName: false);
	}

	public LogEntryField[] QueryLogRuleEventNameList()
	{
		return GetDistinctColumnValues("user.eventtype", "Rule", getName: false);
	}

	public LogEntryField[] QueryLogRuleServiceNameList()
	{
		return GetDistinctColumnValues("user.servicename", "Rule", getName: false);
	}

	public LogEntryField[] QueryRuleNameList()
	{
		return GetDistinctColumnValues("user.rulename", "Rule", getName: false);
	}

	public static string GetColumnNameForWhereClause(LogFilterEntry logFilterEntry)
	{
		return logFilterEntry switch
		{
			LogFilterEntry.Level => "severity.key", 
			LogFilterEntry.SourceName => "objectname", 
			LogFilterEntry.ResourceName => "user.resourcename", 
			LogFilterEntry.ResourceType => "user.resourcetype.key", 
			LogFilterEntry.ResourceHost => "user.resourcelocation", 
			LogFilterEntry.EventName => "user.eventtype", 
			LogFilterEntry.ServiceName => "user.servicename", 
			LogFilterEntry.UserName => "userid", 
			LogFilterEntry.AuditId => "name", 
			LogFilterEntry.AuditCategory => "user.audittype.key", 
			LogFilterEntry.Permission => "user.accessgranted.key", 
			LogFilterEntry.Location => "userlocation", 
			LogFilterEntry.RuleName => "user.rulename", 
			_ => throw new ArgumentOutOfRangeException("logFilterEntry"), 
		};
	}

	private static string GetLogEntryIdName(int entryIdInt)
	{
		if (Enum.IsDefined(typeof(LogEntryId), entryIdInt))
		{
			return ((LogEntryId)Enum.Parse(typeof(LogEntryId), entryIdInt.ToString(CultureInfo.InvariantCulture), ignoreCase: true)/*cast due to constrained. prefix*/).ToString();
		}
		return string.Empty;
	}

	private static LogEntryId GetLogEntryId(string entryName)
	{
		if (Enum.TryParse<LogEntryId>(entryName, ignoreCase: true, out var result))
		{
			return result;
		}
		return LogEntryId.Undefined;
	}

	private static LogLevel GetLogLevelBySeverity(string severity)
	{
		if (Enum.TryParse<LogLevel>(severity, ignoreCase: true, out var result))
		{
			return result;
		}
		return LogLevel.Undefined;
	}

	private static Hashtable GetVariablesTable(LogEntry logEntry)
	{
		Hashtable hashtable = new Hashtable();
		if (logEntry != null && logEntry.Arguments.Length != 0)
		{
			for (int i = 0; i < logEntry.Arguments.Length; i++)
			{
				hashtable.Add("arg" + i.ToString(CultureInfo.InvariantCulture), logEntry.Arguments[i]);
			}
		}
		return hashtable;
	}

	private static Severity GetSeverityByLogLevel(LogLevel logLevel)
	{
		return logLevel switch
		{
			LogLevel.Info => Severity.Info, 
			LogLevel.Warning => Severity.Warning, 
			LogLevel.Error => Severity.Error, 
			_ => Severity.Debug, 
		};
	}

	private static LogEntrySystem GetLogEntrySystem(ArrayList list)
	{
		LogEntrySystem logEntrySystem = null;
		try
		{
			LogServiceResultSystem logServiceResultSystem = new LogServiceResultSystem(list);
			LogEntryId logEntryId = GetLogEntryId(logServiceResultSystem.Name);
			logEntrySystem = new LogEntrySystem(logEntryId);
			DateTime value = DateTime.Parse(logServiceResultSystem.LogTime, CultureInfo.InvariantCulture);
			value = DateTime.SpecifyKind(value, DateTimeKind.Utc);
			logEntrySystem.TimeStampLocal = value.ToLocalTime();
			logEntrySystem.TimeStampUtc = value;
			logEntrySystem.LogLevel = GetLogLevelBySeverity(logServiceResultSystem.Severity);
			logEntrySystem.LogLevelValue = logServiceResultSystem.SeverityValue;
			logEntrySystem.SourceName = logServiceResultSystem.ObjectName;
			logEntrySystem.SourceTypeValue = logServiceResultSystem.SourceTypeValue;
			logEntrySystem.EventTypeId = TryGetGuid(logServiceResultSystem.EventTypeId);
			logEntrySystem.EventType = logServiceResultSystem.EventType;
			logEntrySystem.Description = logServiceResultSystem.Message;
			return logEntrySystem;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

	private static LogEntryAudit GetLogEntryAudit(ArrayList list)
	{
		LogEntryAudit logEntryAudit = null;
		try
		{
			LogServiceResultAudit logServiceResultAudit = new LogServiceResultAudit(list);
			LogEntryId logEntryId = GetLogEntryId(logServiceResultAudit.Name);
			logEntryAudit = new LogEntryAudit(logEntryId);
			DateTime timeStampUtc = DateTime.Parse(logServiceResultAudit.LogTime, CultureInfo.InvariantCulture);
			logEntryAudit.TimeStampLocal = timeStampUtc.ToLocalTime();
			logEntryAudit.TimeStampUtc = timeStampUtc;
			logEntryAudit.LogLevel = GetLogLevelBySeverity(logServiceResultAudit.Severity);
			logEntryAudit.LogLevelValue = logServiceResultAudit.SeverityValue;
			logEntryAudit.UserCredentials = logServiceResultAudit.UserId;
			logEntryAudit.Location = logServiceResultAudit.UserLocation;
			logEntryAudit.ResourceID = logServiceResultAudit.ResourceID;
			logEntryAudit.ResourceName = logServiceResultAudit.ResourceName;
			logEntryAudit.ResourceTypeValue = logServiceResultAudit.ResourceTypeValue;
			logEntryAudit.ResourceLocation = logServiceResultAudit.ResourceLocation;
			logEntryAudit.AccessGrantedValue = logServiceResultAudit.AccessGrantedValue;
			logEntryAudit.AuditTypeValue = logServiceResultAudit.AuditTypeValue;
			logEntryAudit.Description = logServiceResultAudit.Message;
			return logEntryAudit;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

	private static LogEntryRule GetLogEntryRule(ArrayList list)
	{
		LogEntryRule logEntryRule = null;
		try
		{
			LogServiceResultRule logServiceResultRule = new LogServiceResultRule(list);
			LogEntryId logEntryId = GetLogEntryId(logServiceResultRule.Name);
			logEntryRule = new LogEntryRule(logEntryId);
			DateTime timeStampUtc = DateTime.Parse(logServiceResultRule.LogTime, CultureInfo.InvariantCulture);
			logEntryRule.TimeStampLocal = timeStampUtc.ToLocalTime();
			logEntryRule.TimeStampUtc = timeStampUtc;
			logEntryRule.LogLevel = GetLogLevelBySeverity(logServiceResultRule.Severity);
			logEntryRule.LogLevelValue = logServiceResultRule.SeverityValue;
			logEntryRule.SourceName = logServiceResultRule.ObjectName;
			logEntryRule.SourceTypeValue = logServiceResultRule.SourceTypeValue;
			logEntryRule.RuleName = logServiceResultRule.RuleName;
			logEntryRule.EventTypeId = TryGetGuid(logServiceResultRule.EventTypeId);
			logEntryRule.EventType = logServiceResultRule.EventType;
			logEntryRule.ServiceName = logServiceResultRule.ServiceName;
			logEntryRule.GeneratorId = TryGetGuid(logServiceResultRule.GeneratorId);
			logEntryRule.GeneratorTypeValue = logServiceResultRule.GeneratorTypeValue;
			logEntryRule.GeneratorName = logServiceResultRule.GeneratorName;
			logEntryRule.Description = logServiceResultRule.Message;
			return logEntryRule;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

	private static Guid TryGetGuid(string guidToConvert)
	{
		if (Guid.TryParse(guidToConvert, out var result))
		{
			return result;
		}
		return Guid.Empty;
	}

	private static string GetTimeWhereSQLExpr(DateTime endDate)
	{
		return " time < convert(datetime, '" + endDate.ToString("s", DateTimeFormatInfo.InvariantInfo) + "')";
	}

	private static string GetTimeWhereSQLExpr(DateTime startDate, DateTime endDate)
	{
		return " time BETWEEN convert(datetime, '" + startDate.ToString("s", DateTimeFormatInfo.InvariantInfo) + "') AND convert(datetime, '" + endDate.ToString("s", DateTimeFormatInfo.InvariantInfo) + "')";
	}
}
