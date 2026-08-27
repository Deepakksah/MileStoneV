using System;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.LPR.Common.WCF.Client;

public class PluginCommandClientWrapper : CommandClientWrapper<PluginCommandClient, IPluginCommand>
{
	private readonly ServerId _serverId;

	public PluginCommandClientWrapper(ServerId serverId, Uri address)
		: base(LoginSettingsCache.GetLoginSettings(serverId), address, "LPR/PluginCommand", ErrorCode.EventServerCommunicationError)
	{
		_serverId = serverId;
	}

	protected override string GetToken()
	{
		return LoginSettingsCache.GetLoginSettings(_serverId).Token;
	}
}
