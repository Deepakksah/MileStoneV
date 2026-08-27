using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace VideoOS.UI.Common.WPF.Utils;

public static class NativeMethods
{
	public enum IdEnableItem : uint
	{
		SC_CLOSE = 61536u,
		SC_MAXIMIZE = 61488u,
		SC_MINIMIZE = 61472u
	}

	public enum Enable : uint
	{
		MF_BYCOMMAND = 0u,
		MF_BYPOSITION = 1024u,
		MF_DISABLED = 2u,
		MF_ENABLED = 0u,
		MF_GRAYED = 1u
	}

	public struct RECT
	{
		public int Left;

		public int Top;

		public int Right;

		public int Bottom;
	}

	public enum DpiType
	{
		Effective,
		Angular,
		Raw
	}

	public static IntPtr HWND_TOP = IntPtr.Zero;

	public static IntPtr HWND_BOTTOM = new IntPtr(1);

	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hwnd, int index, int value);

	[DllImport("user32.dll")]
	public static extern int GetWindowLong(IntPtr hwnd, int index);

	[DllImport("user32.dll")]
	public static extern IntPtr GetSystemMenu(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bRevert);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool EnableMenuItem(IntPtr hMenu, IdEnableItem uIDEnableItem, Enable uEnable);

	public static void HideMinimizeAndMaximizeButtons(this Window window)
	{
		IntPtr handle = new WindowInteropHelper(window).Handle;
		long num = GetWindowLong(handle, -16);
		SetWindowLong(handle, -16, (int)(num & -196609));
	}

	[DllImport("user32.dll")]
	public static extern uint GetDoubleClickTime();

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

	[DllImport("User32.dll")]
	public static extern IntPtr MonitorFromPoint([In] System.Drawing.Point pt, [In] uint dwFlags);

	[DllImport("Shcore.dll")]
	public static extern IntPtr GetDpiForMonitor([In] IntPtr hmonitor, [In] DpiType dpiType, out uint dpiX, out uint dpiY);
}
