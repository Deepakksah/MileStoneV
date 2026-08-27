using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSContextMenuSmall : VideoOSContextMenuBase
{
	public static class VideoOSContextMenuSmallInternals
	{
		public const double ContextMenuMaxHeight = 281.0;

		public static IMultiValueConverter TextAppearanceConverter { get; } = new LambdaConverter1<bool>((bool isDestructive) => isDestructive ? VideoOSTextBlockBodySmall.Appearances.Destructive : VideoOSTextBlockBodySmall.Appearances.HighEmphasis, "TextAppearanceConverter");
	}

	static VideoOSContextMenuSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSContextMenuSmall), new FrameworkPropertyMetadata(typeof(VideoOSContextMenuSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
