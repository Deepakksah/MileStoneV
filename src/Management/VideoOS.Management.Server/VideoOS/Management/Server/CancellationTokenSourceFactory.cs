using System.Threading;

namespace VideoOS.Management.Server;

public class CancellationTokenSourceFactory : ICancellationTokenSourceFactory
{
	public CancellationTokenSource CreateCancellationTokenSource()
	{
		return new CancellationTokenSource();
	}
}
