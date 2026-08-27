using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Setup;

internal static class NativeMethods
{
	internal enum ShowWindowEnum
	{
		Hide = 0,
		ShowNormal = 1,
		ShowMinimized = 2,
		ShowMaximized = 3,
		Maximize = 3,
		ShowNormalNoActivate = 4,
		Show = 5,
		Minimize = 6,
		ShowMinNoActivate = 7,
		ShowNoActivate = 8,
		Restore = 9,
		ShowDefault = 10,
		ForceMinimized = 11
	}

	internal struct Windowplacement
	{
		public int length;

		public int flags;

		public int showCmd;

		public Point ptMinPosition;

		public Point ptMaxPosition;

		public Rectangle rcNormalPosition;
	}

	public const uint WM_KEYDOWN = 256u;

	public const uint WM_KEYUP = 257u;

	public const uint VK_RETURN = 13u;

	[DllImport("user32.dll", SetLastError = true)]
	public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool SetForegroundWindow(IntPtr hwnd);

	[DllImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool GetWindowPlacement(IntPtr hWnd, ref Windowplacement lpwndpl);

	[DllImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool AttachConsole(int processId);

	[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool FreeConsole();

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern IntPtr GetConsoleWindow();

	public static void AttachConsole()
	{
		AttachConsole(-1);
	}
}
