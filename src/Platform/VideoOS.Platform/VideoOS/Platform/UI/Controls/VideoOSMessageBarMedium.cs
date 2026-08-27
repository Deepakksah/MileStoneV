using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSMessageBarMedium : VideoOSMessageBarBase
{
	public override Thickness InlineActionListMargin => new Thickness(24.0, 0.0, 0.0, 0.0);

	public override Thickness OwnlineActionListMargin => new Thickness(0.0, 5.0, 0.0, 5.0);

	static VideoOSMessageBarMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSMessageBarMedium), new FrameworkPropertyMetadata(typeof(VideoOSMessageBarMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
