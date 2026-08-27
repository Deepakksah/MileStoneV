using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class StartExportResponse : BaseCommandResponse
{
	public Guid ExportId { get; private set; }

	public StartExportResponse(Command command)
		: base(command)
	{
		Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Output);
		if (dictionary.ContainsKey("ExportId"))
		{
			ExportId = new Guid(dictionary["ExportId"]);
		}
	}

	public StartExportResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
