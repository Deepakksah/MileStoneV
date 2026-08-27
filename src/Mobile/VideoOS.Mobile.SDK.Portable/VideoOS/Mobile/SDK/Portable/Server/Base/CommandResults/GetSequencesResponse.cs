using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetSequencesResponse : BaseCommandResponse
{
	public IEnumerable<Sequence> Sequences { get; private set; }

	public GetSequencesResponse(Command command)
		: base(command)
	{
		Sequences = command.Sequences?.SequenceList;
	}

	public GetSequencesResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
