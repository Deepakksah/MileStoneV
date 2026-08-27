using System.Threading;
using System.Threading.Tasks;
using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service.Sample.Services.Rule;

public interface IRuleStateChangeChannel
{
	void TryWrite(StateChangeData counterStateChangeData);

	ValueTask<StateChangeData> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));
}
