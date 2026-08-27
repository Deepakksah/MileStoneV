using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonTertiarySmall : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Small;

	static VideoOSMenuButtonTertiarySmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonTertiarySmall), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonTertiarySmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
