using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetPtzPresetsResponse : BaseCommandResponse
{
	public List<string> Presets { get; private set; }

	public GetPtzPresetsResponse(Command command)
		: base(command)
	{
		Presets = new List<string>();
		foreach (Parameter item in command.OutputParams.Where((Parameter x) => x.ParamName.StartsWith("PtzPreset")))
		{
			Presets.Add(item.ParamValue);
		}
	}

	public GetPtzPresetsResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
