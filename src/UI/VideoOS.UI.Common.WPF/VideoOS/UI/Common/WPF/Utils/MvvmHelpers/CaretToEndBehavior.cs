using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class CaretToEndBehavior : Behavior<TextBox>
{
	public bool IsFocused { private get; set; }

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		if (base.AssociatedObject.IsReadOnly)
		{
			base.AssociatedObject.TextChanged += AssociatedObject_TextChanged;
		}
		else
		{
			base.AssociatedObject.IsKeyboardFocusWithinChanged += AssociatedObject_IsKeyboardFocusWithinChanged;
			if (base.AssociatedObject.IsKeyboardFocusWithin)
			{
				SetCursorAtEnd();
			}
		}
		base.AssociatedObject.Loaded += AssociatedObject_Loaded;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
		base.AssociatedObject.IsKeyboardFocusWithinChanged -= AssociatedObject_IsKeyboardFocusWithinChanged;
		base.AssociatedObject.Loaded -= AssociatedObject_Loaded;
	}

	private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
	{
		ScrollToEnd();
		if (!base.AssociatedObject.IsReadOnly && base.AssociatedObject.IsKeyboardFocusWithin)
		{
			SetCursorAtEnd();
		}
		if (IsFocused)
		{
			base.AssociatedObject.Focus();
		}
	}

	private void AssociatedObject_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		SetCursorAtEnd();
	}

	private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
	{
		ScrollToEnd();
	}

	private void ScrollToEnd()
	{
		base.AssociatedObject.ScrollToHorizontalOffset(base.AssociatedObject.ExtentWidth - base.AssociatedObject.ViewportWidth);
	}

	private void SetCursorAtEnd()
	{
		base.AssociatedObject.CaretIndex = int.MaxValue;
	}
}
