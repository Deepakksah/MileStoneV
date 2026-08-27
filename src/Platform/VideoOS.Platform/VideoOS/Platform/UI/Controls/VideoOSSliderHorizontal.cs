using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSSliderHorizontal : VideoOSSliderBase
{
	public static class VideoOSSliderHorizontalInternals
	{
		public const string ThumbStyleShuttleKey = "VideoOSSliderHorizontalThumbStyleShuttleKey";

		public static IMultiValueConverter ThumbStyleConverter { get; } = new LambdaConverter3<ThumbAppearances, Style, Style>((ThumbAppearances appearance, Style defaultStyle, Style shuttleStyle) => appearance switch
		{
			ThumbAppearances.Default => defaultStyle, 
			ThumbAppearances.Shuttle => shuttleStyle, 
			_ => throw new ArgumentOutOfRangeException("appearance", appearance, null), 
		}, "ThumbStyleConverter");
	}

	public enum ThumbAppearances
	{
		Default,
		Shuttle
	}

	public static readonly DependencyProperty ThumbAppearanceProperty;

	private const double LabelColumnMaxWidthPercent = 0.15;

	private ColumnDefinition _labelLeftColumnDefinition;

	private ColumnDefinition _labelRightColumnDefinition;

	private Grid _track;

	private Grid _trackLeft;

	private ColumnDefinition _trackLeftColumnDefinition;

	private Grid _trackRight;

	private ColumnDefinition _trackRightColumnDefinition;

	private Thumb _thumb;

	private bool _templateApplied;

	public ThumbAppearances ThumbAppearance
	{
		get
		{
			return (ThumbAppearances)GetValue(ThumbAppearanceProperty);
		}
		set
		{
			SetValue(ThumbAppearanceProperty, value);
		}
	}

	private static void ThumbAppearanceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		VideoOSSliderHorizontal videoOSSliderHorizontal = (VideoOSSliderHorizontal)d;
		switch (videoOSSliderHorizontal.ThumbAppearance)
		{
		case ThumbAppearances.Default:
			videoOSSliderHorizontal.ClearValue(VideoOSSliderBase.TrackThumbMarginProperty);
			videoOSSliderHorizontal.ClearValue(VideoOSSliderBase.TrackIncludedThumbMarginProperty);
			videoOSSliderHorizontal.ClearValue(VideoOSSliderBase.TrackExcludedThumbMarginProperty);
			break;
		case ThumbAppearances.Shuttle:
			videoOSSliderHorizontal.TrackThumbMargin = new Thickness(3.0, 0.0, 3.0, 0.0);
			videoOSSliderHorizontal.TrackIncludedThumbMargin = new Thickness(0.0, 0.0, 3.0, 0.0);
			videoOSSliderHorizontal.TrackExcludedThumbMargin = new Thickness(3.0, 0.0, 0.0, 0.0);
			break;
		default:
			throw new ArgumentOutOfRangeException("ThumbAppearance", videoOSSliderHorizontal.ThumbAppearance, null);
		}
	}

	static VideoOSSliderHorizontal()
	{
		ThumbAppearanceProperty = DependencyProperty.Register("ThumbAppearance", typeof(ThumbAppearances), typeof(VideoOSSliderHorizontal), new PropertyMetadata(ThumbAppearances.Default, ThumbAppearanceChangedCallback));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSSliderHorizontal), new FrameworkPropertyMetadata(typeof(VideoOSSliderHorizontal)));
		ThemeManager.InitializeThemingSystem();
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		_labelLeftColumnDefinition = this.GetTemplateChild<ColumnDefinition>("PART_LeftLabelColumnDefinition");
		_labelRightColumnDefinition = this.GetTemplateChild<ColumnDefinition>("PART_RightLabelColumnDefinition");
		_track = this.GetTemplateChild<Grid>("PART_Track");
		_trackLeft = this.GetTemplateChild<Grid>("PART_TrackLeft");
		_trackLeftColumnDefinition = this.GetTemplateChild<ColumnDefinition>("PART_TrackLeftColumnDefinition");
		_trackRight = this.GetTemplateChild<Grid>("PART_TrackRight");
		_trackRightColumnDefinition = this.GetTemplateChild<ColumnDefinition>("PART_TrackRightColumnDefinition");
		_thumb = this.GetTemplateChild<Thumb>("PART_Thumb");
		_trackLeft.MouseLeftButtonDown += Track_MouseLeftButtonDown;
		_trackRight.MouseLeftButtonDown += Track_MouseLeftButtonDown;
		_trackLeft.MouseLeftButtonUp += TrackOrThumb_MouseLeftButtonUp;
		_trackRight.MouseLeftButtonUp += TrackOrThumb_MouseLeftButtonUp;
		_thumb.PreviewMouseLeftButtonDown += Thumb_PreviewMouseLeftButtonDown;
		_thumb.MouseLeftButtonUp += TrackOrThumb_MouseLeftButtonUp;
		_trackLeft.MouseMove += TrackOrThumb_MouseMove;
		_trackRight.MouseMove += TrackOrThumb_MouseMove;
		_thumb.MouseMove += TrackOrThumb_MouseMove;
		_thumb.DragDelta += Thumb_DragDelta;
		_templateApplied = true;
		UpdateThumbPosition();
		UpdateLabelColumnDefinitionsMaxWidth();
		VideoOSStyleFocusVisualStyle.ApplyFocusVisualAdorner(this, _thumb);
	}

	protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
	{
		base.OnRenderSizeChanged(sizeInfo);
		UpdateLabelColumnDefinitionsMaxWidth();
	}

	public override void OnLabelsMinWidthChanged()
	{
		UpdateLabelColumnDefinitionsMaxWidth();
	}

	public override void OnLabelsMaxWidthChanged()
	{
		UpdateLabelColumnDefinitionsMaxWidth();
	}

	public override void OnMinimumChanged()
	{
		UpdateThumbPosition();
	}

	public override void OnMaximumChanged()
	{
		UpdateThumbPosition();
	}

	public override void OnValueChanged()
	{
		UpdateThumbPosition();
	}

	public override void OnAreTrackAndThumbVisibleChanged()
	{
		if (!base.AreTrackAndThumbVisible && _thumb != null && object.Equals(Mouse.Captured, _thumb))
		{
			_thumb.ReleaseMouseCapture();
		}
	}

	private void Track_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (base.Focusable)
		{
			Focus();
		}
		_thumb.CaptureMouse();
		_thumb.SetCurrentValue(DependencyObjectAttachedProperties.IsMouseLeftButtonDownProperty, true);
		base.Value = GetValueAtMouseCursor();
	}

	private void TrackOrThumb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		_thumb.ReleaseMouseCapture();
	}

	private void TrackOrThumb_MouseMove(object sender, MouseEventArgs e)
	{
		UpdateValueBasedOnMousePosition();
	}

	private void Thumb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (base.Focusable)
		{
			Focus();
		}
	}

	private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
	{
		UpdateValueBasedOnMousePosition();
	}

	private void UpdateValueBasedOnMousePosition()
	{
		if (object.Equals(Mouse.Captured, _thumb))
		{
			base.Value = GetValueAtMouseCursor();
		}
	}

	private double GetValueAtMouseCursor()
	{
		double num = Math.Min(_track.ActualWidth, Math.Max(0.0, Mouse.GetPosition(_track).X)) / _track.ActualWidth;
		if (double.IsPositiveInfinity(num))
		{
			num = 0.0;
		}
		return base.Minimum + num * (base.Maximum - base.Minimum);
	}

	private void UpdateLabelColumnDefinitionsMaxWidth()
	{
		if (_templateApplied)
		{
			double val = base.ActualWidth * (base.AreTrackAndThumbVisible ? 0.15 : 1.0) + 2.0;
			double val2 = base.LabelsMaxWidth + 2.0;
			double val3 = base.LabelsMinWidth + 2.0;
			ColumnDefinition labelLeftColumnDefinition = _labelLeftColumnDefinition;
			double maxWidth = (_labelRightColumnDefinition.MaxWidth = Math.Max(val3, Math.Min(val, val2)));
			labelLeftColumnDefinition.MaxWidth = maxWidth;
		}
	}

	private void UpdateThumbPosition()
	{
		if (_templateApplied)
		{
			if (base.Minimum.IsEqualTo(base.Maximum))
			{
				_trackLeftColumnDefinition.Width = new GridLength(0.0, GridUnitType.Star);
				_trackRightColumnDefinition.Width = new GridLength(1.0, GridUnitType.Star);
				return;
			}
			double val = (base.Value - base.Minimum) / (base.Maximum - base.Minimum);
			double num = Math.Max(0.0, Math.Min(1.0, val));
			_trackLeftColumnDefinition.Width = new GridLength(num * 100.0, GridUnitType.Star);
			_trackRightColumnDefinition.Width = new GridLength(100.0 - num * 100.0, GridUnitType.Star);
		}
	}
}
