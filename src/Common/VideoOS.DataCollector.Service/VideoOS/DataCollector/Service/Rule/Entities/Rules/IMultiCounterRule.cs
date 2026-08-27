using System.Threading.Tasks;
using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service.Rule.Entities.Rules;

public interface IMultiCounterRule : IRuleBase
{
	Task<(bool, StateChange?)> SampleStateChange(StateChangeData state);

	void RegisterPerformanceCounterInstance(string performanceCounterInstancePath);

	Task<(bool, StateChange?)> UnRegisterPerformanceCounterInstance(string performanceCounterInstancePath);

	void ResetState();
}
