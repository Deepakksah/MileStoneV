using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class RegisterForNotificationsResponse : BaseCommandResponse
{
	public RegisterForNotificationsResponse(Command command)
		: base(command)
	{
	}

	public RegisterForNotificationsResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
