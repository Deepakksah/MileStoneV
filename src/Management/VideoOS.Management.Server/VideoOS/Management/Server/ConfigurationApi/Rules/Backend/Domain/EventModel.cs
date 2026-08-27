using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

public class EventModel
{
	public EventGroupType EventGroupType { get; set; }

	public Guid PluginEventGroupId { get; set; }

	public Guid EventType { get; set; }

	public IEnumerable<EventSource> EventSources { get; set; }
}
