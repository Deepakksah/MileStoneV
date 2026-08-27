using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Utils.FormIntegration;

public class FormWPFKeyboardHandling
{
	public event System.Windows.Forms.KeyEventHandler KeyDown;

	public FormWPFKeyboardHandling(ElementHost elementHost)
	{
		bool flag = false;
		DependencyObject parent = VisualTreeHelper.GetParent(elementHost.Child);
		if (parent != null && VisualTreeHelper.GetParent(parent) is AdornerDecorator adornerDecorator)
		{
			adornerDecorator.KeyDown += control_KeyDown;
			flag = true;
		}
		if (!flag)
		{
			elementHost.Child.KeyDown += control_KeyDown;
		}
	}

	private void control_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
	{
		KeyDownHandler(sender, e);
	}

	private void KeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
	{
		System.Windows.Forms.KeyEventArgs e2 = ConvertKeys(e);
		if (e2 != null && this.KeyDown != null)
		{
			this.KeyDown(this, e2);
		}
	}

	public static System.Windows.Forms.KeyEventArgs ConvertKeys(System.Windows.Input.KeyEventArgs e)
	{
		System.Windows.Forms.KeyEventArgs result = null;
		Keys keys = (Keys)((e.Key == Key.System) ? KeyInterop.VirtualKeyFromKey(e.SystemKey) : ((e.Key == Key.DeadCharProcessed) ? KeyInterop.VirtualKeyFromKey(e.DeadCharProcessedKey) : ((e.Key != Key.ImeProcessed) ? KeyInterop.VirtualKeyFromKey(e.Key) : KeyInterop.VirtualKeyFromKey(e.ImeProcessedKey))));
		Keys modifierKeys = GetModifierKeys();
		if (keys != Keys.None)
		{
			if (modifierKeys != Keys.None)
			{
				keys |= modifierKeys;
			}
			result = new System.Windows.Forms.KeyEventArgs(keys);
		}
		return result;
	}

	private static Keys GetModifierKeys()
	{
		return Keyboard.Modifiers switch
		{
			ModifierKeys.Alt => Keys.Alt, 
			ModifierKeys.Control => Keys.Control, 
			ModifierKeys.Shift => Keys.Shift, 
			ModifierKeys.Alt | ModifierKeys.Control => Keys.Control | Keys.Alt, 
			ModifierKeys.Alt | ModifierKeys.Shift => Keys.Shift | Keys.Alt, 
			ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift => Keys.Shift | Keys.Control | Keys.Alt, 
			ModifierKeys.Control | ModifierKeys.Shift => Keys.Shift | Keys.Control, 
			_ => Keys.None, 
		};
	}
}
