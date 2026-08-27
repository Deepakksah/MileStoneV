using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class BaseRequestStreamResponsePlayback<T> : BaseRequestStreamResponseDownload<T> where T : StreamParams
{
	public PlaybackParams PlaybackParams { get; set; }

	public BaseRequestStreamResponsePlayback(Command command, StreamParamsHelper.ByteOrder byteOrder = StreamParamsHelper.ByteOrder.LittleEndian)
		: base(command, byteOrder)
	{
	}

	public BaseRequestStreamResponsePlayback(BaseCommandResponse other, StreamParamsHelper.ByteOrder byteOrder)
		: this(other.Command, byteOrder)
	{
	}

	public BaseRequestStreamResponsePlayback(BaseCommandResponse other)
		: this(other.Command, StreamParamsHelper.ByteOrder.LittleEndian)
	{
	}

	public BaseRequestStreamResponsePlayback(RequestStreamResponseDownload other, StreamParamsHelper.ByteOrder byteOrder)
		: base((BaseCommandResponse)other, byteOrder)
	{
	}

	public BaseRequestStreamResponsePlayback(RequestStreamResponseDownload other)
		: base((BaseCommandResponse)other)
	{
	}
}
