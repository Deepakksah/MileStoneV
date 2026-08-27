using System.Drawing;
using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

public static class PointExtension
{
	public static double DistanceSquare(this System.Windows.Point point, System.Windows.Point otherPoint)
	{
		return (point.X - otherPoint.X) * (point.X - otherPoint.X) + (point.Y - otherPoint.Y) * (point.Y - otherPoint.Y);
	}

	public static System.Drawing.Point ToSystemDrawingPoint(this System.Windows.Point point)
	{
		return new System.Drawing.Point((int)point.X, (int)point.Y);
	}
}
