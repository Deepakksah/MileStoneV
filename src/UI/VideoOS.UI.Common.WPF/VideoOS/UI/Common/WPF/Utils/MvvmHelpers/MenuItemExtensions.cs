using System.Windows;
using System.Windows.Controls;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class MenuItemExtensions : FrameworkElement
{
	public static readonly DependencyProperty IconResourceKeyProperty = DependencyProperty.RegisterAttached("IconResourceKey", typeof(object), typeof(MenuItemExtensions), new FrameworkPropertyMetadata(null, IconResourceKeyChanged));

	public static object GetIconResourceKey(DependencyObject owner)
	{
		return owner.GetValue(IconResourceKeyProperty);
	}

	public static void SetIconResourceKey(DependencyObject owner, object value)
	{
		owner.SetValue(IconResourceKeyProperty, value);
	}

	private static void IconResourceKeyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		if (sender is MenuItem menuItem)
		{
			if (e.NewValue == null)
			{
				menuItem.Icon = null;
			}
			else
			{
				menuItem.SetResourceReference(MenuItem.IconProperty, e.NewValue);
			}
		}
	}
}
