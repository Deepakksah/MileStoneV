using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using VideoOS.UI.Common.WPF.Annotations;
using VideoOS.Utilities.Validation;

namespace VideoOS.UI.Common.WPF.Utils;

public static class WindowExtensions
{
	private static class NativeMethods
	{
		public struct FLASHWINFO
		{
			public uint cbSize;

			public IntPtr hwnd;

			public uint dwFlags;

			public uint uCount;

			public uint dwTimeout;
		}

		public struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;
		}

		public const uint FLASHW_STOP = 0u;

		public const uint FLASHW_CAPTION = 1u;

		public const uint FLASHW_TRAY = 2u;

		public const uint FLASHW_ALL = 3u;

		public const uint FLASHW_TIMER = 4u;

		public const uint FLASHW_TIMERNOFG = 12u;

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
	}

	public static Rect UnscaledScreenBounds(this Window window)
	{
		if (new WindowInteropHelper(window).Handle == IntPtr.Zero)
		{
			return new Rect(0.0, 0.0, 0.0, 0.0);
		}
		NativeMethods.GetWindowRect(new WindowInteropHelper(window).Handle, out var lpRect);
		return new Rect(lpRect.Left, lpRect.Top, lpRect.Right - lpRect.Left, lpRect.Bottom - lpRect.Top);
	}

	public static Rect ScaledScreenBounds(this Window window, Screen screen)
	{
		screen.GetDpi(VideoOS.UI.Common.WPF.Utils.NativeMethods.DpiType.Effective, out var dpiScaleX, out var dpiScaleY);
		dpiScaleX /= 96.0;
		dpiScaleY /= 96.0;
		return new Rect
		{
			Width = (double)screen.Bounds.Width / dpiScaleX,
			Height = (double)screen.Bounds.Height / dpiScaleY,
			Location = new System.Windows.Point
			{
				X = (double)screen.Bounds.Left / dpiScaleX,
				Y = (double)screen.Bounds.Top / dpiScaleY
			}
		};
	}

	public static bool Flash(this Window @this)
	{
		NativeMethods.FLASHWINFO pwfi = default(NativeMethods.FLASHWINFO);
		pwfi.cbSize = Convert.ToUInt32(Marshal.SizeOf(pwfi));
		pwfi.hwnd = new WindowInteropHelper(@this).Handle;
		pwfi.dwFlags = 14u;
		pwfi.uCount = uint.MaxValue;
		pwfi.dwTimeout = 0u;
		return NativeMethods.FlashWindowEx(ref pwfi);
	}

	public static Screen GetScreen(this Window window)
	{
		return Screen.FromHandle(new WindowInteropHelper(window).Handle);
	}

	public static void Center(this Window window)
	{
		window.NullCheck("", "Center", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\SC_CommonFiles\\VideoOS.UI.Common.WPF\\Utils\\WindowExtensions\\WindowExtensions.cs", 98);
		System.Windows.Point centerLocation = GetCenterLocation(Screen.FromHandle(new WindowInteropHelper(window).Handle), (int)window.Height, (int)window.Width);
		window.Left = centerLocation.X;
		window.Top = centerLocation.Y;
	}

	public static bool? ShowDialogScaled(this Window window)
	{
		Rect locationAndSize = DetermineScaledWindowLocationAndSize(new System.Windows.Point(window.Left, window.Top), new System.Windows.Size(window.Width, window.Height));
		window.SetWindowsLocationAndSize(locationAndSize);
		return window.ShowDialog();
	}

	public static bool? ShowDialogScaledHorizontalCenter(this Window window, bool ensureWindowIsWithinVirtualScreens)
	{
		Screen screen = Screen.FromPoint(new System.Drawing.Point((int)window.Left, (int)window.Top));
		System.Windows.Size size = new System.Windows.Size(window.Width, window.Height);
		GetScalingRate(screen, out var scalingRateX, out var scalingRateY);
		System.Windows.Size size2 = new System.Windows.Size(size.Width * scalingRateX, size.Height * scalingRateY);
		System.Windows.Point location = new System.Windows.Point(window.Left - size2.Width / 2.0, window.Top);
		location.X /= scalingRateX;
		location.Y /= scalingRateY;
		window.SetWindowsLocationAndSize(new Rect(location, size));
		if (ensureWindowIsWithinVirtualScreens)
		{
			window.EnsureWindowIsWithinVirtualScreen();
		}
		return window.ShowDialog();
	}

	public static int FormsControlScaledHeight(Control control)
	{
		GetScalingRate(Screen.FromControl(control), out var _, out var scalingRateY);
		return (int)((double)control.Height * scalingRateY);
	}

	public static int FormsControlScaleValue(Control control, int value)
	{
		GetScalingRate(Screen.FromControl(control), out var scalingRateX, out var _);
		return (int)((double)value * scalingRateX);
	}

	public static Rectangle FormsControlScaledLocationAndSize(Control control)
	{
		GetScalingRate(Screen.FromControl(control), out var scalingRateX, out var scalingRateY);
		System.Drawing.Size size = new System.Drawing.Size((int)((double)control.Width * scalingRateX), (int)((double)control.Height * scalingRateY));
		System.Drawing.Point location = new System.Drawing.Point(control.Left, control.Top);
		if (location.X < 0)
		{
			location.X = (int)((double)location.X * scalingRateX);
		}
		else if (location.X > 0)
		{
			if (control.Anchor.HasFlag(AnchorStyles.Left))
			{
				location.X += Math.Abs(control.Width - size.Width) / 2;
			}
			else
			{
				location.X += control.Width - size.Width;
			}
		}
		if (location.Y < 0)
		{
			location.Y = (int)((double)location.Y * scalingRateY);
		}
		else if (location.Y > 0)
		{
			if (control.Anchor.HasFlag(AnchorStyles.Top))
			{
				location.Y += Math.Abs(control.Height - size.Height) / 2;
			}
			else
			{
				location.Y += control.Height - size.Height;
			}
		}
		return new Rectangle(location, size);
	}

	public static System.Windows.Point CalculateScaledPoint(System.Windows.Point point, Screen screenPointIsOn)
	{
		GetScalingRate(screenPointIsOn, out var scalingRateX, out var scalingRateY);
		return new System.Windows.Point(point.X * scalingRateX, point.Y * scalingRateY);
	}

	public static Rect DetermineScaledWindowLocationAndSize(System.Windows.Point location, System.Windows.Size size)
	{
		return DetermineScaledWindowLocationAndSize(null, location, size, isCenterScreen: false);
	}

	public static Rect DetermineScaledWindowLocationAndSize([CanBeNull] Screen screen, System.Windows.Size size)
	{
		return DetermineScaledWindowLocationAndSize(screen, new System.Windows.Point(0.0, 0.0), size, isCenterScreen: true);
	}

	public static Rect DetermineScaledWindowLocationAndSize([CanBeNull] Screen screen, System.Windows.Point location, System.Windows.Size size)
	{
		return DetermineScaledWindowLocationAndSize(screen, location, size, isCenterScreen: false);
	}

	private static Rect DetermineScaledWindowLocationAndSize([CanBeNull] Screen screen, System.Windows.Point location, System.Windows.Size size, bool isCenterScreen)
	{
		double scalingRateX;
		double scalingRateY;
		System.Windows.Point location2;
		if (screen != null)
		{
			GetScalingRate(screen, out scalingRateX, out scalingRateY);
			if (isCenterScreen)
			{
				System.Windows.Size size2 = new System.Windows.Size(size.Width * scalingRateX, size.Height * scalingRateY);
				location2 = new System.Windows.Point(((double)screen.Bounds.Width - size2.Width) / 2.0, ((double)screen.Bounds.Height - size2.Height) / 2.0);
			}
			else
			{
				location2 = new System.Windows.Point(location.X, location.Y);
			}
			location2.X += screen.Bounds.X;
			location2.Y += screen.Bounds.Y;
		}
		else
		{
			GetScalingRate(Screen.FromPoint(new System.Drawing.Point((int)location.X, (int)location.Y)), out scalingRateX, out scalingRateY);
			location2 = new System.Windows.Point(location.X, location.Y);
		}
		location2.X /= scalingRateX;
		location2.Y /= scalingRateY;
		return new Rect(location2, size);
	}

	private static void SetWindowsLocationAndSize(this Window window, Rect locationAndSize)
	{
		if (Math.Abs(locationAndSize.Left - window.Left) > 0.0)
		{
			window.Left = locationAndSize.Left;
		}
		if (Math.Abs(locationAndSize.Top - window.Top) > 0.0)
		{
			window.Top = locationAndSize.Top;
		}
		if (Math.Abs(locationAndSize.Width - window.Width) > 0.0)
		{
			window.Width = locationAndSize.Width;
		}
		if (Math.Abs(locationAndSize.Height - window.Height) > 0.0)
		{
			window.Height = locationAndSize.Height;
		}
	}

	private static void GetDpi(this Screen screen, VideoOS.UI.Common.WPF.Utils.NativeMethods.DpiType dpiType, out double dpiScaleX, out double dpiScaleY)
	{
		VideoOS.UI.Common.WPF.Utils.NativeMethods.GetDpiForMonitor(VideoOS.UI.Common.WPF.Utils.NativeMethods.MonitorFromPoint(new System.Drawing.Point(screen.Bounds.Left + 1, screen.Bounds.Top + 1), 2u), dpiType, out var dpiX, out var dpiY);
		dpiScaleX = dpiX;
		dpiScaleY = dpiY;
	}

	private static void GetScalingRate(Screen screen, out double scalingRateX, out double scalingRateY)
	{
		screen.GetDpi(VideoOS.UI.Common.WPF.Utils.NativeMethods.DpiType.Effective, out var dpiScaleX, out var dpiScaleY);
		scalingRateX = dpiScaleX / 96.0;
		scalingRateY = dpiScaleY / 96.0;
	}

	private static System.Windows.Point GetCenterLocation(Screen screen, int height, int width)
	{
		screen.NullCheck("", "GetCenterLocation", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\SC_CommonFiles\\VideoOS.UI.Common.WPF\\Utils\\WindowExtensions\\WindowExtensions.cs", 345);
		int num = screen.WorkingArea.Top + Math.Max(0, (screen.WorkingArea.Height - height) / 2);
		return new System.Windows.Point(screen.WorkingArea.Left + Math.Max(0, (screen.WorkingArea.Width - width) / 2), num);
	}

	private static void EnsureWindowIsWithinVirtualScreen(this Window window)
	{
		Rect rect = new Rect(new System.Windows.Point(SystemParameters.VirtualScreenLeft, SystemParameters.VirtualScreenTop), new System.Windows.Size(SystemParameters.VirtualScreenWidth, SystemParameters.VirtualScreenHeight));
		if (window.Left <= rect.Left)
		{
			window.Left = rect.Left;
		}
		if (window.Left + window.Width > rect.Right)
		{
			window.Left = rect.Right - window.Width;
		}
		if (window.Top + window.Height > rect.Bottom)
		{
			window.Top -= window.Height;
		}
	}
}
