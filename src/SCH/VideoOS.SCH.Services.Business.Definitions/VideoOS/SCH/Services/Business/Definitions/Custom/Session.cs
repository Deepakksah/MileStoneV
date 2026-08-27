using System;
using System.Collections.Generic;

namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class Session
{
	public Guid ID { get; set; }

	public List<int> MessageIDs { get; set; }

	public string ClientName { get; set; }

	public string ClientUserName { get; set; }

	public DateTime ExpiryTime { get; set; }

	public Session()
	{
	}

	public Session(Guid sessionID, List<int> messageIDs, string clientName, string clientUserName, DateTime expiryTime)
	{
		ID = sessionID;
		MessageIDs = messageIDs;
		ClientName = clientName;
		ClientUserName = clientUserName;
		ExpiryTime = expiryTime;
	}
}
