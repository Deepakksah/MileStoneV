using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public static class VideoOSStyleScrollBar
{
	public static class VideoOSStyleScrollBarInternals
	{
		public const string ScrollBarStyleKey = "VideoOSStyleScrollBarScrollBarStyleKey";

		public const string ScrollViewerStyleKey = "VideoOSStyleScrollBarScrollViewerStyleKey";

		public const string ScrollBarScrollTrackRepeatButtonStyleKey = "VideoOSStyleScrollBarScrollBarScrollTrackRepeatButtonStyleKey";

		public const string ScrollBarThumbStyleKey = "VideoOSStyleScrollBarScrollBarThumbStyleKey";

		public const double ScrollBarSize = 16.0;

		private static readonly Thickness ContentBorderVerticalVisibleThickness = new Thickness(0.51, 0.0, 0.0, 0.0);

		private static readonly Thickness ContentBorderHorizontalVisibleThickness = new Thickness(0.0, 0.51, 0.0, 0.0);

		private static readonly Thickness ContentBorderNotVisibleThickness = new Thickness(0.0);

		public static IMultiValueConverter ContentBorderThicknessConverter { get; } = new LambdaConverter4<Orientation, bool, Visibility, Visibility>(delegate(Orientation orientation, bool isContentBorderVisible, Visibility computedVerticalScrollBarVisibility, Visibility computedHorizontalScrollBarVisibility)
		{
			if (!isContentBorderVisible)
			{
				return ContentBorderNotVisibleThickness;
			}
			switch (orientation)
			{
			case Orientation.Vertical:
				if (computedVerticalScrollBarVisibility == Visibility.Visible)
				{
					return ContentBorderVerticalVisibleThickness;
				}
				break;
			case Orientation.Horizontal:
				if (computedHorizontalScrollBarVisibility == Visibility.Visible)
				{
					return ContentBorderHorizontalVisibleThickness;
				}
				break;
			}
			return ContentBorderNotVisibleThickness;
		}, "ContentBorderThicknessConverter");

		public static IMultiValueConverter ThumbFillConverter { get; } = new LambdaConverter4<DependencyObject, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isEnabled, bool isMouseLeftButtonDown)
		{
			string resourceKey = "VideoOSResourceKeysInternalVideoOSScrollBarThumbBackgroundIdleBrush";
			if (!isEnabled)
			{
				resourceKey = "VideoOSResourceKeysInternalVideoOSScrollBarThumbBackgroundDisabledBrush";
			}
			else if (isMouseOver)
			{
				resourceKey = (isMouseLeftButtonDown ? "VideoOSResourceKeysInternalVideoOSScrollBarThumbBackgroundPressedBrush" : "VideoOSResourceKeysInternalVideoOSScrollBarThumbBackgroundHoveredBrush");
			}
			return self.FindResource(resourceKey);
		}, "ThumbFillConverter");

		public static IMultiValueConverter BorderCornerPixelVisibilityConverter { get; } = new LambdaConverter3<bool, Visibility, Visibility>((bool isContentBorderVisible, Visibility verticalScrollBarVisibility, Visibility horizontalScrollBarVisibility) => (isContentBorderVisible && verticalScrollBarVisibility == Visibility.Visible && horizontalScrollBarVisibility == Visibility.Visible) ? ((object)Visibility.Visible) : ((object)Visibility.Collapsed), "BorderCornerPixelVisibilityConverter");
	}

	public static readonly DependencyProperty IsContentBorderVisibleProperty;

	static VideoOSStyleScrollBar()
	{
		IsContentBorderVisibleProperty = DependencyProperty.RegisterAttached("IsContentBorderVisible", typeof(bool), typeof(VideoOSStyleScrollBar), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
		ThemeManager.InitializeThemingSystem();
	}

	public static void SetIsContentBorderVisible(FrameworkElement element, bool value)
	{
		element.SetValue(IsContentBorderVisibleProperty, value);
	}

	public static bool GetIsContentBorderVisible(FrameworkElement element)
	{
		return (bool)element.GetValue(IsContentBorderVisibleProperty);
	}
}
