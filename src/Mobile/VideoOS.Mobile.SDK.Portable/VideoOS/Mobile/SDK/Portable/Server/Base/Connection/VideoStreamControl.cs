using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities.Rects;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class VideoStreamControl : VideoControl, IVideoStreamControlAsync, IVideoStreamControlSync
{
	public VideoStreamControl(IConnection connection, Guid videoId)
		: base(connection, videoId, null)
	{
	}

	public VideoStreamControl(IConnection connection, Guid videoId, OnResponse<BaseCommandResponse> onSuccessChangeStream)
		: base(connection, videoId, null)
	{
	}

	private IAsyncResult SendChangeStreamCommand(Action<VideoParams> onParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.ChangeStream);
		VideoParams videoParams = new VideoParams();
		onParams?.Invoke(videoParams);
		videoParams.VideoId = _videoId;
		videoParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public IAsyncResult Rescale(int width, int height, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		return SendChangeStreamCommand(delegate(VideoParams videoParams)
		{
			videoParams.DestWidth = width;
			videoParams.DestHeight = height;
		}, onSuccess, onFail);
	}

	public IAsyncResult Crop(int left, int top, int right, int bottom, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		return SendChangeStreamCommand(delegate(VideoParams videoParams)
		{
			videoParams.SrcRect = new IntRect(left, top, right, bottom);
		}, onSuccess, onFail);
	}

	public BaseCommandResponse Rescale(int width, int height, TimeSpan timeout)
	{
		IAsyncResult iResult = Rescale(width, height);
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse Crop(int left, int top, int right, int bottom, TimeSpan timeout)
	{
		IAsyncResult iResult = Crop(left, top, right, bottom);
		return _connection.ProcessResult(iResult, timeout);
	}
}
