using System;
using System.Collections.Generic;

namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class CreatedSessionMessageItem
{
	public Guid SessionID { get; set; }

	public Dictionary<int, int> MessageIDs { get; set; }

	public DateTime LatestMessageCreateTime { get; set; }

	public CreatedSessionMessageItem()
	{
	}

	public CreatedSessionMessageItem(Guid sessionID, int messageID, DateTime latestMessageCreateTime)
	{
		SessionID = sessionID;
		MessageIDs = new Dictionary<int, int> { { messageID, messageID } };
		LatestMessageCreateTime = latestMessageCreateTime;
	}
}
