using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetExportResponse : BaseCommandResponse
{
	public IEnumerable<Export> Exports { get; private set; }

	public GetExportResponse(Command command)
		: base(command)
	{
		Exports = command.Exports?.ExportsList;
	}

	public GetExportResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
