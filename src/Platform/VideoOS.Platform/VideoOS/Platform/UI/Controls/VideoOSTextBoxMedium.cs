using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTextBoxMedium : VideoOSTextBoxBase
{
	public static class VideoOSTextBoxMediumInternals
	{
		public static IMultiValueConverter TextBrushConverter { get; } = new LambdaConverter2<DependencyObject, bool>(delegate(DependencyObject self, bool isEnabled)
		{
			string resourceKey = (isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockBodyMediumHighEmphasisForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockBodyMediumHighEmphasisForegroundDisabledBrush");
			return self.FindResource(resourceKey);
		}, "TextBrushConverter");
	}

	static VideoOSTextBoxMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBoxMedium), new FrameworkPropertyMetadata(typeof(VideoOSTextBoxMedium)));
		ThemeManager.InitializeThemingSystem();
		Control.FontSizeProperty.OverrideMetadata(typeof(VideoOSTextBoxMedium), new FrameworkPropertyMetadata(14.0));
		Control.FontWeightProperty.OverrideMetadata(typeof(VideoOSTextBoxMedium), new FrameworkPropertyMetadata(VideoOSTextBlockBodyMedium.VideoOSTextBlockBodyMediumInternals.FontWeight));
	}
}
