using System;
using System.Collections.Generic;

namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class SessionMsgIndicatorData
{
	public int TotalMsgCount { get; set; }

	public DateTime LatestMsgCreateTime { get; set; }

	public HashSet<Guid> SessionsWithMessages { get; set; }

	public SessionMsgIndicatorData()
	{
		TotalMsgCount = 0;
		LatestMsgCreateTime = DateTime.MinValue;
		SessionsWithMessages = new HashSet<Guid>();
	}
}
