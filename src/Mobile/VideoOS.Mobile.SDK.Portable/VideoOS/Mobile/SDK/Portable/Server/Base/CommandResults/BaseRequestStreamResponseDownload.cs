using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class BaseRequestStreamResponseDownload<T> : RequestStreamResponse, IBaseRequestStreamResponseDownload<T> where T : StreamParams
{
	public int SourceWidth { get; private set; }

	public int SourceHeight { get; private set; }

	public T StreamParams { get; set; }

	public BaseRequestStreamResponseDownload(Command command, StreamParamsHelper.ByteOrder byteOrder = StreamParamsHelper.ByteOrder.LittleEndian)
		: base(command, byteOrder)
	{
		Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Output);
		if (dictionary.ContainsKey("SrcWidth"))
		{
			SourceWidth = Convert.ToInt32(dictionary["SrcWidth"]);
		}
		if (dictionary.ContainsKey("SrcHeight"))
		{
			SourceHeight = Convert.ToInt32(dictionary["SrcHeight"]);
		}
	}

	public BaseRequestStreamResponseDownload(BaseCommandResponse other, StreamParamsHelper.ByteOrder byteOrder)
		: this(other.Command, byteOrder)
	{
	}

	public BaseRequestStreamResponseDownload(BaseCommandResponse other)
		: this(other.Command, StreamParamsHelper.ByteOrder.LittleEndian)
	{
	}

	public BaseRequestStreamResponseDownload(BaseRequestStreamResponseDownload<T> other, StreamParamsHelper.ByteOrder byteOrder)
		: this((BaseCommandResponse)other, byteOrder)
	{
		StreamParams = other.StreamParams;
	}

	public BaseRequestStreamResponseDownload(BaseRequestStreamResponseDownload<T> other)
		: this((BaseCommandResponse)other)
	{
		StreamParams = other.StreamParams;
	}
}
