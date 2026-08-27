using System.Collections.Generic;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

public class RulePartModel
{
	public TriggerType TriggerType { get; set; }

	public EventModel EventModel { get; set; }

	public RecurringTimeModel RecurringTime { get; set; }

	public bool IsStart { get; set; }

	public int Delay { get; set; }

	public IDictionary<ConditionType, ConditionModel> Conditions { get; set; }

	public IEnumerable<ActionModel> Actions { get; set; }
}
