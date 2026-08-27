using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class RequestStreamResponsePlayback : BaseRequestStreamResponsePlayback<VideoParams>
{
	public RequestStreamResponsePlayback(Command command, StreamParamsHelper.ByteOrder byteOrder = StreamParamsHelper.ByteOrder.LittleEndian)
		: base(command, byteOrder)
	{
	}

	public RequestStreamResponsePlayback(BaseCommandResponse other, StreamParamsHelper.ByteOrder byteOrder)
		: this(other.Command, byteOrder)
	{
	}

	public RequestStreamResponsePlayback(BaseCommandResponse other)
		: this(other.Command)
	{
	}

	public RequestStreamResponsePlayback(RequestStreamResponseDownload other, StreamParamsHelper.ByteOrder byteOrder)
		: base(other, byteOrder)
	{
	}

	public RequestStreamResponsePlayback(RequestStreamResponseDownload other)
		: base(other)
	{
	}
}
