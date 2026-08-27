using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.IdentityServer.Entities;

public class EventLog
{
	[Key]
	public long Id { get; set; }

	[Required]
	public DateTimeOffset TimeStamp { get; set; }

	[Required]
	public string Event { get; set; }

	[Required]
	public string UserName { get; set; }

	[Required]
	public string RemoteIpAddress { get; set; }

	public string ObjectId { get; set; }

	public string ObjectName { get; set; }

	public string ClientId { get; set; }

	public string ClientName { get; set; }
}
