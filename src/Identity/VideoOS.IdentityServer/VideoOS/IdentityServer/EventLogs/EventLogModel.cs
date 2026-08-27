using System;

namespace VideoOS.IdentityServer.EventLogs;

public abstract class EventLogModel
{
	public string Username { get; }

	public string ObjectId { get; set; }

	public string ObjectName { get; set; }

	public string ClientId { get; set; }

	public string ClientName { get; set; }

	protected EventLogModel(string username)
	{
		Username = username ?? throw new ArgumentNullException("username");
	}
}
