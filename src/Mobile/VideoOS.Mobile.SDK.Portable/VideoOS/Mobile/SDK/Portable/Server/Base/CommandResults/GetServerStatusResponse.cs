using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetServerStatusResponse : BaseCommandResponse
{
	public ServerStatus Status { get; private set; }

	public GetServerStatusResponse(Command command)
		: base(command)
	{
		Status = command.SrvStatus;
	}

	public GetServerStatusResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
