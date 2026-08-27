using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class CreateInvestigationResponse : BaseCommandResponse
{
	public Guid InvestigationId { get; private set; }

	public CreateInvestigationResponse(Command command)
		: base(command)
	{
		InvestigationId = Guid.Empty;
		Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Output);
		if (dictionary.ContainsKey("ItemId"))
		{
			InvestigationId = new Guid(dictionary["ItemId"]);
		}
	}

	public CreateInvestigationResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
