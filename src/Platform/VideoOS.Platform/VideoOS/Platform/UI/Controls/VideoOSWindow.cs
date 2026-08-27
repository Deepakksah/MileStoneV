using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Automation.AutomationPeers;

namespace VideoOS.Platform.UI.Controls;

public class VideoOSWindow : Window
{
	public enum MaximizeModes
	{
		None,
		MaximizeWorkArea,
		MaximizeMonitorArea,
		FullScreenWorkArea,
		FullScreenMonitorArea
	}

	private static class NativeMethods
	{
		public struct RECT
		{
			public int left;

			public int top;

			public int right;

			public int bottom;

			public static readonly RECT Empty;

			public int Width => Math.Abs(right - left);

			public int Height => bottom - top;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class MONITORINFO
		{
			public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

			public RECT rcMonitor;

			public RECT rcWork;

			public uint dwFlags;
		}

		public static class HWND
		{
			public static IntPtr Top = new IntPtr(0);
		}

		public struct WINDOWINFO
		{
			public uint cbSize;

			public RECT rcWindow;

			public RECT rcClient;

			public uint dwStyle;

			public uint dwExStyle;

			public uint dwWindowStatus;

			public uint cxWindowBorders;

			public uint cyWindowBorders;

			public ushort atomWindowType;

			public ushort wCreatorVersion;

			public WINDOWINFO(bool? filler)
			{
				this = default(WINDOWINFO);
				cbSize = (uint)Marshal.SizeOf(typeof(WINDOWINFO));
			}
		}

		public const int WM_ERASEBKGND = 20;

		public const int WM_SIZE = 5;

		public const int WM_INITMENUPOPUP = 279;

		public const int WM_NCLBUTTONDOWN = 161;

		public const int WM_NCHITTEST = 132;

		public const int MONITOR_DEFAULTTONEAREST = 2;

		public const int SWP_SHOWWINDOW = 64;

		public const uint SC_MOVE = 61456u;

		public const uint MF_ENABLED = 0u;

		public const uint MF_DISABLED = 2u;

		public const int GWL_STYLE = -16;

		public const uint WS_MAXIMIZEBOX = 65536u;

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

		[DllImport("user32.dll")]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

		[DllImport("dwmapi.dll")]
		public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateSolidBrush(uint crColor);

		[DllImport("user32.dll")]
		public static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

		[DllImport("user32.dll")]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

		public static RECT GetClientRect(IntPtr hWnd)
		{
			GetClientRect(hWnd, out var lpRect);
			return lpRect;
		}

		[DllImport("user32.dll")]
		public static extern int FillRect(IntPtr hDC, [In] ref RECT lprc, IntPtr hbr);

		[DllImport("User32")]
		public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

		[DllImport("User32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int SWP_flags);
	}

	public static readonly Dictionary<(string, ThemeType), IntPtr> BackgroundColorDictionary;

	public static readonly RoutedEvent TitleBarMouseDownEvent;

	public static readonly RoutedEvent NonClientAreaMouseMoveEvent;

	public static readonly DependencyProperty MaximizeModeProperty;

	private (WindowState State, WindowStyle Style, ResizeMode ResizeMode, Rect RestoreBounds) _preMaximizedModeValues;

	private WindowState _currentWindowState;

	private bool _suppressUpdateMaximizeSize;

	public MaximizeModes MaximizeMode
	{
		get
		{
			return (MaximizeModes)GetValue(MaximizeModeProperty);
		}
		set
		{
			SetValue(MaximizeModeProperty, value);
		}
	}

	public virtual string ThemeBackgroundBrushKey => "VideoOSResourceKeysInternalBackgroundBrushForWindows";

	public virtual string ThemeHighlightBrushKey => "VideoOSResourceKeysInternalHighlightBrushForWindows";

	public virtual string ThemeEmptyStateBrushKey => "VideoOSResourceKeysInternalEmptyStateBrushForWindows";

	public virtual string ThemeSubtleBrushKey => "VideoOSResourceKeysInternalSubtleBrushForWindows";

	public event RoutedEventHandler TitleBarMouseDown
	{
		add
		{
			AddHandler(TitleBarMouseDownEvent, value);
		}
		remove
		{
			RemoveHandler(TitleBarMouseDownEvent, value);
		}
	}

	public event RoutedEventHandler NonClientAreaMouseMove
	{
		add
		{
			AddHandler(NonClientAreaMouseMoveEvent, value);
		}
		remove
		{
			RemoveHandler(NonClientAreaMouseMoveEvent, value);
		}
	}

	static VideoOSWindow()
	{
		BackgroundColorDictionary = new Dictionary<(string, ThemeType), IntPtr>();
		TitleBarMouseDownEvent = EventManager.RegisterRoutedEvent("TitleBarMouseDown", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSWindow));
		NonClientAreaMouseMoveEvent = EventManager.RegisterRoutedEvent("NonClientAreaMouseMove", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSWindow));
		MaximizeModeProperty = DependencyProperty.Register("MaximizeMode", typeof(MaximizeModes), typeof(VideoOSWindow), new PropertyMetadata(MaximizeModes.None, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSWindow)o).MaximizeModeChanged((MaximizeModes)args.OldValue, (MaximizeModes)args.NewValue);
		}));
		ThemeManager.InitializeThemingSystem();
	}

	public Rect GetProperRestoreBounds()
	{
		if (MaximizeMode != MaximizeModes.None)
		{
			return _preMaximizedModeValues.RestoreBounds;
		}
		return base.RestoreBounds;
	}

	public VideoOSWindow()
	{
		base.StateChanged += VideoOSWindow_StateChanged;
		base.Loaded += VideoOSWindow_OnLoaded;
		base.Unloaded += VideoOSWindow_OnUnloaded;
		base.Closed += VideoOSWindow_OnClosed;
		IntPtr intPtr = new WindowInteropHelper(this).EnsureHandle();
		HwndSource.FromHwnd(intPtr).AddHook(WindowProc);
		base.Resources[typeof(ToolTip)] = ThemeManager.Instance.FindStaticResource(VideoOSStaticResourceKeys.ToolTipStyleKey);
		base.Resources[typeof(ScrollViewer)] = ThemeManager.Instance.FindStaticResource(VideoOSStaticResourceKeys.ScrollViewerStyleKey);
		UpdateWindowAndDialogBrushes();
		SetResourceReference(Control.BackgroundProperty, "VideoOSResourceKeysInternalBackgroundBrushForWindowAndDialog");
		VideoOSStyleFocusVisualStyle.SetIsEnabled(this, value: true);
		NativeMethods.GetSystemMenu(intPtr, bRevert: false);
	}

	private void VideoOSWindow_StateChanged(object sender, EventArgs e)
	{
		bool flag = false;
		if (base.WindowState == WindowState.Normal)
		{
			_preMaximizedModeValues.State = WindowState.Normal;
			MaximizeMode = MaximizeModes.None;
		}
		else if (base.WindowState == WindowState.Maximized && MaximizeMode == MaximizeModes.MaximizeMonitorArea && _currentWindowState == WindowState.Minimized)
		{
			flag = true;
		}
		_currentWindowState = base.WindowState;
		if (flag)
		{
			MaximizeMode = MaximizeModes.MaximizeWorkArea;
			MaximizeMode = MaximizeModes.MaximizeMonitorArea;
		}
	}

	private void VideoOSWindow_OnLoaded(object sender, RoutedEventArgs e)
	{
		base.FlowDirection = ClientControl.Instance.FlowDirection;
		SubscribeEvents();
		_currentWindowState = base.WindowState;
		UpdateWindowAndDialogBrushes();
		UpdateTitleBarColor();
	}

	private void VideoOSWindow_OnUnloaded(object sender, RoutedEventArgs e)
	{
		UnsubscribeEvents();
	}

	private void VideoOSWindow_OnClosed(object sender, EventArgs e)
	{
		UnsubscribeEvents();
	}

	private void ClientControl_OnFlowDirectionChanged(object sender, EventArgs e)
	{
		base.FlowDirection = ClientControl.Instance.FlowDirection;
	}

	private void ThemeManager_OnSelectedThemeChanged(object sender, EventArgs e)
	{
		UpdateWindowAndDialogBrushes();
		UpdateTitleBarColor();
	}

	private void MaximizeModeChanged(MaximizeModes oldValue, MaximizeModes newValue)
	{
		switch (newValue)
		{
		case MaximizeModes.None:
			base.WindowState = _preMaximizedModeValues.State;
			base.WindowStyle = _preMaximizedModeValues.Style;
			base.ResizeMode = _preMaximizedModeValues.ResizeMode;
			if (base.WindowState == WindowState.Normal)
			{
				base.Left = _preMaximizedModeValues.RestoreBounds.Left;
				base.Top = _preMaximizedModeValues.RestoreBounds.Top;
				base.Width = _preMaximizedModeValues.RestoreBounds.Width;
				base.Height = _preMaximizedModeValues.RestoreBounds.Height;
			}
			break;
		case MaximizeModes.MaximizeWorkArea:
		case MaximizeModes.MaximizeMonitorArea:
			if (oldValue == MaximizeModes.None)
			{
				_preMaximizedModeValues = (State: base.WindowState, Style: base.WindowStyle, ResizeMode: base.ResizeMode, RestoreBounds: base.RestoreBounds);
			}
			base.ResizeMode = ((newValue == MaximizeModes.MaximizeMonitorArea) ? ResizeMode.CanMinimize : _preMaximizedModeValues.ResizeMode);
			if (newValue == MaximizeModes.MaximizeMonitorArea && base.WindowState == WindowState.Maximized)
			{
				base.StateChanged -= VideoOSWindow_StateChanged;
				base.WindowState = WindowState.Normal;
				base.StateChanged += VideoOSWindow_StateChanged;
			}
			base.WindowState = WindowState.Maximized;
			base.WindowStyle = _preMaximizedModeValues.Style;
			if (newValue == MaximizeModes.MaximizeMonitorArea)
			{
				IntPtr hWnd = new WindowInteropHelper(this).EnsureHandle();
				int windowLong = NativeMethods.GetWindowLong(hWnd, -16);
				NativeMethods.SetWindowLong(hWnd, -16, (uint)((ulong)windowLong | 0x10000uL));
				UpdateMaximizeSize();
			}
			break;
		case MaximizeModes.FullScreenWorkArea:
		case MaximizeModes.FullScreenMonitorArea:
			if (oldValue == MaximizeModes.None)
			{
				_preMaximizedModeValues = (State: base.WindowState, Style: base.WindowStyle, ResizeMode: base.ResizeMode, RestoreBounds: base.RestoreBounds);
			}
			base.WindowState = WindowState.Maximized;
			base.WindowStyle = WindowStyle.None;
			base.ResizeMode = ResizeMode.CanMinimize;
			UpdateMaximizeSize();
			break;
		default:
			throw new ArgumentOutOfRangeException("newValue", newValue, null);
		}
	}

	private void SubscribeEvents()
	{
		UnsubscribeEvents();
		ClientControl.Instance.FlowDirectionChanged += ClientControl_OnFlowDirectionChanged;
		ThemeManager.Instance.SelectedThemeChanged += ThemeManager_OnSelectedThemeChanged;
	}

	private void UnsubscribeEvents()
	{
		ClientControl.Instance.FlowDirectionChanged -= ClientControl_OnFlowDirectionChanged;
		ThemeManager.Instance.SelectedThemeChanged -= ThemeManager_OnSelectedThemeChanged;
	}

	private void UpdateWindowAndDialogBrushes()
	{
		base.Resources["VideoOSResourceKeysInternalBackgroundBrushForWindowAndDialog"] = FindResource(ThemeBackgroundBrushKey);
		base.Resources["VideoOSResourceKeysInternalHighlightBrushForWindowAndDialog"] = FindResource(ThemeHighlightBrushKey);
		base.Resources["VideoOSResourceKeysInternalEmptyStateBrushForWindowAndDialog"] = FindResource(ThemeEmptyStateBrushKey);
		base.Resources["VideoOSResourceKeysInternalSubtleBrushForWindowAndDialog"] = FindResource(ThemeSubtleBrushKey);
	}

	private void UpdateTitleBarColor()
	{
		IntPtr hwnd = new WindowInteropHelper(this).EnsureHandle();
		FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Environment.ExpandEnvironmentVariables("%SystemRoot%\\System32\\kernel32.dll"));
		if (versionInfo.ProductMajorPart < 10 && versionInfo.ProductBuildPart < 17763)
		{
			return;
		}
		int attr = ((versionInfo.ProductBuildPart < 18985) ? 19 : 20);
		int attrValue = ((ThemeManager.Instance.SelectedTheme == ThemeType.Dark) ? 1 : 0);
		if (NativeMethods.DwmSetWindowAttribute(hwnd, attr, ref attrValue, 4) != 0 || !base.IsVisible)
		{
			return;
		}
		if (base.WindowState == WindowState.Maximized)
		{
			WindowStyle windowStyle = base.WindowStyle;
			if (windowStyle != WindowStyle.None)
			{
				_suppressUpdateMaximizeSize = true;
				base.WindowStyle = WindowStyle.None;
				base.WindowStyle = windowStyle;
				_suppressUpdateMaximizeSize = false;
			}
		}
		else
		{
			base.Width += 5.0;
			base.Width -= 5.0;
		}
	}

	private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
	{
		switch (msg)
		{
		case 20:
		{
			if (!BackgroundColorDictionary.ContainsKey((ThemeBackgroundBrushKey, ThemeManager.Instance.SelectedTheme)))
			{
				SolidColorBrush solidColorBrush = (SolidColorBrush)FindResource(ThemeBackgroundBrushKey);
				IntPtr value = NativeMethods.CreateSolidBrush((uint)(((solidColorBrush.Color.B << 16) | (solidColorBrush.Color.G << 8) | solidColorBrush.Color.R) & 0xFFFFFFFFu));
				BackgroundColorDictionary.Add((ThemeBackgroundBrushKey, ThemeManager.Instance.SelectedTheme), value);
			}
			NativeMethods.RECT lprc = NativeMethods.GetClientRect(hwnd);
			IntPtr hbr = BackgroundColorDictionary[(ThemeBackgroundBrushKey, ThemeManager.Instance.SelectedTheme)];
			return (IntPtr)NativeMethods.FillRect(wParam, ref lprc, hbr);
		}
		case 5:
			if ((MaximizeMode == MaximizeModes.MaximizeMonitorArea || MaximizeMode == MaximizeModes.FullScreenWorkArea || MaximizeMode == MaximizeModes.FullScreenMonitorArea) && !_suppressUpdateMaximizeSize)
			{
				UpdateMaximizeSize(hwnd);
			}
			break;
		case 279:
		{
			uint uEnable = ((base.WindowState != WindowState.Normal) ? 2u : 0u);
			NativeMethods.EnableMenuItem(wParam, 61456u, uEnable);
			break;
		}
		case 161:
			RaiseEvent(new RoutedEventArgs(TitleBarMouseDownEvent));
			break;
		case 132:
			if (!base.IsMouseOver)
			{
				RaiseEvent(new RoutedEventArgs(NonClientAreaMouseMoveEvent));
			}
			break;
		}
		return (IntPtr)0;
	}

	private void UpdateMaximizeSize(IntPtr? hwnd = null)
	{
		if (!hwnd.HasValue)
		{
			hwnd = new WindowInteropHelper(this).EnsureHandle();
		}
		IntPtr intPtr = NativeMethods.MonitorFromWindow(hwnd.Value, 2);
		if (intPtr != IntPtr.Zero)
		{
			NativeMethods.MONITORINFO mONITORINFO = new NativeMethods.MONITORINFO();
			NativeMethods.GetMonitorInfo(intPtr, mONITORINFO);
			NativeMethods.WINDOWINFO pwi = default(NativeMethods.WINDOWINFO);
			NativeMethods.GetWindowInfo(hwnd.Value, ref pwi);
			NativeMethods.RECT rECT = ((MaximizeMode == MaximizeModes.MaximizeMonitorArea || MaximizeMode == MaximizeModes.FullScreenMonitorArea) ? mONITORINFO.rcMonitor : mONITORINFO.rcWork);
			int x = mONITORINFO.rcMonitor.left - (int)pwi.cxWindowBorders;
			int y = mONITORINFO.rcMonitor.top - (int)pwi.cyWindowBorders;
			int cx = rECT.Width + (int)(pwi.cxWindowBorders * 2);
			int cy = rECT.Height + (int)(pwi.cyWindowBorders * 2);
			NativeMethods.SetWindowPos(hwnd.Value, NativeMethods.HWND.Top, x, y, cx, cy, 64);
		}
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSWindowAutomationPeer(this);
	}
}
