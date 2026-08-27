namespace VideoOS.DataCollector.Service;

public class StateHandlerFactory : IStateHandlerFactory
{
	public IStateHandler CreateStateHandler(bool triggerPerValue, IRule rule)
	{
		if (triggerPerValue)
		{
			return new StatePerValueHandler(rule);
		}
		return new CounterStateHandler(rule);
	}
}
