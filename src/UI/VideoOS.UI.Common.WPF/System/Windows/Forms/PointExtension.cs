using System.Drawing;

namespace System.Windows.Forms;

public static class PointExtension
{
	public static Point ToWpfPoint(this System.Drawing.Point point)
	{
		return new Point(point.X, point.Y);
	}
}
