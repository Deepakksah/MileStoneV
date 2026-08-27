using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;
using VideoOS.UI.Common.WPF.Timeline.Control.Data;
using VideoOS.UI.Common.WPF.Timeline.Control.UIControls.SelectTimePopup;
using VideoOS.UI.Common.WPF.Timeline.Events;

namespace VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaUpper;

public partial class ControlsAreaUpper : UserControl, IComponentConnector
{
	private TimeAndSpanController _timeAndSpanController = new TimeAndSpanController();

	private AppearanceAndBehaviourController _appearanceAndBehaviourController = new AppearanceAndBehaviourController();

	public static double ControlsAreaUpperHeight => 40.0;

	public event EventHandler<PlaybackNavigationEventArgs> PlaybackNavigationOccurred;

	public ControlsAreaUpper()
	{
		InitializeComponent();
	}

	public void Init(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, ITimelineSequenceCache timelineCache)
	{
		_timeAndSpanController = timeAndSpanController;
		_appearanceAndBehaviourController = appearanceAndBehaviourController;
		_timeAndSpanController.SelectionStateChangedEvent += _timeAndSpanController_SelectionModeChangedEvent;
		_appearanceAndBehaviourController.PropertyChanged += _appearanceAndBehaviourController_PropertyChanged;
	}

	public void Close()
	{
		_timeAndSpanController.SelectionStateChangedEvent -= _timeAndSpanController_SelectionModeChangedEvent;
		_appearanceAndBehaviourController.PropertyChanged -= _appearanceAndBehaviourController_PropertyChanged;
	}

	private void UserControl_Loaded(object sender, RoutedEventArgs e)
	{
		_timeSelectionControl.TimeSelectionModeEnabled = _timeAndSpanController.SelectionState != SelectionState.Disabled;
		_playbackSpeedControl.SetPlaybackSpeeds(_appearanceAndBehaviourController.PlaybackSpeeds, _appearanceAndBehaviourController.PlaybackSpeed);
		ApplyAppearanceAndBehaviourControllerProperty(null);
	}

	private void _timeAndSpanController_SelectionModeChangedEvent(object sender, EventArgs e)
	{
		_timeSelectionControl.IsEnabled = false;
		_timeSelectionControl.TimeSelectionModeEnabled = _timeAndSpanController.SelectionState == SelectionState.Enabled;
		_timeSelectionControl.IsEnabled = true;
	}

	private void _appearanceAndBehaviourController_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		ApplyAppearanceAndBehaviourControllerProperty(e.PropertyName);
	}

	private void _primaryNavigationControl_NavigateNextButtonClicked(object sender, EventArgs e)
	{
		SignalPlaybackNavigationOccurred(PlaybackNavigationEventArgs.PlaybackNavigationType.Next);
	}

	private void _primaryNavigationControl_NavigatePreviousButtonClicked(object sender, EventArgs e)
	{
		SignalPlaybackNavigationOccurred(PlaybackNavigationEventArgs.PlaybackNavigationType.Previous);
	}

	private void _secondaryNavigationControl_NavigateFirstButtonClicked(object sender, EventArgs e)
	{
		SignalPlaybackNavigationOccurred(PlaybackNavigationEventArgs.PlaybackNavigationType.First);
	}

	private void _secondaryNavigationControl_NavigateLastButtonClicked(object sender, EventArgs e)
	{
		SignalPlaybackNavigationOccurred(PlaybackNavigationEventArgs.PlaybackNavigationType.Last);
	}

	private void _secondaryNavigationControl_NavigateNextSequenceButtonClicked(object sender, EventArgs e)
	{
		SignalPlaybackNavigationOccurred(PlaybackNavigationEventArgs.PlaybackNavigationType.NextSequence);
	}

	private void _secondaryNavigationControl_NavigatePreviousSequenceButtonClicked(object sender, EventArgs e)
	{
		SignalPlaybackNavigationOccurred(PlaybackNavigationEventArgs.PlaybackNavigationType.PreviousSequence);
	}

	private void SignalPlaybackNavigationOccurred(PlaybackNavigationEventArgs.PlaybackNavigationType playbackNavigationType)
	{
		if (this.PlaybackNavigationOccurred != null)
		{
			this.PlaybackNavigationOccurred(this, new PlaybackNavigationEventArgs(playbackNavigationType));
		}
	}

	private void _primaryNavigationControl_PlaybackBackwardButtonClicked(object sender, EventArgs e)
	{
		if (_appearanceAndBehaviourController.PlaybackState == PlaybackState.Backward)
		{
			_appearanceAndBehaviourController.PlaybackState = PlaybackState.Stopped;
		}
		else
		{
			_appearanceAndBehaviourController.PlaybackState = PlaybackState.Backward;
		}
	}

	private void _primaryNavigationControl_PlaybackForwardButtonClicked(object sender, EventArgs e)
	{
		if (_appearanceAndBehaviourController.PlaybackState == PlaybackState.Forward)
		{
			_appearanceAndBehaviourController.PlaybackState = PlaybackState.Stopped;
		}
		else
		{
			_appearanceAndBehaviourController.PlaybackState = PlaybackState.Forward;
		}
	}

	private void _primaryNavigationControl_PlaybackSliderValueChanged(object sender, EventArgs e)
	{
		if (_primaryNavigationControl.PlaybackSliderValue != 0.0)
		{
			double speed = Math.Abs(_primaryNavigationControl.PlaybackSliderValue * _appearanceAndBehaviourController.PlaybackSliderMaxSpeed);
			_appearanceAndBehaviourController.PlaybackSpeed = new PlaybackSpeed(speed, string.Empty);
			if (_primaryNavigationControl.PlaybackSliderValue < 0.0)
			{
				_appearanceAndBehaviourController.PlaybackState = PlaybackState.Backward;
			}
			else
			{
				_appearanceAndBehaviourController.PlaybackState = PlaybackState.Forward;
			}
		}
		else
		{
			_appearanceAndBehaviourController.PlaybackSpeed = _playbackSpeedControl.PlaybackSpeed;
			_appearanceAndBehaviourController.PlaybackState = PlaybackState.Stopped;
		}
	}

	private void _playbackSpeedControl_SelectedSpeedChanged(object sender, EventArgs e)
	{
		_appearanceAndBehaviourController.PlaybackSpeed = _playbackSpeedControl.PlaybackSpeed;
	}

	private void _timeSelectionControl_TimeSelectionModeEnabledChanged(object sender, EventArgs e)
	{
		if ((_timeSelectionControl.TimeSelectionModeEnabled || _timeAndSpanController.SelectionState != SelectionState.Disabled) && (!_timeSelectionControl.TimeSelectionModeEnabled || _timeAndSpanController.SelectionState == SelectionState.Disabled))
		{
			if (_timeSelectionControl.TimeSelectionModeEnabled)
			{
				DateTime defaultSelectionFromTime = _timeAndSpanController.DefaultSelectionFromTime;
				DateTime defaultSelectionCenterTime = _timeAndSpanController.DefaultSelectionCenterTime;
				DateTime defaultSelectionToTime = _timeAndSpanController.DefaultSelectionToTime;
				_timeAndSpanController.SetSelectionTimes(defaultSelectionFromTime, defaultSelectionCenterTime, defaultSelectionToTime, adjustToFromTimeIfNessesary: false, adjustToCenterTimeIfNessesary: false, adjustToToTimeIfNessesary: false);
				_timeAndSpanController.SelectionState = SelectionState.Enabled;
			}
			else
			{
				_timeAndSpanController.SelectionState = SelectionState.Disabled;
			}
		}
	}

	private void _timeSelectionControl_SetTimeSelectionTimesButtonClicked(object sender, EventArgs e)
	{
		TimelineSelectTimePopupHelper.OpenAndHandleSelectSelectionTimes(_timeAndSpanController, _appearanceAndBehaviourController, this);
	}

	private void ApplyAppearanceAndBehaviourControllerProperty(string propertyName)
	{
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.ShowTimeSelectionControl.ToString())
		{
			_timeSelectionControl.Visibility = ((!_appearanceAndBehaviourController.ShowTimeSelectionControl) ? Visibility.Collapsed : Visibility.Visible);
		}
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.ShowPlaybackSpeedControl.ToString())
		{
			_playbackSpeedControl.Visibility = ((!_appearanceAndBehaviourController.ShowPlaybackSpeedControl) ? Visibility.Collapsed : Visibility.Visible);
		}
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.ShowPrimaryNavigationControl.ToString())
		{
			_primaryNavigationControl.Visibility = ((!_appearanceAndBehaviourController.ShowPrimaryNavigationControl) ? Visibility.Collapsed : Visibility.Visible);
		}
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.ShowSecondaryNavigationControl.ToString())
		{
			_secondaryNavigationBackwardControl.Visibility = ((!_appearanceAndBehaviourController.ShowSecondaryNavigationControl) ? Visibility.Collapsed : Visibility.Visible);
			_secondaryNavigationForwardControl.Visibility = ((!_appearanceAndBehaviourController.ShowSecondaryNavigationControl) ? Visibility.Collapsed : Visibility.Visible);
		}
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.UserSelectionControlEnabled.ToString())
		{
			_timeSelectionControl.UserControlEnabled = _appearanceAndBehaviourController.UserSelectionControlEnabled;
		}
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.ShowPlaybackPlayPauseControls.ToString())
		{
			_primaryNavigationControl.Model.ShowPlaybackPlayPauseControls = _appearanceAndBehaviourController.ShowPlaybackPlayPauseControls;
		}
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.UserPlaybackNavigationControlEnabled.ToString())
		{
			_primaryNavigationControl.Model.NavigationControlsEnabled = _appearanceAndBehaviourController.UserPlaybackNavigationControlEnabled;
			_secondaryNavigationBackwardControl.Model.NavigationControlsEnabled = _appearanceAndBehaviourController.UserPlaybackNavigationControlEnabled;
			_secondaryNavigationForwardControl.Model.NavigationControlsEnabled = _appearanceAndBehaviourController.UserPlaybackNavigationControlEnabled;
		}
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.PlaybackState.ToString())
		{
			_primaryNavigationControl.Model.PlaybackBackwardPauseState = _appearanceAndBehaviourController.PlaybackState == PlaybackState.Backward;
			_primaryNavigationControl.Model.PlaybackForwardPauseState = _appearanceAndBehaviourController.PlaybackState == PlaybackState.Forward;
		}
		if ((string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.PlaybackSpeed.ToString()) && _primaryNavigationControl.PlaybackSliderValue == 0.0)
		{
			_playbackSpeedControl.PlaybackSpeed = _appearanceAndBehaviourController.PlaybackSpeed;
		}
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.PlaybackPrimaryNavigationSpeed.ToString())
		{
			_primaryNavigationControl.SetPlaybackSliderValue(_appearanceAndBehaviourController.PlaybackPrimaryNavigationSpeed);
		}
		if (!string.IsNullOrEmpty(propertyName) && !(propertyName == AppearanceAndBehaviourController.PropertyNames.PlaybackSpeeds.ToString()))
		{
			return;
		}
		PlaybackSpeed playbackSpeed = _appearanceAndBehaviourController.PlaybackSpeeds[0];
		foreach (PlaybackSpeed playbackSpeed2 in _appearanceAndBehaviourController.PlaybackSpeeds)
		{
			double num = Math.Abs(playbackSpeed2.Speed - _playbackSpeedControl.PlaybackSpeed.Speed);
			double num2 = Math.Abs(playbackSpeed.Speed - _playbackSpeedControl.PlaybackSpeed.Speed);
			if (num < num2)
			{
				playbackSpeed = playbackSpeed2;
			}
		}
		_playbackSpeedControl.SetPlaybackSpeeds(_appearanceAndBehaviourController.PlaybackSpeeds, playbackSpeed);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
