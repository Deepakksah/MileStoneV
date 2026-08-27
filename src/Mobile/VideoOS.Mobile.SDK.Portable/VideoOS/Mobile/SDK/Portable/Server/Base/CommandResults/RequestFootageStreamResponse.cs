using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class RequestFootageStreamResponse : BaseCommandResponse, IRequestFootageStreamResponse
{
	public Guid StreamId { get; private set; }

	public Guid ExportId { get; private set; }

	public RequestFootageStreamResponse(Command command)
		: base(command)
	{
		Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Output);
		if (dictionary.ContainsKey("StreamId"))
		{
			StreamId = new Guid(dictionary["StreamId"]);
		}
		if (dictionary.ContainsKey("ExportId"))
		{
			ExportId = new Guid(dictionary["ExportId"]);
		}
	}

	public RequestFootageStreamResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
