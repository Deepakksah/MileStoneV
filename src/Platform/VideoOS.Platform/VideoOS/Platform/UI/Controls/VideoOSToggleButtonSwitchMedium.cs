using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSToggleButtonSwitchMedium : VideoOSToggleButtonSwitchBase
{
	public static class VideoOSToggleButtonSwitchMediumInternals
	{
		public const string ToggleOnStoryboardKey = "VideoOSToggleButtonSwitchMediumToggleOnStoryboardKey";

		public const string ToggleOffStoryboardKey = "VideoOSToggleButtonSwitchMediumToggleOffStoryboardKey";
	}

	static VideoOSToggleButtonSwitchMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSToggleButtonSwitchMedium), new FrameworkPropertyMetadata(typeof(VideoOSToggleButtonSwitchMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
