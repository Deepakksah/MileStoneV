using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class CheckForChangesResponse : BaseCommandResponse
{
	public string InvestigationsChangesStateId { get; private set; }

	public CheckForChangesResponse(Command command)
		: base(command)
	{
		Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Output);
		InvestigationsChangesStateId = (dictionary.ContainsKey("InvestigationsChangesStateId") ? dictionary["InvestigationsChangesStateId"] : null);
	}

	public CheckForChangesResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
