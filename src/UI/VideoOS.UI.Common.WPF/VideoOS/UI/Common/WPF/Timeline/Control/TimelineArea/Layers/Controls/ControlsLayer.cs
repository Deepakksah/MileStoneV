using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;
using VideoOS.UI.Common.WPF.Timeline.Control.UIControls.SelectTimePopup;
using VideoOS.UI.Common.WPF.Timeline.Events;
using VideoOS.Utilities.DateTimeHelper;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Controls;

public partial class ControlsLayer : LayerBase, IComponentConnector
{
	private ControlsLayerModel Model { get; set; }

	public event EventHandler<EventArgs> CenterTimeMarkerRightClicked;

	public event EventHandler<PlaybackNavigationEventArgs> PlaybackNavigationOccurred;

	public ControlsLayer()
	{
		InitializeComponent();
		Model = new ControlsLayerModel();
		base.DataContext = Model;
	}

	public override void Init(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, ITimelineSequenceCache timelineCache)
	{
		base.Init(timeAndSpanController, appearanceAndBehaviourController, timelineCache);
		base.TimeAndSpanController.BrowseTimeChanged += TimeAndSpanController_BrowseTimeChangedEvent;
		base.TimeAndSpanController.SelectionStateChangedEvent += TimeAndSpanController_SelectionStateChangedEvent;
		base.TimeAndSpanController.SelectionActiveMarkerChangedEvent += TimeAndSpanController_SelectionActiveMarkerChangedEvent;
		base.AppearanceAndBehaviourController.PropertyChanged += appearanceAndBehaviourController_PropertyChanged;
		ThemeManager.Instance.SelectedThemeChanged += ThemeManager_SelectedThemeChanged;
		UpdateTheme();
	}

	public override void Close()
	{
		base.Close();
		base.TimeAndSpanController.BrowseTimeChanged -= TimeAndSpanController_BrowseTimeChangedEvent;
		base.TimeAndSpanController.SelectionStateChangedEvent -= TimeAndSpanController_SelectionStateChangedEvent;
		base.TimeAndSpanController.SelectionActiveMarkerChangedEvent -= TimeAndSpanController_SelectionActiveMarkerChangedEvent;
		base.AppearanceAndBehaviourController.PropertyChanged -= appearanceAndBehaviourController_PropertyChanged;
		ThemeManager.Instance.SelectedThemeChanged -= ThemeManager_SelectedThemeChanged;
	}

	private void appearanceAndBehaviourController_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.TimeZone.ToString())
		{
			UpdateTime();
		}
		else if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.TimelineAreaDisplayMode.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowControlsAreaUpper.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowControlsAreaLower.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineTopLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineBottomLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineCustomDataLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineTopLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineBottomLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineCustomDataLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.UpperTimelineLabel.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.LowerTimelineLabel.ToString())
		{
			UpdateControls();
			UpdateTime();
		}
		else if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowTimelineAreaPlaybackControl.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.PlaybackState.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.UserPlaybackNavigationControlEnabled.ToString())
		{
			UpdatePlaybackControlState();
		}
	}

	private void ThemeManager_SelectedThemeChanged(object sender, EventArgs e)
	{
		UpdateTheme();
	}

	private void LayerBase_Loaded(object sender, RoutedEventArgs e)
	{
		UpdateTime();
		UpdateControls();
		UpdatePlaybackControlState();
	}

	private void _dateOrTimeTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}

	private void _dateOrTimeTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		TimelineSelectTimePopupHelper.OpenAndHandleSelectTimelineTime(base.TimeAndSpanController, base.AppearanceAndBehaviourController, this);
		e.Handled = true;
	}

	private void _dateOrTimeTextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (this.CenterTimeMarkerRightClicked != null)
		{
			this.CenterTimeMarkerRightClicked(this, new EventArgs());
		}
		e.Handled = true;
	}

	private void _marker_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (this.CenterTimeMarkerRightClicked != null)
		{
			this.CenterTimeMarkerRightClicked(this, new EventArgs());
		}
		e.Handled = true;
	}

	private void _navigatePreviousButton_Click(object sender, RoutedEventArgs e)
	{
		if (this.PlaybackNavigationOccurred != null)
		{
			this.PlaybackNavigationOccurred(this, new PlaybackNavigationEventArgs(PlaybackNavigationEventArgs.PlaybackNavigationType.Previous));
		}
	}

	private void _navigateNextButton_Click(object sender, RoutedEventArgs e)
	{
		if (this.PlaybackNavigationOccurred != null)
		{
			this.PlaybackNavigationOccurred(this, new PlaybackNavigationEventArgs(PlaybackNavigationEventArgs.PlaybackNavigationType.Next));
		}
	}

	private void _playbackBackwardButton_Click(object sender, RoutedEventArgs e)
	{
		if (base.AppearanceAndBehaviourController.PlaybackState == PlaybackState.Backward)
		{
			base.AppearanceAndBehaviourController.PlaybackState = PlaybackState.Stopped;
		}
		else
		{
			base.AppearanceAndBehaviourController.PlaybackState = PlaybackState.Backward;
		}
	}

	private void _playbackForwardButton_Click(object sender, RoutedEventArgs e)
	{
		if (base.AppearanceAndBehaviourController.PlaybackState == PlaybackState.Forward)
		{
			base.AppearanceAndBehaviourController.PlaybackState = PlaybackState.Stopped;
		}
		else
		{
			base.AppearanceAndBehaviourController.PlaybackState = PlaybackState.Forward;
		}
	}

	private void TimeAndSpanController_BrowseTimeChangedEvent(object sender, DateTimeEventArgs e)
	{
		UpdateTime();
	}

	private void TimeAndSpanController_SelectionStateChangedEvent(object sender, EventArgs e)
	{
		UpdateControls();
	}

	private void TimeAndSpanController_SelectionActiveMarkerChangedEvent(object sender, SelectionActiveMarkerChangeEventArgs e)
	{
		UpdateControls();
	}

	private void UpdateTime()
	{
		Model.DateTimeText = ((base.AppearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Full) ? (DateTimeFormatter.ConvertAndFormat("G", base.AppearanceAndBehaviourController.TimeZone, base.TimeAndSpanController.TimelineTime, injectMilliseconds: true, injectOffset: false) ?? "") : (DateTimeFormatter.ConvertAndFormat("T", base.AppearanceAndBehaviourController.TimeZone, base.TimeAndSpanController.TimelineTime, injectMilliseconds: true, injectOffset: false) ?? ""));
	}

	private void UpdateControls()
	{
		if (base.TimeAndSpanController.SelectionState == SelectionState.Disabled || base.TimeAndSpanController.SelectionActiveMarker == SelectionMarker.None)
		{
			Model.PlayheadTextAreaVisibility = Visibility.Visible;
			Model.PlayheadLineMargin = new Thickness(0.0, 0.0, 0.0, base.AppearanceAndBehaviourController.TimelineAreaTracksMargin);
		}
		else
		{
			Model.PlayheadTextAreaVisibility = Visibility.Hidden;
			Model.PlayheadLineMargin = new Thickness(0.0, 0.0, 0.0, base.AppearanceAndBehaviourController.TimelineAreaTracksMargin);
		}
		if (base.AppearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Full)
		{
			Model.PlayheadTextAreaHeight = 24;
		}
		else
		{
			Model.PlayheadTextAreaHeight = 20;
		}
	}

	private void UpdatePlaybackControlState()
	{
		if (base.AppearanceAndBehaviourController.ShowTimelineAreaPlaybackControl)
		{
			Model.PlaybackControlsVisibility = Visibility.Visible;
			Model.NavigationControlsEnabled = base.AppearanceAndBehaviourController.UserPlaybackNavigationControlEnabled;
			Model.ShowPlaybackPlayPauseControls = base.AppearanceAndBehaviourController.ShowPlaybackPlayPauseControls;
			Model.PlaybackBackwardPauseState = base.AppearanceAndBehaviourController.PlaybackState == PlaybackState.Backward;
			Model.PlaybackForwardPauseState = base.AppearanceAndBehaviourController.PlaybackState == PlaybackState.Forward;
		}
		else
		{
			Model.PlaybackControlsVisibility = Visibility.Collapsed;
		}
	}

	private void UpdateTheme()
	{
		ThemeManager.Instance.ApplyThemeOnElement(_playheadLine, (ThemeManager.Instance.SelectedTheme == ThemeType.Dark) ? ThemeType.Light : ThemeType.Dark);
		ThemeManager.Instance.ApplyThemeOnElement(_playheadTextArea, (ThemeManager.Instance.SelectedTheme == ThemeType.Dark) ? ThemeType.Light : ThemeType.Dark);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
