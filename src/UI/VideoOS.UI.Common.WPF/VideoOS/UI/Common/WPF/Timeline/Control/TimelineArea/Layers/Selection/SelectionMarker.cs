using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Selection;

public partial class SelectionMarker : Grid, IComponentConnector
{
	private bool _selected;

	private VideoOS.UI.Common.WPF.Timeline.SelectionMarker _selectionMarkerType = VideoOS.UI.Common.WPF.Timeline.SelectionMarker.Center;

	private double _leftBounds = -25.0;

	private double _rightBounds = double.MaxValue;

	public VideoOS.UI.Common.WPF.Timeline.SelectionMarker SelectionMarkerType
	{
		get
		{
			return _selectionMarkerType;
		}
		set
		{
			if (_selectionMarkerType != value)
			{
				_selectionMarkerType = value;
				Update();
			}
		}
	}

	public bool Selected
	{
		set
		{
			if (_selected != value)
			{
				_selected = value;
				Update();
			}
		}
	}

	public string Time
	{
		set
		{
			_timeTekstBlock.Text = value;
		}
	}

	public double LeftBounds
	{
		set
		{
			if (_leftBounds != value)
			{
				_leftBounds = value;
				Update();
			}
		}
	}

	public double RightBounds
	{
		set
		{
			if (_rightBounds != value)
			{
				_rightBounds = value;
				Update();
			}
		}
	}

	public event EventHandler<MouseButtonEventArgs> MarkerMouseLeftButtonDown;

	public event EventHandler<MouseEventArgs> MarkerMouseMove;

	public event EventHandler<MouseButtonEventArgs> MarkerMouseLeftButtonUp;

	public SelectionMarker()
	{
		InitializeComponent();
	}

	public void MarkerCaptureMouse()
	{
		_markerCanvas.CaptureMouse();
	}

	public void MarkerReleaseMouseCapture()
	{
		_markerCanvas.ReleaseMouseCapture();
	}

	public void UpdateOffsets(int textAreaHeight, int markerHeight, int trackOffset, BitmapImage centerImage)
	{
		_timeTextBorder.Height = textAreaHeight;
		_markerCanvas.Height = markerHeight;
		Canvas.SetTop(_fromTriangle, (double)markerHeight / 2.0);
		Canvas.SetTop(_toTriangle, (double)markerHeight / 2.0);
		_centerRectangle.Height = markerHeight - 2;
		_centerImage.Source = centerImage;
		Canvas.SetTop(_centerImage, trackOffset);
		Canvas.SetLeft(_centerImage, 0.0 - (double)(centerImage.PixelWidth - 1) / 2.0 - 1.0);
	}

	public void ApplyTheme(ThemeType themeType)
	{
		ThemeManager.Instance.ApplyThemeOnElement(_timeTextBorder, themeType);
	}

	private void Grid_Loaded(object sender, RoutedEventArgs e)
	{
		Update();
	}

	private void _timeTextBlockGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
	{
		Update();
	}

	private void Update()
	{
		if (base.IsLoaded)
		{
			_fromBorder.Visibility = ((_selectionMarkerType != VideoOS.UI.Common.WPF.Timeline.SelectionMarker.From) ? Visibility.Hidden : Visibility.Visible);
			_fromTriangle.Visibility = ((_selectionMarkerType != VideoOS.UI.Common.WPF.Timeline.SelectionMarker.From) ? Visibility.Hidden : Visibility.Visible);
			_centerRectangle.Visibility = ((_selectionMarkerType != VideoOS.UI.Common.WPF.Timeline.SelectionMarker.Center) ? Visibility.Hidden : Visibility.Visible);
			_centerImage.Visibility = ((_selectionMarkerType != VideoOS.UI.Common.WPF.Timeline.SelectionMarker.Center) ? Visibility.Hidden : Visibility.Visible);
			_toBorder.Visibility = ((_selectionMarkerType != VideoOS.UI.Common.WPF.Timeline.SelectionMarker.To) ? Visibility.Hidden : Visibility.Visible);
			_toTriangle.Visibility = ((_selectionMarkerType != VideoOS.UI.Common.WPF.Timeline.SelectionMarker.To) ? Visibility.Hidden : Visibility.Visible);
			double val = (0.0 - _timeTextGrid.ActualWidth) / 2.0;
			val = Math.Max(_leftBounds, val);
			val = Math.Min(_rightBounds - _timeTextGrid.ActualWidth, val);
			Canvas.SetLeft(_timeTextGrid, val);
			_timeTextGrid.Visibility = ((!_selected) ? Visibility.Hidden : Visibility.Visible);
		}
	}

	private void _markerCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (this.MarkerMouseLeftButtonDown != null)
		{
			this.MarkerMouseLeftButtonDown(this, e);
		}
	}

	private void _markerCanvas_MouseMove(object sender, MouseEventArgs e)
	{
		if (this.MarkerMouseMove != null)
		{
			this.MarkerMouseMove(this, e);
		}
	}

	private void _markerCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (this.MarkerMouseLeftButtonUp != null)
		{
			this.MarkerMouseLeftButtonUp(this, e);
		}
	}
}
