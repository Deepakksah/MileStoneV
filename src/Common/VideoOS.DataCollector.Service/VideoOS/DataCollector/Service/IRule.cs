using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Rule.Entities.Sample;
using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service;

public interface IRule : IRuleBase, IEntity
{
	bool IsActive { get; }

	bool IsDefault { get; }

	bool Refresh(CounterStateRuleConfiguration ruleConfiguration);

	State GetState(ConcurrentQueue<CounterValue> values, State currentState);

	Task SampleIfDue(ISamplerFactory samplerFactory, CancellationToken cancellationToken);

	void OnRecordingServerDown();

	void OnRecordingServerUp();
}
