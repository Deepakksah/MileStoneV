using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.Platform.UI.Controls;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.Events;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

namespace VideoOS.UI.Common.WPF.Timeline;

public partial class Timeline : UserControl, ITimeline, ITimelineStyles, IComponentConnector
{
	private ContextMenu _contextMenu = new ContextMenu();

	private ITimelineSequenceCache _timelineCache;

	public IEnumerable<ISequenceSource> UpperSequenceSources => _timelineControl.UpperSequenceSources;

	public IEnumerable<ISequenceSource> LowerSequenceSources => _timelineControl.LowerSequenceSources;

	public DateTime Time
	{
		get
		{
			return _timelineControl.BrowseTime;
		}
		set
		{
			SetTime(value.ToUniversalTime(), TimeSpan.Zero);
		}
	}

	public TimeSpan Span
	{
		get
		{
			return _timelineControl.Span;
		}
		set
		{
			SetSpan(value, TimeSpan.Zero);
		}
	}

	public TimeSpan TimePerPixel => _timelineControl.TimePerPixel;

	public SelectionMode SelectionMode
	{
		get
		{
			return _timelineControl.SelectionMode;
		}
		set
		{
			_timelineControl.SelectionMode = value;
		}
	}

	public SelectionState SelectionState
	{
		get
		{
			return _timelineControl.SelectionState;
		}
		set
		{
			_timelineControl.SelectionState = value;
		}
	}

	public SelectionMarker SelectionActiveMarker
	{
		get
		{
			return _timelineControl.SelectionActiveMarker;
		}
		set
		{
			_timelineControl.SelectionActiveMarker = value;
		}
	}

	public DateTime SelectionFromTime
	{
		get
		{
			return _timelineControl.SelectionFromTime;
		}
		set
		{
			_timelineControl.SelectionFromTime = value.ToUniversalTime();
		}
	}

	public DateTime SelectionCenterTime
	{
		get
		{
			return _timelineControl.SelectionCenterTime;
		}
		set
		{
			_timelineControl.SelectionCenterTime = value.ToUniversalTime();
		}
	}

	public DateTime SelectionToTime
	{
		get
		{
			return _timelineControl.SelectionToTime;
		}
		set
		{
			_timelineControl.SelectionToTime = value.ToUniversalTime();
		}
	}

	public PlaybackState PlaybackState
	{
		get
		{
			return _timelineControl.PlaybackState;
		}
		set
		{
			_timelineControl.PlaybackState = value;
		}
	}

	public double PlaybackSpeed
	{
		get
		{
			return _timelineControl.PlaybackSpeed;
		}
		set
		{
			_timelineControl.PlaybackSpeed = value;
		}
	}

	public double PlaybackPrimaryNavigationSpeed
	{
		get
		{
			return _timelineControl.PlaybackPrimaryNavigationSpeed;
		}
		set
		{
			_timelineControl.PlaybackPrimaryNavigationSpeed = value;
		}
	}

	public double MinPlaybackSpeed
	{
		get
		{
			return _timelineControl.MinPlaybackSpeed;
		}
		set
		{
			_timelineControl.MinPlaybackSpeed = value;
		}
	}

	public double MaxPlaybackSpeed
	{
		get
		{
			return _timelineControl.MaxPlaybackSpeed;
		}
		set
		{
			_timelineControl.MaxPlaybackSpeed = value;
		}
	}

	public TimeSpan MinSpan
	{
		get
		{
			return _timelineControl.MinSpan;
		}
		set
		{
			_timelineControl.MinSpan = value;
		}
	}

	public TimeSpan MaxSpan
	{
		get
		{
			return _timelineControl.MaxSpan;
		}
		set
		{
			_timelineControl.MaxSpan = value;
		}
	}

	public TimeZoneInfo TimeZone
	{
		get
		{
			return _timelineControl.TimeZone;
		}
		set
		{
			_timelineControl.TimeZone = value;
		}
	}

	public string UpperTimelineLabel
	{
		get
		{
			return _timelineControl.UpperTimelineLabel;
		}
		set
		{
			_timelineControl.UpperTimelineLabel = value;
		}
	}

	public string LowerTimelineLabel
	{
		get
		{
			return _timelineControl.LowerTimelineLabel;
		}
		set
		{
			_timelineControl.LowerTimelineLabel = value;
		}
	}

	public TimelineAreaDisplayMode TimelineAreaDisplayMode
	{
		get
		{
			return _timelineControl.TimelineAreaDisplayMode;
		}
		set
		{
			_timelineControl.TimelineAreaDisplayMode = value;
		}
	}

	public bool ShowLegend
	{
		get
		{
			return _timelineControl.ShowLegend;
		}
		set
		{
			_timelineControl.ShowLegend = value;
		}
	}

	public bool ShowUpperTimelineTopLine
	{
		get
		{
			return _timelineControl.ShowUpperTimelineTopLine;
		}
		set
		{
			_timelineControl.ShowUpperTimelineTopLine = value;
		}
	}

	public bool ShowUpperTimelineBottomLine
	{
		get
		{
			return _timelineControl.ShowUpperTimelineBottomLine;
		}
		set
		{
			_timelineControl.ShowUpperTimelineBottomLine = value;
		}
	}

	public bool ShowUpperTimelineCustomDataLine
	{
		get
		{
			return _timelineControl.ShowUpperTimelineCustomDataLine;
		}
		set
		{
			_timelineControl.ShowUpperTimelineCustomDataLine = value;
		}
	}

	public bool ShowLowerTimelineTopLine
	{
		get
		{
			return _timelineControl.ShowLowerTimelineTopLine;
		}
		set
		{
			_timelineControl.ShowLowerTimelineTopLine = value;
		}
	}

	public bool ShowLowerTimelineBottomLine
	{
		get
		{
			return _timelineControl.ShowLowerTimelineBottomLine;
		}
		set
		{
			_timelineControl.ShowLowerTimelineBottomLine = value;
		}
	}

	public bool ShowLowerTimelineCustomDataLine
	{
		get
		{
			return _timelineControl.ShowLowerTimelineCustomDataLine;
		}
		set
		{
			_timelineControl.ShowLowerTimelineCustomDataLine = value;
		}
	}

	public bool ShowControlsAreaUpper
	{
		get
		{
			return _timelineControl.ShowControlsAreaUpper;
		}
		set
		{
			_timelineControl.ShowControlsAreaUpper = value;
		}
	}

	public bool ShowControlsAreaLower
	{
		get
		{
			return _timelineControl.ShowControlsAreaLower;
		}
		set
		{
			_timelineControl.ShowControlsAreaLower = value;
		}
	}

	public bool ShowTimeSelectionControl
	{
		get
		{
			return _timelineControl.ShowTimeSelectionControl;
		}
		set
		{
			_timelineControl.ShowTimeSelectionControl = value;
		}
	}

	public bool ShowPlaybackSpeedControl
	{
		get
		{
			return _timelineControl.ShowPlaybackSpeedControl;
		}
		set
		{
			_timelineControl.ShowPlaybackSpeedControl = value;
		}
	}

	public bool ShowPrimaryNavigationControl
	{
		get
		{
			return _timelineControl.ShowPrimaryNavigationControl;
		}
		set
		{
			_timelineControl.ShowPrimaryNavigationControl = value;
		}
	}

	public bool ShowSecondaryNavigationControl
	{
		get
		{
			return _timelineControl.ShowSecondaryNavigationControl;
		}
		set
		{
			_timelineControl.ShowSecondaryNavigationControl = value;
		}
	}

	public bool ShowSpanControl
	{
		get
		{
			return _timelineControl.ShowSpanControl;
		}
		set
		{
			_timelineControl.ShowSpanControl = value;
		}
	}

	public bool ShowSetupControl
	{
		get
		{
			return _timelineControl.ShowSetupControl;
		}
		set
		{
			_timelineControl.ShowSetupControl = value;
		}
	}

	public bool ShowTimelineAreaPlaybackControl
	{
		get
		{
			return _timelineControl.ShowTimelineAreaPlaybackControl;
		}
		set
		{
			_timelineControl.ShowTimelineAreaPlaybackControl = value;
		}
	}

	public bool ShowSpanContextMenuItem
	{
		get
		{
			return _timelineControl.ShowSpanContextMenuItem;
		}
		set
		{
			_timelineControl.ShowSpanContextMenuItem = value;
		}
	}

	public bool ShowPlaybackPlayPauseControls
	{
		get
		{
			return _timelineControl.ShowPlaybackPlayPauseControls;
		}
		set
		{
			_timelineControl.ShowPlaybackPlayPauseControls = value;
		}
	}

	public bool UserSelectionMarkerTimeChangeEnabled
	{
		get
		{
			return _timelineControl.UserSelectionMarkerTimeChangeEnabled;
		}
		set
		{
			_timelineControl.UserSelectionMarkerTimeChangeEnabled = value;
		}
	}

	public bool UserSelectionControlEnabled
	{
		get
		{
			return _timelineControl.UserSelectionControlEnabled;
		}
		set
		{
			_timelineControl.UserSelectionControlEnabled = value;
		}
	}

	public bool UserPlaybackNavigationControlEnabled
	{
		get
		{
			return _timelineControl.UserPlaybackNavigationControlEnabled;
		}
		set
		{
			_timelineControl.UserPlaybackNavigationControlEnabled = value;
		}
	}

	public bool SelectionInfoAvailable
	{
		get
		{
			return _timelineControl.SelectionInfoAvailable;
		}
		set
		{
			_timelineControl.SelectionInfoAvailable = value;
		}
	}

	public string NoDataDisplayText
	{
		get
		{
			return _timelineControl.NoDataDisplayText;
		}
		set
		{
			_timelineControl.NoDataDisplayText = value;
		}
	}

	public TimeSpan AnimationDuration
	{
		get
		{
			return _timelineControl.AnimationDuration;
		}
		set
		{
			_timelineControl.AnimationDuration = value;
		}
	}

	public double PlaybackSliderMaxSpeed
	{
		get
		{
			return _timelineControl.PlaybackSliderMaxSpeed;
		}
		set
		{
			_timelineControl.PlaybackSliderMaxSpeed = value;
		}
	}

	public event EventHandler<DateTimeEventArgs> TimeChanged;

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

	public Timeline()
		: this(TimelineFactory.MockLevel)
	{
	}

	public Timeline(TimelineMockLevel mockLevel)
	{
		InitializeComponent();
		_timelineCache = TimelineFactory.BuildCache(mockLevel);
		UpdateCacheTime();
	}

	public void InitCacheSettings(TimelineCacheSettings timelineCacheSettings)
	{
		_timelineCache.InitConfig(timelineCacheSettings);
	}

	public void SetTime(DateTime time, TimeSpan animateDuration)
	{
		_timelineControl.SetTime(time.ToUniversalTime(), animateDuration);
	}

	public void SetSpan(TimeSpan span, TimeSpan animateDuration)
	{
		_timelineControl.SetSpan(span, animateDuration);
	}

	public void SetSequenceSources(IEnumerable<ISequenceSource> upperSequenceSources, IEnumerable<ISequenceSource> lowerSequenceSources)
	{
		ISequenceSource[] sequenceSources = upperSequenceSources.Concat(lowerSequenceSources).Distinct().ToArray();
		_timelineCache.SetSequenceSources(sequenceSources);
		_timelineControl.SetSequenceSources(upperSequenceSources, lowerSequenceSources);
	}

	public void InvalidateSequences(IEnumerable<Guid> sourceIds, DateTime fromTime, DateTime toTime)
	{
		_timelineCache.InvalidateSequences(sourceIds, fromTime, toTime);
	}

	public void InvalidateSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime fromTime, DateTime toTime)
	{
		_timelineCache.InvalidateSequences(sequenceSources, fromTime.ToUniversalTime(), toTime.ToUniversalTime());
	}

	public IEnumerable<Sequence> GetSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime fromTime, DateTime toTime)
	{
		return _timelineCache.GetSequences(sequenceSources, fromTime.ToUniversalTime(), toTime.ToUniversalTime());
	}

	public void FitTimeAndSpan(DateTime minTime, DateTime timelineTime, DateTime maxTime, bool allowSpanDecrease, bool allowSpanIncrease, TimeSpan animateDuration)
	{
		_timelineControl.FitTimeAndSpan(minTime.ToUniversalTime(), timelineTime.ToUniversalTime(), maxTime.ToUniversalTime(), allowSpanDecrease, allowSpanIncrease, animateDuration);
	}

	public void AddContextMenuItem(VideoOSContextMenuItem menuItem, ContextMenuMode contextMenuMode)
	{
		_timelineControl.AddContextMenuItem(menuItem, contextMenuMode);
	}

	public void RemoveContextMenuItem(VideoOSContextMenuItem menuItem, ContextMenuMode contextMenuMode)
	{
		_timelineControl.RemoveContextMenuItem(menuItem, contextMenuMode);
	}

	public void SetDefaultSelectionTimes()
	{
		_timelineControl.SetDefaultSelectionTimes();
	}

	public void IncreaseTimelineSpan()
	{
		_timelineControl.IncreaseTimelineSpan(TimeSpan.Zero);
	}

	public void DecreaseTimelineSpan()
	{
		_timelineControl.DecreaseTimelineSpan(TimeSpan.Zero);
	}

	private void UserControl_Loaded(object sender, RoutedEventArgs e)
	{
		_timelineControl.Init(_timelineCache, this);
	}

	private void UserControl_Unloaded(object sender, RoutedEventArgs e)
	{
		_timelineControl.Close();
	}

	private void _timelineControl_TimeChanged(object sender, DateTimeEventArgs e)
	{
		if (this.TimeChanged != null)
		{
			this.TimeChanged(this, e);
		}
	}

	private void _timelineControl_TimelineTimeChanged(object sender, DateTimeEventArgs e)
	{
		UpdateCacheTime();
	}

	private void _timelineControl_SpanChanged(object sender, TimeSpanEventArgs e)
	{
		UpdateCacheTime();
		if (this.SpanChanged != null)
		{
			this.SpanChanged(this, e);
		}
	}

	private void _timelineControl_SelectionTimesChanged(object sender, EventArgs e)
	{
		if (this.SelectionTimesChanged != null)
		{
			this.SelectionTimesChanged(this, e);
		}
	}

	private void _timelineControl_SelectionActiveMarkerChanging(object sender, SelectionActiveMarkerChangeEventArgs e)
	{
		if (this.SelectionActiveMarkerChanging != null)
		{
			this.SelectionActiveMarkerChanging(this, e);
		}
	}

	private void _timelineControl_SelectionActiveMarkerChanged(object sender, SelectionActiveMarkerChangeEventArgs e)
	{
		if (this.SelectionActiveMarkerChanged != null)
		{
			this.SelectionActiveMarkerChanged(this, e);
		}
	}

	private void _timelineControl_SelectionStateChanged(object sender, EventArgs e)
	{
		if (this.SelectionStateChanged != null)
		{
			this.SelectionStateChanged(this, e);
		}
	}

	private void _timelineControl_SelectionModeChanged(object sender, EventArgs e)
	{
		if (this.SelectionModeChanged != null)
		{
			this.SelectionModeChanged(this, e);
		}
	}

	private void _timelineControl_PlaybackNavigationOccurred(object sender, PlaybackNavigationEventArgs e)
	{
		if (this.PlaybackNavigationOccurred != null)
		{
			this.PlaybackNavigationOccurred(this, e);
		}
	}

	private void _timelineControl_PlaybackStateChanged(object sender, PlaybackStateEventArgs e)
	{
		if (this.PlaybackStateChanged != null)
		{
			this.PlaybackStateChanged(this, e);
		}
	}

	private void _timelineControl_PlaybackSpeedChanged(object sender, PlaybackSpeedEventArgs e)
	{
		if (this.PlaybackSpeedChanged != null)
		{
			this.PlaybackSpeedChanged(this, e);
		}
	}

	private void _timelineControl_UpperTimelineMarkerHovered(object sender, MarkerEventArgs e)
	{
		if (this.UpperTimelineMarkerHovered != null)
		{
			this.UpperTimelineMarkerHovered(this, e);
		}
	}

	private void _timelineControl_LowerTimelineMarkerHovered(object sender, MarkerEventArgs e)
	{
		if (this.LowerTimelineMarkerHovered != null)
		{
			this.LowerTimelineMarkerHovered(this, e);
		}
	}

	private void _timelineControl_SetupClicked(object sender, EventArgs e)
	{
		if (this.SetupClicked != null)
		{
			this.SetupClicked(this, new EventArgs());
		}
	}

	private void UpdateCacheTime()
	{
		if (_timelineCache != null)
		{
			_timelineCache.SetCurrentTimeInterval(new TimeInterval(_timelineControl.TimelineTime.AddTicks(-_timelineControl.Span.Ticks / 2), _timelineControl.Span));
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	object ITimelineStyles.FindResource(object resourceKey)
	{
		return FindResource(resourceKey);
	}

	object ITimelineStyles.TryFindResource(object resourceKey)
	{
		return TryFindResource(resourceKey);
	}
}
