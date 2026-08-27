using System.Threading;

namespace VideoOS.Management.Server;

public interface IScheduledActionData<T>
{
	CancellationToken Token { get; set; }

	T InputData { get; }
}
