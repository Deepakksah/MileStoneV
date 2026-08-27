using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class RequestPlaybackControllerResponse : BaseCommandResponse, IRequestStreamResponse
{
	public Guid VideoId { get; private set; }

	public StreamParamsHelper.ByteOrder ByteOrder { get; private set; }

	public RequestPlaybackControllerResponse(Command tCommand, StreamParamsHelper.ByteOrder byteOrder = StreamParamsHelper.ByteOrder.LittleEndian)
		: base(tCommand)
	{
		Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Output);
		if (dictionary.ContainsKey("PlaybackControllerId"))
		{
			VideoId = new Guid(dictionary["PlaybackControllerId"]);
		}
		ByteOrder = byteOrder;
		if (dictionary.ContainsKey("ByteOrder"))
		{
			ByteOrder = EnumsHelper.EnumConverter<StreamParamsHelper.ByteOrder>(dictionary["ByteOrder"]);
		}
	}

	public RequestPlaybackControllerResponse(BaseCommandResponse other, StreamParamsHelper.ByteOrder byteOrder)
		: this(other.Command, byteOrder)
	{
	}

	public RequestPlaybackControllerResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
