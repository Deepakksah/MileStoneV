using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace VideoOS.UI.Common.WPF.Utils.FormIntegration;

public class ScrollableWindowsFormsHost : WindowsFormsHost
{
	private static class NativeMethods
	{
		public struct RECT(double left, double top, double right, double bottom)
		{
			public int left = (int)left;

			public int top = (int)top;

			public int right = (int)right;

			public int bottom = (int)bottom;
		}

		[Flags]
		public enum SetWindowPosFlags : uint
		{
			AsynchronousWindowPosition = 0x4000u,
			DeferErase = 0x2000u,
			DrawFrame = 0x20u,
			FrameChanged = 0x20u,
			HideWindow = 0x80u,
			DoNotActivate = 0x10u,
			DoNotCopyBits = 0x100u,
			IgnoreMove = 2u,
			DoNotChangeOwnerZOrder = 0x200u,
			DoNotRedraw = 8u,
			DoNotReposition = 0x200u,
			DoNotSendChangingEvent = 0x400u,
			IgnoreResize = 1u,
			IgnoreZOrder = 4u,
			ShowWindow = 0x40u
		}

		[DllImport("GDI32.DLL")]
		public static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);

		[DllImport("User32.dll", SetLastError = true)]
		public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, [MarshalAs(UnmanagedType.Bool)] bool bRedraw);

		[DllImport("User32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

		[DllImport("User32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool InvalidateRect(IntPtr handle, ref RECT rect, [MarshalAs(UnmanagedType.Bool)] bool erase);

		[DllImport("User32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UpdateWindow(IntPtr handle);
	}

	private int _oldLeft = -1;

	private int _oldTop = -1;

	private int _oldRight = -1;

	private int _oldBottom = -1;

	private Rect _boundingBox;

	private bool _disposed;

	private ScrollViewer _scrollViewer;

	private WindowInteropHelper _windowHwnd;

	private DispatcherTimer _refreshTimer;

	public ScrollableWindowsFormsHost()
	{
		base.Margin = new Thickness(0.0);
		base.FontSize = WindowsFormsHostConstants.FontSize;
		base.FontFamily = new System.Windows.Media.FontFamily(WindowsFormsHostConstants.FontFamily);
		base.Loaded += ScrollableWindowsFormsHost_Loaded;
		base.IsVisibleChanged += ScrollableWindowsFormsHost_IsVisibleChanged;
		base.ChildChanged += ScrollableWindowsFormsHost_ChildChanged;
		_refreshTimer = new DispatcherTimer
		{
			Interval = TimeSpan.FromMilliseconds(50.0)
		};
		_refreshTimer.Tick += _refreshTimer_Tick;
	}

	protected override void Dispose(bool disposing)
	{
		if (!disposing)
		{
			return;
		}
		try
		{
			_disposed = true;
			if (_scrollViewer != null)
			{
				_scrollViewer.ScrollChanged -= _scrollViewer_ScrollChanged;
			}
			Window window = Window.GetWindow(this);
			if (window != null)
			{
				window.SizeChanged -= ScrollableWindowsFormsHost_SizeChanged;
			}
			_windowHwnd = null;
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	protected override void OnWindowPositionChanged(Rect rcBoundingBox)
	{
		if (_disposed || _boundingBox == rcBoundingBox)
		{
			return;
		}
		_boundingBox = rcBoundingBox;
		PresentationSource presentationSource = PresentationSource.FromVisual(this);
		if (presentationSource == null)
		{
			return;
		}
		Visual rootVisual = presentationSource.RootVisual;
		if (rootVisual == null || _scrollViewer == null || !_scrollViewer.IsDescendantOf(rootVisual))
		{
			return;
		}
		Rect rect = _scrollViewer.TransformToAncestor(rootVisual).TransformBounds(new Rect(0.0, 0.0, _scrollViewer.ViewportWidth, _scrollViewer.ViewportHeight));
		rect.X = 0.0;
		Rect rect2 = Rect.Intersect(rect, rcBoundingBox);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		if (rect2 != Rect.Empty)
		{
			num = (int)(rect2.TopLeft.X - rcBoundingBox.TopLeft.X);
			num2 = (int)(rect2.TopLeft.Y - rcBoundingBox.TopLeft.Y);
			num3 = (int)(rect2.BottomRight.X - rcBoundingBox.TopLeft.X);
			num4 = (int)(rect2.BottomRight.Y - rcBoundingBox.TopLeft.Y);
			SetWindowPosCore(rcBoundingBox);
		}
		else
		{
			_oldLeft = (_oldTop = (_oldRight = (_oldBottom = 0)));
			IntPtr regionHandle = NativeMethods.CreateRectRgn(_oldLeft, _oldTop, _oldRight, _oldBottom);
			TrySetWindowRegion(regionHandle, redraw: false);
		}
		if (_oldLeft != num || _oldTop != num2 || _oldRight != num3 || _oldBottom != num4)
		{
			IntPtr regionHandle2 = NativeMethods.CreateRectRgn(num, num2, num3, num4);
			TrySetWindowRegion(regionHandle2, redraw: false);
			if (num2 < _oldTop)
			{
				base.Child.Invalidate(new Rectangle(num, num2, num3 - num, _oldTop - num2), invalidateChildren: true);
			}
			if (num2 > _oldTop)
			{
				NativeMethods.RECT rect3 = new NativeMethods.RECT(rect.Left, rect.Top - (double)num2 + (double)_oldTop, rect.Right, rect.Top);
				NativeMethods.InvalidateRect(_windowHwnd.Handle, ref rect3, erase: false);
			}
			if (num4 > _oldBottom)
			{
				base.Child.Invalidate(new Rectangle(num, _oldBottom, num3 - num, num4 - _oldBottom), invalidateChildren: true);
			}
			if (num4 < _oldBottom)
			{
				NativeMethods.RECT rect4 = new NativeMethods.RECT(rect.Left, rect.Bottom, rect.Right, rect.Bottom + (double)_oldBottom - (double)num4);
				NativeMethods.InvalidateRect(_windowHwnd.Handle, ref rect4, erase: false);
			}
			if (num < _oldLeft)
			{
				base.Child.Invalidate(new Rectangle(num, num2, _oldLeft - num, num4 - num2), invalidateChildren: true);
			}
			if (num > _oldLeft)
			{
				NativeMethods.RECT rect5 = new NativeMethods.RECT(rect.Left - (double)num + (double)_oldLeft, rect.Top, rect.Left, rect.Bottom);
				NativeMethods.InvalidateRect(_windowHwnd.Handle, ref rect5, erase: false);
			}
			if (num3 > _oldRight)
			{
				base.Child.Invalidate(new Rectangle(_oldRight, num2, num3 - _oldRight, num4 - num2), invalidateChildren: true);
			}
			if (num3 < _oldRight)
			{
				NativeMethods.RECT rect6 = new NativeMethods.RECT(rect.Right, rect.Top, rect.Right + (double)_oldRight - (double)num3, rect.Bottom);
				NativeMethods.InvalidateRect(_windowHwnd.Handle, ref rect6, erase: false);
			}
			NativeMethods.UpdateWindow(_windowHwnd.Handle);
			_oldLeft = num;
			_oldTop = num2;
			_oldRight = num3;
			_oldBottom = num4;
		}
	}

	private void ScrollableWindowsFormsHost_ChildChanged(object sender, ChildChangedEventArgs e)
	{
		if (e.PreviousChild is System.Windows.Forms.Control control)
		{
			control.BackColorChanged -= Child_BackColorChanged;
		}
		if (base.Child != null)
		{
			base.Child.BackColorChanged += Child_BackColorChanged;
			if (base.Parent == null)
			{
				base.Child.SuspendLayout();
			}
		}
	}

	private void Child_BackColorChanged(object sender, EventArgs e)
	{
		ClearClip();
		ForceRecalculateClip();
	}

	private void ScrollableWindowsFormsHost_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (base.IsVisible && base.Child != null)
		{
			ClearClip();
			ForceRecalculateClip();
			if (base.Child != null && !base.Child.IsDisposed)
			{
				base.Child.Refresh();
			}
		}
	}

	private void ScrollableWindowsFormsHost_Loaded(object sender, RoutedEventArgs e)
	{
		_scrollViewer = this.GetAncestors<ScrollViewer>().FirstOrDefault();
		if (_scrollViewer != null)
		{
			_scrollViewer.ScrollChanged += _scrollViewer_ScrollChanged;
		}
		Window window = Window.GetWindow(this);
		window.SizeChanged += ScrollableWindowsFormsHost_SizeChanged;
		_windowHwnd = new WindowInteropHelper(window);
		if (base.Child != null && !base.Child.IsDisposed)
		{
			base.Child.ResumeLayout();
		}
		ClearClip();
		ForceRecalculateClip();
		if (base.Child != null && !base.Child.IsDisposed)
		{
			base.Child.Refresh();
		}
	}

	private void _scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
	{
		_refreshTimer.Stop();
		_refreshTimer.Start();
	}

	private void _refreshTimer_Tick(object sender, EventArgs e)
	{
		_refreshTimer.Stop();
		ForceRecalculateClip();
	}

	private void ScrollableWindowsFormsHost_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		ForceRecalculateClip();
	}

	private void ForceRecalculateClip()
	{
		InvalidateMeasure();
		_boundingBox = Rect.Empty;
		_oldLeft = 0;
		_oldTop = 0;
		_oldRight = 0;
		_oldBottom = 0;
		if (base.Child != null && !base.Child.IsDisposed)
		{
			base.Child.PerformLayout();
		}
	}

	private void ClearClip()
	{
		TrySetWindowRegion(IntPtr.Zero, redraw: true);
		NativeMethods.SetWindowPos(base.Handle, IntPtr.Zero, 0, 0, 0, 0, NativeMethods.SetWindowPosFlags.DoNotActivate | NativeMethods.SetWindowPosFlags.DoNotCopyBits | NativeMethods.SetWindowPosFlags.DoNotChangeOwnerZOrder | NativeMethods.SetWindowPosFlags.DoNotSendChangingEvent | NativeMethods.SetWindowPosFlags.IgnoreZOrder);
		if (base.Child != null && !base.Child.IsDisposed)
		{
			base.Child.Refresh();
		}
	}

	private bool TrySetWindowRegion(IntPtr regionHandle, bool redraw)
	{
		if (base.Handle == IntPtr.Zero)
		{
			return false;
		}
		if (NativeMethods.SetWindowRgn(base.Handle, regionHandle, redraw) == 0)
		{
			return false;
		}
		return true;
	}

	private void SetWindowPosCore(Rect rcBoundingBox)
	{
		NativeMethods.SetWindowPos(base.Handle, IntPtr.Zero, (int)rcBoundingBox.X, (int)rcBoundingBox.Y, (int)rcBoundingBox.Width, (int)rcBoundingBox.Height, NativeMethods.SetWindowPosFlags.DeferErase | NativeMethods.SetWindowPosFlags.DoNotActivate | NativeMethods.SetWindowPosFlags.DoNotChangeOwnerZOrder | NativeMethods.SetWindowPosFlags.DoNotSendChangingEvent | NativeMethods.SetWindowPosFlags.IgnoreZOrder);
	}
}
