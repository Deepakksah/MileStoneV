using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.BusyIndicators;

public class SprocketControl : Control, IDisposable
{
	public enum Direction
	{
		CLOCKWISE,
		ANTICLOCKWISE
	}

	private struct Spoke(Point pt1, Point pt2)
	{
		public Point StartPoint = pt1;

		public Point EndPoint = pt2;
	}

	private const double DEFAULT_INTERVAL = 60.0;

	private static readonly Color DEFAULT_TICK_COLOR = Color.FromArgb(byte.MaxValue, 58, 58, 58);

	private const double DEFAULT_TICK_WIDTH = 3.0;

	private const int DEFAULT_TICK_COUNT = 12;

	private const double MINIMUM_INNER_RADIUS = 5.0;

	private const double MINIMUM_OUTER_RADIUS = 8.0;

	private readonly Size MINIMUM_CONTROL_SIZE = new Size(28.0, 28.0);

	private const double MINIMUM_PEN_WIDTH = 2.0;

	private const double DEFAULT_START_ANGLE = 270.0;

	private const double INNER_RADIUS_FACTOR = 0.175;

	private const double OUTER_RADIUS_FACTOR = 0.3125;

	private const int ALPHA_UPPER_LIMIT = 250;

	private const int ALPHA_LOWER_LIMIT = 0;

	private const double ALPHA_TICK_PERCENTAGE_LOWER_LIMIT = 10.0;

	private const double DEFAULT_PROGRESS_ALPHA = 10.0;

	private const double DEFAULT_PROGRESS = 0.0;

	private Point centerPoint;

	private double innerRadius;

	private double outerRadius;

	private double alphaChange;

	private double angleIncrement;

	private double renderStartAngle;

	private Timer renderTimer;

	private List<Spoke> spokes;

	public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(double), typeof(SprocketControl), new FrameworkPropertyMetadata(60.0, OnIntervalChanged));

	public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(SprocketControl), new FrameworkPropertyMetadata(true, OnIsIndeterminateChanged));

	public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(double), typeof(SprocketControl), new FrameworkPropertyMetadata(0.0, OnProgressChanged, CoerceProgress));

	public static readonly DependencyProperty RotationProperty = DependencyProperty.Register("Rotation", typeof(Direction), typeof(SprocketControl), new FrameworkPropertyMetadata(Direction.CLOCKWISE, OnRotationChanged));

	public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle", typeof(double), typeof(SprocketControl), new FrameworkPropertyMetadata(270.0, OnStartAngleChanged));

	public static readonly DependencyProperty TickColorProperty = DependencyProperty.Register("TickColor", typeof(Color), typeof(SprocketControl), new FrameworkPropertyMetadata(DEFAULT_TICK_COLOR, OnTickColorChanged));

	public static readonly DependencyProperty TickCountProperty = DependencyProperty.Register("TickCount", typeof(int), typeof(SprocketControl), new FrameworkPropertyMetadata(12, OnTickCountChanged, CoerceTickCount));

	public static readonly DependencyProperty TickStyleProperty = DependencyProperty.Register("TickStyle", typeof(PenLineCap), typeof(SprocketControl), new FrameworkPropertyMetadata(PenLineCap.Round, OnTickStyleChanged));

	public static readonly DependencyProperty TickWidthProperty = DependencyProperty.Register("TickWidth", typeof(double), typeof(SprocketControl), new FrameworkPropertyMetadata(3.0, OnTickWidthChanged, CoerceTickWidth));

	public static readonly DependencyProperty LowestAlphaProperty = DependencyProperty.Register("LowestAlpha", typeof(int), typeof(SprocketControl), new FrameworkPropertyMetadata(0, OnLowestAlphaChanged, CoerceLowestAlpha));

	public static readonly DependencyProperty AlphaTicksPercentageProperty = DependencyProperty.Register("AlphaTicksPercentage", typeof(double), typeof(SprocketControl), new FrameworkPropertyMetadata(100.0, OnAlphaTicksPercentageChanged, CoerceAlphaTicksPercentage));

	public double Interval
	{
		get
		{
			return (double)GetValue(IntervalProperty);
		}
		set
		{
			SetValue(IntervalProperty, value);
		}
	}

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

	public double Progress
	{
		get
		{
			return (double)GetValue(ProgressProperty);
		}
		set
		{
			SetValue(ProgressProperty, value);
		}
	}

	public Direction Rotation
	{
		get
		{
			return (Direction)GetValue(RotationProperty);
		}
		set
		{
			SetValue(RotationProperty, value);
		}
	}

	public double StartAngle
	{
		get
		{
			return (double)GetValue(StartAngleProperty);
		}
		set
		{
			SetValue(StartAngleProperty, value);
		}
	}

	public Color TickColor
	{
		get
		{
			return (Color)GetValue(TickColorProperty);
		}
		set
		{
			SetValue(TickColorProperty, value);
		}
	}

	public int TickCount
	{
		get
		{
			return (int)GetValue(TickCountProperty);
		}
		set
		{
			SetValue(TickCountProperty, value);
		}
	}

	public PenLineCap TickStyle
	{
		get
		{
			return (PenLineCap)GetValue(TickStyleProperty);
		}
		set
		{
			SetValue(TickStyleProperty, value);
		}
	}

	public double TickWidth
	{
		get
		{
			return (double)GetValue(TickWidthProperty);
		}
		set
		{
			SetValue(TickWidthProperty, value);
		}
	}

	public int LowestAlpha
	{
		get
		{
			return (int)GetValue(LowestAlphaProperty);
		}
		set
		{
			SetValue(LowestAlphaProperty, value);
		}
	}

	public double AlphaTicksPercentage
	{
		get
		{
			return (double)GetValue(AlphaTicksPercentageProperty);
		}
		set
		{
			SetValue(AlphaTicksPercentageProperty, value);
		}
	}

	private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SprocketControl obj = (SprocketControl)d;
		double oldInterval = (double)e.OldValue;
		double interval = obj.Interval;
		obj.OnIntervalChanged(oldInterval, interval);
	}

	protected virtual void OnIntervalChanged(double oldInterval, double newInterval)
	{
		if (renderTimer != null)
		{
			bool enabled = renderTimer.Enabled;
			renderTimer.Enabled = false;
			renderTimer.Interval = newInterval;
			renderTimer.Enabled = enabled;
		}
	}

	private static void OnIsIndeterminateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SprocketControl obj = (SprocketControl)d;
		bool oldIsIndeterminate = (bool)e.OldValue;
		bool isIndeterminate = obj.IsIndeterminate;
		obj.OnIsIndeterminateChanged(oldIsIndeterminate, isIndeterminate);
	}

	protected virtual void OnIsIndeterminateChanged(bool oldIsIndeterminate, bool newIsIndeterminate)
	{
		if (oldIsIndeterminate != newIsIndeterminate)
		{
			if (newIsIndeterminate && base.IsVisible)
			{
				Start();
				return;
			}
			Stop();
			InvalidateVisual();
		}
	}

	private static object CoerceProgress(DependencyObject d, object value)
	{
		double num = (double)value;
		if (num < 0.0)
		{
			return 0.0;
		}
		if (num > 100.0)
		{
			return 100.0;
		}
		return value;
	}

	private static void OnProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SprocketControl obj = (SprocketControl)d;
		double oldProgress = (double)e.OldValue;
		double progress = obj.Progress;
		obj.OnProgressChanged(oldProgress, progress);
	}

	protected virtual void OnProgressChanged(double oldProgress, double newProgress)
	{
		InvalidateVisual();
	}

	private static void OnRotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SprocketControl obj = (SprocketControl)d;
		Direction oldRotation = (Direction)e.OldValue;
		Direction rotation = obj.Rotation;
		obj.OnRotationChanged(oldRotation, rotation);
	}

	protected virtual void OnRotationChanged(Direction oldRotation, Direction newRotation)
	{
		CalculateSpokesPoints();
	}

	private static void OnStartAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SprocketControl obj = (SprocketControl)d;
		double oldStartAngle = (double)e.OldValue;
		double startAngle = obj.StartAngle;
		obj.OnStartAngleChanged(oldStartAngle, startAngle);
	}

	protected virtual void OnStartAngleChanged(double oldStartAngle, double newStartAngle)
	{
		CalculateSpokesPoints();
	}

	private static void OnTickColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SprocketControl obj = (SprocketControl)d;
		Color oldTickColor = (Color)e.OldValue;
		Color tickColor = obj.TickColor;
		obj.OnTickColorChanged(oldTickColor, tickColor);
	}

	protected virtual void OnTickColorChanged(Color oldTickColor, Color newTickColor)
	{
		InvalidateVisual();
	}

	private static object CoerceTickCount(DependencyObject d, object value)
	{
		if ((int)value <= 0)
		{
			return 12;
		}
		return value;
	}

	private static void OnTickCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SprocketControl obj = (SprocketControl)d;
		int oldTickCount = (int)e.OldValue;
		int tickCount = obj.TickCount;
		obj.OnTickCountChanged(oldTickCount, tickCount);
	}

	protected virtual void OnTickCountChanged(int oldTickCount, int newTickCount)
	{
		CalculateSpokesPoints();
	}

	private static void OnTickStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SprocketControl obj = (SprocketControl)d;
		PenLineCap oldTickStyle = (PenLineCap)e.OldValue;
		PenLineCap tickStyle = obj.TickStyle;
		obj.OnTickStyleChanged(oldTickStyle, tickStyle);
	}

	protected virtual void OnTickStyleChanged(PenLineCap oldTickStyle, PenLineCap newTickStyle)
	{
		InvalidateVisual();
	}

	private static object CoerceTickWidth(DependencyObject d, object value)
	{
		if ((double)value < 0.0)
		{
			return 3.0;
		}
		return value;
	}

	private static void OnTickWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SprocketControl obj = (SprocketControl)d;
		double oldTickWidth = (double)e.OldValue;
		double tickWidth = obj.TickWidth;
		obj.OnTickWidthChanged(oldTickWidth, tickWidth);
	}

	protected virtual void OnTickWidthChanged(double oldTickWidth, double newTickWidth)
	{
		InvalidateVisual();
	}

	private static void OnLowestAlphaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SprocketControl obj = (SprocketControl)d;
		int oldLowestAlpha = (int)e.OldValue;
		int lowestAlpha = obj.LowestAlpha;
		obj.OnLowestAlphaChanged(oldLowestAlpha, lowestAlpha);
	}

	protected virtual void OnLowestAlphaChanged(int oldLowestAlpha, int newLowestAlpha)
	{
	}

	private static object CoerceLowestAlpha(DependencyObject d, object value)
	{
		_ = (SprocketControl)d;
		int num = (int)value;
		if (num < 0)
		{
			return 0;
		}
		if (num > 250)
		{
			return 250;
		}
		return num;
	}

	private static void OnAlphaTicksPercentageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SprocketControl obj = (SprocketControl)d;
		double oldAlphaTicksPercentage = (double)e.OldValue;
		double alphaTicksPercentage = obj.AlphaTicksPercentage;
		obj.OnAlphaTicksPercentageChanged(oldAlphaTicksPercentage, alphaTicksPercentage);
	}

	protected virtual void OnAlphaTicksPercentageChanged(double oldAlphaTicksPercentage, double newAlphaTicksPercentage)
	{
	}

	private static object CoerceAlphaTicksPercentage(DependencyObject d, object value)
	{
		_ = (SprocketControl)d;
		double num = (double)value;
		if (num > 100.0)
		{
			return 100.0;
		}
		if (num < 10.0)
		{
			return 10.0;
		}
		return num;
	}

	public SprocketControl()
	{
		renderTimer = new Timer(Interval);
		renderTimer.Elapsed += OnRenderTimerElapsed;
		base.MinWidth = MINIMUM_CONTROL_SIZE.Width;
		base.MinWidth = MINIMUM_CONTROL_SIZE.Height;
		CalculateSpokesPoints();
		RoutedEventHandler handler = null;
		handler = delegate
		{
			base.Loaded -= handler;
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				if (IsIndeterminate && base.IsVisible)
				{
					Start();
				}
			});
		};
		base.Loaded += handler;
		base.IsVisibleChanged += OnVisibilityChanged;
	}

	private void Start()
	{
		if (renderTimer != null && !renderTimer.Enabled)
		{
			renderTimer.Interval = Interval;
			renderTimer.Enabled = true;
		}
	}

	private void Stop()
	{
		if (renderTimer != null)
		{
			renderTimer.Enabled = false;
		}
	}

	private double ConvertDegreesToRadians(double degrees)
	{
		return Math.PI / 180.0 * degrees;
	}

	private void CalculateSpokesPoints()
	{
		spokes = new List<Spoke>();
		angleIncrement = 360.0 / (double)TickCount;
		alphaChange = (int)((double)(255 - LowestAlpha) / (AlphaTicksPercentage / 100.0 * (double)TickCount));
		renderStartAngle = StartAngle;
		double num = ((base.Width < base.Height) ? base.Width : base.Height);
		centerPoint = new Point(base.Width / 2.0, base.Height / 2.0);
		innerRadius = (int)(num * 0.175);
		if (innerRadius < 5.0)
		{
			innerRadius = 5.0;
		}
		outerRadius = (int)(num * 0.3125);
		if (outerRadius < 8.0)
		{
			outerRadius = 8.0;
		}
		double num2 = 0.0;
		for (int i = 0; i < TickCount; i++)
		{
			Point pt = new Point(innerRadius * (double)(float)Math.Cos(ConvertDegreesToRadians(num2)), innerRadius * (double)(float)Math.Sin(ConvertDegreesToRadians(num2)));
			Point pt2 = new Point(outerRadius * (double)(float)Math.Cos(ConvertDegreesToRadians(num2)), outerRadius * (double)(float)Math.Sin(ConvertDegreesToRadians(num2)));
			Spoke item = new Spoke(pt, pt2);
			spokes.Add(item);
			if (!IsIndeterminate)
			{
				num2 += angleIncrement;
			}
			else if (Rotation == Direction.CLOCKWISE)
			{
				num2 -= angleIncrement;
			}
			else if (Rotation == Direction.ANTICLOCKWISE)
			{
				num2 += angleIncrement;
			}
		}
	}

	protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
	{
		base.OnRenderSizeChanged(sizeInfo);
		CalculateSpokesPoints();
	}

	protected override void OnRender(DrawingContext dc)
	{
		if (spokes == null)
		{
			return;
		}
		TranslateTransform transform = new TranslateTransform(centerPoint.X, centerPoint.Y);
		dc.PushTransform(transform);
		RotateTransform transform2 = new RotateTransform(renderStartAngle);
		dc.PushTransform(transform2);
		byte b = byte.MaxValue;
		int num = (int)Math.Floor(Progress * (double)TickCount / 100.0);
		for (int i = 0; i < TickCount; i++)
		{
			if (!IsIndeterminate)
			{
				b = (byte)((num <= 0) ? 10 : ((byte)((i < num) ? 255.0 : 10.0)));
			}
			Pen pen = new Pen(new SolidColorBrush(Color.FromArgb(b, TickColor.R, TickColor.G, TickColor.B)), TickWidth);
			PenLineCap startLineCap = (pen.EndLineCap = TickStyle);
			pen.StartLineCap = startLineCap;
			dc.DrawLine(pen, spokes[i].StartPoint, spokes[i].EndPoint);
			if (IsIndeterminate)
			{
				b -= (byte)alphaChange;
				if (b < LowestAlpha)
				{
					b = (byte)LowestAlpha;
				}
			}
		}
		dc.Pop();
		dc.Pop();
	}

	private void OnRenderTimerElapsed(object sender, ElapsedEventArgs e)
	{
		base.Dispatcher.BeginInvoke((Action)delegate
		{
			if (Rotation == Direction.CLOCKWISE)
			{
				renderStartAngle += angleIncrement;
				if (renderStartAngle >= 360.0)
				{
					renderStartAngle -= 360.0;
				}
			}
			else if (Rotation == Direction.ANTICLOCKWISE)
			{
				renderStartAngle -= angleIncrement;
				if (renderStartAngle <= -360.0)
				{
					renderStartAngle += 360.0;
				}
			}
			InvalidateVisual();
		});
	}

	private void OnVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (IsIndeterminate)
		{
			if ((bool)e.NewValue)
			{
				Start();
			}
			else
			{
				Stop();
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	~SprocketControl()
	{
		Dispose(disposing: false);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (renderTimer != null)
			{
				renderTimer.Elapsed -= OnRenderTimerElapsed;
				renderTimer.Dispose();
			}
			base.IsVisibleChanged -= OnVisibilityChanged;
		}
	}
}
