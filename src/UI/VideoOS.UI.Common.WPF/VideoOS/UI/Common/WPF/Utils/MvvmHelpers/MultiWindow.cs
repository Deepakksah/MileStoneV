using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public sealed class MultiWindow
{
	public static readonly DependencyProperty IsWindowSelectedProperty = DependencyProperty.RegisterAttached("IsWindowSelected", typeof(bool), typeof(MultiWindow), new PropertyMetadata(false));

	private MultiWindow()
	{
	}

	public static bool GetIsWindowSelected(DependencyObject dependencyObject)
	{
		return (bool)dependencyObject.GetValue(IsWindowSelectedProperty);
	}

	public static void SetIsWindowSelected(DependencyObject dependencyObject, bool value)
	{
		dependencyObject.SetValue(IsWindowSelectedProperty, value);
	}
}
