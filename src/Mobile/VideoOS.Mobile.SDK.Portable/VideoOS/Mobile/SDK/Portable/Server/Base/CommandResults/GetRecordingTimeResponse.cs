using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetRecordingTimeResponse : BaseCommandResponse
{
	public DateTime RecordingTime { get; private set; }

	public GetRecordingTimeResponse(Command command)
		: base(command)
	{
		Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Output);
		if (dictionary.ContainsKey("Time"))
		{
			RecordingTime = TimeConverter.FromString(dictionary["Time"]);
		}
	}

	public GetRecordingTimeResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
