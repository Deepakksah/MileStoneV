using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Platform.Log;

public class LogClient
{
	public virtual bool Initialized => false;

	public static LogClient Instance { get; set; }

	static LogClient()
	{
		Instance = new LogClient();
	}

	public virtual void SetCulture(string culture)
	{
	}

	public virtual void RegisterDictionary(LogMessageDictionary logMessageDictionary)
	{
	}

	public virtual void NewEntry(string application, string component, string messageId, Item relatedItem, Dictionary<string, string> parameters)
	{
	}

	public virtual void AuditEntry(string application, string component, string messageId, Item relatedItem, Dictionary<string, string> parameters, string permissionState)
	{
	}

	public virtual ArrayList ReadGroups(ServerId serverId)
	{
		return new ArrayList();
	}

	public virtual long GetCount(ServerId serverId, string group, DateTime beginTime, DateTime endTime)
	{
		return 0L;
	}

	public virtual void ReadLog(ServerId serverId, int page, out ArrayList result, out ArrayList names, string group)
	{
		result = new ArrayList();
		names = new ArrayList();
	}

	public virtual void ReadLog(ServerId serverId, int page, out ArrayList result, out ArrayList names, string group, DateTime beginTime, DateTime endTime)
	{
		result = new ArrayList();
		names = new ArrayList();
	}

	public virtual void Close(Guid serverGuid)
	{
	}

	public virtual void Close()
	{
	}
}
