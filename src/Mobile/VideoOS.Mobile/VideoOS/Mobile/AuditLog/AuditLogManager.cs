using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Log;

namespace VideoOS.Mobile.AuditLog;

public static class AuditLogManager
{
	private static readonly Dictionary<Guid, IAuditLog> _userLogs = new Dictionary<Guid, IAuditLog>();

	private static IAuditLog _defaultLog;

	private static readonly object _lockObject = new object();

	public static IAuditLog GetDefaultLog()
	{
		if (_defaultLog == null)
		{
			_defaultLog = new AuditLog(LogClient.Instance);
		}
		return _defaultLog;
	}

	public static IAuditLog GetUserLog(UserContext userContext)
	{
		IAuditLog auditLog = null;
		if (userContext == null)
		{
			return null;
		}
		Guid instanceGuid = userContext.InstanceGuid;
		lock (_lockObject)
		{
			if (!_userLogs.ContainsKey(instanceGuid))
			{
				_userLogs.Add(instanceGuid, new AuditLog(userContext.LogClient));
			}
			return _userLogs[instanceGuid];
		}
	}

	public static void RemoveUserLog(UserContext userContext)
	{
		lock (_lockObject)
		{
			_userLogs.Remove(userContext.InstanceGuid);
		}
	}
}
