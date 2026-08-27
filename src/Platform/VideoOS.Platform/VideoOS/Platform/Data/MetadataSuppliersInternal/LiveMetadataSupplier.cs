using System;
using System.Timers;
using VideoOS.Platform.Live;

namespace VideoOS.Platform.Data.MetadataSuppliersInternal;

public class LiveMetadataSupplier : InternalMetadataSupplier
{
	private MetadataLiveSource _metadataLiveSource;

	private Timer _timer;

	public LiveMetadataSupplier(Item metadataSourceItem)
		: base(metadataSourceItem)
	{
	}

	public override bool Start()
	{
		bool result = Stop();
		SetupTimer();
		_metadataLiveSource = new MetadataLiveSource(_metadataSourceItem);
		_metadataLiveSource.LiveModeStart = true;
		_metadataLiveSource.Init();
		_metadataLiveSource.LiveContentEvent += MetadataLiveSourceLiveNotificationEvent;
		_metadataLiveSource.ErrorEvent += MetadataLiveSourceErrorEvent;
		return result;
	}

	private void SetupTimer()
	{
		_timer = new Timer();
		_timer.Elapsed += _timer_Elapsed;
		_timer.AutoReset = false;
	}

	public override bool Stop()
	{
		if (_metadataLiveSource != null)
		{
			_metadataLiveSource.LiveModeStart = false;
			_metadataLiveSource.LiveContentEvent -= MetadataLiveSourceLiveNotificationEvent;
			_metadataLiveSource.ErrorEvent -= MetadataLiveSourceErrorEvent;
			_metadataLiveSource.Close();
			_metadataLiveSource = null;
		}
		return true;
	}

	private void MetadataLiveSourceLiveNotificationEvent(MetadataLiveSource sender, MetadataLiveContent e)
	{
		if (e.Content != null)
		{
			DateTime utcNow = DateTime.UtcNow;
			if (e.BeginTime >= utcNow - new TimeSpan(0, 0, validTimeInSecondsGreatest))
			{
				_timer.Stop();
				double num = (double)validTimeInSecondsGreatest - (utcNow - e.BeginTime).TotalSeconds;
				_timer.Interval = num * 1000.0;
				_timer.Start();
				ProcessMetadataContent(e.Content, e.BeginTime);
				base.LatestMetadataTimeUtc = e.BeginTime;
			}
		}
	}

	private void _timer_Elapsed(object sender, ElapsedEventArgs e)
	{
		_timer.Stop();
		OnInvalidMetadata(base.LatestMetadataTimeUtc);
	}

	private void MetadataLiveSourceErrorEvent(MetadataLiveSource arg1, Exception arg2)
	{
		OnError(arg2.Message);
	}

	public override void Dispose()
	{
		if (_timer != null)
		{
			_timer.Elapsed -= _timer_Elapsed;
			_timer.Dispose();
			_timer = null;
		}
	}
}
