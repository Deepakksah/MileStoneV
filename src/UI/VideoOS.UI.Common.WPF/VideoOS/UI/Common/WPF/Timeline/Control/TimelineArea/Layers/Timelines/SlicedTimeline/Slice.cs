using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;
using VideoOS.UI.Common.WPF.Utils.Threading;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Timelines.SlicedTimeline;

public class Slice : FrameworkElement, IDisposable
{
	private class StreamGeometryDrawingObject
	{
		private class RectangleGeometry
		{
			public Brush Brush { get; private set; }

			public Pen Pen { get; private set; }

			public StreamGeometry Geometry { get; private set; }

			public RectangleGeometry(Brush brush, Pen pen, StreamGeometry geometry)
			{
				Brush = brush;
				Pen = pen;
				Geometry = geometry;
			}
		}

		private IList<RectangleGeometry> _geometries = new List<RectangleGeometry>();

		public void DrawRectangle(Brush brush, Pen pen, Rect rect)
		{
			StreamGeometry streamGeometry = new StreamGeometry();
			using (StreamGeometryContext streamGeometryContext = streamGeometry.Open())
			{
				streamGeometryContext.BeginFigure(rect.TopLeft, isFilled: true, isClosed: true);
				streamGeometryContext.PolyLineTo(new Point[3] { rect.TopRight, rect.BottomRight, rect.BottomLeft }, isStroked: true, isSmoothJoin: false);
			}
			streamGeometry.Freeze();
			RectangleGeometry item = new RectangleGeometry(brush, pen, streamGeometry);
			_geometries.Add(item);
		}

		public void Render(DrawingContext drawingObject)
		{
			foreach (RectangleGeometry geometry in _geometries)
			{
				drawingObject.DrawGeometry(geometry.Brush, geometry.Pen, geometry.Geometry);
			}
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[StreamGeometryDrawingObject] _geometries.Count = {0}", _geometries.Count);
		}
	}

	private static TaskScheduler _backgroundTaskScheduler;

	private ITimelineSequenceCache _timelineCache;

	private SequenceSourcesInfo _upperSequenceSourcesInfo = new SequenceSourcesInfo();

	private SequenceSourcesInfo _lowerSequenceSourcesInfo = new SequenceSourcesInfo();

	private bool _isNarrow;

	private bool _showUpperTimelineTopLine;

	private bool _showUpperTimelineBottomLine;

	private bool _showUpperTimelineCustomDataLine;

	private bool _showLowerTimelineTopLine;

	private bool _showLowerTimelineBottomLine;

	private bool _showLowerTimelineCustomDataLine;

	private Brush _topLineLowZOrderBrush;

	private Brush _topLineHighZOrderBrush;

	private Brush _bottomLineLowZOrderBrush;

	private Brush _bottomLineHighZOrderBrush;

	private Brush _customDataDefaultBrush;

	private ImageBrush _markerImageBrush;

	private Brush _fillExtraZOrder1Brush;

	private Brush _fillExtraZOrder2Brush;

	private int _markerImageBrushImageSourceWidth;

	private int _markerImageBrushImageSourceHeight;

	private bool _isInvalidated;

	private StreamGeometryDrawingObject _streamDrawingObject;

	private bool _active;

	private DateTime _beginTime;

	private TimeSpan _pixelTimeSpan;

	private long _cachedWith;

	private CancellationTokenSource _buildDrawingObjectCancellation = new CancellationTokenSource();

	private bool IsInvalidated
	{
		get
		{
			return _isInvalidated;
		}
		set
		{
			_isInvalidated = value;
		}
	}

	private StreamGeometryDrawingObject StreamDrawingObject
	{
		get
		{
			return _streamDrawingObject;
		}
		set
		{
			_streamDrawingObject = (Active ? value : null);
			InvalidateVisual();
		}
	}

	public bool Active
	{
		get
		{
			if (_active)
			{
				return base.Width > 0.0;
			}
			return false;
		}
		set
		{
			if (_active != value)
			{
				_active = value;
				Invalidate();
			}
		}
	}

	public DateTime BeginTime
	{
		get
		{
			return _beginTime;
		}
		set
		{
			if (_beginTime != value)
			{
				_beginTime = value;
				Invalidate();
			}
		}
	}

	public TimeSpan PixelTimeSpan
	{
		get
		{
			return _pixelTimeSpan;
		}
		set
		{
			if (_pixelTimeSpan != value)
			{
				_pixelTimeSpan = value;
				Invalidate();
			}
		}
	}

	public int CachedWith
	{
		get
		{
			return (int)Interlocked.Read(ref _cachedWith);
		}
		set
		{
			if (value != Interlocked.Exchange(ref _cachedWith, value))
			{
				Invalidate();
			}
		}
	}

	public DateTime EndTime => BeginTime.AddMilliseconds(PixelTimeSpan.TotalMilliseconds * base.Width);

	public Slice(ITimelineSequenceCache timelineCache)
	{
		if (_backgroundTaskScheduler == null)
		{
			DispatcherExtension.Create("Background slice renderer").Invoke(() => _backgroundTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext(), DispatcherPriority.ContextIdle);
		}
		_timelineCache = timelineCache;
		base.SnapsToDevicePixels = true;
		base.Loaded += Slice_Loaded;
		base.Unloaded += Slice_Unloaded;
	}

	protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
	{
		base.OnPropertyChanged(e);
		if (e.Property == FrameworkElement.WidthProperty)
		{
			CachedWith = (int)(double)e.NewValue;
		}
	}

	private void Slice_Unloaded(object sender, RoutedEventArgs e)
	{
		StreamDrawingObject = null;
	}

	private void Slice_Loaded(object sender, RoutedEventArgs e)
	{
		if (StreamDrawingObject == null)
		{
			Invalidate();
		}
	}

	public void SetSequenceSourcesInfo(SequenceSourcesInfo upperSequenceSourcesInfo, SequenceSourcesInfo lowerSequenceSourcesInfo)
	{
		_upperSequenceSourcesInfo = upperSequenceSourcesInfo;
		_lowerSequenceSourcesInfo = lowerSequenceSourcesInfo;
		Invalidate();
	}

	public void SetSliceAppearance(bool isNarrow, bool showUpperTimelineTopLine, bool showUpperTimelineBottomLine, bool showUpperTimelineCustomDataLine, bool showLowerTimelineTopLine, bool showLowerTimelineBottomLine, bool showLowerTimelineCustomDataLine, Brush topLineLowZOrderBrush, Brush topLineHighZOrderBrush, Brush bottomLineLowZOrderBrush, Brush bottomLineHighZOrderBrush, BitmapImage markerImage, Brush fillExtraZOrder1Brush, Brush fillExtraZOrder2Brush)
	{
		_isNarrow = isNarrow;
		_showUpperTimelineTopLine = showUpperTimelineTopLine;
		_showUpperTimelineBottomLine = showUpperTimelineBottomLine;
		_showUpperTimelineCustomDataLine = showUpperTimelineCustomDataLine;
		_showLowerTimelineTopLine = showLowerTimelineTopLine;
		_showLowerTimelineBottomLine = showLowerTimelineBottomLine;
		_showLowerTimelineCustomDataLine = showLowerTimelineCustomDataLine;
		_topLineLowZOrderBrush = topLineLowZOrderBrush;
		_topLineHighZOrderBrush = topLineHighZOrderBrush;
		_bottomLineLowZOrderBrush = bottomLineLowZOrderBrush;
		_bottomLineHighZOrderBrush = bottomLineHighZOrderBrush;
		_customDataDefaultBrush = new SolidColorBrush(Colors.LightBlue);
		_markerImageBrush = new ImageBrush(markerImage)
		{
			AlignmentY = AlignmentY.Top,
			TileMode = TileMode.None,
			Stretch = Stretch.None
		};
		_fillExtraZOrder1Brush = fillExtraZOrder1Brush;
		_fillExtraZOrder2Brush = fillExtraZOrder2Brush;
		if (markerImage != null)
		{
			_markerImageBrushImageSourceWidth = (int)markerImage.Width;
			_markerImageBrushImageSourceHeight = (int)markerImage.Height;
		}
		else
		{
			_markerImageBrushImageSourceWidth = 0;
			_markerImageBrushImageSourceHeight = 0;
		}
		Invalidate();
	}

	public void Update()
	{
		Invalidate();
		DoUpdate(updateOnUiThread: true);
	}

	private void UpdateAsync()
	{
		DoUpdate(updateOnUiThread: false);
	}

	private void DoUpdate(bool updateOnUiThread)
	{
		if (Active)
		{
			if (!updateOnUiThread && !IsInvalidated)
			{
				return;
			}
			IsInvalidated = false;
			if (updateOnUiThread)
			{
				StreamGeometryDrawingObject streamDrawingObject = BuildDrawingObject((int)base.Width, CancellationToken.None);
				StreamDrawingObject = streamDrawingObject;
				return;
			}
			_buildDrawingObjectCancellation = new CancellationTokenSource();
			Task<StreamGeometryDrawingObject> task = new Task<StreamGeometryDrawingObject>((object w) => BuildDrawingObject((int)w, _buildDrawingObjectCancellation.Token), (int)base.Width, _buildDrawingObjectCancellation.Token);
			task.ContinueWith((Task<StreamGeometryDrawingObject> t) => StreamDrawingObject = t.Result, _buildDrawingObjectCancellation.Token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());
			task.ContinueWith((Task<StreamGeometryDrawingObject> t) => StreamDrawingObject = null, TaskContinuationOptions.OnlyOnFaulted);
			task.Start(_backgroundTaskScheduler);
		}
		else
		{
			StreamDrawingObject = null;
		}
	}

	public void Invalidate()
	{
		_buildDrawingObjectCancellation.Cancel();
		if (Active && base.IsLoaded)
		{
			IsInvalidated = true;
			base.Dispatcher.BeginInvoke(UpdateAsync);
		}
	}

	protected override void OnRender(DrawingContext drawingContext)
	{
		base.OnRender(drawingContext);
		if (Active && StreamDrawingObject != null)
		{
			StreamDrawingObject.Render(drawingContext);
		}
	}

	private StreamGeometryDrawingObject BuildDrawingObject(int width, CancellationToken token)
	{
		StreamGeometryDrawingObject streamGeometryDrawingObject = new StreamGeometryDrawingObject();
		DrawContent(streamGeometryDrawingObject, width, token);
		return streamGeometryDrawingObject;
	}

	private void DrawContent(StreamGeometryDrawingObject drawingObject, int width, CancellationToken token)
	{
		bool flag = _showUpperTimelineTopLine || _showUpperTimelineBottomLine || _showUpperTimelineCustomDataLine;
		bool flag2 = _showLowerTimelineTopLine || _showLowerTimelineBottomLine || _showLowerTimelineCustomDataLine;
		if (flag)
		{
			GetTimelineHeight(showTimeline: true, _isNarrow);
			int upperTimelineOffset = GetUpperTimelineOffset(_isNarrow);
			DrawTimeline(drawingObject, _upperSequenceSourcesInfo, _showUpperTimelineTopLine, _showUpperTimelineBottomLine, _showUpperTimelineCustomDataLine, upperTimelineOffset, width, token);
		}
		if (flag2)
		{
			GetTimelineHeight(showTimeline: true, _isNarrow);
			int lowerTimelineOffset = GetLowerTimelineOffset(flag, _isNarrow);
			DrawTimeline(drawingObject, _lowerSequenceSourcesInfo, _showLowerTimelineTopLine, _showLowerTimelineBottomLine, _showLowerTimelineCustomDataLine, lowerTimelineOffset, width, token);
		}
	}

	public static int GetUpperTimelineOffset(bool isNarrow)
	{
		if (!isNarrow)
		{
			return 6;
		}
		return 4;
	}

	public static int GetTimelineHeight(bool showTimeline, bool isNarrow)
	{
		if (!showTimeline)
		{
			return 0;
		}
		if (!isNarrow)
		{
			return 14;
		}
		return 10;
	}

	public static int GetLowerTimelineOffset(bool showUpperTimeline, bool isNarrow)
	{
		if (showUpperTimeline)
		{
			if (!isNarrow)
			{
				return 26;
			}
			return 18;
		}
		return GetUpperTimelineOffset(isNarrow);
	}

	private void DrawTimeline(StreamGeometryDrawingObject drawingObject, SequenceSourcesInfo sequenceSourcesInfo, bool displayTopLine, bool displayBottomLine, bool displayCustomDataLine, int yOffset, int width, CancellationToken token)
	{
		DrawFillSequenceSources(drawingObject, sequenceSourcesInfo.FillExtraZOrder1SequenceSources, _fillExtraZOrder1Brush, width, _isNarrow ? 10 : 14, yOffset, token);
		DrawFillSequenceSources(drawingObject, sequenceSourcesInfo.FillExtraZOrder2SequenceSources, _fillExtraZOrder2Brush, width, _isNarrow ? 10 : 14, yOffset, token);
		if (displayTopLine)
		{
			int num = yOffset;
			int lineHeight;
			if (displayBottomLine && displayCustomDataLine)
			{
				num = num;
				lineHeight = (_isNarrow ? 4 : 6);
			}
			else if (displayBottomLine)
			{
				num = num;
				lineHeight = (_isNarrow ? 7 : 10);
			}
			else if (displayCustomDataLine)
			{
				num = num;
				lineHeight = (_isNarrow ? 7 : 10);
			}
			else
			{
				num = num;
				lineHeight = (_isNarrow ? 10 : 14);
			}
			DrawFillSequenceSources(drawingObject, sequenceSourcesInfo.FillTopLineLowZOrderSequenceSources, _topLineLowZOrderBrush, width, lineHeight, num, token);
			DrawFillSequenceSources(drawingObject, sequenceSourcesInfo.FillTopLineHighZOrderSequenceSources, _topLineHighZOrderBrush, width, lineHeight, num, token);
		}
		if (displayBottomLine)
		{
			int num2 = yOffset;
			int lineHeight2;
			if (displayTopLine && displayCustomDataLine)
			{
				num2 += (_isNarrow ? 4 : 6);
				lineHeight2 = (_isNarrow ? 3 : 4);
			}
			else if (displayTopLine)
			{
				num2 += (_isNarrow ? 7 : 10);
				lineHeight2 = (_isNarrow ? 3 : 4);
			}
			else if (displayCustomDataLine)
			{
				num2 = num2;
				lineHeight2 = (_isNarrow ? 5 : 7);
			}
			else
			{
				num2 = num2;
				lineHeight2 = (_isNarrow ? 10 : 14);
			}
			DrawFillSequenceSources(drawingObject, sequenceSourcesInfo.FillBottomLineLowZOrderSequenceSources, _bottomLineLowZOrderBrush, width, lineHeight2, num2, token);
			DrawFillSequenceSources(drawingObject, sequenceSourcesInfo.FillBottomLineHighZOrderSequenceSources, _bottomLineHighZOrderBrush, width, lineHeight2, num2, token);
		}
		if (displayCustomDataLine)
		{
			int num3 = yOffset;
			int lineHeight3;
			if (displayTopLine && displayBottomLine)
			{
				num3 += (_isNarrow ? 7 : 10);
				lineHeight3 = (_isNarrow ? 3 : 4);
			}
			else if (displayTopLine)
			{
				num3 += (_isNarrow ? 7 : 10);
				lineHeight3 = (_isNarrow ? 3 : 4);
			}
			else if (displayBottomLine)
			{
				num3 += (_isNarrow ? 5 : 7);
				lineHeight3 = (_isNarrow ? 5 : 7);
			}
			else
			{
				num3 = num3;
				lineHeight3 = (_isNarrow ? 10 : 14);
			}
			DrawFillSequenceSources(drawingObject, sequenceSourcesInfo.FillCustomDataLineSequenceSources, _customDataDefaultBrush, width, lineHeight3, num3, token);
		}
		DrawMarkerSequenceSources(drawingObject, sequenceSourcesInfo.MarkerSequenceSources, _markerImageBrush, width, _markerImageBrushImageSourceHeight, yOffset, (_markerImageBrushImageSourceWidth - 1) / 2, _markerImageBrushImageSourceWidth, token);
	}

	private void DrawFillSequenceSources(StreamGeometryDrawingObject drawingObject, IEnumerable<ISequenceSource> sequenceSources, Brush brush, int width, int lineHeight, int lineYOffset, CancellationToken token)
	{
		if (!sequenceSources.Any())
		{
			return;
		}
		ISequenceSource[] timelineData = _timelineCache.GetTimelineData(sequenceSources, BeginTime, PixelTimeSpan, width);
		for (int i = 0; i < timelineData.Length; i++)
		{
			if (token.IsCancellationRequested)
			{
				break;
			}
			if (timelineData[i] != null)
			{
				int num = i;
				do
				{
					i++;
				}
				while (i < timelineData.Length && timelineData[i] == timelineData[num]);
				drawingObject.DrawRectangle((timelineData[num].CustomDataBrush == null) ? brush : timelineData[num].CustomDataBrush, null, new Rect(num, lineYOffset, i - num, lineHeight));
				i--;
			}
		}
	}

	private void DrawMarkerSequenceSources(StreamGeometryDrawingObject drawingObject, IEnumerable<ISequenceSource> sequenceSources, Brush brush, int width, int lineHeight, int lineYOffset, int markerLeft, int markerWidth, CancellationToken token)
	{
		if (!sequenceSources.Any())
		{
			return;
		}
		DateTime beginTime = new DateTime(BeginTime.Ticks - PixelTimeSpan.Ticks * markerWidth);
		ISequenceSource[] timelineData = _timelineCache.GetTimelineData(sequenceSources, beginTime, PixelTimeSpan, width + markerWidth * 2);
		for (int i = 0; i < timelineData.Length; i++)
		{
			if (token.IsCancellationRequested)
			{
				break;
			}
			if (timelineData[i] != null)
			{
				drawingObject.DrawRectangle((timelineData[i].CustomDataBrush == null) ? brush : timelineData[i].CustomDataBrush, null, new Rect(i - markerLeft - markerWidth, lineYOffset, markerWidth, lineHeight));
			}
		}
	}

	public void Dispose()
	{
		_buildDrawingObjectCancellation.Dispose();
	}
}
