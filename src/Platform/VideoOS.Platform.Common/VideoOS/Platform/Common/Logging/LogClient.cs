using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Log;
using VideoOS.Platform.Common.Properties;
using VideoOS.Platform.Log;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.Common.Logging;

public class LogClient : VideoOS.Platform.Log.LogClient
{
	public delegate void ServerLogger(string logType, string logEntryId, string resourceType, string resourceName, string resourceLocation, string auditType, string description, Dictionary<string, string> parameters, string accessGranted);

	public delegate(LogLevel logLevel, string culture) GetServerLogSettings();

	private const string FieldTypeString = "string";

	private const string FieldTypeEnum = "enum";

	private List<LogMessageDictionary> _allMessages = new List<LogMessageDictionary>();

	private Dictionary<Guid, LogClientManager> _logClientManagers = new Dictionary<Guid, LogClientManager>();

	private string _culture = "en-US";

	private UserContext _userContext;

	private readonly ServerLogger _serverLogger;

	private readonly GetServerLogSettings _getServerLogSettings;

	private (LogLevel logLevel, string culture)? _serverLogSettings;

	public override bool Initialized => true;

	public LogClient()
	{
	}

	public LogClient(ServerLogger serverLogger, GetServerLogSettings serverLogSettings)
	{
		_serverLogger = serverLogger;
		_getServerLogSettings = serverLogSettings;
	}

	public LogClient(UserContext userContext)
	{
		_userContext = userContext;
	}

	public override void SetCulture(string culture)
	{
		_culture = culture;
	}

	public override void RegisterDictionary(LogMessageDictionary logMessageDictionary)
	{
		foreach (LogMessageDictionary allMessage in _allMessages)
		{
			if (allMessage.Application == logMessageDictionary.Application && allMessage.Version == logMessageDictionary.Version && allMessage.Culture == logMessageDictionary.Culture)
			{
				EnvironmentManager.Instance.Log(error: true, "LogClient", "Duplicate LogMessageDictionary for " + logMessageDictionary.Application);
				return;
			}
		}
		_allMessages.Add(logMessageDictionary);
		foreach (LogClientManager value in _logClientManagers.Values)
		{
			value.AddLogDictionary(logMessageDictionary);
		}
	}

	public override void AuditEntry(string application, string component, string messageId, Item relatedItem, Dictionary<string, string> parameters, string permissionState)
	{
		AddEntry(application, component, messageId, relatedItem, parameters, permissionState);
	}

	public override void NewEntry(string application, string component, string messageId, Item relatedItem, Dictionary<string, string> parameters)
	{
		AddEntry(application, component, messageId, relatedItem, parameters, "");
	}

	private void AddEntry(string application, string component, string id, Item relatedItem, Dictionary<string, string> parameters, string permissionState)
	{
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Invalid comparison between Unknown and I4
		try
		{
			if (relatedItem == null)
			{
				relatedItem = (_userContext?.Configuration ?? Configuration.Instance).GetItem(EnvironmentManager.Instance.MasterSite);
			}
			LogClientManager logClientManager = LookupLogClient(relatedItem);
			if (!_serverLogSettings.HasValue && _getServerLogSettings != null)
			{
				_serverLogSettings = _getServerLogSettings();
			}
			string preferedCulture = (_serverLogSettings.HasValue ? _serverLogSettings.Value.culture : null);
			LogMessageDictionary logMessageDictionary = logClientManager.CheckForUpload(application, component, preferedCulture);
			if (logMessageDictionary != null && logMessageDictionary.LogMessages.TryGetValue(id, out var value))
			{
				LoginSettings loginSettings = _userContext?.GetLoginSettings(relatedItem.FQID) ?? LoginSettingsCache.GetLoginSettings(relatedItem.FQID);
				if (loginSettings != null)
				{
					string objectType = ConvertToObjectType(relatedItem.FQID.Kind);
					Hashtable hashtable = new Hashtable();
					if (parameters != null)
					{
						foreach (string key in parameters.Keys)
						{
							hashtable[key] = parameters[key];
						}
					}
					List<LogMessageUserField> list = new List<LogMessageUserField>();
					LogMessageUserField item = default(LogMessageUserField);
					item.FieldName = "SourceType";
					item.Type = "enum";
					item.Value = "Application";
					list.Add(item);
					LogMessageUserField item2 = default(LogMessageUserField);
					item2.FieldName = "ResourceType";
					item2.Type = "enum";
					item2.Value = logMessageDictionary.ResourceType;
					list.Add(item2);
					LogMessageUserField item3 = default(LogMessageUserField);
					item3.FieldName = "ResourceId";
					item3.Type = "string";
					item3.Value = Environment.MachineName;
					list.Add(item3);
					LogMessageUserField item4 = default(LogMessageUserField);
					item4.FieldName = "ResourceName";
					item4.Type = "string";
					item4.Value = application;
					list.Add(item4);
					if (permissionState != "")
					{
						LogMessageUserField item5 = default(LogMessageUserField);
						item5.FieldName = "AccessGranted";
						item5.Type = "enum";
						item5.Value = permissionState;
						list.Add(item5);
						LogMessageUserField item6 = default(LogMessageUserField);
						item6.FieldName = "AuditType";
						item6.Type = "enum";
						item6.Value = value.CategoryName;
						list.Add(item6);
					}
					logClientManager.LogClient.LogWrite(loginSettings.Token, objectType, relatedItem.FQID.ObjectId.ToString(), relatedItem.Name, logMessageDictionary.ResourceType, Environment.MachineName, DateTime.UtcNow, logMessageDictionary.Version, logMessageDictionary.Application, logMessageDictionary.Component, "", value.Group, value.Category, value.Status, value.Severity, id, value.Message, _culture, hashtable, list);
					if (_serverLogger != null && _serverLogSettings.HasValue && (int)_serverLogSettings.Value.logLevel != 6)
					{
						object[] array = new object[hashtable.Count];
						hashtable.CopyTo(array, 0);
						_serverLogger(GetLocalizedLogType(value.Group), id, logMessageDictionary.ResourceType, relatedItem.Name, Environment.MachineName, value.CategoryName, value.Message, parameters, permissionState);
					}
				}
				else
				{
					EnvironmentManager.Instance.Log(error: true, "LogClient", "Unable to log messages on server, when not logged in");
				}
			}
			else
			{
				EnvironmentManager.Instance.Log(error: true, "LogClient", "Unable to find log message with id=" + id);
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "LogClient", "Exception: " + ex.Message);
		}
	}

	private LogClientManager LookupLogClient(Item relatedItem)
	{
		ServerId serverId = relatedItem.FQID.ServerId;
		Item item = relatedItem;
		while (item.FQID.ServerId.ServerType == "XPCORS")
		{
			item = item.GetParent();
			if (item == null)
			{
				break;
			}
			serverId = item.FQID.ServerId;
		}
		return LookupLogClient(serverId);
	}

	private LogClientManager LookupLogClient(ServerId serverId)
	{
		lock (_logClientManagers)
		{
			if (!_logClientManagers.ContainsKey(serverId.Id))
			{
				LogClientManager logClientManager = new LogClientManager(serverId, _culture, _userContext?.GetLoginSettings(serverId));
				if (logClientManager.LogClient == null)
				{
					throw new MIPException("Server not logged on");
				}
				logClientManager.AddRangeLogDictionary(_allMessages);
				_logClientManagers.Add(serverId.Id, logClientManager);
				return logClientManager;
			}
			return _logClientManagers[serverId.Id];
		}
	}

	public override ArrayList ReadGroups(ServerId serverId)
	{
		return LookupLogClient(serverId).LogClient.ReadGroups();
	}

	public override long GetCount(ServerId serverId, string group, DateTime beginTime, DateTime endTime)
	{
		return LookupLogClient(serverId).LogClient.GetCount(group, beginTime, endTime);
	}

	public override void ReadLog(ServerId serverId, int page, out ArrayList result, out ArrayList names, string group)
	{
		LookupLogClient(serverId).LogClient.ReadLogs(page, out result, out names, group);
	}

	public override void ReadLog(ServerId serverId, int page, out ArrayList result, out ArrayList names, string group, DateTime beginTime, DateTime endTime)
	{
		LookupLogClient(serverId).LogClient.ReadLogs(page, out result, out names, group, beginTime, endTime);
	}

	public override void Close(Guid serverGuid)
	{
		lock (_logClientManagers)
		{
			if (_logClientManagers.TryGetValue(serverGuid, out var value))
			{
				value.Close();
				_logClientManagers.Remove(serverGuid);
			}
		}
	}

	public override void Close()
	{
		lock (_logClientManagers)
		{
			foreach (KeyValuePair<Guid, LogClientManager> item in _logClientManagers.ToList())
			{
				item.Value.Close();
				_logClientManagers.Remove(item.Key);
			}
		}
	}

	private static string ConvertToObjectType(Guid kind)
	{
		if (!(Kind.DefaultTypeToNameTable[kind] is string result))
		{
			return "";
		}
		return result;
	}

	private static string GetLocalizedLogType(string logType)
	{
		switch (logType)
		{
		case "Audit":
			return VideoOS.Platform.Common.Properties.Resources.Enum_ServerLogType_Audit;
		case "System":
			return VideoOS.Platform.Common.Properties.Resources.Enum_ServerLogType_System;
		case "Rules":
			return VideoOS.Platform.Common.Properties.Resources.Enum_ServerLogType_Rules;
		case "Event":
			return VideoOS.Platform.Common.Properties.Resources.Enum_ServerLogType_Event;
		default:
			EnvironmentManager.Instance.Log(error: false, "LogClient", "Unknown LogType encountered: '" + logType + "'");
			return logType;
		}
	}
}
