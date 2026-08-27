using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSBadgeExtraSmall : VideoOSBadgeBase
{
	static VideoOSBadgeExtraSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSBadgeExtraSmall), new FrameworkPropertyMetadata(typeof(VideoOSBadgeExtraSmall)));
		ThemeManager.InitializeThemingSystem();
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		TextBlock templateChild = this.GetTemplateChild<TextBlock>("PART_TextBlock");
		ThemeManager.Instance.ApplyThemeOnElement(templateChild, ThemeType.Dark);
	}
}
