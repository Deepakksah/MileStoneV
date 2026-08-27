using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class BindingProxy : Freezable
{
	public static readonly DependencyProperty DataContextProperty = FrameworkElement.DataContextProperty.AddOwner(typeof(BindingProxy));

	public object DataContext
	{
		get
		{
			return GetValue(DataContextProperty);
		}
		set
		{
			SetValue(DataContextProperty, value);
		}
	}

	protected override Freezable CreateInstanceCore()
	{
		return new BindingProxy();
	}
}
