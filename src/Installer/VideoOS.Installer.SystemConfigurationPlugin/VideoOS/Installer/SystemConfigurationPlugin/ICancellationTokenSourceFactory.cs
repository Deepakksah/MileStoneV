using System.Threading;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public interface ICancellationTokenSourceFactory
{
	CancellationTokenSource CreateTokenSource();
}
