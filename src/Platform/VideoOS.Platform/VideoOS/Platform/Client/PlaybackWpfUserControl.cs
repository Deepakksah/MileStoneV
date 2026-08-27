using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public sealed partial class PlaybackWpfUserControl : UserControl, IComponentConnector
{
	private PlaybackWpfUserControlInternal _internalControl;

	public bool ShowTallUserControl
	{
		get
		{
			return _internalControl.ShowTallUserControl;
		}
		set
		{
			_internalControl.ShowTallUserControl = value;
		}
	}

	public bool ShowSpeedControl
	{
		get
		{
			return _internalControl.ShowSpeedControl;
		}
		set
		{
			_internalControl.ShowSpeedControl = value;
		}
	}

	public bool ShowTimeSpanControl
	{
		get
		{
			return _internalControl.ShowTimeSpanControl;
		}
		set
		{
			_internalControl.ShowTimeSpanControl = value;
		}
	}

	public DateTime SelectionFromTime
	{
		get
		{
			return _internalControl.SelectionFromTime;
		}
		set
		{
			_internalControl.SelectionFromTime = value;
		}
	}

	public DateTime SelectionToTime
	{
		get
		{
			return _internalControl.SelectionToTime;
		}
		set
		{
			_internalControl.SelectionToTime = value;
		}
	}

	public TimeSpan TimeSpan
	{
		get
		{
			return _internalControl.TimeSpan;
		}
		set
		{
			_internalControl.TimeSpan = value;
		}
	}

	public PlaybackWpfUserControl()
		: this(null)
	{
	}

	public PlaybackWpfUserControl(WindowInformation windowInformation)
	{
		_internalControl = ClientControl.Instance.CreatePlaybackWpfUserControlInternal(windowInformation);
		if (_internalControl != null)
		{
			_internalControl.HorizontalAlignment = HorizontalAlignment.Stretch;
			_internalControl.VerticalAlignment = VerticalAlignment.Stretch;
			base.Content = _internalControl;
		}
	}

	public void Init(FQID playbackControllerFQID)
	{
		_internalControl.Init(playbackControllerFQID);
		new TrackPageViewData(typeof(PlaybackWpfUserControl)).Send();
	}

	public void Close()
	{
		_internalControl.Close();
	}

	public void SetCameras(List<FQID> sources)
	{
		_internalControl.SetCameras(sources);
	}

	public void SetEnabled(bool enable)
	{
		_internalControl.SetEnabled(enable);
	}

	public void SetSequence(DateTime startTime, DateTime endTime)
	{
		_internalControl.SetSequence(startTime, endTime);
	}
}
