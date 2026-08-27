using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class UnregisterForNotificationsResponse : BaseCommandResponse
{
	public UnregisterForNotificationsResponse(Command command)
		: base(command)
	{
	}

	public UnregisterForNotificationsResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
