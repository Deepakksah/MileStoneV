using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace VideoOS.Mobile.UI.Behaviors;

public class ShowWindowIcon
{
	private const int GwlExstyle = -20;

	private const int SwpFramechanged = 32;

	private const int SwpNomove = 2;

	private const int SwpNosize = 1;

	private const int SwpNozorder = 4;

	private const int WsExDlgmodalframe = 1;

	public static readonly DependencyProperty ShowIconProperty = DependencyProperty.RegisterAttached("ShowIcon", typeof(bool), typeof(ShowWindowIcon), new FrameworkPropertyMetadata(true, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		RemoveIcon((Window)d);
	}));

	public static bool GetShowIcon(UIElement element)
	{
		return (bool)element.GetValue(ShowIconProperty);
	}

	public static void RemoveIcon(Window window)
	{
		window.SourceInitialized += delegate
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			int windowLong = GetWindowLong(handle, -20);
			SetWindowLong(handle, -20, windowLong | 1);
			SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 39u);
		};
	}

	public static void SetShowIcon(UIElement element, bool value)
	{
		element.SetValue(ShowIconProperty, value);
	}

	[DllImport("user32.dll")]
	private static extern int GetWindowLong(IntPtr hwnd, int index);

	[DllImport("user32.dll")]
	private static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll")]
	private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

	[DllImport("user32.dll")]
	private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);
}
