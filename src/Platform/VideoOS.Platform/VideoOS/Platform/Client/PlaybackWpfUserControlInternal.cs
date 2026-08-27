using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace VideoOS.Platform.Client;

public abstract class PlaybackWpfUserControlInternal : UserControl
{
	public virtual bool ShowTallUserControl
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual bool ShowSpeedControl
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual bool ShowTimeSpanControl
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual DateTime SelectionFromTime
	{
		get
		{
			return DateTime.UtcNow;
		}
		set
		{
		}
	}

	public virtual DateTime SelectionToTime
	{
		get
		{
			return DateTime.UtcNow;
		}
		set
		{
		}
	}

	public virtual TimeSpan TimeSpan
	{
		get
		{
			return TimeSpan.FromHours(1.0);
		}
		set
		{
		}
	}

	public virtual void Init(FQID playbackControllerFQID)
	{
	}

	public virtual void Close()
	{
	}

	public virtual void SetCameras(List<FQID> sources)
	{
	}

	public virtual void SetEnabled(bool enable)
	{
	}

	public virtual void SetSequence(DateTime startTime, DateTime endTime)
	{
	}
}
