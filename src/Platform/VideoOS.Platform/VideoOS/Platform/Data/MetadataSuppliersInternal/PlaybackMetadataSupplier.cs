using System;
using System.Collections.Generic;
using System.Threading;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.Data.MetadataSuppliersInternal;

public class PlaybackMetadataSupplier : InternalMetadataSupplier
{
	private readonly Queue<DateTime> _timeQueue = new Queue<DateTime>();

	private readonly object _queueLocker = new object();

	private Thread _fetchThread;

	private bool _stopFetchThread;

	private MetadataPlaybackSource _metadataSource;

	private DateTime _lastPresentedTimeUtc = DateTime.MinValue;

	private object _playbackTimeChangedHandler;

	private WindowInformation _windowInformation;

	private FQID _playbackControllerFqid;

	private const int MaximalQueueLength = 3;

	private const double DataTimeoutStoppedSeconds = 0.1;

	public PlaybackMetadataSupplier(Item metadataSourceItem, WindowInformation windowInformation)
		: base(metadataSourceItem)
	{
		_windowInformation = windowInformation;
	}

	public override bool Start()
	{
		bool result = Stop();
		try
		{
			if (_windowInformation != null)
			{
				_playbackControllerFqid = ClientControl.Instance.GetPlaybackController(_windowInformation);
			}
			_playbackTimeChangedHandler = EnvironmentManager.Instance.RegisterReceiver(PlaybackTimeChangedHandler, new MessageIdFilter("SmartClient.PlaybackCurrentTimeIndication"));
			_metadataSource = new MetadataPlaybackSource(_metadataSourceItem);
			_metadataSource.Init();
			ClearQueue();
			_stopFetchThread = false;
			_fetchThread = new Thread(MetadataFetchThread);
			_fetchThread.Start();
		}
		catch (Exception)
		{
			CloseMetaDataSource();
			result = false;
		}
		return result;
	}

	public override bool Stop()
	{
		bool result = true;
		try
		{
			_stopFetchThread = true;
			if (_playbackTimeChangedHandler != null)
			{
				EnvironmentManager.Instance.UnRegisterReceiver(_playbackTimeChangedHandler);
				_playbackTimeChangedHandler = null;
			}
			_playbackControllerFqid = null;
			ClearQueue();
		}
		catch (Exception)
		{
			result = false;
		}
		CloseMetaDataSource();
		return result;
	}

	private void CloseMetaDataSource()
	{
		if (_metadataSource != null)
		{
			_metadataSource.Close();
			_metadataSource = null;
		}
	}

	private bool IsForMe(FQID sender)
	{
		if (null == _playbackControllerFqid)
		{
			if (null == sender || sender.ObjectId == Guid.Empty)
			{
				return true;
			}
			return false;
		}
		return _playbackControllerFqid.EqualGuids(sender);
	}

	private object PlaybackTimeChangedHandler(Message message, FQID dest, FQID sender)
	{
		if (IsForMe(sender))
		{
			DateTime time = (DateTime)message.Data;
			TimeChangedHandler(time);
		}
		return null;
	}

	private void TimeChangedHandler(DateTime time)
	{
		AddTimeToQueue(time);
	}

	private void ClearQueue()
	{
		lock (_queueLocker)
		{
			_timeQueue.Clear();
		}
	}

	private void AddTimeToQueue(DateTime time)
	{
		lock (_queueLocker)
		{
			_timeQueue.Enqueue(time);
			if (_timeQueue.Count > 3)
			{
				_timeQueue.Dequeue();
			}
		}
	}

	private DateTime? RemoveTimeFromQueue(DateTime currentTime)
	{
		DateTime? result = null;
		lock (_queueLocker)
		{
			while (_timeQueue.Count > 0)
			{
				DateTime dateTime = _timeQueue.Dequeue();
				if (dateTime != currentTime)
				{
					result = dateTime;
					return result;
				}
			}
		}
		return result;
	}

	private void MetadataFetchThread()
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(validTimeInSecondsGreatest);
		DateTime dateTime = DateTime.MaxValue;
		try
		{
			while (!_stopFetchThread)
			{
				DateTime? dateTime2 = RemoveTimeFromQueue(_lastPresentedTimeUtc);
				if (dateTime2.HasValue)
				{
					DateTime value = dateTime2.Value;
					_lastPresentedTimeUtc = ((value.Kind == DateTimeKind.Local) ? value.ToUniversalTime() : value);
					DateTime lastPresentedTimeUtc = _lastPresentedTimeUtc;
					MetadataPlaybackData atOrBefore = _metadataSource.GetAtOrBefore(lastPresentedTimeUtc);
					if (atOrBefore != null)
					{
						DateTime dateTime3 = atOrBefore.DateTime;
						TimeSpan timeSpan2 = lastPresentedTimeUtc - dateTime3;
						if (lastPresentedTimeUtc < base.LatestMetadataTimeUtc)
						{
							OnInvalidMetadata(base.LatestMetadataTimeUtc);
							base.LatestMetadataTimeUtc = DateTime.MinValue;
						}
						if (lastPresentedTimeUtc > dateTime)
						{
							OnInvalidMetadata(base.LatestMetadataTimeUtc);
							dateTime = DateTime.MaxValue;
							base.LatestMetadataTimeUtc = DateTime.MinValue;
						}
						if (base.LatestMetadataTimeUtc != dateTime3 && timeSpan2 < timeSpan)
						{
							ProcessMetadataContent(atOrBefore.Content, atOrBefore.DateTime, atOrBefore.PreviousDateTime, atOrBefore.NextDateTime);
							dateTime = dateTime3 + timeSpan;
							base.LatestMetadataTimeUtc = dateTime3;
						}
					}
					else if (lastPresentedTimeUtc < base.LatestMetadataTimeUtc)
					{
						OnInvalidMetadata(base.LatestMetadataTimeUtc);
						base.LatestMetadataTimeUtc = DateTime.MinValue;
					}
				}
				Thread.Sleep(10);
			}
		}
		catch (ThreadInterruptedException ex)
		{
			OnError("Unexpected error. Playback thread has been shut down. Exception: " + ex.Message);
		}
		_stopFetchThread = false;
		_fetchThread = null;
	}

	public override void Dispose()
	{
	}
}
