using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;
using VideoOS.Platform.UI.Controls;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;
using VideoOS.UI.Common.WPF.Timeline.Control.Data;
using VideoOS.UI.Common.WPF.Timeline.Events;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

namespace VideoOS.UI.Common.WPF.Timeline.Control;

public partial class TimelineControl : UserControl, IComponentConnector
{
	private static BitmapImage MarkerImageDarkTheme;

	private static BitmapImage MarkerImageLightTheme;

	private TimeAndSpanController _timeAndSpanController = new TimeAndSpanController();

	private AppearanceAndBehaviourController _appearanceAndBehaviourController = new AppearanceAndBehaviourController();

	public IEnumerable<ISequenceSource> UpperSequenceSources => _appearanceAndBehaviourController.UpperSequenceSources;

	public IEnumerable<ISequenceSource> LowerSequenceSources => _appearanceAndBehaviourController.LowerSequenceSources;

	public DateTime BrowseTime => _timeAndSpanController.BrowseTime;

	public DateTime TimelineTime => _timeAndSpanController.TimelineTime;

	public TimeSpan Span => _timeAndSpanController.TargetTimelineSpan;

	public TimeSpan TimePerPixel => new TimeSpan(_timeAndSpanController.GetTicksPerPixel());

	public SelectionMode SelectionMode
	{
		get
		{
			return _timeAndSpanController.SelectionMode;
		}
		set
		{
			_timeAndSpanController.SelectionMode = value;
		}
	}

	public SelectionState SelectionState
	{
		get
		{
			return _timeAndSpanController.SelectionState;
		}
		set
		{
			_timeAndSpanController.SelectionState = value;
		}
	}

	public SelectionMarker SelectionActiveMarker
	{
		get
		{
			return _timeAndSpanController.SelectionActiveMarker;
		}
		set
		{
			_timeAndSpanController.SelectionActiveMarker = value;
		}
	}

	public DateTime SelectionFromTime
	{
		get
		{
			return _timeAndSpanController.SelectionFromTime;
		}
		set
		{
			_timeAndSpanController.SetSelectionTimes(value, _timeAndSpanController.SelectionCenterTime, _timeAndSpanController.SelectionToTime, adjustToFromTimeIfNessesary: true, adjustToCenterTimeIfNessesary: false, adjustToToTimeIfNessesary: false);
		}
	}

	public DateTime SelectionCenterTime
	{
		get
		{
			return _timeAndSpanController.SelectionCenterTime;
		}
		set
		{
			_timeAndSpanController.SetSelectionTimes(_timeAndSpanController.SelectionFromTime, value, _timeAndSpanController.SelectionToTime, adjustToFromTimeIfNessesary: false, adjustToCenterTimeIfNessesary: true, adjustToToTimeIfNessesary: false);
		}
	}

	public DateTime SelectionToTime
	{
		get
		{
			return _timeAndSpanController.SelectionToTime;
		}
		set
		{
			_timeAndSpanController.SetSelectionTimes(_timeAndSpanController.SelectionFromTime, _timeAndSpanController.SelectionCenterTime, value, adjustToFromTimeIfNessesary: false, adjustToCenterTimeIfNessesary: false, adjustToToTimeIfNessesary: true);
		}
	}

	public PlaybackState PlaybackState
	{
		get
		{
			return _appearanceAndBehaviourController.PlaybackState;
		}
		set
		{
			_appearanceAndBehaviourController.PlaybackState = value;
		}
	}

	public double PlaybackSpeed
	{
		get
		{
			return _appearanceAndBehaviourController.PlaybackSpeed.Speed;
		}
		set
		{
			PlaybackSpeed playbackSpeed = _appearanceAndBehaviourController.PlaybackSpeeds.Last();
			foreach (PlaybackSpeed playbackSpeed2 in _appearanceAndBehaviourController.PlaybackSpeeds)
			{
				if (playbackSpeed2.Speed >= value)
				{
					playbackSpeed = playbackSpeed2;
					break;
				}
			}
			_appearanceAndBehaviourController.PlaybackSpeed = playbackSpeed;
		}
	}

	public double PlaybackPrimaryNavigationSpeed
	{
		get
		{
			return _appearanceAndBehaviourController.PlaybackPrimaryNavigationSpeed;
		}
		set
		{
			_appearanceAndBehaviourController.PlaybackPrimaryNavigationSpeed = value;
		}
	}

	public double MinPlaybackSpeed
	{
		get
		{
			return _appearanceAndBehaviourController.PlaybackSpeeds.First().Speed;
		}
		set
		{
			_appearanceAndBehaviourController.UpdatePlaybackSpeeds(value, _appearanceAndBehaviourController.PlaybackSpeeds.Last().Speed);
		}
	}

	public double MaxPlaybackSpeed
	{
		get
		{
			return _appearanceAndBehaviourController.PlaybackSpeeds.Last().Speed;
		}
		set
		{
			_appearanceAndBehaviourController.UpdatePlaybackSpeeds(_appearanceAndBehaviourController.PlaybackSpeeds.First().Speed, value);
		}
	}

	public TimeSpan MinSpan
	{
		get
		{
			return _appearanceAndBehaviourController.TimelineSpans.First().TimeSpan;
		}
		set
		{
			_appearanceAndBehaviourController.UpdateTimelineSpans(value, _appearanceAndBehaviourController.TimelineSpans.Last().TimeSpan);
		}
	}

	public TimeSpan MaxSpan
	{
		get
		{
			return _appearanceAndBehaviourController.TimelineSpans.Last().TimeSpan;
		}
		set
		{
			_appearanceAndBehaviourController.UpdateTimelineSpans(_appearanceAndBehaviourController.TimelineSpans.First().TimeSpan, value);
		}
	}

	public TimeZoneInfo TimeZone
	{
		get
		{
			return _appearanceAndBehaviourController.TimeZone;
		}
		set
		{
			_appearanceAndBehaviourController.TimeZone = value;
		}
	}

	public string UpperTimelineLabel
	{
		get
		{
			return _appearanceAndBehaviourController.UpperTimelineLabel;
		}
		set
		{
			_appearanceAndBehaviourController.UpperTimelineLabel = value;
		}
	}

	public string LowerTimelineLabel
	{
		get
		{
			return _appearanceAndBehaviourController.LowerTimelineLabel;
		}
		set
		{
			_appearanceAndBehaviourController.LowerTimelineLabel = value;
		}
	}

	public TimelineAreaDisplayMode TimelineAreaDisplayMode
	{
		get
		{
			return _appearanceAndBehaviourController.TimelineAreaDisplayMode;
		}
		set
		{
			_appearanceAndBehaviourController.TimelineAreaDisplayMode = value;
		}
	}

	public bool ShowLegend
	{
		get
		{
			return _appearanceAndBehaviourController.ShowLegend;
		}
		set
		{
			_appearanceAndBehaviourController.ShowLegend = value;
		}
	}

	public bool ShowUpperTimelineTopLine
	{
		get
		{
			return _appearanceAndBehaviourController.ShowUpperTimelineTopLine;
		}
		set
		{
			_appearanceAndBehaviourController.ShowUpperTimelineTopLine = value;
		}
	}

	public bool ShowUpperTimelineBottomLine
	{
		get
		{
			return _appearanceAndBehaviourController.ShowUpperTimelineBottomLine;
		}
		set
		{
			_appearanceAndBehaviourController.ShowUpperTimelineBottomLine = value;
		}
	}

	public bool ShowUpperTimelineCustomDataLine
	{
		get
		{
			return _appearanceAndBehaviourController.ShowUpperTimelineCustomDataLine;
		}
		set
		{
			_appearanceAndBehaviourController.ShowUpperTimelineCustomDataLine = value;
		}
	}

	public bool ShowLowerTimelineTopLine
	{
		get
		{
			return _appearanceAndBehaviourController.ShowLowerTimelineTopLine;
		}
		set
		{
			_appearanceAndBehaviourController.ShowLowerTimelineTopLine = value;
		}
	}

	public bool ShowLowerTimelineBottomLine
	{
		get
		{
			return _appearanceAndBehaviourController.ShowLowerTimelineBottomLine;
		}
		set
		{
			_appearanceAndBehaviourController.ShowLowerTimelineBottomLine = value;
		}
	}

	public bool ShowLowerTimelineCustomDataLine
	{
		get
		{
			return _appearanceAndBehaviourController.ShowLowerTimelineCustomDataLine;
		}
		set
		{
			_appearanceAndBehaviourController.ShowLowerTimelineCustomDataLine = value;
		}
	}

	public bool ShowControlsAreaUpper
	{
		get
		{
			return _appearanceAndBehaviourController.ShowControlsAreaUpper;
		}
		set
		{
			_appearanceAndBehaviourController.ShowControlsAreaUpper = value;
		}
	}

	public bool ShowControlsAreaLower
	{
		get
		{
			return _appearanceAndBehaviourController.ShowControlsAreaLower;
		}
		set
		{
			_appearanceAndBehaviourController.ShowControlsAreaLower = value;
		}
	}

	public bool ShowTimeSelectionControl
	{
		get
		{
			return _appearanceAndBehaviourController.ShowTimeSelectionControl;
		}
		set
		{
			_appearanceAndBehaviourController.ShowTimeSelectionControl = value;
		}
	}

	public bool ShowPlaybackSpeedControl
	{
		get
		{
			return _appearanceAndBehaviourController.ShowPlaybackSpeedControl;
		}
		set
		{
			_appearanceAndBehaviourController.ShowPlaybackSpeedControl = value;
		}
	}

	public bool ShowPrimaryNavigationControl
	{
		get
		{
			return _appearanceAndBehaviourController.ShowPrimaryNavigationControl;
		}
		set
		{
			_appearanceAndBehaviourController.ShowPrimaryNavigationControl = value;
		}
	}

	public bool ShowSecondaryNavigationControl
	{
		get
		{
			return _appearanceAndBehaviourController.ShowSecondaryNavigationControl;
		}
		set
		{
			_appearanceAndBehaviourController.ShowSecondaryNavigationControl = value;
		}
	}

	public bool ShowSpanControl
	{
		get
		{
			return _appearanceAndBehaviourController.ShowSpanControl;
		}
		set
		{
			_appearanceAndBehaviourController.ShowSpanControl = value;
		}
	}

	public bool ShowSetupControl
	{
		get
		{
			return _appearanceAndBehaviourController.ShowSetupControl;
		}
		set
		{
			_appearanceAndBehaviourController.ShowSetupControl = value;
		}
	}

	public bool ShowTimelineAreaPlaybackControl
	{
		get
		{
			return _appearanceAndBehaviourController.ShowTimelineAreaPlaybackControl;
		}
		set
		{
			_appearanceAndBehaviourController.ShowTimelineAreaPlaybackControl = value;
		}
	}

	public bool ShowSpanContextMenuItem
	{
		get
		{
			return _appearanceAndBehaviourController.ShowSpanContextMenuItem;
		}
		set
		{
			_appearanceAndBehaviourController.ShowSpanContextMenuItem = value;
		}
	}

	public bool ShowPlaybackPlayPauseControls
	{
		get
		{
			return _appearanceAndBehaviourController.ShowPlaybackPlayPauseControls;
		}
		set
		{
			_appearanceAndBehaviourController.ShowPlaybackPlayPauseControls = value;
		}
	}

	public bool UserSelectionMarkerTimeChangeEnabled
	{
		get
		{
			return _appearanceAndBehaviourController.UserSelectionMarkerTimeChangeEnabled;
		}
		set
		{
			_appearanceAndBehaviourController.UserSelectionMarkerTimeChangeEnabled = value;
		}
	}

	public bool UserSelectionControlEnabled
	{
		get
		{
			return _appearanceAndBehaviourController.UserSelectionControlEnabled;
		}
		set
		{
			_appearanceAndBehaviourController.UserSelectionControlEnabled = value;
		}
	}

	public bool UserPlaybackNavigationControlEnabled
	{
		get
		{
			return _appearanceAndBehaviourController.UserPlaybackNavigationControlEnabled;
		}
		set
		{
			_appearanceAndBehaviourController.UserPlaybackNavigationControlEnabled = value;
		}
	}

	public bool SelectionInfoAvailable
	{
		get
		{
			return _appearanceAndBehaviourController.SelectionInfoAvailable;
		}
		set
		{
			_appearanceAndBehaviourController.SelectionInfoAvailable = value;
		}
	}

	public string NoDataDisplayText
	{
		get
		{
			return _appearanceAndBehaviourController.NoDataDisplayText;
		}
		set
		{
			_appearanceAndBehaviourController.NoDataDisplayText = value;
		}
	}

	public TimeSpan AnimationDuration
	{
		get
		{
			return _timeAndSpanController.AnimationDuration;
		}
		set
		{
			_timeAndSpanController.AnimationDuration = value;
		}
	}

	public double PlaybackSliderMaxSpeed
	{
		get
		{
			return _appearanceAndBehaviourController.PlaybackSliderMaxSpeed;
		}
		set
		{
			_appearanceAndBehaviourController.PlaybackSliderMaxSpeed = value;
		}
	}

	public event EventHandler<DateTimeEventArgs> BrowseTimeChanged;

	public event EventHandler<DateTimeEventArgs> TimelineTimeChanged;

	public event EventHandler<TimeSpanEventArgs> SpanChanged;

	public event EventHandler<EventArgs> SelectionModeChanged;

	public event EventHandler<EventArgs> SelectionStateChanged;

	public event EventHandler<SelectionActiveMarkerChangeEventArgs> SelectionActiveMarkerChanging;

	public event EventHandler<SelectionActiveMarkerChangeEventArgs> SelectionActiveMarkerChanged;

	public event EventHandler<EventArgs> SelectionTimesChanged;

	public event EventHandler<PlaybackNavigationEventArgs> PlaybackNavigationOccurred;

	public event EventHandler<PlaybackStateEventArgs> PlaybackStateChanged;

	public event EventHandler<PlaybackSpeedEventArgs> PlaybackSpeedChanged;

	public event EventHandler<MarkerEventArgs> UpperTimelineMarkerHovered;

	public event EventHandler<MarkerEventArgs> LowerTimelineMarkerHovered;

	public event EventHandler<EventArgs> SetupClicked;

	static TimelineControl()
	{
		MarkerImageDarkTheme = new BitmapImage(new Uri("pack://application:,,,/VideoOS.UI.Common.WPF;component/Timeline/Control/Resources/Marker.png"));
		MarkerImageDarkTheme.Freeze();
		MarkerImageLightTheme = new BitmapImage(new Uri("pack://application:,,,/VideoOS.UI.Common.WPF;component/Timeline/Control/Resources/Marker.png"));
		MarkerImageLightTheme.Freeze();
	}

	public TimelineControl()
	{
		InitializeComponent();
		_timeAndSpanController.Init(base.Dispatcher);
		_timeAndSpanController.BrowseTimeChanged += _timeAndSpanController_BrowseTimeChanged;
		_timeAndSpanController.TimelineTimeChanged += _timeAndSpanController_TimelineTimeChanged;
		_timeAndSpanController.TargetTimelineSpanChanged += _timeAndSpanController_TargetTimelineSpanChanged;
		_timeAndSpanController.SelectionModeChangedEvent += _timeAndSpanController_SelectionModeChangedEvent;
		_timeAndSpanController.SelectionStateChangedEvent += _timeAndSpanController_SelectionStateChanged;
		_timeAndSpanController.SelectionActiveMarkerChangingEvent += _timeAndSpanController_SelectionActiveMarkerChangingEvent;
		_timeAndSpanController.SelectionActiveMarkerChangedEvent += _timeAndSpanController_SelectionActiveMarkerChangedEvent;
		_timeAndSpanController.SelectionFromTimeChanged += _timeAndSpanController_SelectionTimeChanged;
		_timeAndSpanController.SelectionCenterTimeChanged += _timeAndSpanController_SelectionTimeChanged;
		_timeAndSpanController.SelectionToTimeChanged += _timeAndSpanController_SelectionTimeChanged;
		_appearanceAndBehaviourController.PropertyChanged += _appearanceAndBehaviourController_PropertyChanged;
	}

	public void IncreaseTimelineSpan(TimeSpan animateDuration)
	{
		int num = _appearanceAndBehaviourController.TimelineSpans.FindIndex((TimelineSpan x) => x.TimeSpan == _timeAndSpanController.TimelineSpan);
		if (num < _appearanceAndBehaviourController.TimelineSpans.Count - 1)
		{
			_timeAndSpanController.SetSpan(_appearanceAndBehaviourController.TimelineSpans[num + 1].TimeSpan, animateDuration);
		}
	}

	public void DecreaseTimelineSpan(TimeSpan animateDuration)
	{
		int num = _appearanceAndBehaviourController.TimelineSpans.FindIndex((TimelineSpan x) => x.TimeSpan == _timeAndSpanController.TimelineSpan);
		if (num > 0)
		{
			_timeAndSpanController.SetSpan(_appearanceAndBehaviourController.TimelineSpans[num - 1].TimeSpan, animateDuration);
		}
	}

	public void Init(ITimelineSequenceCache timelineCache, ITimelineStyles timelineStyles)
	{
		ThemeManager.Instance.SelectedThemeChanged -= ThemeManager_SelectedThemeChanged;
		ThemeManager.Instance.SelectedThemeChanged += ThemeManager_SelectedThemeChanged;
		ApplyThemeColors();
		_appearanceAndBehaviourController.TimelineStyles = timelineStyles;
		_timelineArea.Init(_timeAndSpanController, _appearanceAndBehaviourController, timelineCache);
		_controlsAreaUpper.Init(_timeAndSpanController, _appearanceAndBehaviourController, timelineCache);
		_controlsAreaLower.Init(_timeAndSpanController, _appearanceAndBehaviourController, timelineCache);
	}

	public void Close()
	{
		ThemeManager.Instance.SelectedThemeChanged -= ThemeManager_SelectedThemeChanged;
		_timelineArea.Close();
		_controlsAreaUpper.Close();
		_controlsAreaLower.Close();
	}

	public void SetSequenceSources(IEnumerable<ISequenceSource> upperSequenceSources, IEnumerable<ISequenceSource> lowerSequenceSources)
	{
		_appearanceAndBehaviourController.SetSequenceSources(upperSequenceSources, lowerSequenceSources);
	}

	public void SetTime(DateTime time, TimeSpan animateDuration)
	{
		if (_timeAndSpanController.BrowseTime != time)
		{
			_timelineArea.StopThrow();
			_timeAndSpanController.SetTimelineTime(time, animateDuration);
		}
	}

	public void SetSpan(TimeSpan span, TimeSpan animateDuration)
	{
		if (!(_timeAndSpanController.TimelineSpan != span))
		{
			return;
		}
		_timelineArea.StopThrow();
		TimelineSpan timelineSpan = _appearanceAndBehaviourController.TimelineSpans.Last();
		foreach (TimelineSpan timelineSpan2 in _appearanceAndBehaviourController.TimelineSpans)
		{
			if (timelineSpan2.TimeSpan >= span)
			{
				timelineSpan = timelineSpan2;
				break;
			}
		}
		_timeAndSpanController.SetSpan(timelineSpan.TimeSpan, animateDuration);
	}

	public void FitTimeAndSpan(DateTime minTime, DateTime timelineTime, DateTime maxTime, bool allowSpanDecrease, bool allowSpanIncrease, TimeSpan animateDuration)
	{
		_timelineArea.StopThrow();
		_timeAndSpanController.FitTimeAndSpan(minTime, timelineTime, maxTime, allowSpanDecrease, allowSpanIncrease, _appearanceAndBehaviourController.TimelineSpans, animateDuration);
	}

	public void AddContextMenuItem(VideoOSContextMenuItem menuItem, ContextMenuMode contextMenuMode)
	{
		_timelineArea.AddContextMenuItem(menuItem, contextMenuMode);
	}

	public void RemoveContextMenuItem(VideoOSContextMenuItem menuItem, ContextMenuMode contextMenuMode)
	{
		_timelineArea.RemoveContextMenuItem(menuItem, contextMenuMode);
	}

	public void SetDefaultSelectionTimes()
	{
		DateTime defaultSelectionFromTime = _timeAndSpanController.DefaultSelectionFromTime;
		DateTime defaultSelectionCenterTime = _timeAndSpanController.DefaultSelectionCenterTime;
		DateTime defaultSelectionToTime = _timeAndSpanController.DefaultSelectionToTime;
		_timeAndSpanController.SetSelectionTimes(defaultSelectionFromTime, defaultSelectionCenterTime, defaultSelectionToTime, adjustToFromTimeIfNessesary: false, adjustToCenterTimeIfNessesary: false, adjustToToTimeIfNessesary: false);
	}

	private void _appearanceAndBehaviourController_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		ApplyAppearanceAndBehaviourControllerProperty(e.PropertyName);
	}

	private void ApplyAppearanceAndBehaviourControllerProperty(string propertyName)
	{
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.ShowControlsAreaUpper.ToString())
		{
			if (_appearanceAndBehaviourController.ShowControlsAreaUpper)
			{
				_controlsAreaUpper.Visibility = Visibility.Visible;
			}
			else
			{
				_controlsAreaUpper.Visibility = Visibility.Collapsed;
			}
		}
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.ShowControlsAreaLower.ToString())
		{
			if (_appearanceAndBehaviourController.ShowControlsAreaLower)
			{
				_controlsAreaLower.Visibility = Visibility.Visible;
			}
			else
			{
				_controlsAreaLower.Visibility = Visibility.Collapsed;
			}
		}
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.PlaybackState.ToString())
		{
			_timelineArea.StopThrow();
			if (this.PlaybackStateChanged != null)
			{
				this.PlaybackStateChanged(this, new PlaybackStateEventArgs(_appearanceAndBehaviourController.PlaybackState));
			}
		}
		if ((string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.PlaybackSpeed.ToString()) && this.PlaybackSpeedChanged != null)
		{
			this.PlaybackSpeedChanged(this, new PlaybackSpeedEventArgs(_appearanceAndBehaviourController.PlaybackSpeed.Speed));
		}
	}

	private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		_timelineArea.StopThrow();
	}

	private void TimelineControl_OnLoaded(object sender, RoutedEventArgs e)
	{
		ApplyAppearanceAndBehaviourControllerProperty(null);
	}

	private void _timelineArea_UpperTimelineMarkerHovered(object sender, MarkerEventArgs e)
	{
		if (this.UpperTimelineMarkerHovered != null)
		{
			this.UpperTimelineMarkerHovered(this, e);
		}
	}

	private void _timelineArea_LowerTimelineMarkerHovered(object sender, MarkerEventArgs e)
	{
		if (this.LowerTimelineMarkerHovered != null)
		{
			this.LowerTimelineMarkerHovered(this, e);
		}
	}

	private void playbackNavigationOccurred(object sender, PlaybackNavigationEventArgs e)
	{
		_timelineArea.StopThrow();
		if (this.PlaybackNavigationOccurred != null)
		{
			this.PlaybackNavigationOccurred(this, e);
		}
	}

	private void _controlsArea_SetupClicked(object sender, EventArgs e)
	{
		if (this.SetupClicked != null)
		{
			this.SetupClicked(this, new EventArgs());
		}
	}

	private void _timeAndSpanController_TargetTimelineSpanChanged(object sender, TimeSpanEventArgs e)
	{
		if (this.SpanChanged != null)
		{
			this.SpanChanged(this, e);
		}
	}

	private void _timeAndSpanController_BrowseTimeChanged(object sender, DateTimeEventArgs e)
	{
		if (this.BrowseTimeChanged != null)
		{
			this.BrowseTimeChanged(this, e);
		}
	}

	private void _timeAndSpanController_TimelineTimeChanged(object sender, DateTimeEventArgs e)
	{
		if (this.TimelineTimeChanged != null)
		{
			this.TimelineTimeChanged(this, e);
		}
	}

	private void _timeAndSpanController_SelectionTimeChanged(object sender, DateTimeEventArgs e)
	{
		if (this.SelectionTimesChanged != null)
		{
			this.SelectionTimesChanged(this, new EventArgs());
		}
	}

	private void _timeAndSpanController_SelectionModeChangedEvent(object sender, EventArgs e)
	{
		if (this.SelectionModeChanged != null)
		{
			this.SelectionModeChanged(this, new EventArgs());
		}
	}

	private void _timeAndSpanController_SelectionStateChanged(object sender, EventArgs e)
	{
		if (this.SelectionStateChanged != null)
		{
			this.SelectionStateChanged(this, new EventArgs());
		}
	}

	private void _timeAndSpanController_SelectionActiveMarkerChangingEvent(object sender, SelectionActiveMarkerChangeEventArgs e)
	{
		if (this.SelectionActiveMarkerChanging != null)
		{
			this.SelectionActiveMarkerChanging(this, e);
		}
	}

	private void _timeAndSpanController_SelectionActiveMarkerChangedEvent(object sender, SelectionActiveMarkerChangeEventArgs e)
	{
		if (this.SelectionActiveMarkerChanged != null)
		{
			this.SelectionActiveMarkerChanged(this, e);
		}
	}

	private void ThemeManager_SelectedThemeChanged(object sender, EventArgs e)
	{
		ApplyThemeColors();
	}

	private void ApplyThemeColors()
	{
		_appearanceAndBehaviourController.TimelineLineBackgroundBrush = (Brush)FindResource(VideoOSInternalResourceKeys.TimelineLineBackgroundBrush);
		_appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillTopLineLowZOrderBrush = (Brush)FindResource(VideoOSInternalResourceKeys.TimelineSequenceSourceVisualAppearanceFillTopLineLowZOrderBrush);
		_appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillTopLineHighZOrderBrush = (Brush)FindResource(VideoOSInternalResourceKeys.TimelineSequenceSourceVisualAppearanceFillTopLineHighZOrderBrush);
		_appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush = (Brush)FindResource(VideoOSInternalResourceKeys.TimelineSequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush);
		_appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush = (Brush)FindResource(VideoOSInternalResourceKeys.TimelineSequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush);
		_appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillExtraZOrder1Brush = (Brush)FindResource(VideoOSInternalResourceKeys.TimelineSequenceSourceVisualAppearanceFillExtraZOrder1Brush);
		_appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillExtraZOrder2Brush = (Brush)FindResource(VideoOSInternalResourceKeys.TimelineSequenceSourceVisualAppearanceFillExtraZOrder2Brush);
		_appearanceAndBehaviourController.SequenceSourceVisualAppearanceMarkerImage = ((ThemeManager.Instance.SelectedTheme == ThemeType.Dark) ? MarkerImageDarkTheme : MarkerImageLightTheme);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
