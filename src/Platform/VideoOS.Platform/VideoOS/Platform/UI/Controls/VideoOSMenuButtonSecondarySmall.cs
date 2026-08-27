using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonSecondarySmall : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Small;

	static VideoOSMenuButtonSecondarySmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonSecondarySmall), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonSecondarySmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
