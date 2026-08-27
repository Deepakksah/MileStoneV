using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using VideoOS.Platform.UI.Automation.AutomationPeers;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSTextBlockBase : TextBlock
{
	private const double _fixConstraintRoundingErrorWidthPadding = 1.0;

	public static readonly DependencyProperty MinLinesProperty;

	public static readonly DependencyProperty MaxLinesProperty;

	public static readonly DependencyProperty IsAutoToolTipEnabledProperty;

	private bool _autoToolTipUpdateInProgress;

	public int MinLines
	{
		get
		{
			return (int)GetValue(MinLinesProperty);
		}
		set
		{
			SetValue(MinLinesProperty, value);
		}
	}

	public int MaxLines
	{
		get
		{
			return (int)GetValue(MaxLinesProperty);
		}
		set
		{
			SetValue(MaxLinesProperty, value);
		}
	}

	public bool IsAutoToolTipEnabled
	{
		get
		{
			return (bool)GetValue(IsAutoToolTipEnabledProperty);
		}
		set
		{
			SetValue(IsAutoToolTipEnabledProperty, value);
		}
	}

	static VideoOSTextBlockBase()
	{
		MinLinesProperty = DependencyProperty.Register("MinLines", typeof(int), typeof(VideoOSTextBlockBase), new PropertyMetadata(1, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTextBlockBase)o).UpdateMinMaxHeights(minLinesPropertyChanged: true, maxLinesPropertyChanged: false);
		}));
		MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof(int), typeof(VideoOSTextBlockBase), new PropertyMetadata(int.MaxValue, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTextBlockBase)o).UpdateMinMaxHeights(minLinesPropertyChanged: false, maxLinesPropertyChanged: true);
		}));
		IsAutoToolTipEnabledProperty = DependencyProperty.Register("IsAutoToolTipEnabled", typeof(bool), typeof(VideoOSTextBlockBase), new PropertyMetadata(true, IsAutoToolTipEnabledChangedCallback));
		TextBlock.TextTrimmingProperty.OverrideMetadata(typeof(VideoOSTextBlockBase), new FrameworkPropertyMetadata(TextTrimming.CharacterEllipsis));
		FrameworkElement.ToolTipProperty.OverrideMetadata(typeof(VideoOSTextBlockBase), new FrameworkPropertyMetadata(PropertyChangedCallback, CoerceToolTipValueCallback));
	}

	private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		VideoOSTextBlockBase videoOSTextBlockBase = (VideoOSTextBlockBase)d;
		if (!videoOSTextBlockBase._autoToolTipUpdateInProgress && videoOSTextBlockBase.IsAutoToolTipEnabled)
		{
			videoOSTextBlockBase.SetCurrentValue(FrameworkElement.ToolTipProperty, " ");
		}
	}

	private static object CoerceToolTipValueCallback(DependencyObject d, object baseValue)
	{
		VideoOSTextBlockBase videoOSTextBlockBase = (VideoOSTextBlockBase)d;
		if (videoOSTextBlockBase._autoToolTipUpdateInProgress || !videoOSTextBlockBase.IsAutoToolTipEnabled)
		{
			return baseValue;
		}
		return " ";
	}

	private static void IsAutoToolTipEnabledChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		VideoOSTextBlockBase videoOSTextBlockBase = (VideoOSTextBlockBase)d;
		if (videoOSTextBlockBase.IsAutoToolTipEnabled)
		{
			videoOSTextBlockBase.SetCurrentValue(FrameworkElement.ToolTipProperty, " ");
		}
		else
		{
			videoOSTextBlockBase.ClearValue(FrameworkElement.ToolTipProperty);
		}
	}

	public VideoOSTextBlockBase()
	{
		base.Loaded += OnLoaded;
		base.Unloaded += OnUnloaded;
		if (IsAutoToolTipEnabled)
		{
			SetCurrentValue(FrameworkElement.ToolTipProperty, " ");
		}
		base.ToolTipOpening += VideoOSTextBlockBase_ToolTipOpening;
	}

	private void VideoOSTextBlockBase_ToolTipOpening(object sender, ToolTipEventArgs e)
	{
		if (IsAutoToolTipEnabled)
		{
			string value = ResolveTooltipText();
			if (!string.IsNullOrWhiteSpace(value))
			{
				_autoToolTipUpdateInProgress = true;
				SetCurrentValue(FrameworkElement.ToolTipProperty, value);
				_autoToolTipUpdateInProgress = false;
			}
			else
			{
				e.Handled = true;
			}
		}
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		AddLinesRelatedPropertyChangedHandlers();
		LinesRelatedPropertyChanged(null, EventArgs.Empty);
	}

	private void OnUnloaded(object sender, RoutedEventArgs e)
	{
		RemoveLinesRelatedPropertyChangedHandlers();
	}

	private void AddLinesRelatedPropertyChangedHandlers()
	{
		RemoveLinesRelatedPropertyChangedHandlers();
		DependencyPropertyDescriptor.FromProperty(TextBlock.FontSizeProperty, typeof(VideoOSTextBlockBase)).AddValueChanged(this, LinesRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(TextBlock.FontFamilyProperty, typeof(VideoOSTextBlockBase)).AddValueChanged(this, LinesRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(TextBlock.LineHeightProperty, typeof(VideoOSTextBlockBase)).AddValueChanged(this, LinesRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(VideoOSTextBlockBase)).AddValueChanged(this, LinesRelatedPropertyChanged);
	}

	private void RemoveLinesRelatedPropertyChangedHandlers()
	{
		DependencyPropertyDescriptor.FromProperty(TextBlock.FontSizeProperty, typeof(VideoOSTextBlockBase)).RemoveValueChanged(this, LinesRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(TextBlock.FontFamilyProperty, typeof(VideoOSTextBlockBase)).RemoveValueChanged(this, LinesRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(TextBlock.LineHeightProperty, typeof(VideoOSTextBlockBase)).RemoveValueChanged(this, LinesRelatedPropertyChanged);
		DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(VideoOSTextBlockBase)).RemoveValueChanged(this, LinesRelatedPropertyChanged);
	}

	private void LinesRelatedPropertyChanged(object sender, EventArgs e)
	{
		UpdateMinMaxHeights(minLinesPropertyChanged: false, maxLinesPropertyChanged: false);
	}

	private bool IsNullOrWhiteSpace()
	{
		return string.IsNullOrWhiteSpace(GetText());
	}

	private void UpdateMinMaxHeights(bool minLinesPropertyChanged, bool maxLinesPropertyChanged)
	{
		if (MinLines > 1)
		{
			base.MinHeight = (double)MinLines * GetLineHeight();
		}
		else if (minLinesPropertyChanged)
		{
			ClearValue(FrameworkElement.MinHeightProperty);
		}
		if (IsNullOrWhiteSpace() && MinLines == 0)
		{
			base.MaxHeight = 0.0;
		}
		else if (MaxLines < 0 || MaxLines == int.MaxValue)
		{
			ClearValue(FrameworkElement.MaxHeightProperty);
		}
		else if (MaxLines >= 0 && MaxLines != int.MaxValue)
		{
			base.MaxHeight = (double)MaxLines * GetLineHeight();
		}
		else if (maxLinesPropertyChanged)
		{
			ClearValue(FrameworkElement.MaxHeightProperty);
		}
	}

	private double GetLineHeight()
	{
		double lineHeight = base.LineHeight;
		double num = Math.Ceiling(base.FontSize * base.FontFamily.LineSpacing);
		if (!double.IsNaN(lineHeight))
		{
			return Math.Max(lineHeight, num);
		}
		return num;
	}

	public bool WouldTextBeConstrained(int allowedLineCount)
	{
		return ResolveTextConstrained(allowedLineCount);
	}

	public bool ResolveTextConstrained(int? allowedLineCount = null)
	{
		Size size = MeasureOverride(new Size(base.ActualWidth + 1.0, base.ActualHeight));
		Size size2 = MeasureOverride(new Size(double.PositiveInfinity, double.PositiveInfinity));
		InvalidateMeasure();
		if (base.TextWrapping != TextWrapping.NoWrap)
		{
			double allowedHeight = (allowedLineCount.HasValue ? (GetLineHeight() * (double)allowedLineCount.Value) : base.ActualHeight);
			return IsWrappingTextConstrained(this, allowedHeight);
		}
		return size2 != size;
	}

	private string GetText()
	{
		return (string.IsNullOrEmpty(base.Text) ? string.Join("", from r in base.Inlines.OfType<Run>()
			select r.Text) : base.Text).TrimEnd();
	}

	public static bool IsWrappingTextConstrained(TextBlock textBlock, double allowedHeight, string text = null)
	{
		Typeface typeface = new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch);
		FormattedText formattedText = new FormattedText(text ?? (textBlock as VideoOSTextBlockBase)?.GetText() ?? textBlock.Text, Thread.CurrentThread.CurrentCulture, textBlock.FlowDirection, typeface, textBlock.FontSize, textBlock.Foreground, VisualTreeHelper.GetDpi(textBlock).PixelsPerDip)
		{
			MaxTextWidth = textBlock.ActualWidth + 1.0
		};
		if ((int)formattedText.Height <= (int)allowedHeight)
		{
			return formattedText.MinWidth > formattedText.MaxTextWidth;
		}
		return true;
	}

	public string ResolveTooltipText()
	{
		string text = string.Empty;
		DependencyObject dependencyObject = this;
		while (true)
		{
			dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
			if (dependencyObject == null)
			{
				break;
			}
			if (dependencyObject is FrameworkElement frameworkElement)
			{
				object value = frameworkElement.GetValue(FrameworkElement.ToolTipProperty);
				if (value is string text2)
				{
					text = text2;
					break;
				}
				if (value != null)
				{
					break;
				}
			}
		}
		string text3 = string.Empty;
		if (ResolveTextConstrained())
		{
			text3 = GetText();
		}
		if (!string.IsNullOrEmpty(text))
		{
			if (!string.IsNullOrEmpty(text3))
			{
				text3 = text3 + Environment.NewLine + Environment.NewLine;
			}
			text3 += text;
		}
		return text3;
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSTextBlockBaseAutomationPeer(this);
	}
}
