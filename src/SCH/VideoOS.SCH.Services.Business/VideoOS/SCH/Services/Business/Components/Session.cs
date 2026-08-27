using System;
using System.Collections.Generic;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.SCH.Services.DataAccess;
using VideoOS.SCH.Services.DataAccess.Generic;

namespace VideoOS.SCH.Services.Business.Components;

public class Session
{
	public static DateTime GetSessionExpiryTime(DateTime lastActiveTime)
	{
		return lastActiveTime.AddSeconds(ConfigSettings.SCHSessionTimeoutSecs);
	}

	public static void SetSessionExpireTime(IEnumerable<Guid> sessionIDs)
	{
		GenericDataMgr.GetSessionDataMgr().SetSessionExpireTime(sessionIDs, GetSessionExpiryTime(DateTime.UtcNow));
	}

	public static void SetSessionExpireTime(Guid sessionID)
	{
		SetSessionExpireTime(new Guid[1] { sessionID });
	}

	private bool ValidateSessionData(string clientName, string clientUserName)
	{
		if (string.IsNullOrEmpty(clientName))
		{
			ServiceContext.Current.AddErrAndLog(-201003, "Invalid Session data; null or empty ClientName not allowed.", LogType.Error);
			return false;
		}
		if (clientName.Length > 50)
		{
			ServiceContext.Current.AddErrAndLog(-201003, "Invalid Session data; Client name too long, cannot exceed " + 50 + " characters.", LogType.Error);
			return false;
		}
		if (clientUserName != null && clientUserName.Length > 50)
		{
			ServiceContext.Current.AddErrAndLog(-201005, "Invalid Session data; Client user name too long, cannot exceed " + 50 + " characters.", LogType.Error);
			return false;
		}
		return true;
	}

	public bool BeginSession(string clientName, out Guid sessionID, out int recommendedMsgChkInterval)
	{
		recommendedMsgChkInterval = ConfigSettings.RecommendedMsgChkIntervalMs;
		string text = ServiceContext.Current.ClientUserID;
		if (!string.IsNullOrEmpty(ServiceContext.Current.ClientUserDomain))
		{
			text = ServiceContext.Current.ClientUserDomain + "\\" + text;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "Anonymous";
		}
		if (!ValidateSessionData(clientName, text))
		{
			sessionID = Guid.Empty;
			recommendedMsgChkInterval = 0;
			return false;
		}
		return GenericDataMgr.GetSessionDataMgr().CreateSession(clientName, text, GetSessionExpiryTime(DateTime.UtcNow), out sessionID);
	}

	public bool EndSession(Guid sessionID)
	{
		return GenericDataMgr.GetSessionDataMgr().DeleteSession(sessionID);
	}

	public List<ServiceChannelClient> GetAllClients()
	{
		return GenericDataMgr.GetSessionDataMgr().GetAllClients();
	}

	public bool DeleteExpiredSessions()
	{
		bool result = true;
		ISessionDataMgr sessionDataMgr = GenericDataMgr.GetSessionDataMgr();
		foreach (Guid item in sessionDataMgr.GetSessionIDsWithExpiryTimeLessThan(DateTime.UtcNow))
		{
			if (!sessionDataMgr.DeleteSession(item))
			{
				result = false;
			}
		}
		return result;
	}
}
