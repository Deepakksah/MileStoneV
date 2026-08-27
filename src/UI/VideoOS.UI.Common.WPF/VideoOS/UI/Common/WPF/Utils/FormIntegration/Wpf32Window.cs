using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace VideoOS.UI.Common.WPF.Utils.FormIntegration;

public class Wpf32Window : System.Windows.Forms.IWin32Window
{
	public IntPtr Handle { get; private set; }

	public Wpf32Window(Window wpfWindow)
	{
		Handle = new WindowInteropHelper(wpfWindow).Handle;
	}
}
