using Microsoft.Extensions.Logging;
using VideoOS.Installer.Common.Dism;

namespace VideoOS.GatewayService.Utilities;

public interface IDismSessionFactory
{
	IDismSession CreateSession(ILogger logger);
}
