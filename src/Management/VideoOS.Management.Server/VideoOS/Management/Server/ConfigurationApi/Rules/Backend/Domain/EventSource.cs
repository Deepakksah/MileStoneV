using System;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

public class EventSource
{
	public Guid Id { get; set; }

	public string ItemType { get; set; }

	public bool IsGroup { get; set; }
}
