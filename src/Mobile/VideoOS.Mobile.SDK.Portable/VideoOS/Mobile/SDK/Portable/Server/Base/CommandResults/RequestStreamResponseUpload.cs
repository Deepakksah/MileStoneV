using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class RequestStreamResponseUpload : RequestStreamResponse
{
	public RequestStreamResponseUpload(Command command, StreamParamsHelper.ByteOrder byteOrder = StreamParamsHelper.ByteOrder.LittleEndian)
		: base(command, byteOrder)
	{
	}

	public RequestStreamResponseUpload(BaseCommandResponse other, StreamParamsHelper.ByteOrder byteOrder)
		: this(other.Command, byteOrder)
	{
	}

	public RequestStreamResponseUpload(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
