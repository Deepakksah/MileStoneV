using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMenuButtonTextSmall : VideoOSMenuButtonBase
{
	public override MenuSizes MenuSize => MenuSizes.Small;

	static VideoOSMenuButtonTextSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMenuButtonTextSmall), new FrameworkPropertyMetadata(typeof(VideoOSMenuButtonTextSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
