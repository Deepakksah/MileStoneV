using System;
using System.Collections.Generic;
using VideoOS.Platform.UI.Controls;
using VideoOS.UI.Common.WPF.Timeline.Events;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

namespace VideoOS.UI.Common.WPF.Timeline;

public interface ITimeline
{
	IEnumerable<ISequenceSource> UpperSequenceSources { get; }

	IEnumerable<ISequenceSource> LowerSequenceSources { get; }

	DateTime Time { get; set; }

	TimeSpan Span { get; set; }

	TimeSpan TimePerPixel { get; }

	SelectionMode SelectionMode { get; set; }

	SelectionState SelectionState { get; set; }

	SelectionMarker SelectionActiveMarker { get; }

	DateTime SelectionFromTime { get; set; }

	DateTime SelectionCenterTime { get; set; }

	DateTime SelectionToTime { get; set; }

	PlaybackState PlaybackState { get; set; }

	double PlaybackSpeed { get; set; }

	double MinPlaybackSpeed { get; set; }

	double MaxPlaybackSpeed { get; set; }

	TimeSpan MinSpan { get; set; }

	TimeSpan MaxSpan { get; set; }

	TimeZoneInfo TimeZone { get; set; }

	string UpperTimelineLabel { get; set; }

	string LowerTimelineLabel { get; set; }

	TimelineAreaDisplayMode TimelineAreaDisplayMode { get; set; }

	bool ShowUpperTimelineTopLine { get; set; }

	bool ShowUpperTimelineBottomLine { get; set; }

	bool ShowLowerTimelineTopLine { get; set; }

	bool ShowLowerTimelineBottomLine { get; set; }

	bool ShowControlsAreaUpper { get; set; }

	bool ShowControlsAreaLower { get; set; }

	bool ShowTimeSelectionControl { get; set; }

	bool ShowPlaybackSpeedControl { get; set; }

	bool ShowPrimaryNavigationControl { get; set; }

	bool ShowSecondaryNavigationControl { get; set; }

	bool ShowSpanControl { get; set; }

	bool ShowSetupControl { get; set; }

	bool ShowTimelineAreaPlaybackControl { get; set; }

	bool ShowSpanContextMenuItem { get; set; }

	bool SelectionInfoAvailable { get; set; }

	bool UserSelectionMarkerTimeChangeEnabled { get; set; }

	bool UserSelectionControlEnabled { get; set; }

	bool UserPlaybackNavigationControlEnabled { get; set; }

	event EventHandler<DateTimeEventArgs> TimeChanged;

	event EventHandler<TimeSpanEventArgs> SpanChanged;

	event EventHandler<EventArgs> SelectionModeChanged;

	event EventHandler<EventArgs> SelectionStateChanged;

	event EventHandler<SelectionActiveMarkerChangeEventArgs> SelectionActiveMarkerChanging;

	event EventHandler<SelectionActiveMarkerChangeEventArgs> SelectionActiveMarkerChanged;

	event EventHandler<EventArgs> SelectionTimesChanged;

	event EventHandler<PlaybackNavigationEventArgs> PlaybackNavigationOccurred;

	event EventHandler<PlaybackStateEventArgs> PlaybackStateChanged;

	event EventHandler<PlaybackSpeedEventArgs> PlaybackSpeedChanged;

	event EventHandler<MarkerEventArgs> UpperTimelineMarkerHovered;

	event EventHandler<MarkerEventArgs> LowerTimelineMarkerHovered;

	event EventHandler<EventArgs> SetupClicked;

	void SetSequenceSources(IEnumerable<ISequenceSource> upperSequenceSources, IEnumerable<ISequenceSource> lowerSequenceSources);

	void InvalidateSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime fromTime, DateTime toTime);

	IEnumerable<Sequence> GetSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime fromTime, DateTime toTime);

	void SetTime(DateTime time, TimeSpan animateDuration);

	void SetSpan(TimeSpan span, TimeSpan animateDuration);

	void FitTimeAndSpan(DateTime minTime, DateTime timelineTime, DateTime maxTime, bool allowSpanDecrease, bool allowSpanIncrease, TimeSpan animateDuration);

	void AddContextMenuItem(VideoOSContextMenuItem menuItem, ContextMenuMode contextMenuMode);

	void RemoveContextMenuItem(VideoOSContextMenuItem menuItem, ContextMenuMode contextMenuMode);
}
