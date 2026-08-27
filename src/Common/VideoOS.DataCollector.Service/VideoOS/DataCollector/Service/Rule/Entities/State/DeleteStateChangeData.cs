namespace VideoOS.DataCollector.Service.Rule.Entities.State;

public record DeleteStateChangeData : StateChangeData
{
	public DeleteStateChangeData(int dataCollectorId, string counterRuleId, string counterPath, int state, double triggerValue)
		: base(dataCollectorId, counterRuleId, counterPath, state, triggerValue)
	{
	}
}
