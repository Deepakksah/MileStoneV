using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public static class IconHelper
{
	private static class NativeMethods
	{
		[DllImport("user32.dll")]
		public static extern int GetWindowLong(IntPtr hwnd, int index);

		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);
	}

	private const int GWL_EXSTYLE = -20;

	private const int WS_EX_DLGMODALFRAME = 1;

	private const int SWP_NOSIZE = 1;

	private const int SWP_NOMOVE = 2;

	private const int SWP_NOZORDER = 4;

	private const int SWP_FRAMECHANGED = 32;

	[SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "Calling native methods with side effects")]
	public static void RemoveIcon(Window window)
	{
		IntPtr handle = new WindowInteropHelper(window).Handle;
		int windowLong = NativeMethods.GetWindowLong(handle, -20);
		NativeMethods.SetWindowLong(handle, -20, windowLong | 1);
		NativeMethods.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 39u);
	}
}
