using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMessageBarSmall : VideoOSMessageBarBase
{
	public override Thickness InlineActionListMargin => new Thickness(24.0, 0.0, 0.0, 0.0);

	public override Thickness OwnlineActionListMargin => new Thickness(0.0, 5.0, 0.0, 5.0);

	static VideoOSMessageBarSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMessageBarSmall), new FrameworkPropertyMetadata(typeof(VideoOSMessageBarSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
