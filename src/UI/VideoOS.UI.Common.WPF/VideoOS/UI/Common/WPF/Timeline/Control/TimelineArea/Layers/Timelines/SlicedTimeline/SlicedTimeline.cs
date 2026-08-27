using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Timelines.SlicedTimeline;

public partial class SlicedTimeline : Canvas, IComponentConnector
{
	private const int SLICE_WIDTH = 100;

	private bool _initialized;

	private TimeAndSpanController _timeAndSpanController;

	private AppearanceAndBehaviourController _appearanceAndBehaviourController;

	private ITimelineSequenceCache _timelineCache;

	private SequenceSourcesInfo _upperSequenceSourcesInfo = new SequenceSourcesInfo();

	private SequenceSourcesInfo _lowerSequenceSourcesInfo = new SequenceSourcesInfo();

	private List<Slice> _timelineSlices = new List<Slice>();

	private List<Slice> _freeTimelineSlices = new List<Slice>();

	private long _lastTicksPerPixel;

	private bool _delayedSpanTweenUpdatePending;

	public SlicedTimeline()
	{
		InitializeComponent();
	}

	public void Init(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, ITimelineSequenceCache timelineCache)
	{
		_timeAndSpanController = timeAndSpanController;
		_appearanceAndBehaviourController = appearanceAndBehaviourController;
		_timelineCache = timelineCache;
		_timeAndSpanController.TimelinePixelTimesChanged += _timeAndSpanController_TimelinePixelTimesChangedEvent;
		_timeAndSpanController.TimelineSpanTweenComplete += _timeAndSpanController_TimelineSpanTweenCompleteEvent;
		_appearanceAndBehaviourController.PropertyChanged += _appearanceAndBehaviourController_PropertyChanged;
		_timelineCache.NewSequencesAvailable += _timelineCache_NewSequencesAvailableEvent;
		_upperSequenceSourcesInfo = new SequenceSourcesInfo(_appearanceAndBehaviourController.UpperSequenceSources);
		_lowerSequenceSourcesInfo = new SequenceSourcesInfo(_appearanceAndBehaviourController.LowerSequenceSources);
		_initialized = true;
		SetAppearancesOfAllSlices();
		SetSequenceSourcesOfAllSlices();
		Update();
	}

	public void Close()
	{
		_timeAndSpanController.TimelinePixelTimesChanged -= _timeAndSpanController_TimelinePixelTimesChangedEvent;
		_timeAndSpanController.TimelineSpanTweenComplete -= _timeAndSpanController_TimelineSpanTweenCompleteEvent;
		_appearanceAndBehaviourController.PropertyChanged -= _appearanceAndBehaviourController_PropertyChanged;
		_timelineCache.NewSequencesAvailable -= _timelineCache_NewSequencesAvailableEvent;
		_initialized = false;
	}

	public bool IsPointOverUpperTimeline(Point point)
	{
		if (point.X >= 0.0 && point.X < base.ActualWidth)
		{
			int upperTimelineOffset = Slice.GetUpperTimelineOffset(_appearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Narrow);
			int timelineHeight = Slice.GetTimelineHeight(_appearanceAndBehaviourController.ShowUpperTimelineTopLine || _appearanceAndBehaviourController.ShowUpperTimelineBottomLine || _appearanceAndBehaviourController.ShowUpperTimelineCustomDataLine, _appearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Narrow);
			if (point.Y >= (double)upperTimelineOffset && point.Y < (double)(upperTimelineOffset + timelineHeight))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsPointOverLowerTimeline(Point point)
	{
		if (point.X >= 0.0 && point.X < base.ActualWidth)
		{
			int lowerTimelineOffset = Slice.GetLowerTimelineOffset(_appearanceAndBehaviourController.ShowUpperTimelineTopLine || _appearanceAndBehaviourController.ShowUpperTimelineBottomLine || _appearanceAndBehaviourController.ShowUpperTimelineCustomDataLine, _appearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Narrow);
			int timelineHeight = Slice.GetTimelineHeight(_appearanceAndBehaviourController.ShowLowerTimelineTopLine || _appearanceAndBehaviourController.ShowLowerTimelineBottomLine || _appearanceAndBehaviourController.ShowLowerTimelineCustomDataLine, _appearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Narrow);
			if (point.Y >= (double)lowerTimelineOffset && point.Y < (double)(lowerTimelineOffset + timelineHeight))
			{
				return true;
			}
		}
		return false;
	}

	private void _appearanceAndBehaviourController_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.TimelineAreaDisplayMode.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineTopLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineBottomLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineCustomDataLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineTopLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineBottomLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineCustomDataLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceFillTopLineLowZOrderBrush.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceFillTopLineHighZOrderBrush.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceMarkerImage.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceFillExtraZOrder1Brush.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceFillExtraZOrder2Brush.ToString())
		{
			SetAppearancesOfAllSlices();
		}
		else if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSources.ToString())
		{
			_upperSequenceSourcesInfo = new SequenceSourcesInfo(_appearanceAndBehaviourController.UpperSequenceSources);
			_lowerSequenceSourcesInfo = new SequenceSourcesInfo(_appearanceAndBehaviourController.LowerSequenceSources);
			SetSequenceSourcesOfAllSlices();
		}
	}

	private void SetAppearancesOfAllSlices()
	{
		foreach (Slice timelineSlice in _timelineSlices)
		{
			timelineSlice.SetSliceAppearance(_appearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Narrow, _appearanceAndBehaviourController.ShowUpperTimelineTopLine, _appearanceAndBehaviourController.ShowUpperTimelineBottomLine, _appearanceAndBehaviourController.ShowUpperTimelineCustomDataLine, _appearanceAndBehaviourController.ShowLowerTimelineTopLine, _appearanceAndBehaviourController.ShowLowerTimelineBottomLine, _appearanceAndBehaviourController.ShowLowerTimelineCustomDataLine, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillTopLineLowZOrderBrush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillTopLineHighZOrderBrush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceMarkerImage, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillExtraZOrder1Brush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillExtraZOrder2Brush);
		}
		foreach (Slice freeTimelineSlice in _freeTimelineSlices)
		{
			freeTimelineSlice.SetSliceAppearance(_appearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Narrow, _appearanceAndBehaviourController.ShowUpperTimelineTopLine, _appearanceAndBehaviourController.ShowUpperTimelineBottomLine, _appearanceAndBehaviourController.ShowUpperTimelineCustomDataLine, _appearanceAndBehaviourController.ShowLowerTimelineTopLine, _appearanceAndBehaviourController.ShowLowerTimelineBottomLine, _appearanceAndBehaviourController.ShowLowerTimelineCustomDataLine, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillTopLineLowZOrderBrush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillTopLineHighZOrderBrush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceMarkerImage, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillExtraZOrder1Brush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillExtraZOrder2Brush);
		}
	}

	private void SetSequenceSourcesOfAllSlices()
	{
		foreach (Slice timelineSlice in _timelineSlices)
		{
			timelineSlice.SetSequenceSourcesInfo(_upperSequenceSourcesInfo, _lowerSequenceSourcesInfo);
		}
		foreach (Slice freeTimelineSlice in _freeTimelineSlices)
		{
			freeTimelineSlice.SetSequenceSourcesInfo(_upperSequenceSourcesInfo, _lowerSequenceSourcesInfo);
		}
	}

	private void _timelineCache_NewSequencesAvailableEvent(object sender, SequenceSourcesEventArgs e)
	{
		int num = 0;
		foreach (Slice timelineSlice in _timelineSlices)
		{
			num++;
			foreach (SourceQueryResult result in e.Results)
			{
				if (result.IntersectsWith(timelineSlice.BeginTime, timelineSlice.EndTime))
				{
					timelineSlice.Invalidate();
					break;
				}
			}
		}
	}

	private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		Update();
	}

	private void _timeAndSpanController_TimelinePixelTimesChangedEvent(object sender, EventArgs e)
	{
		Update();
	}

	private void _timeAndSpanController_TimelineSpanTweenCompleteEvent(object sender, EventArgs e)
	{
		if (_delayedSpanTweenUpdatePending)
		{
			_delayedSpanTweenUpdatePending = false;
			base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Action)delegate
			{
				Update();
			});
		}
	}

	private void Update()
	{
		if (!_initialized)
		{
			return;
		}
		if (_timeAndSpanController.IsSpanTweening || _delayedSpanTweenUpdatePending)
		{
			foreach (Slice timelineSlice in _timelineSlices)
			{
				Canvas.SetLeft(timelineSlice, _timeAndSpanController.GetPixelOffset(timelineSlice.BeginTime));
				double scaleX = (double)timelineSlice.PixelTimeSpan.Ticks / (double)_timeAndSpanController.GetTicksPerPixel();
				timelineSlice.RenderTransform = new ScaleTransform(scaleX, 1.0);
			}
			_delayedSpanTweenUpdatePending = true;
			return;
		}
		bool flag = UpdateSlices();
		foreach (Slice timelineSlice2 in _timelineSlices)
		{
			int pixelOffset = _timeAndSpanController.GetPixelOffset(timelineSlice2.BeginTime);
			Canvas.SetLeft(timelineSlice2, pixelOffset);
		}
		if (!flag)
		{
			return;
		}
		foreach (Slice timelineSlice3 in _timelineSlices)
		{
			timelineSlice3.Update();
		}
	}

	private bool UpdateSlices()
	{
		bool result = false;
		if (_lastTicksPerPixel != _timeAndSpanController.GetTicksPerPixel())
		{
			_lastTicksPerPixel = _timeAndSpanController.GetTicksPerPixel();
			base.Children.Clear();
			_freeTimelineSlices.AddRange(_timelineSlices);
			_timelineSlices.Clear();
			result = true;
		}
		while (_timelineSlices.Count > 0 && _timeAndSpanController.GetPixelOffset(_timelineSlices.First().BeginTime) + 100 < 0)
		{
			RemoveSlice(_timelineSlices.First());
		}
		while (_timelineSlices.Count > 0 && (double)_timeAndSpanController.GetPixelOffset(_timelineSlices.Last().BeginTime) >= base.ActualWidth)
		{
			RemoveSlice(_timelineSlices.Last());
		}
		if (_timeAndSpanController.GetTicksPerPixel() > 0)
		{
			if (_timelineSlices.Count == 0)
			{
				_timelineSlices.Add(CreateSlice(_timeAndSpanController.GetTimelineMinTime(), 100));
			}
			while (true)
			{
				int pixelOffset = _timeAndSpanController.GetPixelOffset(_timelineSlices.First().BeginTime);
				if (pixelOffset <= 0)
				{
					break;
				}
				_timelineSlices.Insert(0, CreateSlice(_timeAndSpanController.GetPixelTime(pixelOffset - 100), 100));
			}
			while (true)
			{
				int pixelOffset2 = _timeAndSpanController.GetPixelOffset(_timelineSlices.Last().BeginTime);
				if (!((double)(pixelOffset2 + 100) < base.ActualWidth))
				{
					break;
				}
				_timelineSlices.Add(CreateSlice(_timeAndSpanController.GetPixelTime(pixelOffset2 + 100), 100));
			}
		}
		return result;
	}

	private void RemoveSlice(Slice slice)
	{
		slice.Active = false;
		base.Children.Remove(slice);
		_timelineSlices.Remove(slice);
		_freeTimelineSlices.Add(slice);
	}

	private Slice CreateSlice(DateTime beginTime, int width)
	{
		Slice slice;
		bool flag;
		if (_freeTimelineSlices.Count > 0)
		{
			slice = _freeTimelineSlices[0];
			_freeTimelineSlices.RemoveAt(0);
			slice.RenderTransform = new ScaleTransform(1.0, 1.0);
			flag = true;
		}
		else
		{
			slice = new Slice(_timelineCache);
			slice.Height = base.Height;
			slice.SetSequenceSourcesInfo(_upperSequenceSourcesInfo, _lowerSequenceSourcesInfo);
			slice.SetSliceAppearance(_appearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Narrow, _appearanceAndBehaviourController.ShowUpperTimelineTopLine, _appearanceAndBehaviourController.ShowUpperTimelineBottomLine, _appearanceAndBehaviourController.ShowUpperTimelineCustomDataLine, _appearanceAndBehaviourController.ShowLowerTimelineTopLine, _appearanceAndBehaviourController.ShowLowerTimelineBottomLine, _appearanceAndBehaviourController.ShowLowerTimelineCustomDataLine, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillTopLineLowZOrderBrush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillTopLineHighZOrderBrush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceMarkerImage, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillExtraZOrder1Brush, _appearanceAndBehaviourController.SequenceSourceVisualAppearanceFillExtraZOrder2Brush);
			flag = false;
		}
		slice.BeginTime = beginTime;
		slice.Width = width;
		slice.PixelTimeSpan = new TimeSpan(_timeAndSpanController.GetTicksPerPixel());
		slice.SnapsToDevicePixels = true;
		slice.Active = true;
		base.Children.Add(slice);
		if (flag)
		{
			slice.Update();
		}
		return slice;
	}
}
