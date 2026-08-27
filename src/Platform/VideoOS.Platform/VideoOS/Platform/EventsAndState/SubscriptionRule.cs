namespace VideoOS.Platform.EventsAndState;

public class SubscriptionRule
{
	public Modifier Modifier { get; }

	public ResourceTypes ResourceTypes { get; }

	public SourceIds SourceIds { get; }

	public EventTypes EventTypes { get; }

	public SubscriptionRule(Modifier Modifier, ResourceTypes ResourceTypes, SourceIds SourceIds, EventTypes EventTypes)
	{
		this.Modifier = Modifier;
		this.ResourceTypes = ResourceTypes;
		this.SourceIds = SourceIds;
		this.EventTypes = EventTypes;
	}
}
