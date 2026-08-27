using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults.Processors;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Video;

public class PushVideo : PushStream<VideoParams, CameraCapabilities, VideoStreamControl, PushVideoFrame>, IBookmarkCreateAsync, IBookmarkCreateSync, IPushVideo, IPushVideoCounter
{
	private bool _disposed;

	private int _maxQueueSize = 3;

	public IFpsMeter FpsMeter => (_pushStreamSub as IPushVideoCounter).FpsMeter;

	public int QueueSize
	{
		get
		{
			lock (_locker)
			{
				return _queue.Count;
			}
		}
	}

	public int MaxQueueSize
	{
		get
		{
			return _maxQueueSize;
		}
		set
		{
			_maxQueueSize = value;
			lock (_locker)
			{
				while (_queue.Count > _maxQueueSize)
				{
					_queue.Dequeue();
				}
			}
		}
	}

	public IAsyncResult CreateBookmark(BookmarkParams bookmarkParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.CreateBookmark);
		bookmarkParams.VideoId = base.StreamId;
		bookmarkParams.ConvertToCommand(ref tCommand);
		return base.Connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public BaseCommandResponse CreateBookmark(BookmarkParams bookmarkParams, TimeSpan timeout)
	{
		IAsyncResult iResult = CreateBookmark(bookmarkParams);
		return base.Connection.ProcessResult(iResult, timeout);
	}

	public PushVideo(RequestStreamResponseUpload response, IConnection iConnection, IOnVideoStream onVideoStream = null, string alias = "XProtectMobile/Video")
		: base(response, iConnection, onVideoStream, alias)
	{
		_disposed = false;
	}

	public override VideoStreamControl CreateStreamControl(IConnection connection, Guid streamId)
	{
		return new VideoStreamControl(connection, streamId, null);
	}

	protected override void Dispose(bool disposing, bool quiet)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				_queue.Clear();
			}
			_disposed = true;
		}
		base.Dispose(disposing, quiet);
	}

	public override void EnqueueFrame(PushVideoFrame frame)
	{
		lock (_locker)
		{
			_queue.Enqueue(frame);
			if (_queue.Count > _maxQueueSize)
			{
				_queue.Dequeue();
			}
		}
	}

	protected override void Reconnect(ReconnectSource source)
	{
		base.Reconnect(source);
		lock (_locker)
		{
			_queue.Clear();
		}
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.RequestStream);
		command.InputParams.Add(new Parameter("SignalType", EnumsHelper.GetDescription(StreamParamsHelper.SignalType.Upload)));
		(base.Connection as IConnectionInternal).SendCommandInternal(command, OnReconnectSuccess, null, new ReconnectStreamAsyncResult(base.Connection, this));
	}

	private void OnReconnectSuccess(BaseCommandResponse response)
	{
		base.StateChanger.ChangeState(ConnectionStates.Connected);
		ResetFailedFramesCounter();
	}
}
