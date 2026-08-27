using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSProgressSpinner : RangeBase
{
	private Path _trackPath;

	private PathFigure _trackPathFigure;

	private ArcSegment _trackArcSegment;

	private Path _indicatorPath;

	private PathFigure _indicatorPathFigure;

	private ArcSegment _indicatorArcSegment;

	public static readonly DependencyProperty IsIndeterminateProperty;

	public bool IsIndeterminate
	{
		get
		{
			return (bool)GetValue(IsIndeterminateProperty);
		}
		set
		{
			SetValue(IsIndeterminateProperty, value);
		}
	}

	static VideoOSProgressSpinner()
	{
		IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(VideoOSProgressSpinner), new FrameworkPropertyMetadata(false));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSProgressSpinner), new FrameworkPropertyMetadata(typeof(VideoOSProgressSpinner)));
		ThemeManager.InitializeThemingSystem();
		Control.IsTabStopProperty.OverrideMetadata(typeof(VideoOSProgressSpinner), new FrameworkPropertyMetadata(false));
	}

	public VideoOSProgressSpinner()
	{
		base.Loaded += OnLoaded;
		base.Unloaded += OnUnloaded;
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		_trackPath = this.GetTemplateChild<Path>("PART_TrackPath");
		_trackPathFigure = this.GetTemplateChild<PathFigure>("PART_TrackPathFigure");
		_trackArcSegment = this.GetTemplateChild<ArcSegment>("PART_TrackArcSegment");
		_indicatorPath = this.GetTemplateChild<Path>("PART_IndicatorPath");
		_indicatorPathFigure = this.GetTemplateChild<PathFigure>("PART_IndicatorPathFigure");
		_indicatorArcSegment = this.GetTemplateChild<ArcSegment>("PART_IndicatorArcSegment");
		ProgressRingRelatedPropertyChanged(this, EventArgs.Empty);
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		AddProgressRingRelatedPropertyChangedHandlers();
		ProgressRingRelatedPropertyChanged(null, EventArgs.Empty);
	}

	private void OnUnloaded(object sender, RoutedEventArgs e)
	{
		RemoveProgressRingRelatedPropertyChangedHandlers();
	}

	private void AddProgressRingRelatedPropertyChangedHandlers()
	{
		RemoveProgressRingRelatedPropertyChangedHandlers();
		DependencyPropertyDescriptor.FromProperty(FrameworkElement.ActualWidthProperty, typeof(VideoOSProgressSpinner)).AddValueChanged(this, ProgressRingRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(FrameworkElement.ActualHeightProperty, typeof(VideoOSProgressSpinner)).AddValueChanged(this, ProgressRingRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(IsIndeterminateProperty, typeof(VideoOSProgressSpinner)).AddValueChanged(this, ProgressRingRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(RangeBase.MinimumProperty, typeof(VideoOSProgressSpinner)).AddValueChanged(this, ProgressRingRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(RangeBase.MaximumProperty, typeof(VideoOSProgressSpinner)).AddValueChanged(this, ProgressRingRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(RangeBase.ValueProperty, typeof(VideoOSProgressSpinner)).AddValueChanged(this, ProgressRingRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(FrameworkElement.TagProperty, typeof(Path)).AddValueChanged(_indicatorPath, ProgressRingRelatedPropertyChanged);
	}

	private void RemoveProgressRingRelatedPropertyChangedHandlers()
	{
		DependencyPropertyDescriptor.FromProperty(FrameworkElement.ActualWidthProperty, typeof(VideoOSProgressSpinner)).RemoveValueChanged(this, ProgressRingRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(FrameworkElement.ActualHeightProperty, typeof(VideoOSProgressSpinner)).RemoveValueChanged(this, ProgressRingRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(IsIndeterminateProperty, typeof(VideoOSProgressSpinner)).RemoveValueChanged(this, ProgressRingRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(RangeBase.MinimumProperty, typeof(VideoOSProgressSpinner)).RemoveValueChanged(this, ProgressRingRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(RangeBase.MaximumProperty, typeof(VideoOSProgressSpinner)).RemoveValueChanged(this, ProgressRingRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(RangeBase.ValueProperty, typeof(VideoOSProgressSpinner)).RemoveValueChanged(this, ProgressRingRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(FrameworkElement.TagProperty, typeof(Path)).RemoveValueChanged(_indicatorPath, ProgressRingRelatedPropertyChanged);
	}

	private void ProgressRingRelatedPropertyChanged(object sender, EventArgs e)
	{
		double num2;
		double num3;
		if (IsIndeterminate)
		{
			double num = ((_indicatorPath.Tag is double) ? ((double)_indicatorPath.Tag) : 0.0);
			num2 = 360.0 * num;
			num3 = num2 + 90.0;
			if (num3 > 360.0)
			{
				num3 -= 360.0;
			}
		}
		else
		{
			num2 = 0.0;
			num3 = (base.Value - base.Minimum) / (base.Maximum - base.Minimum) * 360.0;
			num3 = Math.Max(0.0, num3);
			num3 = Math.Min(360.0, num3);
		}
		RenderProgressRing(num2, num3);
	}

	private void RenderProgressRing(double startAngle, double endAngle)
	{
		if (_trackPath != null && !(base.ActualWidth <= 0.0) && !(base.ActualHeight <= 0.0))
		{
			double num = _indicatorPath.StrokeThickness / 2.0 + 1.0;
			double num2 = Math.Max(0.0, base.ActualWidth / 2.0 - num);
			double num3 = Math.Max(0.0, base.ActualHeight / 2.0 - num);
			Point startPoint = CalculateEllipsePoint(startAngle, num2, num3);
			startPoint.X += num2 + num;
			startPoint.Y += num3 + num;
			Point point = CalculateEllipsePoint(endAngle, num2, num3);
			point.X += num2 + num;
			point.Y += num3 + num;
			if (Math.Abs(startPoint.X - point.X) < 0.01)
			{
				point.X -= 0.01;
			}
			bool isLargeArc = ((!(startAngle <= endAngle)) ? (Math.Abs(startAngle - endAngle) <= 180.0) : (Math.Abs(startAngle - endAngle) > 180.0));
			Size size = new Size(num2, num3);
			_trackPathFigure.StartPoint = new Point(num2 + num, num);
			_trackArcSegment.Point = new Point(num2 + num - 0.01, num);
			_trackArcSegment.Size = size;
			_trackArcSegment.IsLargeArc = true;
			_indicatorPathFigure.StartPoint = startPoint;
			_indicatorArcSegment.Point = point;
			_indicatorArcSegment.Size = size;
			_indicatorArcSegment.IsLargeArc = isLargeArc;
		}
	}

	private static Point CalculateEllipsePoint(double angle, double radiusX, double radiusY)
	{
		double num = Math.PI / 180.0 * (angle - 90.0);
		double x = radiusX * Math.Cos(num);
		double y = radiusY * Math.Sin(num);
		return new Point(x, y);
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSProgressSpinnerAutomationPeer(this);
	}
}
