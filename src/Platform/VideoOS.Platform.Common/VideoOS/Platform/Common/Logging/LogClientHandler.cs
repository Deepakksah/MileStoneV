using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Log;
using VideoOS.LogServer.Communication.LogReaderServiceReference;
using VideoOS.LogServer.Communication.Readers;

namespace VideoOS.Platform.Common.Logging;

public class LogClientHandler : ILogClientHandler
{
	private VideoOS.Log.LogClient _logClient;

	private ILogReader _logClientReader;

	private readonly string _culture;

	public LogClientHandler(string culture, ServerId serverId)
	{
		_culture = culture;
		_logClient = new VideoOS.Log.LogClient(_culture);
		if (serverId != null)
		{
			_logClientReader = LogReaderFactory.Create((ILogger)(object)new Logger(), serverId);
		}
		else
		{
			_logClientReader = LogReaderFactory.Create((ILogger)(object)new Logger());
		}
	}

	public void SetUserId(string userName)
	{
		_logClient.SetUserId(userName);
	}

	public bool UploadCultureFile(string fileName, bool forceupload, bool asynchronous)
	{
		return _logClient.UploadCultureFile(fileName, forceupload, asynchronous);
	}

	public int GetColumnValues(string column, string culture, string table, string filter, int page, int pagesize, out ArrayList result)
	{
		return _logClient.GetColumnValues(column, culture, table, filter, page, pagesize, out result);
	}

	public void GetLogMessages(string query, string culture, int page, int pagesize, out ArrayList result, out ArrayList names)
	{
		_logClient.GetLogMessages(query, culture, page, pagesize, out result, out names);
	}

	public void LogWrite(string token, string objectType, string objectId, string objectName, string objectLocationId, string objectLocationName, DateTime timestamp, string version, string application, string component, string module, string group, string category, string status, string severity, string messageId, string messageText, string messageCulture, Hashtable variables, List<LogMessageUserField> fields)
	{
		_logClient.LogWrite(token, objectType, objectId, objectName, objectLocationId, objectLocationName, timestamp, version, application, component, module, group, category, status.ToClientStatus(), severity.ToClientSeverity(), messageId, messageText, messageCulture, variables, fields);
	}

	public void Dispose()
	{
		_logClient.Dispose();
	}

	public ArrayList ReadGroups()
	{
		return new ArrayList { "System", "Audit", "Rules" };
	}

	public long GetCount(string group, DateTime beginTime, DateTime endTime)
	{
		QueryFilters queryFilters = new QueryFilters
		{
			LogType = group.ToLogReaderLogType(),
			FromTime = beginTime,
			ToTime = endTime
		};
		Task.Run(async delegate
		{
			await _logClientReader.StartQuerySessionAsync(queryFilters, _culture);
		}).Wait();
		return _logClientReader.TotalLoglinesCount;
	}

	public void ReadLogs(int page, out ArrayList result, out ArrayList names, string group)
	{
		ReadLogs(page, out result, out names, group, DateTime.UtcNow.AddYears(-20), DateTime.UtcNow.AddYears(20));
	}

	public void ReadLogs(int page, out ArrayList result, out ArrayList names, string group, DateTime beginTime, DateTime endTime)
	{
		QueryFilters queryFilters = new QueryFilters
		{
			LogType = group.ToLogReaderLogType(),
			FromTime = beginTime,
			ToTime = endTime
		};
		names = BuildColumnNames(group);
		_logClientReader.BatchSize = 1000;
		Task.Run(async delegate
		{
			await _logClientReader.StartQuerySessionAsync(queryFilters, _culture);
		}).Wait();
		int offset = (page - 1) * _logClientReader.BatchSize;
		switch (queryFilters.LogType)
		{
		case LogType.Audit:
		{
			Task<IEnumerable<AuditLogline>> task3 = Task.Run(async () => await _logClientReader.FetchAuditLoglinesAsync(offset));
			result = BuildResults(task3.Result, offset);
			break;
		}
		case LogType.Rule:
		{
			Task<IEnumerable<RuleLogline>> task2 = Task.Run(async () => await _logClientReader.FetchRuleLoglinesAsync(offset));
			result = BuildResults(task2.Result, offset);
			break;
		}
		case LogType.System:
		{
			Task<IEnumerable<SystemLogline>> task = Task.Run(async () => await _logClientReader.FetchSystemLoglinesAsync(offset));
			result = BuildResults(task.Result, offset);
			break;
		}
		case LogType.Unknown:
		{
			Task<IEnumerable<SystemLogline>> task = Task.Run(async () => await _logClientReader.FetchSystemLoglinesAsync(offset));
			result = BuildResults(task.Result, offset);
			break;
		}
		default:
		{
			Task<IEnumerable<SystemLogline>> task = Task.Run(async () => await _logClientReader.FetchSystemLoglinesAsync(offset));
			result = BuildResults(task.Result, offset);
			break;
		}
		}
	}

	private static ArrayList BuildColumnNames(string group)
	{
		return group.ToLogReaderLogType() switch
		{
			LogType.Audit => new ArrayList { "Local time", "Message text", "Permission", "Category", "Source type", "Source name", "User", "User location" }, 
			LogType.Rule => new ArrayList { "Local time", "Message text", "Category", "Source type", "Source name", "Event type", "Rule name", "Service name" }, 
			LogType.System => new ArrayList { "Log level", "Local time", "Message text", "Category", "Source type", "Source name", "Event type" }, 
			LogType.Unknown => new ArrayList { "Log level", "Local time", "Message text", "Category", "Source type", "Source name", "Event type" }, 
			_ => new ArrayList { "Log level", "Local time", "Message text", "Category", "Source type", "Source name", "Event type" }, 
		};
	}

	private static ArrayList BuildResults(IEnumerable<AuditLogline> loglines, int offset)
	{
		ArrayList arrayList = new ArrayList();
		int num = offset;
		foreach (AuditLogline logline in loglines)
		{
			ArrayList value = new ArrayList
			{
				num.ToString(CultureInfo.InvariantCulture),
				logline.ClientTimestampUtc.ToLocalTime().ToString(CultureInfo.InvariantCulture),
				logline.MessageText,
				logline.PermissionType.ToString(),
				logline.Category,
				logline.SourceType,
				logline.SourceName,
				logline.User,
				logline.UserLocation
			};
			arrayList.Add(value);
			num++;
		}
		return arrayList;
	}

	private static ArrayList BuildResults(IEnumerable<SystemLogline> loglines, int offset)
	{
		ArrayList arrayList = new ArrayList();
		int num = offset;
		foreach (SystemLogline logline in loglines)
		{
			ArrayList value = new ArrayList
			{
				num.ToString(CultureInfo.InvariantCulture),
				logline.LogLevel.ToString(),
				logline.ClientTimestampUtc.ToLocalTime().ToString(CultureInfo.InvariantCulture),
				logline.MessageText,
				logline.Category,
				logline.SourceType,
				logline.SourceName,
				logline.EventType
			};
			arrayList.Add(value);
			num++;
		}
		return arrayList;
	}

	private static ArrayList BuildResults(IEnumerable<RuleLogline> loglines, int offset)
	{
		ArrayList arrayList = new ArrayList();
		int num = offset;
		foreach (RuleLogline logline in loglines)
		{
			ArrayList value = new ArrayList
			{
				num.ToString(CultureInfo.InvariantCulture),
				logline.ClientTimestampUtc.ToLocalTime().ToString(CultureInfo.InvariantCulture),
				logline.MessageText,
				logline.Category,
				logline.SourceType,
				logline.SourceName,
				logline.EventType,
				logline.RuleName,
				logline.ServiceName
			};
			arrayList.Add(value);
			num++;
		}
		return arrayList;
	}
}
