using System;

namespace VideoOS.Platform.Client;

public abstract class PlaybackController
{
	public enum PlaybackModeType
	{
		Forward,
		Stop,
		Reverse,
		Custom
	}

	public delegate void BrowseGotoChangedEventHandler(object sender, TimeEventArgs e);

	public delegate void PlaybackModeChangedEventHandler(object sender, PlaybackModeEventArgs e);

	public delegate void BrowseTimeChangedEventHandler(object sender, TimeEventArgs e);

	public class TimeEventArgs : EventArgs
	{
		protected DateTime _time;

		public virtual DateTime Time
		{
			get
			{
				return _time;
			}
			set
			{
				_time = value;
			}
		}

		public TimeEventArgs(DateTime time)
		{
			_time = time;
		}
	}

	public class PlaybackModeEventArgs : EventArgs
	{
		protected PlaybackModeType _playbackMode;

		public virtual PlaybackModeType PlaybackMode
		{
			get
			{
				return _playbackMode;
			}
			set
			{
				_playbackMode = value;
			}
		}

		public PlaybackModeEventArgs(PlaybackModeType playbackMode)
		{
			_playbackMode = playbackMode;
		}
	}

	public class PlaybackSpeedEventArgs : EventArgs
	{
		protected double _speed;

		public virtual double Speed
		{
			get
			{
				return _speed;
			}
			set
			{
				_speed = value;
			}
		}

		public PlaybackSpeedEventArgs(double speed)
		{
			_speed = speed;
		}
	}

	public class ProgressChangedEventArgs : EventArgs
	{
		public float Progress { get; private set; }

		public ProgressChangedEventArgs(float progress)
		{
			Progress = progress;
		}
	}

	protected class BrowseTimeInfo
	{
		private Guid _deviceId;

		private DateTime _previousTime;

		private DateTime _currentTime;

		private DateTime _nextTime;

		public Guid DeviceId
		{
			get
			{
				return _deviceId;
			}
			set
			{
				_deviceId = value;
			}
		}

		public DateTime PreviousTime
		{
			get
			{
				return _previousTime;
			}
			set
			{
				_previousTime = value;
			}
		}

		public DateTime CurrentTime
		{
			get
			{
				return _currentTime;
			}
			set
			{
				_currentTime = value;
			}
		}

		public DateTime NextTime
		{
			get
			{
				return _nextTime;
			}
			set
			{
				_nextTime = value;
			}
		}

		public BrowseTimeInfo(Guid deviceId, DateTime previousTime, DateTime currentTime, DateTime nextTime)
		{
			_deviceId = deviceId;
			_previousTime = previousTime;
			_currentTime = currentTime;
			_nextTime = nextTime;
		}
	}

	public virtual FQID FQID { get; set; }

	public virtual PlaybackModeType PlaybackMode { get; set; }

	public virtual DateTime PlaybackTime { get; set; }

	public virtual double PlaybackSpeed { get; set; }

	public virtual bool SkipGaps { get; set; }

	public virtual event EventHandler<ProgressChangedEventArgs> SequenceProgressChanged;

	public virtual event EventHandler<TimeEventArgs> PlaybackTimeChangedEvent;

	public virtual event EventHandler<PlaybackModeEventArgs> PlaybackModeChangedEvent;

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	public virtual void SetSequence(DateTime startTime, DateTime endTime)
	{
	}

	protected void OnPlaybackModeChangedEvent(PlaybackModeType playbackMode)
	{
		this.PlaybackModeChangedEvent?.Invoke(this, new PlaybackModeEventArgs(playbackMode));
	}

	protected void OnPlaybackTimeChanged(DateTime playbackTime)
	{
		this.PlaybackTimeChangedEvent?.Invoke(this, new TimeEventArgs(playbackTime));
	}

	protected void OnSequenceProgressChanged(ProgressChangedEventArgs e)
	{
		EventHandler<ProgressChangedEventArgs> eventHandler = this.SequenceProgressChanged;
		if (eventHandler != null)
		{
			try
			{
				eventHandler(this, e);
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("PlaybackController::SequenceProgressChanged", exception);
			}
		}
	}
}
