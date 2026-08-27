using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class ButtonWithPressedPropertyBehavior : Behavior<Button>
{
	public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register("IsPressed", typeof(bool), typeof(ButtonWithPressedPropertyBehavior), new FrameworkPropertyMetadata(false));

	private static readonly DependencyProperty IsPressedPrivateProperty = DependencyProperty.Register("IsPressedPrivate", typeof(bool), typeof(ButtonWithPressedPropertyBehavior), new FrameworkPropertyMetadata(false, IsPressedPrivateChanged));

	public bool IsPressed
	{
		get
		{
			return (bool)((DependencyObject)(object)this).GetValue(IsPressedProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(IsPressedProperty, (object)value);
		}
	}

	private static void IsPressedPrivateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		ButtonWithPressedPropertyBehavior obj = d as ButtonWithPressedPropertyBehavior;
		Binding binding = BindingOperations.GetBinding(d, IsPressedProperty);
		((DependencyObject)(object)obj).SetCurrentValue(IsPressedProperty, (object)(bool)e.NewValue);
	}

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		BindingOperations.SetBinding((DependencyObject)(object)this, IsPressedPrivateProperty, new Binding
		{
			Source = base.AssociatedObject,
			Path = new PropertyPath(ButtonBase.IsPressedProperty)
		});
	}
}
