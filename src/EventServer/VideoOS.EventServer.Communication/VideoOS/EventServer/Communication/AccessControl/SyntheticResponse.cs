using VideoOS.EventServer.Shared.Aggregation;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.AccessControl;

public class SyntheticResponse<T>
{
	public T Response { get; set; }

	public AggregationResult<ServerId, T> AggregationResult { get; set; }
}
