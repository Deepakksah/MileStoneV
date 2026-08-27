using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class CreateExportDownloadLinkResponse : BaseCommandResponse
{
	public string ExportLink { get; private set; }

	public CreateExportDownloadLinkResponse(Command command)
		: base(command)
	{
		Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Output);
		if (dictionary.ContainsKey("ExportLink"))
		{
			ExportLink = dictionary["ExportLink"];
		}
	}

	public CreateExportDownloadLinkResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
