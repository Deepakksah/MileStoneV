using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonAccentSmall : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Small;

	static VideoOSMenuButtonAccentSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonAccentSmall), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonAccentSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
