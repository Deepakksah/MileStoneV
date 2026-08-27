using System;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public abstract class Stream<TParams, TCapabilities, TControl> : IDisposable, IDisposableQuiet, IErrorResult, IBaseStream, IStreamReconnection where TParams : StreamParams, new() where TCapabilities : StreamingDeviceCapabilities, new() where TControl : StreamControl<TParams>
{
	protected enum ReconnectSource
	{
		Internal,
		External
	}

	private RequestStreamResponse _reqStreamResponse;

	private readonly IErrorResult _iErrorResult;

	private readonly IOnVideoStream _iOnVideoStream;

	private bool _disposed;

	private readonly StateChanger _stateChanger = new StateChanger();

	protected readonly IConnection _iConnection;

	private readonly TCapabilities _deviceCapabilities = new TCapabilities();

	private const int FailedFramesTrigger = 3;

	private int _consecutiveFailedFrames;

	protected IStateChangeInternal StateChanger => _stateChanger;

	public IStateChange StateChange => _stateChanger;

	public ICapabilities Capabilities => _deviceCapabilities;

	public TCapabilities DeviceCapabilities => _deviceCapabilities;

	public TControl StreamControl { get; }

	public Command.CommandResultTypes Result => _iErrorResult.Result;

	public string ErrorMessage => _iErrorResult.ErrorMessage;

	public ErrorCodes ErrorCode => _iErrorResult.ErrorCode;

	public Guid StreamId => _reqStreamResponse.VideoId;

	public IConnection Connection => _iConnection;

	public Stream(RequestStreamResponse response, IConnection iConnection, IOnVideoStream onVideoStream = null, string alias = "XProtectMobile/Video")
	{
		_disposed = false;
		_iConnection = iConnection;
		_iErrorResult = _iConnection as IErrorResult;
		_iOnVideoStream = onVideoStream;
		Initialize(response);
		RegisterCreation();
		StreamControl = CreateStreamControl(iConnection, response.VideoId);
	}

	public abstract TControl CreateStreamControl(IConnection connection, Guid streamId);

	public virtual void Initialize(RequestStreamResponse response)
	{
		_reqStreamResponse = response;
		_deviceCapabilities.InitFromCommand(response.OutputParams);
	}

	private void RegisterCreation()
	{
		_iOnVideoStream?.CreatedVideo(this);
	}

	private void RegisterDestruction()
	{
		_iOnVideoStream?.DestroyedVideo(this);
	}

	public void Reconnect()
	{
		Reconnect(ReconnectSource.External);
	}

	protected virtual void Reconnect(ReconnectSource source)
	{
		Logger.Instance.Log(LogContent.Debug, "BaseVideo.Reconnect()", "Reconnect has been trigered !" + source);
	}

	~Stream()
	{
		Dispose();
	}

	public void Dispose()
	{
		DisposeInternal(disposing: true, quiet: false);
	}

	public void DisposeQuiet()
	{
		DisposeInternal(disposing: true, quiet: true);
	}

	private void DisposeInternal(bool disposing, bool quiet)
	{
		Dispose(disposing, quiet);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing, bool quiet)
	{
		if (_disposed)
		{
			return;
		}
		if (disposing)
		{
			if (!quiet)
			{
				CloseStream();
			}
			RegisterDestruction();
		}
		_disposed = true;
	}

	private bool CloseStream(TimeSpan timeout)
	{
		IAsyncResult asyncResult = CloseStream();
		_iConnection.ProcessResult(asyncResult, timeout);
		return asyncResult.IsCompleted;
	}

	private IAsyncResult CloseStream()
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.CloseStream);
		command.InputParams.Add(new Parameter("VideoId", _reqStreamResponse.VideoId.ToString()));
		return _iConnection.SendCommand(command);
	}

	protected void ResetFailedFramesCounter()
	{
		_consecutiveFailedFrames = 0;
	}

	protected void IncrementFailedFramesCounter()
	{
		_consecutiveFailedFrames++;
	}

	protected bool FailedFramesThersholdReached()
	{
		return _consecutiveFailedFrames == 3;
	}

	protected bool FailedFramesThersholdPassed()
	{
		return _consecutiveFailedFrames >= 3;
	}

	protected void ProcessFrame(bool success)
	{
		if (!success)
		{
			if (StateChanger.State != ConnectionStates.Disconnected)
			{
				StateChanger.ChangeState(ConnectionStates.Disconnecting);
			}
			IncrementFailedFramesCounter();
			if (FailedFramesThersholdReached())
			{
				StateChanger.ChangeState(ConnectionStates.Disconnected);
				Reconnect(ReconnectSource.Internal);
			}
			else
			{
				OnFialedFrame();
			}
		}
		else
		{
			StateChanger.ChangeState(ConnectionStates.Connected);
			ResetFailedFramesCounter();
		}
	}

	protected virtual void OnFialedFrame()
	{
	}
}
