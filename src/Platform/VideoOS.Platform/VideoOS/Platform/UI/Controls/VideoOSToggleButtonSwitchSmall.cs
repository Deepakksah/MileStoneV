using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSToggleButtonSwitchSmall : VideoOSToggleButtonSwitchBase
{
	public static class VideoOSToggleButtonSwitchSmallInternals
	{
		public const string ToggleOnStoryboardKey = "VideoOSToggleButtonSwitchSmallToggleOnStoryboardKey";

		public const string ToggleOffStoryboardKey = "VideoOSToggleButtonSwitchSmallToggleOffStoryboardKey";
	}

	static VideoOSToggleButtonSwitchSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSToggleButtonSwitchSmall), new FrameworkPropertyMetadata(typeof(VideoOSToggleButtonSwitchSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
