using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VideoOS.UI.Common.WPF.Utils.Automation;

public class AutomationShape : Shape
{
	public Geometry Polygon = Geometry.Empty;

	protected override Geometry DefiningGeometry => Polygon;

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new AutomationShapePeer(this);
	}

	public Rect GetScreenBoundingBox()
	{
		Rect bounds = Polygon.Bounds;
		if (base.Parent is Canvas canvas)
		{
			Point point = canvas.PointToScreen(bounds.TopLeft);
			Point point2 = canvas.PointToScreen(bounds.BottomRight);
			return new Rect(point, point2);
		}
		return bounds;
	}
}
