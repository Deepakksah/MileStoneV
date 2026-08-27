using System.ComponentModel;

namespace VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaUpper.Controls;

public class PrimaryNavigationControlModel : INotifyPropertyChanged
{
	private bool _playbackForwardPauseState;

	private bool _playbackBackwardPauseState;

	private bool _navigationControlsEnabled;

	private bool _showPlaybackPlayPauseControls = true;

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

	public event PropertyChangedEventHandler PropertyChanged;

	private void FirePropChanged(string propName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
	}
}
