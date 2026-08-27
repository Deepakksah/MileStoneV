using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.EventsAndState;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class SubscriptionFilter
{
	public Modifier Modifier { get; set; }

	public IEnumerable<string> ResourceTypes { get; }

	public IEnumerable<string> SourceIds { get; set; }

	public IEnumerable<string> EventTypes { get; set; }

	public SubscriptionFilter(SubscriptionRule subscriptionRule)
	{
		Modifier = subscriptionRule.Modifier;
		IEnumerable<string> enumerable;
		if (!subscriptionRule.ResourceTypes.IsAny)
		{
			enumerable = subscriptionRule.ResourceTypes.Ids;
		}
		else
		{
			IEnumerable<string> enumerable2 = new string[1] { "*" };
			enumerable = enumerable2;
		}
		ResourceTypes = enumerable;
		IEnumerable<string> sourceIds;
		if (!subscriptionRule.SourceIds.IsAny)
		{
			sourceIds = subscriptionRule.SourceIds.Ids.Select((Guid x) => x.ToString());
		}
		else
		{
			IEnumerable<string> enumerable2 = new string[1] { "*" };
			sourceIds = enumerable2;
		}
		SourceIds = sourceIds;
		IEnumerable<string> eventTypes;
		if (!subscriptionRule.EventTypes.IsAny)
		{
			eventTypes = subscriptionRule.EventTypes.Ids.Select((Guid x) => x.ToString());
		}
		else
		{
			IEnumerable<string> enumerable2 = new string[1] { "*" };
			eventTypes = enumerable2;
		}
		EventTypes = eventTypes;
	}
}
