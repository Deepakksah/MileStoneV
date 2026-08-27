using System;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Platform.Login;

namespace VideoOS.LPR.Common.WCF.Client;

public class ServerCommandClientWrapper : CommandClientWrapper<ServerCommandClient, IServerCommand>
{
	public ServerCommandClientWrapper(Uri address)
		: base((LoginSettings)null, address, "LPR/ServerCommand", ErrorCode.LPRServerCommunicationError)
	{
	}

	protected override string GetToken()
	{
		return string.Empty;
	}
}
