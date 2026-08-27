using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetInvestigationsResponse : BaseCommandResponse
{
	public IEnumerable<IInvestigationEntry> Investigations { get; private set; }

	public GetInvestigationsResponse(Command command)
		: base(command)
	{
		Investigations = SerializableInvestigation.GetImplementersList(command.Items);
	}

	public GetInvestigationsResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
