using System.ComponentModel;
using System.Windows;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Controls;

public class ControlsLayerModel : INotifyPropertyChanged
{
	private string _dateTimeText;

	private bool _playbackForwardPauseState;

	private bool _playbackBackwardPauseState;

	private bool _navigationControlsEnabled;

	private bool _showPlaybackPlayPauseControls = true;

	private Visibility _playbackControlsVisibility;

	private Visibility _playheadTextAreaVisibility;

	private Thickness _playheadLineMargin;

	private int _playheadTextAreaHeight;

	public string DateTimeText
	{
		get
		{
			return _dateTimeText;
		}
		set
		{
			if (_dateTimeText != value)
			{
				_dateTimeText = value;
				FirePropChanged("DateTimeText");
			}
		}
	}

	public bool PlaybackForwardPauseState
	{
		get
		{
			return _playbackForwardPauseState;
		}
		set
		{
			if (_playbackForwardPauseState != value)
			{
				_playbackForwardPauseState = value;
				FirePropChanged("PlaybackForwardPauseState");
			}
		}
	}

	public bool PlaybackBackwardPauseState
	{
		get
		{
			return _playbackBackwardPauseState;
		}
		set
		{
			if (_playbackBackwardPauseState != value)
			{
				_playbackBackwardPauseState = value;
				FirePropChanged("PlaybackBackwardPauseState");
			}
		}
	}

	public bool NavigationControlsEnabled
	{
		get
		{
			return _navigationControlsEnabled;
		}
		set
		{
			if (_navigationControlsEnabled != value)
			{
				_navigationControlsEnabled = value;
				FirePropChanged("NavigationControlsEnabled");
			}
		}
	}

	public bool ShowPlaybackPlayPauseControls
	{
		get
		{
			return _showPlaybackPlayPauseControls;
		}
		set
		{
			if (_showPlaybackPlayPauseControls != value)
			{
				_showPlaybackPlayPauseControls = value;
				FirePropChanged("ShowPlaybackPlayPauseControls");
			}
		}
	}

	public Visibility PlaybackControlsVisibility
	{
		get
		{
			return _playbackControlsVisibility;
		}
		set
		{
			if (_playbackControlsVisibility != value)
			{
				_playbackControlsVisibility = value;
				FirePropChanged("PlaybackControlsVisibility");
			}
		}
	}

	public Visibility PlayheadTextAreaVisibility
	{
		get
		{
			return _playheadTextAreaVisibility;
		}
		set
		{
			if (_playheadTextAreaVisibility != value)
			{
				_playheadTextAreaVisibility = value;
				FirePropChanged("PlayheadTextAreaVisibility");
			}
		}
	}

	public Thickness PlayheadLineMargin
	{
		get
		{
			return _playheadLineMargin;
		}
		set
		{
			if (_playheadLineMargin != value)
			{
				_playheadLineMargin = value;
				FirePropChanged("PlayheadLineMargin");
			}
		}
	}

	public int PlayheadTextAreaHeight
	{
		get
		{
			return _playheadTextAreaHeight;
		}
		set
		{
			if (_playheadTextAreaHeight != value)
			{
				_playheadTextAreaHeight = value;
				FirePropChanged("PlayheadTextAreaHeight");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void FirePropChanged(string propName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
	}
}
