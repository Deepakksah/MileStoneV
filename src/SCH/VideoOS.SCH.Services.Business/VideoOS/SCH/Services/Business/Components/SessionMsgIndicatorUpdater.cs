using System;
using System.Collections.Generic;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.SCH.Services.DataAccess;
using VideoOS.SCH.Services.DataAccess.Generic;

namespace VideoOS.SCH.Services.Business.Components;

public class SessionMsgIndicatorUpdater
{
	public static bool UpdateSessionMsgIndicatorData()
	{
		int totalMsgCount = SessionMsgIndicator.Current.TotalMsgCount;
		DateTime latestMsgCreateTime = SessionMsgIndicator.Current.LatestMsgCreateTime;
		IMessageDataMgr messageDataMgr = GenericDataMgr.GetMessageDataMgr();
		if (!messageDataMgr.GetTotalSessionMsgInfo(out var totalMsgCount2, out var latestMsgCreateTime2))
		{
			return false;
		}
		if (totalMsgCount != totalMsgCount2 || latestMsgCreateTime != latestMsgCreateTime2)
		{
			List<SessionMsgInfo> sessionMsgInfoList = messageDataMgr.GetSessionMsgInfoList();
			SessionMsgIndicatorData sessionMsgIndicatorData = new SessionMsgIndicatorData();
			foreach (SessionMsgInfo item in sessionMsgInfoList)
			{
				sessionMsgIndicatorData.SessionsWithMessages.Add(item.SessionId);
				sessionMsgIndicatorData.TotalMsgCount += item.MsgCount;
				if (item.LatestMsgCreateTime > sessionMsgIndicatorData.LatestMsgCreateTime)
				{
					sessionMsgIndicatorData.LatestMsgCreateTime = item.LatestMsgCreateTime;
				}
			}
			SessionMsgIndicator.Current.UpdateData(sessionMsgIndicatorData);
		}
		return true;
	}
}
