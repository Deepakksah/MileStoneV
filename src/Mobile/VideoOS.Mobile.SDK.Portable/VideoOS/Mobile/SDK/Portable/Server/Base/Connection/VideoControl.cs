using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class VideoControl : StreamControl<VideoParams>
{
	public VideoParams VideoParams => base.StreamParams;

	public VideoControl(IConnection connection)
		: this(connection, Guid.Empty, null)
	{
	}

	public VideoControl(IConnection connection, Guid videoId, OnResponse<BaseCommandResponse> onSuccessChangeStream)
		: base(connection, videoId, onSuccessChangeStream)
	{
	}

	public IAsyncResult ControlCarousel(CarouselParams carouselParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.ControlCarousel);
		carouselParams.ConvertToCommand(ref command);
		return _connection.SendCommand(command, onSuccess, onFail);
	}

	public BaseCommandResponse ControlCarousel(CarouselParams carouselParams, TimeSpan timeout)
	{
		IAsyncResult iResult = ControlCarousel(carouselParams);
		return _connection.ProcessResult(iResult, timeout);
	}

	public async Task<BaseCommandResponse> ControlCarouselAsync(CarouselParams carouselParams, TimeSpan timeout)
	{
		IAsyncResult asyncResult = ControlCarousel(carouselParams);
		return await _connection.ProcessResultTask(asyncResult, timeout);
	}
}
