using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils.FormIntegration;

public static class AttachedProperties
{
	public static readonly DependencyProperty ShowNativeWindowsHintProperty = DependencyProperty.RegisterAttached("ShowNativeWindowsHint", typeof(Visibility), typeof(AttachedProperties), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.Inherits));

	public static Visibility GetShowNativeWindowsHint(DependencyObject d)
	{
		return (Visibility)d.GetValue(ShowNativeWindowsHintProperty);
	}

	public static void SetShowNativeWindowsHint(DependencyObject d, Visibility value)
	{
		d.SetValue(ShowNativeWindowsHintProperty, value);
	}
}
