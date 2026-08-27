using System.Windows;
using VideoOS.Platform.Client;

namespace VideoOS.Platform.UI.Controls;

public static class VideoOSTheming
{
	public static void SetTheme(FrameworkElement frameworkElement, ThemeType theme)
	{
		ThemeManager.Instance.ApplyThemeOnElement(frameworkElement, theme);
	}
}
