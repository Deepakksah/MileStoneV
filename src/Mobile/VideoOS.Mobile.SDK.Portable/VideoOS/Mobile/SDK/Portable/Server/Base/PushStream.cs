using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;
using VideoOS.Mobile.SDK.Portable.Server.HTTP;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public abstract class PushStream<TParams, TCapab, TControll, TFrame> : Stream<TParams, TCapab, TControll>, IPushStream<TFrame>, IPushVideoFail, IVideoUpload where TParams : StreamParams, new() where TCapab : StreamingDeviceCapabilities, new() where TControll : StreamControl<TParams> where TFrame : PushStreamData, new()
{
	public enum QueueProcessing
	{
		Sync,
		Async
	}

	private bool _disposed;

	private CycleTaskMember _task;

	private readonly IConnection _connection;

	private readonly string _alias;

	private IPushVideoFail _pushStreamFail;

	private IPushVideoSuccess _pushStreamSuccess;

	protected readonly object _locker = new object();

	protected IPushStreamSub<TFrame> _pushStreamSub;

	protected readonly Queue<TFrame> _queue = new Queue<TFrame>();

	public long TotalFailCount
	{
		get
		{
			if (_pushStreamFail != null)
			{
				return _pushStreamFail.TotalFailCount;
			}
			return 0L;
		}
	}

	public OnFrameCompletion FailCallback { get; set; }

	public OnFrameCompletion SuccessCallback { get; set; }

	public QueueProcessing QueueProcessingType { get; set; }

	public PushStream(RequestStreamResponseUpload response, IConnection iConnection, IOnVideoStream onVideoStream = null, string alias = "XProtectMobile/Video")
		: base((RequestStreamResponse)response, iConnection, onVideoStream, alias)
	{
		_disposed = false;
		_connection = iConnection;
		_alias = alias;
		Initialize(response);
		_task = new CycleTaskMember((Action)RunningThread, (Action)null, (Action)null, 10);
		_task.StartTask();
		QueueProcessingType = QueueProcessing.Sync;
		base.StateChanger.ChangeState(ConnectionStates.Connected);
	}

	public void Initialize(RequestStreamResponseUpload response)
	{
		base.Initialize(response);
		DenitPushStreamFail();
		DenitPushStreamSuccess();
		Disposing.DisposeMember(ref _pushStreamSub);
		IConnectionEndPoint connectionEndPoint = _connection as IConnectionEndPoint;
		switch (connectionEndPoint?.ChannelType)
		{
		case ChannelTypes.HTTP:
			_pushStreamSub = new HttpVideoPush<TFrame>(response, _alias, connectionEndPoint);
			break;
		case ChannelTypes.HTTPSecure:
			_pushStreamSub = new HttpSecureVideoPush<TFrame>(response, _alias, connectionEndPoint);
			break;
		default:
			throw new NotImplementedException("PushStream - channel type not implemented");
		}
		InitPushStreamFail(_pushStreamSub);
		InitPushStreamSuccess(_pushStreamSub);
	}

	public virtual void EnqueueFrame(TFrame frame)
	{
		lock (_locker)
		{
			_queue.Enqueue(frame);
		}
	}

	protected virtual void RunningThread()
	{
		try
		{
			if (FailedFramesThersholdPassed())
			{
				return;
			}
			TFrame val = null;
			lock (_locker)
			{
				if (_queue.Count > 0)
				{
					val = _queue.Dequeue();
				}
			}
			if (val != null)
			{
				_pushStreamSub.SendFrame(val, QueueProcessingType == QueueProcessing.Sync);
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "PushStream.RunningThread", ex.Message);
		}
	}

	protected override void Dispose(bool disposing, bool quiet)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				Disposing.DisposeMember(ref _task);
				Disposing.DisposeMember(ref _pushStreamSub);
			}
			_disposed = true;
		}
		base.Dispose(disposing, quiet);
	}

	private void InitPushStreamFail(IPushStreamSub<TFrame> iPushVideoSub)
	{
		_pushStreamFail = iPushVideoSub as IPushVideoFail;
		if (_pushStreamFail != null)
		{
			_pushStreamFail.FailCallback = FailCallbackInternal;
		}
	}

	private void FailCallbackInternal(int consecutiveFailCount)
	{
		if (FailCallback != null)
		{
			FailCallback(consecutiveFailCount);
		}
		ProcessFrame(success: false);
	}

	private void DenitPushStreamFail()
	{
		if (_pushStreamFail != null)
		{
			_pushStreamFail.FailCallback = null;
		}
		_pushStreamFail = null;
	}

	private void InitPushStreamSuccess(IPushStreamSub<TFrame> iPushVideoSub)
	{
		_pushStreamSuccess = iPushVideoSub as IPushVideoSuccess;
		if (_pushStreamSuccess != null)
		{
			_pushStreamSuccess.SuccessCallback = SuccessCallbackInternal;
		}
	}

	private void SuccessCallbackInternal(int consecutiveFailCount)
	{
		if (SuccessCallback != null)
		{
			SuccessCallback(consecutiveFailCount);
		}
		ProcessFrame(success: true);
	}

	private void DenitPushStreamSuccess()
	{
		if (_pushStreamSuccess != null)
		{
			_pushStreamSuccess.SuccessCallback = null;
		}
		_pushStreamSuccess = null;
	}
}
