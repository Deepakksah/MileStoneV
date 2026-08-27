using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetSubItemsResponse : BaseCommandResponse
{
	public SubViews SubItems { get; private set; }

	public GetSubItemsResponse(Command command)
		: base(command)
	{
		SubItems = command.SubViews;
	}

	public GetSubItemsResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
