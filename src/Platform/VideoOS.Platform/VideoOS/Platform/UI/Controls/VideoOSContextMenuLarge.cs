using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSContextMenuLarge : VideoOSContextMenuBase
{
	public static class VideoOSContextMenuLargeInternals
	{
		public const double ContextMenuMaxHeight = 276.0;

		public static IMultiValueConverter TextAppearanceConverter { get; } = new LambdaConverter1<bool>((bool isDestructive) => isDestructive ? VideoOSTextBlockBodyMedium.Appearances.Destructive : VideoOSTextBlockBodyMedium.Appearances.HighEmphasis, "TextAppearanceConverter");
	}

	static VideoOSContextMenuLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSContextMenuLarge), new FrameworkPropertyMetadata(typeof(VideoOSContextMenuLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
