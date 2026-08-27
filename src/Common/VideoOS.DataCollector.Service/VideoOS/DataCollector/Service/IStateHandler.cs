using VideoOS.DataCollector.Service.Rule.Entities.Sample;

namespace VideoOS.DataCollector.Service;

public interface IStateHandler
{
	(bool, StateChange?) AddCounterValue(PerformanceCounterSample counterValue);

	(bool, StateChange?) AddStateChange(StateChange stateChange);

	void Reset();

	(bool, StateChange?) SetUndefined();
}
