using System;
using System.Runtime.InteropServices;

namespace VideoOS.Common.Controls;

public static class ExceptionDialogNativeMethods
{
	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool MessageBeep(ExceptionDialogForm.BeepType type);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr GetSystemMenu(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bRevert);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool EnableMenuItem(IntPtr hMenu, uint itemId, uint uEnable);
}
