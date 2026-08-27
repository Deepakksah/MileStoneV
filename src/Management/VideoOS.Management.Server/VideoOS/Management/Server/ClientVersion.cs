using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public class ClientVersion
{
	public SmartClientVersion SmartClientVersion { get; private set; }

	public SmartClientUpgrade SmartClientUpgrade { get; private set; }

	public ClientVersion(SmartClientVersion smartClientVersion, SmartClientUpgrade smartClientUpgrade)
	{
		SmartClientVersion = smartClientVersion;
		SmartClientUpgrade = smartClientUpgrade;
	}
}
