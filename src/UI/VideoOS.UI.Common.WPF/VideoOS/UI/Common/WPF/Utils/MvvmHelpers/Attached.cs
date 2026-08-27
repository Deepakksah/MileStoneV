using System.Windows;
using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public static class Attached
{
	public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("Value", typeof(object), typeof(Attached));

	public static readonly DependencyProperty LabelProperty = DependencyProperty.RegisterAttached("Label", typeof(string), typeof(Attached));

	public static readonly DependencyProperty InheritedValueProperty = DependencyProperty.RegisterAttached("InheritedValue", typeof(object), typeof(Attached), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

	public static readonly DependencyProperty ExtraInputBindingsProperty = DependencyProperty.RegisterAttached("ExtraInputBindings", typeof(InputBindingCollection), typeof(Attached), new FrameworkPropertyMetadata(null, ExtraInputBindingsChanged));

	public static object GetValue(DependencyObject instance)
	{
		return instance.GetValue(ValueProperty);
	}

	public static void SetValue(DependencyObject instance, object value)
	{
		instance.SetValue(ValueProperty, value);
	}

	public static string GetLabel(DependencyObject instance)
	{
		return (string)instance.GetValue(LabelProperty);
	}

	public static void SetLabel(DependencyObject instance, string value)
	{
		instance.SetValue(LabelProperty, value);
	}

	public static object GetInheritedValue(DependencyObject instance)
	{
		return instance.GetValue(InheritedValueProperty);
	}

	public static void SetInheritedValue(DependencyObject instance, object value)
	{
		instance.SetValue(InheritedValueProperty, value);
	}

	public static void SetExtraInputBindings(DependencyObject owner, InputBindingCollection value)
	{
		owner.SetValue(ExtraInputBindingsProperty, value);
	}

	public static InputBindingCollection GetExtraInputBindings(DependencyObject owner)
	{
		return (InputBindingCollection)owner.GetValue(ExtraInputBindingsProperty);
	}

	private static void ExtraInputBindingsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		FrameworkElement frameworkElement = sender as FrameworkElement;
		InputBindingCollection inputBindingCollection = (InputBindingCollection)e.NewValue;
		if (inputBindingCollection != null)
		{
			frameworkElement.InputBindings.AddRange(inputBindingCollection);
		}
	}
}
