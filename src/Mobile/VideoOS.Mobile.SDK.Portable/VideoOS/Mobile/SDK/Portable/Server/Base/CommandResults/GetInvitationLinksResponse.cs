using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetInvitationLinksResponse : BaseCommandResponse
{
	public InvitationParams InvitationParams { get; private set; }

	public GetInvitationLinksResponse(Command command)
		: base(command)
	{
		InvitationParams = new InvitationParams(command, 125u, Command.ParamsType.Output);
	}

	public GetInvitationLinksResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
