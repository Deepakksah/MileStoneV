using System;

namespace VideoOS.IdentityServer.Common.Model.Responses;

public class EventLogModel
{
	public long Id { get; set; }

	public DateTimeOffset TimeStamp { get; set; }

	public string Event { get; set; }

	public string UserName { get; set; }

	public string RemoteIpAddress { get; set; }

	public string ObjectId { get; set; }

	public string ObjectName { get; set; }

	public string ClientId { get; set; }

	public string ClientName { get; set; }
}
