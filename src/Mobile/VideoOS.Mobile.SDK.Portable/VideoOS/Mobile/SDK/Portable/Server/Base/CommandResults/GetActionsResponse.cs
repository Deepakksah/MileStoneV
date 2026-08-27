using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetActionsResponse : BaseCommandResponse
{
	public List<OEHeaderGroup> OeHeaderGroups { get; private set; }

	public GetActionsResponse(Command command)
		: base(command)
	{
		OeHeaderGroups = command.OutputsAndEvents;
	}

	public GetActionsResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
