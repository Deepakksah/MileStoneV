using System;
using VideoOS.Mobile.Portable;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Platform;
using VideoOS.Platform.Live;

namespace VideoOS.Mobile.VideoSource;

public abstract class LiveStreamSource<TData> : IRsStream<TData>, IVideoSourceQuality, IControl, IRsControl, IDisposable
{
	private enum States
	{
		Stopped,
		GoingToLive,
		Live,
		GoingToStop
	}

	protected LiveSource LiveSource;

	protected readonly ConnectionQualityMeter QualityMeter;

	private uint _eventsSent;

	private readonly object _mutex;

	private bool _disposed;

	private States _currState;

	private LiveStatusEventArgs _lastReceivedEvents;

	public Item Item { get; private set; }

	public OnStreamData<TData> OnData { get; set; }

	public OnEventData OnEvent { get; set; }

	public OnFailCallback OnFail { get; set; }

	public bool IsPaused
	{
		get
		{
			return !LiveSource.LiveModeStart;
		}
		set
		{
			LiveSource.LiveModeStart = !value;
		}
	}

	public LiveStreamSource(Item item)
	{
		_mutex = new object();
		_currState = States.Stopped;
		Item = item;
		_eventsSent = 0u;
		CreateRsChannel();
		QualityMeter = new ConnectionQualityMeter(new TimeSpan(0, 0, 5));
		_disposed = false;
	}

	protected virtual void LiveSourceContentEventHandler(object sender, EventArgs e)
	{
	}

	public abstract bool CreateRsChannel();

	public bool InitRsChannel()
	{
		LiveSource?.Init();
		return LiveSource != null;
	}

	public virtual bool CloseRsChannel()
	{
		bool result = LiveSource != null;
		LiveSource?.Close();
		LiveSource = null;
		return result;
	}

	private void StartStreaming()
	{
		if (_currState == States.Stopped)
		{
			_currState = States.GoingToLive;
			LiveSource.LiveStatusEvent += LiveSourceStatusEventHandler;
			LiveSource.LiveContentEvent += LiveSourceContentEventHandler;
			LiveSource.LiveModeStart = true;
			_currState = States.Live;
		}
	}

	public virtual void Start()
	{
		lock (_mutex)
		{
			StartStreaming();
		}
	}

	public virtual void Stop()
	{
		lock (_mutex)
		{
			StopStreaming();
		}
	}

	private void StopStreaming()
	{
		if (_currState == States.Live)
		{
			_currState = States.GoingToStop;
			LiveSource.LiveModeStart = false;
			LiveSource.LiveStatusEvent -= LiveSourceStatusEventHandler;
			LiveSource.LiveContentEvent -= LiveSourceContentEventHandler;
			_currState = States.Stopped;
		}
	}

	protected virtual void LiveSourceStatusEventHandler(object sender, EventArgs e)
	{
		if (e is LiveStatusEventArgs e2)
		{
			if (OnEvent != null && (e2.ChangedStatusFlags != StatusFlags.None || _eventsSent < 5))
			{
				OnEvent(e2);
				_eventsSent++;
			}
			if (IsChangedAndSetFlag(e2, StatusFlags.CameraConnectionLost) || IsChangedAndSetFlag(e2, StatusFlags.ClientLiveStopped) || IsChangedAndSetFlag(e2, StatusFlags.DatabaseFail))
			{
				OnFail?.Invoke(e2);
			}
		}
	}

	private LiveStatusEventArgs CreateConnectionLostArgs()
	{
		return new LiveStatusEventArgs
		{
			ChangedStatusFlags = StatusFlags.CameraConnectionLost,
			CurrentStatusFlags = StatusFlags.CameraConnectionLost
		};
	}

	protected void LiveSourceContentExceptionEvent(object sender, Exception e)
	{
		LiveStatusEventArgs e2 = CreateConnectionLostArgs();
		LiveSourceStatusEventHandler(sender, e2);
	}

	private void PrintStatusEvents(LiveStatusEventArgs args)
	{
		if ((args.ChangedStatusFlags & StatusFlags.Motion) != StatusFlags.None)
		{
			_ = args.CurrentStatusFlags & StatusFlags.Motion;
		}
		if ((args.ChangedStatusFlags & StatusFlags.Recording) != StatusFlags.None)
		{
			_ = args.CurrentStatusFlags & StatusFlags.Recording;
		}
		if (_lastReceivedEvents != null && _lastReceivedEvents.CurrentStatusFlags != args.CurrentStatusFlags && args.ChangedStatusFlags == StatusFlags.None)
		{
			Log.Error("+-+-+-+-+-+-+-+-+-+-+-+-+", "Inconsistent settings !!!");
		}
		_lastReceivedEvents = args;
	}

	private bool IsChangedAndSetFlag(LiveStatusEventArgs args, StatusFlags flag)
	{
		if ((args.ChangedStatusFlags & flag) != StatusFlags.None)
		{
			return (args.CurrentStatusFlags & flag) != 0;
		}
		return false;
	}

	public ConnectionQuality GetInputConnectionQuality()
	{
		return QualityMeter.Quality;
	}

	public Camera GetCameraParams()
	{
		return new Camera
		{
			CameraId = Item.FQID.ObjectId,
			CameraName = Item.Name.SanitizedCustom("<>'"),
			RecordingServerId = Item.FQID.ServerId.Id,
			RecordingServerName = Item.FQID.ServerId.ServerHostname
		};
	}

	~LiveStreamSource()
	{
		Dispose(disposing: true);
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
			if (disposing)
			{
				Stop();
				CloseRsChannel();
			}
			_disposed = true;
		}
	}
}
