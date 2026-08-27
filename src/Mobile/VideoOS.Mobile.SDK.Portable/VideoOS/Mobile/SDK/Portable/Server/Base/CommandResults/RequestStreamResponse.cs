using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class RequestStreamResponse : BaseCommandResponse, IRequestStreamResponse
{
	public Guid VideoId { get; private set; }

	public Guid StreamId { get; private set; }

	public StreamParamsHelper.ByteOrder ByteOrder { get; private set; }

	public RequestStreamResponse(Command command, StreamParamsHelper.ByteOrder byteOrder = StreamParamsHelper.ByteOrder.LittleEndian)
		: base(command)
	{
		Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Output);
		if (dictionary.ContainsKey("VideoId"))
		{
			VideoId = new Guid(dictionary["VideoId"]);
		}
		if (dictionary.ContainsKey("StreamId"))
		{
			StreamId = new Guid(dictionary["StreamId"]);
		}
		ByteOrder = byteOrder;
		if (dictionary.ContainsKey("ByteOrder"))
		{
			ByteOrder = EnumsHelper.EnumConverter<StreamParamsHelper.ByteOrder>(dictionary["ByteOrder"]);
		}
	}

	public RequestStreamResponse(BaseCommandResponse other, StreamParamsHelper.ByteOrder byteOrder)
		: this(other.Command, byteOrder)
	{
	}

	public RequestStreamResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
