using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSPasswordBoxMedium : VideoOSPasswordBoxBase
{
	public static class VideoOSPasswordBoxMediumInternals
	{
		public static IMultiValueConverter TextBrushConverter { get; } = new LambdaConverter2<DependencyObject, bool>(delegate(DependencyObject self, bool isEnabled)
		{
			string resourceKey = (isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockBodyMediumHighEmphasisForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockBodyMediumHighEmphasisForegroundDisabledBrush");
			return self.FindResource(resourceKey);
		}, "TextBrushConverter");
	}

	static VideoOSPasswordBoxMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSPasswordBoxMedium), new FrameworkPropertyMetadata(typeof(VideoOSPasswordBoxMedium)));
		ThemeManager.InitializeThemingSystem();
		Control.FontSizeProperty.OverrideMetadata(typeof(VideoOSPasswordBoxMedium), new FrameworkPropertyMetadata(14.0));
		Control.FontWeightProperty.OverrideMetadata(typeof(VideoOSPasswordBoxMedium), new FrameworkPropertyMetadata(VideoOSTextBlockBodyMedium.VideoOSTextBlockBodyMediumInternals.FontWeight));
	}
}
