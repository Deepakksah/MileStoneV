using System.Drawing;
using VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

namespace System.Windows;

public static class PointExtension
{
	public static double DistanceSqr(this Point point, Point otherPoint)
	{
		return point.DistanceSquare(otherPoint);
	}

	public static System.Drawing.Point ToWinFormsPoint(this Point point)
	{
		return point.ToSystemDrawingPoint();
	}
}
