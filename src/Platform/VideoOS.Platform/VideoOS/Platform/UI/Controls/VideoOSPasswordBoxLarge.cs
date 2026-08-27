using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSPasswordBoxLarge : VideoOSPasswordBoxBase
{
	public static class VideoOSPasswordBoxLargeInternals
	{
		public static IMultiValueConverter TextBrushConverter { get; } = new LambdaConverter2<DependencyObject, bool>(delegate(DependencyObject self, bool isEnabled)
		{
			string resourceKey = (isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockBodyMediumHighEmphasisForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockBodyMediumHighEmphasisForegroundDisabledBrush");
			return self.FindResource(resourceKey);
		}, "TextBrushConverter");
	}

	static VideoOSPasswordBoxLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSPasswordBoxLarge), new FrameworkPropertyMetadata(typeof(VideoOSPasswordBoxLarge)));
		ThemeManager.InitializeThemingSystem();
		Control.FontSizeProperty.OverrideMetadata(typeof(VideoOSPasswordBoxLarge), new FrameworkPropertyMetadata(14.0));
		Control.FontWeightProperty.OverrideMetadata(typeof(VideoOSPasswordBoxLarge), new FrameworkPropertyMetadata(VideoOSTextBlockBodyMedium.VideoOSTextBlockBodyMediumInternals.FontWeight));
	}
}
