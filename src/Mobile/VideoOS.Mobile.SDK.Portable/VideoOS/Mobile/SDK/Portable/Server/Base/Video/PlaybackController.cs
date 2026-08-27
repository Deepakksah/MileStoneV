using System;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.HTTP;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Video;

public class PlaybackController : IErrorResult, IVideo, IVideoSub, IControl
{
	private readonly RequestPlaybackControllerResponse _response;

	private readonly string _alias;

	private readonly IConnection _iConnection;

	private readonly IErrorResult _iErrorResult;

	private readonly IConnectionAsync _iAsyncMode;

	private readonly IConnectionSync _iSyncMode;

	private IVideo _iVideo;

	private bool _disposed;

	public PlaybackControllerCommands PlaybackControl { get; }

	public Command.CommandResultTypes Result => _iErrorResult.Result;

	public string ErrorMessage => _iErrorResult.ErrorMessage;

	public ErrorCodes ErrorCode => _iErrorResult.ErrorCode;

	public OnVideoFrame NewFrame { get; set; }

	public Guid ControllerId { get; }

	public PlaybackController(RequestPlaybackControllerResponse response, IConnection iConnection, string alias = "XProtectMobile/Video")
	{
		_response = response;
		ControllerId = response.VideoId;
		_alias = alias;
		_iConnection = iConnection;
		_iErrorResult = _iConnection as IErrorResult;
		_iAsyncMode = _iConnection as IConnectionAsync;
		_iSyncMode = _iConnection as IConnectionSync;
		Initialize(response);
		PlaybackControl = new PlaybackControllerCommands(iConnection, (iConnection as VideoOS.Mobile.SDK.Portable.Server.Base.Connection.Connection)?.Video, ControllerId);
	}

	public void Initialize(RequestPlaybackControllerResponse response)
	{
		Disposing.DisposeMember(ref _iVideo);
		_iVideo = HttpStreamFactory.CreateStream(_iConnection, response, _alias);
		_iVideo.NewFrame = OnVideoFrame;
	}

	private void OnVideoFrame(VideoFrame frame)
	{
		if (!_disposed && NewFrame != null)
		{
			NewFrame(frame);
		}
	}

	public void GetNextFrame()
	{
		if (!_disposed && _iVideo is IVideoSub videoSub)
		{
			videoSub.GetNextFrame();
		}
	}

	~PlaybackController()
	{
		Dispose();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			Stop();
			if (disposing)
			{
				Disposing.DisposeMember(ref _iVideo);
			}
			_disposed = true;
		}
	}

	public void Start()
	{
		if (_iVideo is IControl control)
		{
			control.Start();
		}
	}

	public void Stop()
	{
		if (_iVideo is IControl control)
		{
			control.Stop();
		}
	}
}
