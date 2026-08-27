using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class ServerCapabilitiesResponse : BaseCommandResponse
{
	public IServerFeatures ServerFeatures { get; private set; }

	public ServerCapabilitiesResponse(Command command)
		: base(command)
	{
		ServerFeatures serverFeatures = new ServerFeatures();
		Dictionary<string, string> serverParams = command.ConvertToDictionary(Command.ParamsType.Output);
		serverFeatures.InitFromCommand(serverParams);
		ServerFeatures = serverFeatures;
	}

	public ServerCapabilitiesResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
