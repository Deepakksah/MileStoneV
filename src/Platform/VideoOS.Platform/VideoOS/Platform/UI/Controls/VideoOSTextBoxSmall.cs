using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTextBoxSmall : VideoOSTextBoxBase
{
	public static class VideoOSTextBoxSmallInternals
	{
		public static IMultiValueConverter TextBrushConverter { get; } = new LambdaConverter2<DependencyObject, bool>(delegate(DependencyObject self, bool isEnabled)
		{
			string resourceKey = (isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockBodySmallHighEmphasisForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockBodySmallHighEmphasisForegroundDisabledBrush");
			return self.FindResource(resourceKey);
		}, "TextBrushConverter");
	}

	static VideoOSTextBoxSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBoxSmall), new FrameworkPropertyMetadata(typeof(VideoOSTextBoxSmall)));
		ThemeManager.InitializeThemingSystem();
		Control.FontSizeProperty.OverrideMetadata(typeof(VideoOSTextBoxSmall), new FrameworkPropertyMetadata(12.0));
		Control.FontWeightProperty.OverrideMetadata(typeof(VideoOSTextBoxSmall), new FrameworkPropertyMetadata(VideoOSTextBlockBodySmall.VideoOSTextBlockBodySmallInternals.FontWeight));
	}
}
