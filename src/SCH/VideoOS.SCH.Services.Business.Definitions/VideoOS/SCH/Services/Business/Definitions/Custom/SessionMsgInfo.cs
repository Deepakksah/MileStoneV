using System;

namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class SessionMsgInfo
{
	public Guid SessionId { get; set; }

	public int MsgCount { get; set; }

	public DateTime LatestMsgCreateTime { get; set; }

	public SessionMsgInfo(Guid sessionId, int msgCount, DateTime latestMsgCreateTime)
	{
		SessionId = sessionId;
		MsgCount = msgCount;
		LatestMsgCreateTime = latestMsgCreateTime;
	}
}
