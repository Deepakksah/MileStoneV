using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonPrimarySmall : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Small;

	static VideoOSMenuButtonPrimarySmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonPrimarySmall), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonPrimarySmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
