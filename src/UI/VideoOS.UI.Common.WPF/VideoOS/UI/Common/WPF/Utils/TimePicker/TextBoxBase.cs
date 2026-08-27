using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.Utils.TimePicker;

public abstract class TextBoxBase : TextBox
{
	protected bool _isSettingTextInternal;

	public event EventHandler OnValidated;

	public TextBoxBase()
	{
		AddHandler(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), handledEventsToo: true);
		AddHandler(UIElement.GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), handledEventsToo: true);
		AddHandler(Control.MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), handledEventsToo: true);
	}

	private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
	{
		if (sender is TextBox { IsKeyboardFocusWithin: false } textBox)
		{
			textBox.Focus();
			e.Handled = true;
		}
	}

	private static void SelectAllText(object sender, RoutedEventArgs e)
	{
		if (e.OriginalSource is TextBox textBox)
		{
			textBox.SelectAll();
		}
	}

	public void Invalidate()
	{
		SetText(base.Text);
	}

	public abstract void Increase();

	public abstract void Decrease();

	protected void FireOnValidated()
	{
		if (this.OnValidated != null)
		{
			this.OnValidated(this, new EventArgs());
		}
	}

	protected void SetText(string text)
	{
		_isSettingTextInternal = true;
		base.Text = text;
		_isSettingTextInternal = false;
	}
}
