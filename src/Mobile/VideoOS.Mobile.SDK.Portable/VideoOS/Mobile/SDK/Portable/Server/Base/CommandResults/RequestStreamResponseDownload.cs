using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class RequestStreamResponseDownload : BaseRequestStreamResponseDownload<VideoParams>
{
	public RequestStreamResponseDownload(Command command, StreamParamsHelper.ByteOrder byteOrder = StreamParamsHelper.ByteOrder.LittleEndian)
		: base(command, byteOrder)
	{
	}

	public RequestStreamResponseDownload(BaseCommandResponse other, StreamParamsHelper.ByteOrder byteOrder)
		: base(other.Command, byteOrder)
	{
	}

	public RequestStreamResponseDownload(BaseCommandResponse other)
		: base(other.Command, StreamParamsHelper.ByteOrder.LittleEndian)
	{
	}

	public RequestStreamResponseDownload(RequestStreamResponseDownload other, StreamParamsHelper.ByteOrder byteOrder)
		: base((BaseRequestStreamResponseDownload<VideoParams>)other, byteOrder)
	{
	}

	public RequestStreamResponseDownload(RequestStreamResponseDownload other)
		: base((BaseRequestStreamResponseDownload<VideoParams>)other)
	{
	}
}
