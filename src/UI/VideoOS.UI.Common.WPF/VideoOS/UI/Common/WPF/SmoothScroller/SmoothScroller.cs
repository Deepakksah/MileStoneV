using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using VideoOS.UI.Common.WPF.Tween;

namespace VideoOS.UI.Common.WPF.SmoothScroller;

public partial class SmoothScroller : UserControl, IComponentConnector
{
	private class DragMouseMoveInfo
	{
		public DateTime Time { get; set; }

		public double Position { get; set; }
	}

	private class ItemDataCache
	{
		public object[] ItemDataArray { get; set; }

		public int BeginIndex { get; set; }

		public int EndIndex { get; set; }
	}

	public delegate object[] GetItemsEventHandler(object sender, GetItemsEventArgs e);

	public delegate void ItemMouseClickEventHandler(object sender, ItemEventArgs e);

	public delegate void MouseOverItemChangedEventHandler(object sender, ItemEventArgs e);

	public delegate void FirstVisibleItemChangedEventHandler(object sender, ItemEventArgs e);

	public delegate void LastVisibleItemChangedEventHandler(object sender, ItemEventArgs e);

	public delegate void IsUserDraggingChangedEventHandler(object sender, EventArgs e);

	private delegate void DoScrollDelegate();

	public const int MaximumMaxIndex = 1000000000;

	public const int MinimumMinIndex = -1000000000;

	private const double _throwingScrollDeaccellerationFactor = 0.98;

	private bool _initialized;

	private Size _itemSizeExternal = new Size(200.0, 160.0);

	private Size _itemSizeInternal = new Size(200.0, 160.0);

	private bool _fillBeginEndSlices;

	private bool _fillWithSlices;

	private int _extraBeginEndSlices;

	private int _minIndexExternal = -1000000000;

	private int _maxIndexExternal = 1000000000;

	private int _minIndexInternal = -1000000000;

	private int _maxIndexInternal = 1000000000;

	private int _bufferedSlices = 2;

	private int _scrollFPS = 30;

	private Orientation _scrollOrientation;

	private int _itemsPerSlice;

	private double _dragLastMousePosition = double.NaN;

	private List<Slice> _sliceList = new List<Slice>();

	private List<Slice> _freeSliceList = new List<Slice>();

	private List<FrameworkElement> _freeItemRendererList = new List<FrameworkElement>();

	private double _scrollSpeed;

	private double _scrollDeaccellerationFactor = 1.0;

	private DragMouseMoveInfo _dragMouseMoveInfoNew;

	private DragMouseMoveInfo _dragMouseMoveInfoOld;

	private DateTime _dragLastMouseMoveTime;

	private ScrollTimerThread _scrollTimerThread;

	private VideoOS.UI.Common.WPF.Tween.Tween _scrollSlicesTween;

	private bool _moveAllowed;

	private ItemDataCache _itemDataCache = new ItemDataCache();

	public static DependencyProperty ItemRendererTypeProperty = DependencyProperty.Register("ItemRendererType", typeof(Type), typeof(SmoothScroller), new PropertyMetadata(typeof(DefaultItemRenderer), ItemRendererType_PropertyChanged, ItemRendererType_CoerceValue));

	private object __firstVisibleItem;

	private object __lastVisibleItem;

	public static readonly DependencyProperty MouseOverItemCommandProperty = DependencyProperty.Register("MouseOverItemCommand", typeof(ICommand), typeof(SmoothScroller));

	private object __mouseOverItem;

	private bool __isUserDragging;

	public Size ItemSize
	{
		get
		{
			return _itemSizeExternal;
		}
		set
		{
			if (!_itemSizeExternal.Equals(value))
			{
				_itemSizeExternal = value;
				UpdateScrollerLayout();
			}
		}
	}

	public int MinIndex
	{
		get
		{
			return _minIndexExternal;
		}
		set
		{
			if (value > 1000000000 || value < -1000000000)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (_minIndexExternal != value)
			{
				_minIndexExternal = value;
				UpdateScrollerLayout();
			}
		}
	}

	public int MaxIndex
	{
		get
		{
			return _maxIndexExternal;
		}
		set
		{
			if (value > 1000000000 || value < -1000000000)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (_maxIndexExternal != value)
			{
				_maxIndexExternal = value;
				UpdateScrollerLayout();
			}
		}
	}

	public int ScrollFPS
	{
		get
		{
			return _scrollFPS;
		}
		set
		{
			if (_scrollFPS != value)
			{
				_scrollFPS = value;
			}
		}
	}

	public Orientation ScrollOrientation
	{
		get
		{
			return _scrollOrientation;
		}
		set
		{
			if (_scrollOrientation != value)
			{
				_scrollOrientation = value;
				UpdateScrollerLayout();
			}
		}
	}

	public bool FillBeginEndSlices
	{
		get
		{
			return _fillBeginEndSlices;
		}
		set
		{
			if (_fillBeginEndSlices != value)
			{
				_fillBeginEndSlices = value;
				UpdateScrollerLayout();
			}
		}
	}

	public bool FillWithSlices
	{
		get
		{
			return _fillWithSlices;
		}
		set
		{
			if (_fillWithSlices != value)
			{
				_fillWithSlices = value;
				UpdateScrollerLayout();
			}
		}
	}

	public int ExtraBeginEndSlices
	{
		get
		{
			return _extraBeginEndSlices;
		}
		set
		{
			if (_extraBeginEndSlices != value)
			{
				_extraBeginEndSlices = value;
				UpdateScrollerLayout();
			}
		}
	}

	public int BufferedSlices
	{
		get
		{
			return _bufferedSlices;
		}
		set
		{
			if (_bufferedSlices != value)
			{
				_bufferedSlices = value;
				UpdateScrollerLayout();
			}
		}
	}

	public Type ItemRendererType
	{
		get
		{
			return (Type)GetValue(ItemRendererTypeProperty);
		}
		set
		{
			SetValue(ItemRendererTypeProperty, value);
		}
	}

	public object FirstVisibleItem
	{
		get
		{
			return __firstVisibleItem;
		}
		private set
		{
			if (value != __firstVisibleItem)
			{
				__firstVisibleItem = value;
				if (this.FirstVisibleItemChangedEvent != null)
				{
					this.FirstVisibleItemChangedEvent(this, new ItemEventArgs(__firstVisibleItem));
				}
			}
		}
	}

	public object LastVisibleItem
	{
		get
		{
			return __lastVisibleItem;
		}
		private set
		{
			if (value != __lastVisibleItem)
			{
				__lastVisibleItem = value;
				if (this.LastVisibleItemChangedEvent != null)
				{
					this.LastVisibleItemChangedEvent(this, new ItemEventArgs(__lastVisibleItem));
				}
			}
		}
	}

	public ICommand MouseOverItemCommand
	{
		get
		{
			return (ICommand)GetValue(MouseOverItemCommandProperty);
		}
		set
		{
			SetValue(MouseOverItemCommandProperty, value);
		}
	}

	public object MouseOverItem
	{
		get
		{
			return __mouseOverItem;
		}
		private set
		{
			if (value != __mouseOverItem)
			{
				__mouseOverItem = value;
				if (this.MouseOverItemChangedEvent != null)
				{
					this.MouseOverItemChangedEvent(this, new ItemEventArgs(__mouseOverItem));
				}
				if (MouseOverItemCommand != null && MouseOverItemCommand.CanExecute(__mouseOverItem))
				{
					MouseOverItemCommand.Execute(__mouseOverItem);
				}
			}
		}
	}

	public bool IsUserDragging
	{
		get
		{
			return __isUserDragging;
		}
		private set
		{
			if (value != __isUserDragging)
			{
				__isUserDragging = value;
				if (this.IsUserDraggingChangedEvent != null)
				{
					this.IsUserDraggingChangedEvent(this, new EventArgs());
				}
			}
		}
	}

	public event GetItemsEventHandler GetItemsEvent;

	public event ItemMouseClickEventHandler ItemMouseClickEvent;

	public event MouseOverItemChangedEventHandler MouseOverItemChangedEvent;

	public event FirstVisibleItemChangedEventHandler FirstVisibleItemChangedEvent;

	public event LastVisibleItemChangedEventHandler LastVisibleItemChangedEvent;

	public event IsUserDraggingChangedEventHandler IsUserDraggingChangedEvent;

	public SmoothScroller()
	{
		InitializeComponent();
	}

	private static object ItemRendererType_CoerceValue(DependencyObject obj, object value)
	{
		if (value == null)
		{
			return typeof(DefaultItemRenderer);
		}
		return value;
	}

	private static void ItemRendererType_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
	{
		if (obj is SmoothScroller smoothScroller)
		{
			smoothScroller.OnItemRendererTypePropertyChanged();
		}
	}

	private void OnItemRendererTypePropertyChanged()
	{
		Slice centerSlice = GetCenterSlice();
		int num = int.MinValue;
		Point initialSlicePos = default(Point);
		if (centerSlice != null)
		{
			num = centerSlice.BeginIndex;
			initialSlicePos = ((_scrollOrientation != Orientation.Horizontal) ? new Point(0.0, Canvas.GetTop(centerSlice)) : new Point(Canvas.GetLeft(centerSlice), 0.0));
		}
		RemoveAllSlices();
		_freeItemRendererList.Clear();
		if (num != int.MinValue)
		{
			UpdateScrollerLayout(num, initialSlicePos);
		}
		else
		{
			UpdateScrollerLayout();
		}
	}

	private object[] FireGetItemsEventHandler(int beginIndex, int endIndex)
	{
		if (this.GetItemsEvent != null)
		{
			return this.GetItemsEvent(this, new GetItemsEventArgs(beginIndex, endIndex));
		}
		return new object[endIndex - beginIndex + 1];
	}

	private void FireItemMouseClickEvent(object item)
	{
		if (this.ItemMouseClickEvent != null)
		{
			this.ItemMouseClickEvent(this, new ItemEventArgs(item));
		}
	}

	public void Close()
	{
		_itemDataCache = new ItemDataCache();
		RemoveAllSlices();
		_minIndexExternal = -1000000000;
		_maxIndexExternal = 1000000000;
		_initialized = false;
	}

	public void Initialize()
	{
		Initialize(-1000000000, 1000000000);
	}

	public void Initialize(int minIndex, int maxIndex)
	{
		if (minIndex > 1000000000 || minIndex < -1000000000)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (maxIndex > 1000000000 || maxIndex < -1000000000)
		{
			throw new ArgumentOutOfRangeException();
		}
		Close();
		_minIndexExternal = minIndex;
		_maxIndexExternal = maxIndex;
		_initialized = true;
		UpdateScrollerLayout();
	}

	public Rect GetItemBounds(object item)
	{
		foreach (Slice slice in _sliceList)
		{
			for (int i = slice.BeginIndex; i <= slice.EndIndex; i++)
			{
				FrameworkElement itemRenderer = slice.GetItemRenderer(i);
				if (itemRenderer.DataContext == item)
				{
					Point point = new Point(Canvas.GetLeft(itemRenderer), Canvas.GetTop(itemRenderer));
					point = PointFromScreen(slice.PointToScreen(point));
					return new Rect(point.X, point.Y, itemRenderer.ActualWidth, itemRenderer.ActualHeight);
				}
			}
		}
		return Rect.Empty;
	}

	public void StartScroll(double speed)
	{
		if (_initialized)
		{
			StartScroll(speed, 1.0);
		}
	}

	public void StartScroll(double speed, double deaccellerationFactor)
	{
		if (!_initialized)
		{
			return;
		}
		if (speed == 0.0)
		{
			StopScroll();
			return;
		}
		_scrollSpeed = speed * 100.0;
		_scrollDeaccellerationFactor = deaccellerationFactor;
		if (_scrollTimerThread == null)
		{
			_scrollTimerThread = new ScrollTimerThread(1000 / _scrollFPS, new DoScrollDelegate(DoScroll), base.Dispatcher);
		}
	}

	public void StopScroll()
	{
		if (_initialized)
		{
			if (_scrollTimerThread != null)
			{
				_scrollTimerThread.Stop();
				_scrollTimerThread = null;
			}
			_scrollSpeed = 0.0;
			if (_scrollSlicesTween != null && _scrollSlicesTween.IsTweening)
			{
				_scrollSlicesTween.Stop();
			}
		}
	}

	public void ScrollToVisible(object item)
	{
		ScrollToVisible(item, TimeSpan.Zero);
	}

	public void ScrollToVisible(object item, TimeSpan animateDuration)
	{
		if (!_initialized)
		{
			return;
		}
		Rect itemBounds = GetItemBounds(item);
		if (itemBounds == Rect.Empty)
		{
			return;
		}
		double num = 0.0;
		if (_scrollOrientation == Orientation.Horizontal)
		{
			if (itemBounds.Left < 0.0)
			{
				num = 0.0 - itemBounds.Left;
			}
			else if (itemBounds.Right >= _scrollerCanvas.ActualWidth)
			{
				num = _scrollerCanvas.ActualWidth - itemBounds.Right;
			}
		}
		else if (itemBounds.Top < 0.0)
		{
			num = 0.0 - itemBounds.Top;
		}
		else if (itemBounds.Bottom >= _scrollerCanvas.ActualHeight)
		{
			num = _scrollerCanvas.ActualHeight - itemBounds.Bottom;
		}
		if (num != 0.0)
		{
			StopScroll();
			ScrollPixels(num, animateDuration);
		}
	}

	public void ScrollToCenter(object item)
	{
		ScrollToCenter(item, TimeSpan.Zero);
	}

	public void ScrollToCenter(object item, TimeSpan animateDuration)
	{
		if (!_initialized)
		{
			return;
		}
		Rect itemBounds = GetItemBounds(item);
		if (!(itemBounds == Rect.Empty))
		{
			double num = 0.0;
			num = ((_scrollOrientation != Orientation.Horizontal) ? (_scrollerCanvas.ActualHeight / 2.0 - (itemBounds.Top + itemBounds.Height / 2.0)) : (_scrollerCanvas.ActualWidth / 2.0 - (itemBounds.Left + itemBounds.Width / 2.0)));
			if (num != 0.0)
			{
				StopScroll();
				ScrollPixels(num, animateDuration);
			}
		}
	}

	public void ScrollPages(int pages)
	{
		ScrollPages(pages, TimeSpan.Zero);
	}

	public void ScrollPages(int pages, TimeSpan animateDuration)
	{
		if (_initialized)
		{
			double pixels = ((_scrollOrientation != Orientation.Horizontal) ? ((0.0 - _scrollerCanvas.ActualHeight) * (double)pages) : ((0.0 - _scrollerCanvas.ActualWidth) * (double)pages));
			ScrollPixels(pixels, animateDuration);
		}
	}

	public void ScrollSlices(int slices)
	{
		ScrollSlices(slices, TimeSpan.Zero);
	}

	public void ScrollSlices(int slices, TimeSpan animateDuration)
	{
		if (_initialized)
		{
			double pixels = ((_scrollOrientation != Orientation.Horizontal) ? ((0.0 - _itemSizeInternal.Height) * (double)slices) : ((0.0 - _itemSizeInternal.Width) * (double)slices));
			ScrollPixels(pixels, animateDuration);
		}
	}

	public void ScrollPixels(double pixels)
	{
		ScrollPixels(pixels, TimeSpan.Zero);
	}

	public void ScrollPixels(double pixels, TimeSpan animateDuration)
	{
		if (!_initialized)
		{
			return;
		}
		if (animateDuration == TimeSpan.Zero)
		{
			MoveSlices(pixels, updateItems: true);
			return;
		}
		if (_scrollSlicesTween == null)
		{
			_scrollSlicesTween = new VideoOS.UI.Common.WPF.Tween.Tween(base.Dispatcher);
			_scrollSlicesTween.TweenUpdateEvent += _scrollSlicesTween_TweenUpdateEvent;
			_scrollSlicesTween.TweenType = TweenType.EaseOutCubic;
		}
		double num = 0.0;
		if (_scrollSlicesTween.IsTweening)
		{
			num = _scrollSlicesTween.EndValues[0] - _scrollSlicesTween.CurrentValues[0];
		}
		StopScroll();
		_scrollSlicesTween.Duration = animateDuration;
		_scrollSlicesTween.StartValues = new double[1];
		_scrollSlicesTween.EndValues = new double[1] { pixels + num };
		_scrollSlicesTween.FPS = _scrollFPS;
		_scrollSlicesTween.Start();
	}

	private void _scrollSlicesTween_TweenUpdateEvent(object sender, TweenEventArgs e)
	{
		MoveSlices(e.NewValues[0] - e.OldValues[0], updateItems: true);
	}

	private void UserControl_Unloaded(object sender, RoutedEventArgs e)
	{
		StopScroll();
	}

	private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (_initialized && e.NewSize.Height != 0.0 && e.NewSize.Width != 0.0)
		{
			UpdateScrollerLayout();
		}
	}

	private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if (_initialized)
		{
			StopScroll();
			if (_scrollOrientation == Orientation.Horizontal)
			{
				_dragLastMousePosition = e.GetPosition(_dragRectangle).X;
			}
			else
			{
				_dragLastMousePosition = e.GetPosition(_dragRectangle).Y;
			}
			_dragMouseMoveInfoNew = new DragMouseMoveInfo
			{
				Position = _dragLastMousePosition,
				Time = DateTime.Now
			};
			_dragMouseMoveInfoOld = null;
			IsUserDragging = true;
		}
	}

	private void Rectangle_MouseMove(object sender, MouseEventArgs e)
	{
		if (!_initialized)
		{
			return;
		}
		if (!double.IsNaN(_dragLastMousePosition))
		{
			double num = ((_scrollOrientation != Orientation.Horizontal) ? e.GetPosition(_dragRectangle).Y : e.GetPosition(_dragRectangle).X);
			double num2 = num - _dragLastMousePosition;
			MoveSlices(num2, updateItems: true);
			_dragLastMousePosition += num2;
			_dragLastMouseMoveTime = DateTime.Now;
			if (Math.Abs(_dragMouseMoveInfoNew.Position - num) >= 50.0 && new TimeSpan(DateTime.Now.Ticks - _dragMouseMoveInfoNew.Time.Ticks).TotalMilliseconds >= 100.0)
			{
				_dragMouseMoveInfoOld = _dragMouseMoveInfoNew;
				_dragMouseMoveInfoNew = new DragMouseMoveInfo
				{
					Position = _dragLastMousePosition,
					Time = DateTime.Now
				};
			}
		}
		UpdateMouseOverItem(e.GetPosition(_scrollerCanvas));
	}

	private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
	{
		if (_initialized)
		{
			_dragLastMousePosition = double.NaN;
			UpdateMouseOverItem(new Point(-1.0, -1.0));
			IsUserDragging = false;
		}
	}

	private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
	{
		if (!_initialized)
		{
			return;
		}
		_dragLastMousePosition = double.NaN;
		if (_dragMouseMoveInfoNew == null || _dragMouseMoveInfoOld == null)
		{
			Point position = e.GetPosition(_scrollerCanvas);
			foreach (Slice slice in _sliceList)
			{
				Rect rect = new Rect(Canvas.GetLeft(slice), Canvas.GetTop(slice), slice.ActualWidth, slice.ActualHeight);
				if (rect.Contains(position))
				{
					FrameworkElement itemRendererAtPoint = slice.GetItemRendererAtPoint(position.X - rect.Left, position.Y - rect.Top);
					if (itemRendererAtPoint != null)
					{
						FireItemMouseClickEvent(itemRendererAtPoint.DataContext);
						break;
					}
				}
			}
		}
		else if (new TimeSpan(DateTime.Now.Ticks - _dragLastMouseMoveTime.Ticks).TotalMilliseconds < 50.0)
		{
			double num = _dragMouseMoveInfoNew.Position - _dragMouseMoveInfoOld.Position;
			double totalSeconds = new TimeSpan(_dragMouseMoveInfoNew.Time.Ticks - _dragMouseMoveInfoOld.Time.Ticks).TotalSeconds;
			double num2 = num / totalSeconds;
			StartScroll(0.0 - num2 / 3200.0, 0.98);
		}
		IsUserDragging = false;
	}

	private void DoScroll()
	{
		MoveSlices(-(int)(_scrollSpeed / (50.0 / (1000.0 / (double)_scrollFPS))), updateItems: true);
		if (_scrollDeaccellerationFactor != 1.0)
		{
			_scrollSpeed *= _scrollDeaccellerationFactor;
			if (Math.Abs(_scrollSpeed) < 5.0)
			{
				StopScroll();
			}
		}
	}

	private void UpdateScrollerLayout()
	{
		Slice centerSlice = GetCenterSlice();
		int initialSliceBeginIndex;
		Point initialSlicePos;
		if (centerSlice == null)
		{
			initialSliceBeginIndex = (_maxIndexExternal - _minIndexExternal) / 2 + _minIndexExternal;
			initialSlicePos = ((_scrollOrientation != Orientation.Horizontal) ? new Point(0.0, _scrollerCanvas.ActualHeight / 2.0 - _itemSizeInternal.Height / 2.0) : new Point(_scrollerCanvas.ActualWidth / 2.0 - _itemSizeInternal.Width / 2.0, 0.0));
		}
		else
		{
			initialSliceBeginIndex = centerSlice.BeginIndex;
			initialSlicePos = ((_scrollOrientation != Orientation.Horizontal) ? new Point(0.0, Canvas.GetTop(centerSlice)) : new Point(Canvas.GetLeft(centerSlice), 0.0));
		}
		UpdateScrollerLayout(initialSliceBeginIndex, initialSlicePos);
	}

	private void UpdateScrollerLayout(int initialSliceBeginIndex, Point initialSlicePos)
	{
		if (!_initialized && base.IsInitialized)
		{
			return;
		}
		if (_minIndexExternal > _maxIndexExternal)
		{
			RemoveAllSlices();
			return;
		}
		_itemSizeInternal = _itemSizeExternal;
		if (_itemSizeInternal.Width == double.MaxValue)
		{
			_itemSizeInternal.Width = _scrollerCanvas.ActualWidth;
		}
		if (_itemSizeInternal.Height == double.MaxValue)
		{
			_itemSizeInternal.Height = _scrollerCanvas.ActualHeight;
		}
		if (_scrollOrientation == Orientation.Horizontal)
		{
			_itemsPerSlice = (int)(_scrollerCanvas.ActualHeight / _itemSizeInternal.Height);
		}
		else
		{
			_itemsPerSlice = (int)(_scrollerCanvas.ActualWidth / _itemSizeInternal.Width);
		}
		_itemsPerSlice = Math.Max(_itemsPerSlice, 1);
		RemoveAllSlices();
		initialSliceBeginIndex = Math.Max(initialSliceBeginIndex, _minIndexExternal);
		initialSliceBeginIndex = Math.Min(initialSliceBeginIndex, _maxIndexExternal);
		_minIndexInternal = _minIndexExternal;
		_maxIndexInternal = _maxIndexExternal;
		if (_fillBeginEndSlices)
		{
			int num = (initialSliceBeginIndex - _minIndexExternal) % _itemsPerSlice;
			if (num > 0 && num < _itemsPerSlice)
			{
				_minIndexInternal -= _itemsPerSlice - num;
			}
			int num2 = (_maxIndexExternal - initialSliceBeginIndex + 1) % _itemsPerSlice;
			if (num2 > 0 && num2 < _itemsPerSlice)
			{
				_maxIndexInternal += _itemsPerSlice - num2;
			}
		}
		_minIndexInternal -= _extraBeginEndSlices * _itemsPerSlice;
		_maxIndexInternal += _extraBeginEndSlices * _itemsPerSlice;
		AddInitialSlice(initialSliceBeginIndex, initialSlicePos);
		UpdateSlices(updateItems: false);
		_moveAllowed = true;
		if (_sliceList.Count > 0)
		{
			if (_scrollOrientation == Orientation.Horizontal)
			{
				double left = Canvas.GetLeft(_sliceList.Last());
				if (left + _itemSizeInternal.Width < _scrollerCanvas.ActualWidth)
				{
					double num3 = _scrollerCanvas.ActualWidth - (left + _itemSizeInternal.Width);
					foreach (Slice slice in _sliceList)
					{
						Canvas.SetLeft(slice, Canvas.GetLeft(slice) + num3);
					}
					UpdateSlices(updateItems: false);
				}
				double left2 = Canvas.GetLeft(_sliceList.First());
				if (left2 > 0.0)
				{
					double num4 = 0.0 - left2;
					foreach (Slice slice2 in _sliceList)
					{
						Canvas.SetLeft(slice2, Canvas.GetLeft(slice2) + num4);
					}
					UpdateSlices(updateItems: false);
				}
			}
			else
			{
				double top = Canvas.GetTop(_sliceList.Last());
				if (top + _itemSizeInternal.Height < _scrollerCanvas.ActualHeight)
				{
					double num5 = _scrollerCanvas.ActualHeight - (top + _itemSizeInternal.Height);
					foreach (Slice slice3 in _sliceList)
					{
						Canvas.SetTop(slice3, Canvas.GetTop(slice3) + num5);
					}
					UpdateSlices(updateItems: false);
				}
				double top2 = Canvas.GetTop(_sliceList.First());
				if (top2 > 0.0)
				{
					double num6 = 0.0 - top2;
					foreach (Slice slice4 in _sliceList)
					{
						Canvas.SetTop(slice4, Canvas.GetTop(slice4) + num6);
					}
					UpdateSlices(updateItems: false);
				}
			}
			int count;
			do
			{
				if (_scrollOrientation == Orientation.Horizontal)
				{
					double num7 = Canvas.GetLeft(_sliceList.Last()) + _itemSizeInternal.Width - Canvas.GetLeft(_sliceList.First());
					if (num7 < _scrollerCanvas.ActualWidth)
					{
						double num8 = (_scrollerCanvas.ActualWidth - num7) / 2.0 - Canvas.GetLeft(_sliceList.First());
						foreach (Slice slice5 in _sliceList)
						{
							Canvas.SetLeft(slice5, Canvas.GetLeft(slice5) + num8);
						}
						if (_fillWithSlices)
						{
							_minIndexInternal -= _itemsPerSlice;
							_maxIndexInternal += _itemsPerSlice;
						}
						_moveAllowed = false;
					}
				}
				else
				{
					double num9 = Canvas.GetTop(_sliceList.Last()) + _itemSizeInternal.Height - Canvas.GetTop(_sliceList.First());
					if (num9 < _scrollerCanvas.ActualHeight)
					{
						double num10 = (_scrollerCanvas.ActualHeight - num9) / 2.0 - Canvas.GetTop(_sliceList.First());
						foreach (Slice slice6 in _sliceList)
						{
							Canvas.SetTop(slice6, Canvas.GetTop(slice6) + num10);
						}
						if (_fillWithSlices)
						{
							_minIndexInternal -= _itemsPerSlice;
							_maxIndexInternal += _itemsPerSlice;
						}
						_moveAllowed = false;
					}
					else
					{
						MoveSlices(0.0, updateItems: false);
					}
				}
				count = _sliceList.Count;
				UpdateSlices(updateItems: false);
			}
			while (count != _sliceList.Count);
		}
		else
		{
			_moveAllowed = false;
		}
		UpdateItemData();
		UpdateFirstLastItems();
		UpdateMouseOverItem(Mouse.GetPosition(_scrollerCanvas));
	}

	private void UpdateSlices(bool updateItems)
	{
		int num = 0;
		int num2 = 0;
		if (_sliceList.Count > 0)
		{
			num = _sliceList.First().BeginIndex;
			num2 = _sliceList.Last().EndIndex;
		}
		if (_sliceList.Count == 0)
		{
			int beginIndex;
			Point position;
			if (num2 >= _maxIndexInternal)
			{
				beginIndex = _maxIndexInternal - _itemsPerSlice + 1;
				position = ((_scrollOrientation != Orientation.Horizontal) ? new Point(0.0, _scrollerCanvas.ActualHeight - _itemSizeInternal.Height) : new Point(_scrollerCanvas.ActualWidth - _itemSizeInternal.Width, 0.0));
			}
			else if (num <= _minIndexInternal)
			{
				beginIndex = _minIndexInternal;
				position = new Point(0.0, 0.0);
			}
			else
			{
				beginIndex = num;
				position = new Point(0.0, 0.0);
			}
			AddInitialSlice(beginIndex, position);
		}
		AddNecessarySlices();
		RemoveUnnecessarySlices();
		if (_sliceList.Count > 0 && updateItems && (num != _sliceList.First().BeginIndex || num2 != _sliceList.Last().EndIndex))
		{
			UpdateItemData();
		}
		if (updateItems)
		{
			UpdateFirstLastItems();
			UpdateMouseOverItem(Mouse.GetPosition(_scrollerCanvas));
		}
	}

	private void UpdateItemData()
	{
		if (_sliceList.Count == 0)
		{
			return;
		}
		int beginIndex = _sliceList.First().BeginIndex;
		int endIndex = _sliceList.Last().EndIndex;
		if (_itemDataCache.BeginIndex != beginIndex || _itemDataCache.EndIndex != endIndex)
		{
			_itemDataCache.ItemDataArray = FireGetItemsEventHandler(beginIndex, endIndex);
			_itemDataCache.BeginIndex = beginIndex;
			_itemDataCache.EndIndex = endIndex;
		}
		foreach (Slice slice in _sliceList)
		{
			if (slice.HasItems)
			{
				continue;
			}
			for (int i = slice.BeginIndex; i <= slice.EndIndex; i++)
			{
				FrameworkElement frameworkElement;
				if (_freeItemRendererList.Count > 0)
				{
					frameworkElement = _freeItemRendererList.First();
					_freeItemRendererList.Remove(frameworkElement);
				}
				else
				{
					frameworkElement = (FrameworkElement)Activator.CreateInstance(ItemRendererType);
				}
				frameworkElement.DataContext = _itemDataCache.ItemDataArray[i - beginIndex];
				if (frameworkElement is DefaultItemRenderer)
				{
					((DefaultItemRenderer)frameworkElement).Index = i;
				}
				slice.SetItemRenderer(frameworkElement, i);
			}
			slice.HasItems = true;
		}
	}

	private void AddInitialSlice(int beginIndex, Point position)
	{
		beginIndex = Math.Min(beginIndex, _maxIndexInternal - _itemsPerSlice + 1);
		beginIndex = Math.Max(beginIndex, _minIndexInternal);
		int endIndex = Math.Min(beginIndex + _itemsPerSlice - 1, _maxIndexInternal);
		Slice slice = CreateSlice(beginIndex, endIndex, position);
		_scrollerCanvas.Children.Add(slice);
		_sliceList.Add(slice);
	}

	private void AddNecessarySlices()
	{
		if (_sliceList.Count == 0)
		{
			return;
		}
		while (_sliceList.First().BeginIndex > _minIndexInternal)
		{
			bool flag = false;
			Point position = new Point(0.0, 0.0);
			if (_scrollOrientation == Orientation.Horizontal)
			{
				double left = Canvas.GetLeft(_sliceList.First());
				if (left > 0.0 - _itemSizeInternal.Width * (double)_bufferedSlices)
				{
					flag = true;
					position.X = left - _itemSizeInternal.Width;
				}
			}
			else
			{
				double top = Canvas.GetTop(_sliceList.First());
				if (top > 0.0 - _itemSizeInternal.Height * (double)_bufferedSlices)
				{
					flag = true;
					position.Y = top - _itemSizeInternal.Height;
				}
			}
			if (!flag)
			{
				break;
			}
			Slice slice = CreateSlice(Math.Max(_sliceList.First().BeginIndex - _itemsPerSlice, _minIndexInternal), _sliceList.First().BeginIndex - 1, position);
			slice.EmptyPreItems = _itemsPerSlice - (slice.EndIndex - slice.BeginIndex) - 1;
			_scrollerCanvas.Children.Add(slice);
			_sliceList.Insert(0, slice);
		}
		while (_sliceList.Last().EndIndex < _maxIndexInternal)
		{
			bool flag2 = false;
			Point position2 = new Point(0.0, 0.0);
			if (_scrollOrientation == Orientation.Horizontal)
			{
				double left2 = Canvas.GetLeft(_sliceList.Last());
				if (left2 < _scrollerCanvas.ActualWidth + _itemSizeInternal.Width * (double)(_bufferedSlices - 1))
				{
					flag2 = true;
					position2.X = left2 + _itemSizeInternal.Width;
				}
			}
			else
			{
				double top2 = Canvas.GetTop(_sliceList.Last());
				if (top2 < _scrollerCanvas.ActualHeight + _itemSizeInternal.Height * (double)(_bufferedSlices - 1))
				{
					flag2 = true;
					position2.Y = top2 + _itemSizeInternal.Height;
				}
			}
			if (flag2)
			{
				Slice slice2 = CreateSlice(_sliceList.Last().EndIndex + 1, Math.Min(_sliceList.Last().EndIndex + _itemsPerSlice, _maxIndexInternal), position2);
				_scrollerCanvas.Children.Add(slice2);
				_sliceList.Add(slice2);
				continue;
			}
			break;
		}
	}

	private void RemoveUnnecessarySlices()
	{
		while (_sliceList.Count > 0)
		{
			bool flag = false;
			if (_scrollOrientation == Orientation.Horizontal)
			{
				if ((double)_sliceList.First().GetValue(Canvas.LeftProperty) <= 0.0 - _itemSizeInternal.Width * (double)(_bufferedSlices + 1))
				{
					flag = true;
				}
			}
			else if ((double)_sliceList.First().GetValue(Canvas.TopProperty) <= 0.0 - _itemSizeInternal.Height * (double)(_bufferedSlices + 1))
			{
				flag = true;
			}
			if (!flag)
			{
				break;
			}
			DeleteSlice(_sliceList.First());
		}
		while (_sliceList.Count > 0)
		{
			bool flag2 = false;
			if (_scrollOrientation == Orientation.Horizontal)
			{
				if ((double)_sliceList.Last().GetValue(Canvas.LeftProperty) > _scrollerCanvas.ActualWidth + _itemSizeInternal.Width * (double)_bufferedSlices)
				{
					flag2 = true;
				}
			}
			else if ((double)_sliceList.Last().GetValue(Canvas.TopProperty) > _scrollerCanvas.ActualHeight + _itemSizeInternal.Height * (double)_bufferedSlices)
			{
				flag2 = true;
			}
			if (flag2)
			{
				DeleteSlice(_sliceList.Last());
				continue;
			}
			break;
		}
	}

	private void RemoveAllSlices()
	{
		while (_sliceList.Count > 0)
		{
			DeleteSlice(_sliceList.First());
		}
	}

	private Slice CreateSlice(int beginIndex, int endIndex, Point position)
	{
		Slice slice;
		if (_freeSliceList.Count > 0)
		{
			slice = _freeSliceList.First();
			_freeSliceList.Remove(slice);
		}
		else
		{
			slice = new Slice();
		}
		slice.BeginIndex = beginIndex;
		slice.EndIndex = endIndex;
		slice.EmptyPreItems = 0;
		slice.ItemSize = _itemSizeInternal;
		if (_scrollOrientation == Orientation.Horizontal)
		{
			slice.Orientation = Orientation.Vertical;
			slice.Width = _itemSizeInternal.Width;
			slice.Height = _scrollerCanvas.ActualHeight;
		}
		else
		{
			slice.Orientation = Orientation.Horizontal;
			slice.Width = _scrollerCanvas.ActualWidth;
			slice.Height = _itemSizeInternal.Height;
		}
		Canvas.SetLeft(slice, position.X);
		Canvas.SetTop(slice, position.Y);
		return slice;
	}

	private void DeleteSlice(Slice slice)
	{
		_scrollerCanvas.Children.Remove(slice);
		_freeSliceList.Add(slice);
		_sliceList.Remove(slice);
		_freeItemRendererList.AddRange(slice.ClearItems());
		slice.HasItems = false;
	}

	private void UpdateFirstLastItems()
	{
		Slice firstVisibleSlice = GetFirstVisibleSlice();
		if (firstVisibleSlice != null)
		{
			FirstVisibleItem = firstVisibleSlice.GetItemRenderer(firstVisibleSlice.BeginIndex).DataContext;
		}
		else
		{
			FirstVisibleItem = null;
		}
		Slice lastVisibleSlice = GetLastVisibleSlice();
		if (lastVisibleSlice != null)
		{
			LastVisibleItem = lastVisibleSlice.GetItemRenderer(lastVisibleSlice.EndIndex).DataContext;
		}
		else
		{
			LastVisibleItem = null;
		}
	}

	private void UpdateMouseOverItem(Point mouseScrollerCanvasPosition)
	{
		object mouseOverItem = null;
		if (mouseScrollerCanvasPosition.X >= 0.0 && mouseScrollerCanvasPosition.Y != 0.0 && mouseScrollerCanvasPosition.X < _scrollerCanvas.ActualWidth && mouseScrollerCanvasPosition.Y < _scrollerCanvas.ActualHeight)
		{
			foreach (Slice slice in _sliceList)
			{
				Rect rect = new Rect(Canvas.GetLeft(slice), Canvas.GetTop(slice), slice.ActualWidth, slice.ActualHeight);
				if (rect.Contains(mouseScrollerCanvasPosition))
				{
					FrameworkElement itemRendererAtPoint = slice.GetItemRendererAtPoint(mouseScrollerCanvasPosition.X - rect.Left, mouseScrollerCanvasPosition.Y - rect.Top);
					if (itemRendererAtPoint != null)
					{
						mouseOverItem = itemRendererAtPoint.DataContext;
					}
				}
			}
		}
		MouseOverItem = mouseOverItem;
	}

	private Slice GetFirstVisibleSlice()
	{
		foreach (Slice slice in _sliceList)
		{
			if (_scrollOrientation == Orientation.Horizontal)
			{
				if (Canvas.GetLeft(slice) + slice.ActualWidth >= 0.0)
				{
					return slice;
				}
			}
			else if (Canvas.GetTop(slice) + slice.ActualHeight >= 0.0)
			{
				return slice;
			}
		}
		return null;
	}

	private Slice GetLastVisibleSlice()
	{
		for (int num = _sliceList.Count - 1; num >= 0; num--)
		{
			Slice slice = _sliceList[num];
			if (_scrollOrientation == Orientation.Horizontal)
			{
				if (Canvas.GetLeft(slice) < _scrollerCanvas.ActualWidth)
				{
					return slice;
				}
			}
			else if (Canvas.GetTop(slice) < _scrollerCanvas.ActualHeight)
			{
				return slice;
			}
		}
		return null;
	}

	private Slice GetCenterSlice()
	{
		foreach (Slice slice in _sliceList)
		{
			if (_scrollOrientation == Orientation.Horizontal)
			{
				double left = Canvas.GetLeft(slice);
				if (left <= _scrollerCanvas.ActualWidth / 2.0 && left + slice.ActualWidth > _scrollerCanvas.ActualWidth / 2.0)
				{
					return slice;
				}
			}
			else
			{
				double top = Canvas.GetTop(slice);
				if (top <= _scrollerCanvas.ActualHeight / 2.0 && top + slice.ActualHeight > _scrollerCanvas.ActualHeight / 2.0)
				{
					return slice;
				}
			}
		}
		return null;
	}

	private void MoveSlices(double delta, bool updateItems)
	{
		if (!_moveAllowed)
		{
			return;
		}
		if (_sliceList.Last().EndIndex >= _maxIndexInternal)
		{
			if (_scrollOrientation == Orientation.Horizontal)
			{
				double num = Canvas.GetLeft(_sliceList.Last()) + _itemSizeInternal.Width;
				if (num + delta < _scrollerCanvas.ActualWidth)
				{
					delta = _scrollerCanvas.ActualWidth - num;
					StopScroll();
				}
			}
			else
			{
				double num2 = Canvas.GetTop(_sliceList.Last()) + _itemSizeInternal.Height;
				if (num2 + delta < _scrollerCanvas.ActualHeight)
				{
					delta = _scrollerCanvas.ActualHeight - num2;
					StopScroll();
				}
			}
		}
		if (_sliceList.First().BeginIndex <= _minIndexInternal)
		{
			if (_scrollOrientation == Orientation.Horizontal)
			{
				double left = Canvas.GetLeft(_sliceList.First());
				if (left + delta > 0.0)
				{
					delta = 0.0 - left;
					StopScroll();
				}
			}
			else
			{
				double top = Canvas.GetTop(_sliceList.First());
				if (top + delta > 0.0)
				{
					delta = 0.0 - top;
					StopScroll();
				}
			}
		}
		foreach (Slice slice in _sliceList)
		{
			if (_scrollOrientation == Orientation.Horizontal)
			{
				Canvas.SetLeft(slice, Canvas.GetLeft(slice) + delta);
			}
			else
			{
				Canvas.SetTop(slice, Canvas.GetTop(slice) + delta);
			}
		}
		UpdateSlices(updateItems);
	}
}
