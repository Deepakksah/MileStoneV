using System.Threading;

namespace VideoOS.Management.Server;

public class ScheduledActionData<T> : IScheduledActionData<T>
{
	public T InputData { get; private set; }

	public CancellationToken Token { get; set; }

	public ScheduledActionData(T inputData, CancellationToken token)
	{
		InputData = inputData;
		Token = token;
	}
}
