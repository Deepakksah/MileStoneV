using System.Threading;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class CancellationTokenSourceFactory : ICancellationTokenSourceFactory
{
	public CancellationTokenSource CreateTokenSource()
	{
		return new CancellationTokenSource();
	}
}
