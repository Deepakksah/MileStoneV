using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class RequestStreamResponseLive : RequestStreamResponseDownload
{
	public RequestStreamResponseLive(Command command, StreamParamsHelper.ByteOrder byteOrder = StreamParamsHelper.ByteOrder.LittleEndian)
		: base(command, byteOrder)
	{
	}

	public RequestStreamResponseLive(BaseCommandResponse other, StreamParamsHelper.ByteOrder byteOrder)
		: this(other.Command, byteOrder)
	{
	}

	public RequestStreamResponseLive(BaseCommandResponse other)
		: this(other.Command)
	{
	}

	public RequestStreamResponseLive(RequestStreamResponseDownload other, StreamParamsHelper.ByteOrder byteOrder)
		: base(other, byteOrder)
	{
	}

	public RequestStreamResponseLive(RequestStreamResponseDownload other)
		: base(other)
	{
	}
}
