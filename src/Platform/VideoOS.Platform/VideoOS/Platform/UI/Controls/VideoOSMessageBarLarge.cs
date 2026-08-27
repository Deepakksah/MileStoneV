using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMessageBarLarge : VideoOSMessageBarBase
{
	public override Thickness InlineActionListMargin => new Thickness(24.0, 4.0, 0.0, 0.0);

	public override Thickness OwnlineActionListMargin => new Thickness(0.0, 5.0, 0.0, 5.0);

	static VideoOSMessageBarLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMessageBarLarge), new FrameworkPropertyMetadata(typeof(VideoOSMessageBarLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
