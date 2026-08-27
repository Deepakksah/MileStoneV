using Microsoft.Extensions.Logging;
using VideoOS.Installer.Common.Dism;

namespace VideoOS.GatewayService.Utilities;

public class DismSessionFactory : IDismSessionFactory
{
	public IDismSession CreateSession(ILogger logger)
	{
		DismSession dismSession = new DismSession(logger);
		dismSession.Initialize(null);
		return dismSession;
	}
}
