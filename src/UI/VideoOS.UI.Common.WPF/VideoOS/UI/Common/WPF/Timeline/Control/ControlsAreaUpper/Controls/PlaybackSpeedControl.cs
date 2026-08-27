using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.Platform.UI.Controls;
using VideoOS.UI.Common.WPF.Timeline.Control.Data;

namespace VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaUpper.Controls;

public partial class PlaybackSpeedControl : UserControl, IComponentConnector
{
	private PlaybackSpeed _playbackSpeed;

	public PlaybackSpeed PlaybackSpeed
	{
		get
		{
			return _playbackSpeed;
		}
		set
		{
			if (_playbackSpeed == value)
			{
				return;
			}
			_playbackSpeed = value;
			foreach (VideoOSActionSelect action in _customPlaybackSpeedMenuButton.Actions)
			{
				if ((PlaybackSpeed)action.Tag == PlaybackSpeed)
				{
					action.IsSelected = true;
					_customPlaybackSpeedMenuButton.Content = action.Text;
				}
				else
				{
					action.IsSelected = false;
				}
			}
			if (this.SelectedSpeedChanged != null)
			{
				this.SelectedSpeedChanged(this, new EventArgs());
			}
		}
	}

	public event EventHandler SelectedSpeedChanged;

	public PlaybackSpeedControl()
	{
		InitializeComponent();
	}

	public void SetPlaybackSpeeds(List<PlaybackSpeed> customPlaybackSpeeds, PlaybackSpeed playbackSpeed)
	{
		List<VideoOSActionBase> list = new List<VideoOSActionBase>();
		foreach (PlaybackSpeed customPlaybackSpeed in customPlaybackSpeeds)
		{
			VideoOSActionSelect videoOSActionSelect = new VideoOSActionSelect();
			videoOSActionSelect.Text = customPlaybackSpeed.Text;
			videoOSActionSelect.InMenuSelectedAppearance = VideoOSActionSelect.InMenuSelectedAppearances.Highlighted;
			videoOSActionSelect.InMenuStaysOpenOnClick = false;
			videoOSActionSelect.Tag = customPlaybackSpeed;
			videoOSActionSelect.IsSelectedChanged += Action_IsSelectedChanged;
			list.Add(videoOSActionSelect);
		}
		_customPlaybackSpeedMenuButton.Actions = list;
		PlaybackSpeed = playbackSpeed;
	}

	private void Action_IsSelectedChanged(object sender, EventArgs e)
	{
		VideoOSActionSelect videoOSActionSelect = (VideoOSActionSelect)sender;
		if (videoOSActionSelect.IsSelected)
		{
			PlaybackSpeed = (PlaybackSpeed)videoOSActionSelect.Tag;
			_customPlaybackSpeedMenuButton.Content = videoOSActionSelect.Text;
		}
	}
}
