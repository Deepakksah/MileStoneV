using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class ConnectResponse : BaseCommandResponse
{
	public List<ServerEndpoint> Servers { get; set; }

	public ConnectResponse(Command command)
		: base(command)
	{
		Servers = SerializableServerEndpoint.GetServerEndPoints(command.Items);
	}

	public ConnectResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
