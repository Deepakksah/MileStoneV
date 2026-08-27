using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class FocusProperties : Behavior<FrameworkElement>
{
	public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.Register("IsFocused", typeof(bool), typeof(FocusProperties), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

	public static readonly DependencyProperty IsKeyboardFocusedProperty = DependencyProperty.Register("IsKeyboardFocused", typeof(bool), typeof(FocusProperties), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

	public static readonly DependencyProperty IsKeyboardFocusWithinProperty = DependencyProperty.Register("IsKeyboardFocusWithin", typeof(bool), typeof(FocusProperties), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

	public bool IsFocused
	{
		get
		{
			return (bool)((DependencyObject)(object)this).GetValue(IsFocusedProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(IsFocusedProperty, (object)value);
		}
	}

	public bool IsKeyboardFocused
	{
		get
		{
			return (bool)((DependencyObject)(object)this).GetValue(IsKeyboardFocusedProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(IsKeyboardFocusedProperty, (object)value);
		}
	}

	public bool IsKeyboardFocusWithin
	{
		get
		{
			return (bool)((DependencyObject)(object)this).GetValue(IsKeyboardFocusWithinProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(IsKeyboardFocusWithinProperty, (object)value);
		}
	}

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.GotFocus += AssociatedObject_GotFocus;
		base.AssociatedObject.LostFocus += AssociatedObject_LostFocus;
		base.AssociatedObject.IsKeyboardFocusedChanged += AssociatedObject_IsKeyboardFocusedChanged;
		base.AssociatedObject.IsKeyboardFocusWithinChanged += AssociatedObject_IsKeyboardFocusWithinChanged;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
		base.AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
		base.AssociatedObject.IsKeyboardFocusedChanged -= AssociatedObject_IsKeyboardFocusedChanged;
		base.AssociatedObject.IsKeyboardFocusWithinChanged -= AssociatedObject_IsKeyboardFocusWithinChanged;
	}

	private void AssociatedObject_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		UpdateProperties();
	}

	private void AssociatedObject_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		UpdateProperties();
	}

	private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
	{
		UpdateProperties();
	}

	private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
	{
		UpdateProperties();
	}

	private void UpdateProperties()
	{
		((DependencyObject)(object)this).SetCurrentValue(IsFocusedProperty, (object)base.AssociatedObject.IsFocused);
		((DependencyObject)(object)this).SetCurrentValue(IsKeyboardFocusedProperty, (object)base.AssociatedObject.IsKeyboardFocused);
		((DependencyObject)(object)this).SetCurrentValue(IsKeyboardFocusWithinProperty, (object)base.AssociatedObject.IsKeyboardFocusWithin);
	}
}
