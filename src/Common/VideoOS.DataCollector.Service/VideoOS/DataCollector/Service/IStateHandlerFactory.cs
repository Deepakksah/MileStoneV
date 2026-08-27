namespace VideoOS.DataCollector.Service;

public interface IStateHandlerFactory
{
	IStateHandler CreateStateHandler(bool triggerPerValue, IRule rule);
}
