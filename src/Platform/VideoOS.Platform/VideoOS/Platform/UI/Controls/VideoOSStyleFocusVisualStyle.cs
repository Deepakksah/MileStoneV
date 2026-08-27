using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public static class VideoOSStyleFocusVisualStyle
{
	public static class VideoOSStyleFocusVisualStyleInternals
	{
		public const string FocusVisualStyleKey = "VideoOSStyleFocusVisualStyleFocusVisualStyleKey";

		public static readonly double FocusVisualStrokeThickness = 1.0;

		public static readonly DoubleCollection FocusVisualStrokeDashArray = new DoubleCollection(new List<double> { 6.0, 3.0 });

		public static readonly DashStyle FocusVisualDashStyle = new DashStyle(FocusVisualStrokeDashArray, 0.0);
	}

	private class FocusVisualAdorner : Adorner
	{
		public FocusVisualAdorner(FrameworkElement host, UIElement adornedElement)
			: base(adornedElement)
		{
			base.IsClipEnabled = false;
			base.IsHitTestVisible = false;
			MultiBinding binding = new MultiBinding
			{
				Converter = FocusVisualAdornerVisibilityConverter,
				Bindings = 
				{
					(BindingBase)new Binding("IsKeyboardFocusWithin")
					{
						Source = host
					},
					(BindingBase)new Binding
					{
						Source = host,
						Path = new PropertyPath(IsKeyboardInputActiveProperty)
					}
				}
			};
			SetBinding(UIElement.VisibilityProperty, binding);
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			Rect rectangle = new Rect(base.AdornedElement.RenderSize);
			Brush brush = (Brush)FindResource("VideoOSResourceKeysInternalForegroundBrush");
			GuidelineSet guidelineSet = new GuidelineSet();
			guidelineSet.GuidelinesX.Add(rectangle.Left + 0.5);
			guidelineSet.GuidelinesX.Add(rectangle.Right + 0.5);
			guidelineSet.GuidelinesY.Add(rectangle.Top + 0.5);
			guidelineSet.GuidelinesY.Add(rectangle.Bottom + 0.5);
			drawingContext.PushGuidelineSet(guidelineSet);
			drawingContext.DrawRectangle(Brushes.Transparent, new Pen(brush, VideoOSStyleFocusVisualStyleInternals.FocusVisualStrokeThickness)
			{
				DashStyle = VideoOSStyleFocusVisualStyleInternals.FocusVisualDashStyle
			}, rectangle);
		}
	}

	private static readonly Style _netFrameworkDefaultFocusVisualStyle;

	private static readonly Style _videoOSFocusVisualStyle;

	public static readonly DependencyProperty IsEnabledProperty;

	private static readonly DependencyProperty IsKeyboardInputActiveProperty;

	private static IMultiValueConverter FocusVisualAdornerVisibilityConverter { get; }

	static VideoOSStyleFocusVisualStyle()
	{
		IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(VideoOSStyleFocusVisualStyle), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, IsEnabledChanged));
		IsKeyboardInputActiveProperty = DependencyProperty.RegisterAttached("IsKeyboardInputActive", typeof(bool), typeof(VideoOSStyleFocusVisualStyle), new PropertyMetadata(false));
		FocusVisualAdornerVisibilityConverter = new LambdaConverter2<bool, bool>((bool isKeyboardFocusWithin, bool isKeyboardInputActive) => (isKeyboardFocusWithin && isKeyboardInputActive) ? ((object)Visibility.Visible) : ((object)Visibility.Collapsed), "FocusVisualAdornerVisibilityConverter");
		_netFrameworkDefaultFocusVisualStyle = (Style)FrameworkElement.FocusVisualStyleProperty.DefaultMetadata.DefaultValue;
		ThemeManager.InitializeThemingSystem();
		_videoOSFocusVisualStyle = (Style)ThemeManager.Instance.FindStaticResource("VideoOSStyleFocusVisualStyleFocusVisualStyleKey");
	}

	public static void SetIsEnabled(FrameworkElement frameworkElement, bool value)
	{
		frameworkElement.SetValue(IsEnabledProperty, value);
	}

	public static bool GetIsEnabled(FrameworkElement frameworkElement)
	{
		return (bool)frameworkElement.GetValue(IsEnabledProperty);
	}

	private static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (!(d is FrameworkElement frameworkElement))
		{
			return;
		}
		if (object.Equals(e.NewValue, true))
		{
			if (frameworkElement.GetValue(FrameworkElement.FocusVisualStyleProperty) == _netFrameworkDefaultFocusVisualStyle)
			{
				frameworkElement.SetValue(FrameworkElement.FocusVisualStyleProperty, _videoOSFocusVisualStyle);
			}
		}
		else if (frameworkElement.GetValue(FrameworkElement.FocusVisualStyleProperty) == _videoOSFocusVisualStyle)
		{
			frameworkElement.ClearValue(FrameworkElement.FocusVisualStyleProperty);
		}
	}

	private static void SetIsKeyboardInputActive(DependencyObject element, bool value)
	{
		element.SetValue(IsKeyboardInputActiveProperty, value);
	}

	public static void ApplyFocusVisualAdorner(FrameworkElement host, UIElement adornedElement)
	{
		AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
		if (adornerLayer != null)
		{
			adornerLayer.Add(new FocusVisualAdorner(host, adornedElement));
			SetIsEnabled(host, value: false);
			host.FocusVisualStyle = null;
			WeakEventManager<FrameworkElement, RoutedEventArgs>.RemoveHandler(host, "GotFocus", HostGotFocusHandler);
			WeakEventManager<FrameworkElement, RoutedEventArgs>.AddHandler(host, "GotFocus", HostGotFocusHandler);
		}
	}

	private static void HostGotFocusHandler(object sender, RoutedEventArgs e)
	{
		SetIsKeyboardInputActive((DependencyObject)sender, InputManager.Current.MostRecentInputDevice is KeyboardDevice);
	}
}
