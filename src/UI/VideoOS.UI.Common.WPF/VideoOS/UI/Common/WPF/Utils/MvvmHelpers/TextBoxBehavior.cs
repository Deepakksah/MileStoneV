using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public static class TextBoxBehavior
{
	public static readonly DependencyProperty SelectTextOnFocusEnterProperty = DependencyProperty.RegisterAttached("SelectTextOnFocusEnter", typeof(bool), typeof(TextBoxBehavior), new FrameworkPropertyMetadata(false, OnSelectTextOnFocusEnterChanged));

	public static readonly DependencyProperty SelectTextOnUpdateSourceProperty = DependencyProperty.RegisterAttached("SelectTextOnUpdateSource", typeof(bool), typeof(TextBoxBehavior), new FrameworkPropertyMetadata(false));

	public static readonly DependencyProperty UpdateOnEnterPressProperty = DependencyProperty.RegisterAttached("UpdateOnEnterPress", typeof(bool), typeof(TextBoxBehavior), new FrameworkPropertyMetadata(false, OnUpdateOnEnterPressChanged));

	public static readonly DependencyProperty RollbackOnRejectedInputProperty = DependencyProperty.RegisterAttached("RollbackOnRejectedInput", typeof(bool), typeof(TextBoxBehavior), new FrameworkPropertyMetadata(false, RollbackOnRejectedInputChanged));

	public static readonly DependencyProperty ValidateOnTextChangedProperty = DependencyProperty.RegisterAttached("ValidateOnTextChanged", typeof(bool), typeof(TextBoxBehavior), new FrameworkPropertyMetadata(false, ValidateOnTextChangedChanged));

	public static bool GetSelectTextOnFocusEnter(DependencyObject obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		return (bool)obj.GetValue(SelectTextOnFocusEnterProperty);
	}

	public static void SetSelectTextOnFocusEnter(DependencyObject obj, bool value)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		obj.SetValue(SelectTextOnFocusEnterProperty, value);
	}

	public static bool GetSelectTextOnUpdateSource(DependencyObject obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		return (bool)obj.GetValue(SelectTextOnUpdateSourceProperty);
	}

	public static void SetSelectTextOnUpdateSource(DependencyObject obj, bool value)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		obj.SetValue(SelectTextOnUpdateSourceProperty, value);
	}

	public static bool GetUpdateOnEnterPress(DependencyObject obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		return (bool)obj.GetValue(UpdateOnEnterPressProperty);
	}

	public static void SetUpdateOnEnterPress(DependencyObject obj, bool value)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		obj.SetValue(UpdateOnEnterPressProperty, value);
	}

	private static void OnSelectTextOnFocusEnterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is FrameworkElement frameworkElement && e.NewValue is bool)
		{
			if ((bool)e.NewValue)
			{
				frameworkElement.GotFocus += FrameworkElement_GotFocus;
				frameworkElement.PreviewMouseDown += FrameworkElement_PreviewMouseDown;
			}
			else
			{
				frameworkElement.GotFocus -= FrameworkElement_GotFocus;
				frameworkElement.PreviewMouseDown -= FrameworkElement_PreviewMouseDown;
			}
		}
	}

	private static void FrameworkElement_GotFocus(object sender, RoutedEventArgs e)
	{
		FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
		if (frameworkElement is TextBox)
		{
			((TextBoxBase)frameworkElement).SelectAll();
		}
		else if (frameworkElement is PasswordBox)
		{
			((PasswordBox)frameworkElement).SelectAll();
		}
	}

	private static void FrameworkElement_PreviewMouseDown(object sender, MouseButtonEventArgs e)
	{
		if (sender is FrameworkElement { IsFocused: false } frameworkElement)
		{
			e.Handled = true;
			frameworkElement.Focus();
		}
	}

	private static void OnUpdateOnEnterPressChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
	{
		if (obj is FrameworkElement frameworkElement && e.NewValue is bool)
		{
			if ((bool)e.NewValue)
			{
				frameworkElement.PreviewKeyDown += TextBox_OnPreviewKeyDown;
			}
			else
			{
				frameworkElement.PreviewKeyDown -= TextBox_OnPreviewKeyDown;
			}
		}
	}

	private static void TextBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.OriginalSource is TextBox textBox && e.Key == Key.Return)
		{
			BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty)?.UpdateSource();
			if (GetSelectTextOnUpdateSource(textBox))
			{
				textBox.SelectAll();
			}
		}
	}

	public static bool GetRollbackOnRejectedInput(DependencyObject obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		return (bool)obj.GetValue(RollbackOnRejectedInputProperty);
	}

	public static void SetRollbackOnRejectedInput(DependencyObject obj, bool value)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		obj.SetValue(RollbackOnRejectedInputProperty, value);
	}

	private static void RollbackOnRejectedInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is FrameworkElement frameworkElement && e.NewValue is bool)
		{
			if ((bool)e.NewValue)
			{
				frameworkElement.LostFocus += FrameworkElement_LostFocus;
				frameworkElement.PreviewKeyDown += FrameworkElement_OnPreviewKeyDown;
			}
			else
			{
				frameworkElement.LostFocus -= FrameworkElement_LostFocus;
				frameworkElement.PreviewKeyDown -= FrameworkElement_OnPreviewKeyDown;
			}
		}
	}

	private static void FrameworkElement_LostFocus(object sender, RoutedEventArgs e)
	{
		if (e.OriginalSource is FrameworkElement fe)
		{
			ValidateAndRejectInvalidInput(fe);
		}
	}

	private static void FrameworkElement_OnPreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.OriginalSource is FrameworkElement fe && e.Key == Key.Return)
		{
			ValidateAndRejectInvalidInput(fe);
		}
	}

	private static void ValidateAndRejectInvalidInput(FrameworkElement fe)
	{
		BindingExpression bindingExpression = BindingOperations.GetBindingExpression(fe, TextBox.TextProperty);
		bindingExpression?.ValidateWithoutUpdate();
		if (bindingExpression != null && bindingExpression.HasError)
		{
			bindingExpression.UpdateTarget();
		}
	}

	public static bool GetValidateOnTextChanged(DependencyObject obj)
	{
		return (bool)obj?.GetValue(ValidateOnTextChangedProperty);
	}

	public static void SetValidateOnTextChanged(DependencyObject obj, bool value)
	{
		obj?.SetValue(ValidateOnTextChangedProperty, value);
	}

	private static void ValidateOnTextChangedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		if (!(sender is TextBox textBox))
		{
			return;
		}
		object newValue = e.NewValue;
		if (newValue is bool)
		{
			if ((bool)newValue)
			{
				textBox.TextChanged += TextBox_TextChanged;
			}
			else
			{
				textBox.TextChanged -= TextBox_TextChanged;
			}
		}
	}

	private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (sender is TextBox textBox)
		{
			textBox.GetBindingExpression(TextBox.TextProperty)?.ValidateWithoutUpdate();
		}
	}
}
