using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Utils;

public static class PopupUtils
{
	public enum CustomPlacementAnchoring
	{
		Begin,
		Center,
		End
	}

	public enum CustomVerticalPlacement
	{
		Top,
		Bottom
	}

	private static class NativeMethods
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		public class MonitorInfoEx
		{
			public int cbSize = Marshal.SizeOf(typeof(MonitorInfoEx));

			public Rect rcMonitor;

			public readonly Rect rcWork;

			public int dwFlags;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public char[] szDevice = new char[32];
		}

		public struct PointStruct(int x, int y)
		{
			public int x = x;

			public int y = y;
		}

		public struct Rect
		{
			public int left;

			public int top;

			public int right;

			public int bottom;
		}

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetMonitorInfo(HandleRef hMonitor, [In][Out] MonitorInfoEx info);

		[DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr MonitorFromPoint(PointStruct pt, int flags = 2);
	}

	public static CustomPopupVerticalPlacementResult CustomPopupVerticalPlacementCallback(Size popupSize, Size targetSize, Point offset, UIElement placementTarget, CustomVerticalPlacement placement, CustomPlacementAnchoring anchoring, Thickness trim = default(Thickness))
	{
		double num = 3.0;
		double screenScaleX = placementTarget.GetScreenScaleX();
		double screenScaleY = placementTarget.GetScreenScaleY();
		offset = new Point(offset.X * screenScaleX, offset.Y * screenScaleY);
		trim = new Thickness(trim.Left * screenScaleX, trim.Top * screenScaleY, trim.Right * screenScaleX, trim.Bottom * screenScaleY);
		num *= screenScaleY;
		double? suggestedMaxHeight = null;
		Point point = placementTarget.PointToScreen(new Point(0.0, 0.0));
		IntPtr handle = NativeMethods.MonitorFromPoint(new NativeMethods.PointStruct((int)point.X, (int)point.Y));
		NativeMethods.MonitorInfoEx monitorInfoEx = new NativeMethods.MonitorInfoEx();
		if (NativeMethods.GetMonitorInfo(new HandleRef(null, handle), monitorInfoEx))
		{
			double val = point.Y - (double)monitorInfoEx.rcWork.top;
			double val2 = (double)monitorInfoEx.rcWork.bottom - point.Y - targetSize.Height;
			suggestedMaxHeight = Math.Max(val, val2);
		}
		CustomPopupPlacement item;
		CustomPopupPlacement item2;
		CustomPopupPlacement item3;
		CustomPopupPlacement item4;
		CustomPopupPlacement item5;
		CustomPopupPlacement item6;
		if (FrameworkElement.GetFlowDirection(placementTarget) == FlowDirection.LeftToRight)
		{
			item = new CustomPopupPlacement(new Point(offset.X - trim.Left, offset.Y - trim.Top + targetSize.Height), PopupPrimaryAxis.Vertical);
			item2 = new CustomPopupPlacement(new Point(offset.X - trim.Left / 2.0 + (targetSize.Width - popupSize.Width) / 2.0 + trim.Right / 2.0, offset.Y - trim.Top + targetSize.Height), PopupPrimaryAxis.Vertical);
			item3 = new CustomPopupPlacement(new Point(offset.X - trim.Left - (popupSize.Width - targetSize.Width) + trim.Right, offset.Y - trim.Top + targetSize.Height), PopupPrimaryAxis.Vertical);
			item4 = new CustomPopupPlacement(new Point(offset.X - trim.Left, 0.0 - offset.Y - popupSize.Height + trim.Bottom - num), PopupPrimaryAxis.Vertical);
			item5 = new CustomPopupPlacement(new Point(offset.X - trim.Left + (targetSize.Width - popupSize.Width) / 2.0 + trim.Right / 2.0, 0.0 - offset.Y - popupSize.Height + trim.Bottom - num), PopupPrimaryAxis.Vertical);
			item6 = new CustomPopupPlacement(new Point(offset.X - trim.Left - (popupSize.Width - targetSize.Width) + trim.Right, 0.0 - offset.Y - popupSize.Height + trim.Bottom - num), PopupPrimaryAxis.Vertical);
		}
		else
		{
			item = new CustomPopupPlacement(new Point(0.0 - popupSize.Width + offset.X - trim.Left, offset.Y - trim.Top + targetSize.Height), PopupPrimaryAxis.Vertical);
			item2 = new CustomPopupPlacement(new Point(0.0 - popupSize.Width - offset.X - trim.Left / 2.0 - (targetSize.Width - popupSize.Width) / 2.0 - trim.Right / 2.0, offset.Y - trim.Top + targetSize.Height), PopupPrimaryAxis.Vertical);
			item3 = new CustomPopupPlacement(new Point(0.0 - targetSize.Width - trim.Right, offset.Y - trim.Top + targetSize.Height), PopupPrimaryAxis.Vertical);
			item4 = new CustomPopupPlacement(new Point(0.0 - popupSize.Width + offset.X - trim.Left, 0.0 - offset.Y - popupSize.Height + trim.Bottom - num), PopupPrimaryAxis.Vertical);
			item5 = new CustomPopupPlacement(new Point(0.0 - popupSize.Width - offset.X - trim.Left / 2.0 - (targetSize.Width - popupSize.Width) / 2.0 - trim.Right / 2.0, 0.0 - offset.Y - popupSize.Height + trim.Bottom - num), PopupPrimaryAxis.Vertical);
			item6 = new CustomPopupPlacement(new Point(0.0 - targetSize.Width - trim.Right, 0.0 - offset.Y - popupSize.Height + trim.Bottom - num), PopupPrimaryAxis.Vertical);
		}
		List<CustomPopupPlacement> list = new List<CustomPopupPlacement>();
		foreach (CustomVerticalPlacement item7 in new List<CustomVerticalPlacement>
		{
			placement,
			(placement == CustomVerticalPlacement.Top) ? CustomVerticalPlacement.Bottom : CustomVerticalPlacement.Top
		})
		{
			switch (item7)
			{
			case CustomVerticalPlacement.Top:
				switch (anchoring)
				{
				case CustomPlacementAnchoring.Begin:
					list.Add(item4);
					break;
				case CustomPlacementAnchoring.Center:
					list.Add(item5);
					break;
				case CustomPlacementAnchoring.End:
					list.Add(item6);
					break;
				}
				break;
			case CustomVerticalPlacement.Bottom:
				switch (anchoring)
				{
				case CustomPlacementAnchoring.Begin:
					list.Add(item);
					break;
				case CustomPlacementAnchoring.Center:
					list.Add(item2);
					break;
				case CustomPlacementAnchoring.End:
					list.Add(item3);
					break;
				}
				break;
			}
		}
		return new CustomPopupVerticalPlacementResult(list.ToArray(), suggestedMaxHeight);
	}
}
