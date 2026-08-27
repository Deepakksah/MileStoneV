using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Commands;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSSliderBase : Control
{
	public static class VideoOSSliderBaseInternals
	{
		public const string ThumbStyleDefaultKey = "VideoOSSliderBaseThumbStyleDefaultKey";

		public const string TrackStyleKey = "VideoOSSliderBaseTrackStyleKey";

		public const string IncludedTrackStyleKey = "VideoOSSliderBaseIncludedTrackStyleKey";

		public const string ExcludedTrackStyleKey = "VideoOSSliderBaseExcludedTrackStyleKey";

		public const string LabelMinimumStyleKey = "VideoOSSliderBaseLabelMinimumStyleKey";

		public const string LabelMaximumStyleKey = "VideoOSSliderBaseLabelMaximumStyleKey";

		public const string DecreaseButtonStyleKey = "VideoOSSliderBaseDecreaseButtonStyleKey";

		public const string IncreaseButtonStyleKey = "VideoOSSliderBaseIncreaseButtonStyleKey";

		public const double LabelsMargin = 2.0;

		public static readonly Thickness LabelMinimumMargin = new Thickness(0.0, 0.0, 2.0, 0.0);

		public static readonly Thickness LabelMaximumMargin = new Thickness(2.0, 0.0, 0.0, 0.0);

		public static IMultiValueConverter TrackFillBrushConverter { get; } = new LambdaConverter3<DependencyObject, bool, TrackAppearances>((DependencyObject self, bool isEnabled, TrackAppearances trackAppearance) => self.FindResource(trackAppearance switch
		{
			TrackAppearances.Default => isEnabled ? "VideoOSResourceKeysInternalVideoOSSliderTrackAppearanceDefaultBrush" : "VideoOSResourceKeysInternalVideoOSSliderTrackAppearanceDefaultDisabledBrush", 
			TrackAppearances.Neutral => isEnabled ? "VideoOSResourceKeysInternalVideoOSSliderTrackAppearanceNeutralBrush" : "VideoOSResourceKeysInternalVideoOSSliderTrackAppearanceNeutralDisabledBrush", 
			TrackAppearances.Primary => isEnabled ? "VideoOSResourceKeysInternalVideoOSSliderTrackAppearancePrimaryBrush" : "VideoOSResourceKeysInternalVideoOSSliderTrackAppearancePrimaryDisabledBrush", 
			_ => throw new ArgumentOutOfRangeException("trackAppearance", trackAppearance, null), 
		}), "TrackFillBrushConverter");

		public static IMultiValueConverter ThumbOutlineBrushConverter { get; } = new LambdaConverter4<DependencyObject, bool, bool, bool>(delegate(DependencyObject self, bool isEnabled, bool isMouseOver, bool isPressed)
		{
			string resourceKey = ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSSliderThumbOutlineDisabledBrush" : (isPressed ? "VideoOSResourceKeysInternalVideoOSSliderThumbOutlinePressedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSSliderThumbOutlineIdleBrush" : "VideoOSResourceKeysInternalVideoOSSliderThumbOutlineHoverBrush")));
			return self.FindResource(resourceKey);
		}, "ThumbOutlineBrushConverter");

		public static IMultiValueConverter ThumbFillBrushConverter { get; } = new LambdaConverter4<DependencyObject, bool, bool, bool>(delegate(DependencyObject self, bool isEnabled, bool isMouseOver, bool isPressed)
		{
			string resourceKey = ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSSliderThumbFillDisabledBrush" : (isPressed ? "VideoOSResourceKeysInternalVideoOSSliderThumbFillPressedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSSliderThumbFillIdleBrush" : "VideoOSResourceKeysInternalVideoOSSliderThumbFillHoverBrush")));
			return self.FindResource(resourceKey);
		}, "ThumbFillBrushConverter");

		public static IMultiValueConverter ThumbDropShadowEffectConverter { get; } = new LambdaConverter2<DependencyObject, bool>((DependencyObject self, bool isEnabled) => (!isEnabled) ? null : self.FindResource(VideoOSResourceKeys.DropShadowEffect), "ThumbDropShadowEffectConverter");

		public static IValueConverter LabelMaxWidthConverter { get; } = new LambdaConverter1<double>((double labelColumnMaxWidth) => labelColumnMaxWidth - 2.0, "LabelMaxWidthConverter");

		public static IValueConverter LabelColumnMinWidthConverter { get; } = new LambdaConverter1<double>((double minWidth) => minWidth.IsGreaterThan(0.0) ? ((object)(minWidth + 2.0)) : ((object)0.0), "LabelColumnMinWidthConverter");

		public static IMultiValueConverter SmallChangeButtonIsEnabledConverter { get; } = new LambdaConverter2<double, double>((double minMaxValue, double value) => !value.IsEqualTo(minMaxValue), "SmallChangeButtonIsEnabledConverter");

		public static IMultiValueConverter FillColumnWidthConverter { get; } = new LambdaConverter3<bool, HorizontalAlignment, HorizontalAlignment>(delegate(bool areTrackAndThumbVisible, HorizontalAlignment horizontalContentAlignment, HorizontalAlignment columnSide)
		{
			if (areTrackAndThumbVisible)
			{
				return new GridLength(0.0);
			}
			switch (horizontalContentAlignment)
			{
			case HorizontalAlignment.Left:
				return (columnSide == HorizontalAlignment.Left) ? new GridLength(0.0) : new GridLength(1.0, GridUnitType.Star);
			case HorizontalAlignment.Right:
				return (columnSide == HorizontalAlignment.Right) ? new GridLength(0.0) : new GridLength(1.0, GridUnitType.Star);
			case HorizontalAlignment.Center:
			case HorizontalAlignment.Stretch:
				return new GridLength(1.0, GridUnitType.Star);
			default:
				throw new ArgumentOutOfRangeException("horizontalContentAlignment", horizontalContentAlignment, null);
			}
		}, "FillColumnWidthConverter");

		public static IValueConverter TrackAndThumbColumnWidthConverter { get; } = new LambdaConverter1<bool>((bool areTrackAndThumbVisible) => areTrackAndThumbVisible ? new GridLength(1.0, GridUnitType.Star) : new GridLength(0.0), "TrackAndThumbColumnWidthConverter");
	}

	public enum TrackAppearances
	{
		Default,
		Neutral,
		Primary
	}

	public delegate string ValueToStringCallbackHandler(double value);

	public static readonly DependencyProperty TrackAppearanceProperty = DependencyProperty.Register("TrackAppearance", typeof(TrackAppearances), typeof(VideoOSSliderBase), new PropertyMetadata(TrackAppearances.Default));

	public static readonly DependencyProperty TrackAndThumbToolTipProperty = DependencyProperty.Register("TrackAndThumbToolTip", typeof(string), typeof(VideoOSSliderBase), new PropertyMetadata(null));

	public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(VideoOSSliderBase));

	public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(VideoOSSliderBase), new FrameworkPropertyMetadata(0.0, OnMinimumChanged), IsValidDoubleValue);

	public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(VideoOSSliderBase), new FrameworkPropertyMetadata(1.0, OnMaximumChanged, CoerceMaximum), IsValidDoubleValue);

	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(VideoOSSliderBase), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, OnValueChanged, CoerceValue), IsValidDoubleValue);

	public static readonly DependencyProperty TickFrequencyProperty = DependencyProperty.Register("TickFrequency", typeof(double), typeof(VideoOSSliderBase), new PropertyMetadata(0.1, TickFrequencyChanged), IsZeroOrPositiveDoubleValue);

	public static readonly DependencyProperty IsSnapToTickEnabledProperty = DependencyProperty.Register("IsSnapToTickEnabled", typeof(bool), typeof(VideoOSSliderBase), new PropertyMetadata(false, IsSnapToTickEnabledChanged));

	public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register("SmallChange", typeof(double), typeof(VideoOSSliderBase), new PropertyMetadata(0.1), IsZeroOrPositiveDoubleValue);

	public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register("LargeChange", typeof(double), typeof(VideoOSSliderBase), new PropertyMetadata(1.0), IsZeroOrPositiveDoubleValue);

	public static readonly DependencyProperty SmallDecreaseButtonIconSourceProperty = DependencyProperty.Register("SmallDecreaseButtonIconSource", typeof(VideoOSIconSourceBase), typeof(VideoOSSliderBase), new PropertyMetadata(null));

	public static readonly DependencyProperty SmallDecreaseButtonToolTipProperty = DependencyProperty.Register("SmallDecreaseButtonToolTip", typeof(string), typeof(VideoOSSliderBase), new PropertyMetadata(null));

	public static readonly DependencyProperty SmallIncreaseButtonIconSourceProperty = DependencyProperty.Register("SmallIncreaseButtonIconSource", typeof(VideoOSIconSourceBase), typeof(VideoOSSliderBase), new PropertyMetadata(null));

	public static readonly DependencyProperty SmallIncreaseButtonToolTipProperty = DependencyProperty.Register("SmallIncreaseButtonToolTip", typeof(string), typeof(VideoOSSliderBase), new PropertyMetadata(null));

	public static readonly DependencyProperty MinimumLabelProperty = DependencyProperty.Register("MinimumLabel", typeof(string), typeof(VideoOSSliderBase), new PropertyMetadata(null));

	public static readonly DependencyProperty MaximumLabelProperty = DependencyProperty.Register("MaximumLabel", typeof(string), typeof(VideoOSSliderBase), new PropertyMetadata(null));

	public static readonly DependencyProperty AreTrackAndThumbVisibleProperty = DependencyProperty.Register("AreTrackAndThumbVisible", typeof(bool), typeof(VideoOSSliderBase), new PropertyMetadata(true, OnAreTrackAndThumbVisibleChanged));

	public static readonly DependencyProperty LabelsMinWidthProperty = DependencyProperty.Register("LabelsMinWidth", typeof(double), typeof(VideoOSSliderBase), new PropertyMetadata(0.0, OnLabelsMinWidthChanged), IsZeroOrPositiveDoubleValue);

	public static readonly DependencyProperty LabelsMaxWidthProperty = DependencyProperty.Register("LabelsMaxWidth", typeof(double), typeof(VideoOSSliderBase), new PropertyMetadata(double.MaxValue, OnLabelsMaxWidthChanged), IsZeroOrPositiveDoubleValue);

	public static readonly DependencyProperty ValueToStringCallbackProperty = DependencyProperty.Register("ValueToStringCallback", typeof(ValueToStringCallbackHandler), typeof(VideoOSSliderBase), new PropertyMetadata(null));

	public static readonly DependencyProperty TrackThumbMarginProperty = DependencyProperty.Register("TrackThumbMargin", typeof(Thickness), typeof(VideoOSSliderBase), new PropertyMetadata(new Thickness(6.0, 0.0, 6.0, 0.0)));

	public static readonly DependencyProperty TrackIncludedThumbMarginProperty = DependencyProperty.Register("TrackIncludedThumbMargin", typeof(Thickness), typeof(VideoOSSliderBase), new PropertyMetadata(new Thickness(0.0, 0.0, 6.0, 0.0)));

	public static readonly DependencyProperty TrackExcludedThumbMarginProperty = DependencyProperty.Register("TrackExcludedThumbMargin", typeof(Thickness), typeof(VideoOSSliderBase), new PropertyMetadata(new Thickness(6.0, 0.0, 0.0, 0.0)));

	public static readonly DependencyProperty KeyDownCommandProperty = DependencyProperty.Register("KeyDownCommand", typeof(ICommand), typeof(VideoOSSliderBase), new PropertyMetadata((object)null));

	public static readonly DependencyProperty DecreaseValueCommandProperty = DependencyProperty.Register("DecreaseValueCommand", typeof(ICommand), typeof(VideoOSSliderBase), new PropertyMetadata((object)null));

	public static readonly DependencyProperty IncreaseValueCommandProperty = DependencyProperty.Register("IncreaseValueCommand", typeof(ICommand), typeof(VideoOSSliderBase), new PropertyMetadata((object)null));

	public TrackAppearances TrackAppearance
	{
		get
		{
			return (TrackAppearances)GetValue(TrackAppearanceProperty);
		}
		set
		{
			SetValue(TrackAppearanceProperty, value);
		}
	}

	public string TrackAndThumbToolTip
	{
		get
		{
			return (string)GetValue(TrackAndThumbToolTipProperty);
		}
		set
		{
			SetValue(TrackAndThumbToolTipProperty, value);
		}
	}

	public double Minimum
	{
		get
		{
			return (double)GetValue(MinimumProperty);
		}
		set
		{
			SetValue(MinimumProperty, value);
		}
	}

	public double Maximum
	{
		get
		{
			return (double)GetValue(MaximumProperty);
		}
		set
		{
			SetValue(MaximumProperty, value);
		}
	}

	public double Value
	{
		get
		{
			return (double)GetValue(ValueProperty);
		}
		set
		{
			SetValue(ValueProperty, value);
		}
	}

	public double TickFrequency
	{
		get
		{
			return (double)GetValue(TickFrequencyProperty);
		}
		set
		{
			SetValue(TickFrequencyProperty, value);
		}
	}

	public bool IsSnapToTickEnabled
	{
		get
		{
			return (bool)GetValue(IsSnapToTickEnabledProperty);
		}
		set
		{
			SetValue(IsSnapToTickEnabledProperty, value);
		}
	}

	public double SmallChange
	{
		get
		{
			return (double)GetValue(SmallChangeProperty);
		}
		set
		{
			SetValue(SmallChangeProperty, value);
		}
	}

	public double LargeChange
	{
		get
		{
			return (double)GetValue(LargeChangeProperty);
		}
		set
		{
			SetValue(LargeChangeProperty, value);
		}
	}

	public VideoOSIconSourceBase SmallDecreaseButtonIconSource
	{
		get
		{
			return (VideoOSIconSourceBase)GetValue(SmallDecreaseButtonIconSourceProperty);
		}
		set
		{
			SetValue(SmallDecreaseButtonIconSourceProperty, value);
		}
	}

	public string SmallDecreaseButtonToolTip
	{
		get
		{
			return (string)GetValue(SmallDecreaseButtonToolTipProperty);
		}
		set
		{
			SetValue(SmallDecreaseButtonToolTipProperty, value);
		}
	}

	public VideoOSIconSourceBase SmallIncreaseButtonIconSource
	{
		get
		{
			return (VideoOSIconSourceBase)GetValue(SmallIncreaseButtonIconSourceProperty);
		}
		set
		{
			SetValue(SmallIncreaseButtonIconSourceProperty, value);
		}
	}

	public string SmallIncreaseButtonToolTip
	{
		get
		{
			return (string)GetValue(SmallIncreaseButtonToolTipProperty);
		}
		set
		{
			SetValue(SmallIncreaseButtonToolTipProperty, value);
		}
	}

	public string MinimumLabel
	{
		get
		{
			return (string)GetValue(MinimumLabelProperty);
		}
		set
		{
			SetValue(MinimumLabelProperty, value);
		}
	}

	public string MaximumLabel
	{
		get
		{
			return (string)GetValue(MaximumLabelProperty);
		}
		set
		{
			SetValue(MaximumLabelProperty, value);
		}
	}

	public bool AreTrackAndThumbVisible
	{
		get
		{
			return (bool)GetValue(AreTrackAndThumbVisibleProperty);
		}
		set
		{
			SetValue(AreTrackAndThumbVisibleProperty, value);
		}
	}

	public double LabelsMinWidth
	{
		get
		{
			return (double)GetValue(LabelsMinWidthProperty);
		}
		set
		{
			SetValue(LabelsMinWidthProperty, value);
		}
	}

	public double LabelsMaxWidth
	{
		get
		{
			return (double)GetValue(LabelsMaxWidthProperty);
		}
		set
		{
			SetValue(LabelsMaxWidthProperty, value);
		}
	}

	public ValueToStringCallbackHandler ValueToStringCallback
	{
		get
		{
			return (ValueToStringCallbackHandler)GetValue(ValueToStringCallbackProperty);
		}
		set
		{
			SetValue(ValueToStringCallbackProperty, value);
		}
	}

	public Thickness TrackThumbMargin
	{
		get
		{
			return (Thickness)GetValue(TrackThumbMarginProperty);
		}
		set
		{
			SetValue(TrackThumbMarginProperty, value);
		}
	}

	public Thickness TrackIncludedThumbMargin
	{
		get
		{
			return (Thickness)GetValue(TrackIncludedThumbMarginProperty);
		}
		set
		{
			SetValue(TrackIncludedThumbMarginProperty, value);
		}
	}

	public Thickness TrackExcludedThumbMargin
	{
		get
		{
			return (Thickness)GetValue(TrackExcludedThumbMarginProperty);
		}
		set
		{
			SetValue(TrackExcludedThumbMarginProperty, value);
		}
	}

	public ICommand KeyDownCommand
	{
		get
		{
			return (ICommand)GetValue(KeyDownCommandProperty);
		}
		set
		{
			SetValue(KeyDownCommandProperty, value);
		}
	}

	public ICommand DecreaseValueCommand
	{
		get
		{
			return (ICommand)GetValue(DecreaseValueCommandProperty);
		}
		set
		{
			SetValue(DecreaseValueCommandProperty, value);
		}
	}

	public ICommand IncreaseValueCommand
	{
		get
		{
			return (ICommand)GetValue(IncreaseValueCommandProperty);
		}
		set
		{
			SetValue(IncreaseValueCommandProperty, value);
		}
	}

	public event RoutedPropertyChangedEventHandler<double> ValueChanged
	{
		add
		{
			AddHandler(ValueChangedEvent, value);
		}
		remove
		{
			RemoveHandler(ValueChangedEvent, value);
		}
	}

	private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		VideoOSSliderBase obj = (VideoOSSliderBase)d;
		obj.CoerceValue(MaximumProperty);
		obj.CoerceValue(ValueProperty);
		obj.OnMinimumChanged();
	}

	private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		VideoOSSliderBase obj = (VideoOSSliderBase)d;
		obj.CoerceValue(ValueProperty);
		obj.OnMaximumChanged();
	}

	private static object CoerceMaximum(DependencyObject d, object value)
	{
		double minimum = ((VideoOSSliderBase)d).Minimum;
		if (!((double)value < minimum))
		{
			return value;
		}
		return minimum;
	}

	private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		VideoOSSliderBase obj = (VideoOSSliderBase)d;
		obj.OnValueChanged();
		obj.RaiseEvent(new RoutedPropertyChangedEventArgs<double>((double)e.OldValue, (double)e.NewValue)
		{
			RoutedEvent = ValueChangedEvent
		});
	}

	private static object CoerceValue(DependencyObject d, object value)
	{
		VideoOSSliderBase videoOSSliderBase = (VideoOSSliderBase)d;
		double minimum = videoOSSliderBase.Minimum;
		double maximum = videoOSSliderBase.Maximum;
		double value2 = videoOSSliderBase.Value;
		double num = (double)value;
		if (num.IsLessThanOrEqualTo(minimum))
		{
			return minimum;
		}
		if (num.IsGreaterThanOrEqualTo(maximum))
		{
			return maximum;
		}
		if (!videoOSSliderBase.IsSnapToTickEnabled)
		{
			return num;
		}
		double tickFrequency = videoOSSliderBase.TickFrequency;
		double num2 = (tickFrequency.IsEqualTo(0.0) ? maximum : (minimum + tickFrequency));
		while (num2.IsLessThan(num))
		{
			num2 = Math.Round(num2 + tickFrequency, 10);
		}
		double num3 = (tickFrequency.IsEqualTo(0.0) ? minimum : (num2 - tickFrequency));
		double x = num - num3;
		double y = num2 - num;
		double num4 = (x.IsLessThanOrEqualTo(y) ? num3 : num2);
		return num4.IsEqualTo(value2) ? value2 : num4;
	}

	private static void TickFrequencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		((VideoOSSliderBase)d).CoerceValue(ValueProperty);
	}

	private static void IsSnapToTickEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		((VideoOSSliderBase)d).CoerceValue(ValueProperty);
	}

	private static void OnAreTrackAndThumbVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		((VideoOSSliderBase)d).OnAreTrackAndThumbVisibleChanged();
	}

	private static void OnLabelsMinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		((VideoOSSliderBase)d).OnLabelsMinWidthChanged();
	}

	private static void OnLabelsMaxWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		((VideoOSSliderBase)d).OnLabelsMaxWidthChanged();
	}

	private static bool IsZeroOrPositiveDoubleValue(object value)
	{
		double x = (double)value;
		if (IsValidDoubleValue(value))
		{
			return x.IsGreaterThanOrEqualTo(0.0);
		}
		return false;
	}

	private static bool IsValidDoubleValue(object value)
	{
		double d = (double)value;
		if (!double.IsNaN(d))
		{
			return !double.IsInfinity(d);
		}
		return false;
	}

	public VideoOSSliderBase()
	{
		KeyDownCommand = new DelegateCommand(HandleKeyDown);
		DecreaseValueCommand = new DelegateCommand(delegate
		{
			SetCurrentValue(ValueProperty, CoerceValue(this, Value - SmallChange));
		});
		IncreaseValueCommand = new DelegateCommand(delegate
		{
			SetCurrentValue(ValueProperty, CoerceValue(this, Value + SmallChange));
		});
	}

	public virtual void OnMinimumChanged()
	{
	}

	public virtual void OnMaximumChanged()
	{
	}

	public virtual void OnValueChanged()
	{
	}

	public virtual void OnLabelsMinWidthChanged()
	{
	}

	public virtual void OnLabelsMaxWidthChanged()
	{
	}

	public virtual void OnAreTrackAndThumbVisibleChanged()
	{
	}

	private void HandleKeyDown(object args)
	{
		KeyEventArgs e = (KeyEventArgs)((EventsToCommandMappingArgs)args).OriginalEventArgs;
		bool flag = true;
		switch (e.Key)
		{
		case Key.Home:
			SetCurrentValue(ValueProperty, Minimum);
			break;
		case Key.End:
			SetCurrentValue(ValueProperty, Maximum);
			break;
		case Key.Left:
			SetCurrentValue(ValueProperty, CoerceValue(this, Value + ((base.FlowDirection == FlowDirection.LeftToRight) ? (0.0 - SmallChange) : SmallChange)));
			break;
		case Key.Down:
			SetCurrentValue(ValueProperty, CoerceValue(this, Value - SmallChange));
			break;
		case Key.Right:
			SetCurrentValue(ValueProperty, CoerceValue(this, Value + ((base.FlowDirection == FlowDirection.LeftToRight) ? SmallChange : (0.0 - SmallChange))));
			break;
		case Key.Up:
			SetCurrentValue(ValueProperty, CoerceValue(this, Value + SmallChange));
			break;
		case Key.Next:
			SetCurrentValue(ValueProperty, CoerceValue(this, Value - LargeChange));
			break;
		case Key.Prior:
			SetCurrentValue(ValueProperty, CoerceValue(this, Value + LargeChange));
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			e.Handled = true;
		}
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSSliderBaseAutomationPeer(this);
	}
}
