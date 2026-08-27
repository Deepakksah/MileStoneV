using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSPasswordBoxSmall : VideoOSPasswordBoxBase
{
	public static class VideoOSPasswordBoxSmallInternals
	{
		public static IMultiValueConverter TextBrushConverter { get; } = new LambdaConverter2<DependencyObject, bool>(delegate(DependencyObject self, bool isEnabled)
		{
			string resourceKey = (isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockBodySmallHighEmphasisForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockBodySmallHighEmphasisForegroundDisabledBrush");
			return self.FindResource(resourceKey);
		}, "TextBrushConverter");
	}

	static VideoOSPasswordBoxSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSPasswordBoxSmall), new FrameworkPropertyMetadata(typeof(VideoOSPasswordBoxSmall)));
		ThemeManager.InitializeThemingSystem();
		Control.FontSizeProperty.OverrideMetadata(typeof(VideoOSPasswordBoxSmall), new FrameworkPropertyMetadata(12.0));
		Control.FontWeightProperty.OverrideMetadata(typeof(VideoOSPasswordBoxSmall), new FrameworkPropertyMetadata(VideoOSTextBlockBodySmall.VideoOSTextBlockBodySmallInternals.FontWeight));
	}
}
