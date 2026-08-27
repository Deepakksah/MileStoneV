using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonAccentExtraSmall : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Small;

	static VideoOSMenuButtonAccentExtraSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonAccentExtraSmall), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonAccentExtraSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
