using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults.Processors;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.HTTP;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Video;

public abstract class LiveStream<TParams, TCapabilities, TControl> : Stream<TParams, TCapabilities, TControl>, IVideo, IVideoSub, IControl where TParams : StreamParams, new() where TCapabilities : StreamingDeviceCapabilities, new() where TControl : StreamControl<TParams>
{
	private IVideo _iVideo;

	private readonly string _alias;

	private bool _disposed;

	private bool _reconnectRequestDuringReconnect;

	public TParams StreamParams { get; }

	public OnVideoFrame NewFrame { get; set; }

	public LiveStream(BaseRequestStreamResponseDownload<TParams> response, IConnection iConnection, IOnVideoStream onVideoStream = null, string alias = "XProtectMobile/Video")
		: base((RequestStreamResponse)response, iConnection, onVideoStream, alias)
	{
		_disposed = false;
		_alias = alias;
		StreamParams = response.StreamParams;
		Initialize(response);
	}

	protected void OnSuccessChangeStream(BaseCommandResponse responseParams)
	{
		StreamParams.InitFromAnother(base.StreamControl.StreamParams);
	}

	public override void Initialize(RequestStreamResponse response)
	{
		base.Initialize(response);
		Disposing.DisposeMember(ref _iVideo);
		_iVideo = HttpStreamFactory.CreateStream(_iConnection, response, _alias);
		_iVideo.NewFrame = OnVideoFrame;
	}

	protected override void Reconnect(ReconnectSource source)
	{
		base.Reconnect(source);
		if (base.StateChanger.State == ConnectionStates.Connecting)
		{
			_reconnectRequestDuringReconnect = true;
		}
		else if (base.StateChanger.State != ConnectionStates.Connected)
		{
			base.StateChanger.ChangeState(ConnectionStates.Connecting);
			Command tCommand = new Command();
			tCommand.SetCommandKind(Command.CommandKinds.RequestStream);
			StreamParams.ConvertToCommand(ref tCommand);
			ExtendReconnectCommand(tCommand);
			(base.Connection as IConnectionInternal).SendCommandInternal(tCommand, OnReconnectSuccess, OnReconnectFail, new ReconnectStreamAsyncResult(base.Connection, this));
		}
	}

	public virtual void ExtendReconnectCommand(Command command)
	{
	}

	private void OnReconnectSuccess(BaseCommandResponse response)
	{
		base.StateChanger.ChangeState(ConnectionStates.Connected);
		ResetFailedFramesCounter();
		Start();
	}

	private void OnReconnectFail(BaseCommandResponse response)
	{
		base.StateChanger.ChangeState(ConnectionStates.Disconnected);
		if (_reconnectRequestDuringReconnect)
		{
			_reconnectRequestDuringReconnect = false;
			Reconnect(ReconnectSource.Internal);
		}
	}

	private void OnVideoFrame(VideoFrame frame)
	{
		if (!_disposed)
		{
			if (NewFrame != null)
			{
				NewFrame(frame);
			}
			ProcessNextFrame(frame);
			OnNewFrame(frame);
		}
	}

	protected virtual void OnNewFrame(VideoFrame frame)
	{
	}

	public void GetNextFrame()
	{
		if (!_disposed && !FailedFramesThersholdPassed())
		{
			(_iVideo as IVideoSub)?.GetNextFrame();
		}
	}

	protected override void Dispose(bool disposing, bool quiet)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				Disposing.DisposeMember(ref _iVideo);
			}
			_disposed = true;
		}
		base.Dispose(disposing, quiet);
	}

	public void Start()
	{
		(_iVideo as IControl)?.Start();
	}

	public void Stop()
	{
		(_iVideo as IControl)?.Stop();
	}

	private void ProcessNextFrame(VideoFrame frame)
	{
		ProcessFrame(frame != null);
	}

	protected override void OnFialedFrame()
	{
		base.OnFialedFrame();
		if (StreamParams != null && StreamParamsHelper.MethodType.Push == StreamParams.MethodType)
		{
			Start();
		}
	}
}
