using System.Runtime.InteropServices;
using System.Windows;

namespace VideoOS.Utilities.Mouse;

public static class MouseUtilities
{
	private struct Win32Point
	{
		public int X;

		public int Y;
	}

	private const int VK_LBUTTON = 1;

	private const int VK_RBUTTON = 2;

	private const int SM_SWAPBUTTON = 23;

	public static bool IsLeftMouseButtonPressed
	{
		get
		{
			short num = ((!GetSystemMetrics(23)) ? GetAsyncKeyState(1) : GetAsyncKeyState(2));
			if (num < 0)
			{
				return true;
			}
			return false;
		}
	}

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetCursorPos(ref Win32Point pt);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.U2)]
	private static extern short GetAsyncKeyState(int nVirtKey);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetSystemMetrics(int nIndex);

	public static Point GetMouseScreenPosition()
	{
		Win32Point pt = default(Win32Point);
		GetCursorPos(ref pt);
		return new Point(pt.X, pt.Y);
	}
}
