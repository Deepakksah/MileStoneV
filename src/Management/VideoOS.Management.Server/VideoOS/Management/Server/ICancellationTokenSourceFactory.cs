using System.Threading;

namespace VideoOS.Management.Server;

public interface ICancellationTokenSourceFactory
{
	CancellationTokenSource CreateCancellationTokenSource();
}
