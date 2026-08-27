using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.Util;

public static class AnalyticsOverlayBuilder
{
	private enum DrawType
	{
		Object,
		Rule,
		BoundingBox,
		Motion
	}

	private static readonly System.Drawing.Color DefaultObjectPathColor = System.Drawing.Color.Red;

	private static readonly System.Drawing.Color DefaultRulePathColor = System.Drawing.Color.Blue;

	private static readonly System.Drawing.Color DefaultMotionPathColor = System.Drawing.Color.Green;

	public static Bitmap BuildOverlay(int width, int height, BaseEvent baseEvent)
	{
		if (baseEvent is AnalyticsEvent analyticsEvent)
		{
			return BuildOverlay(width, height, analyticsEvent.ObjectList, analyticsEvent.RuleList);
		}
		return BuildOverlayFromText(width, height, baseEvent.EventHeader.Source.Name, baseEvent.EventHeader.Message);
	}

	public static List<Shape> BuildShapeOverlay(int width, int height, BaseEvent baseEvent)
	{
		if (baseEvent is AnalyticsEvent analyticsEvent)
		{
			return BuildShapeOverlay(width, height, analyticsEvent.ObjectList, analyticsEvent.RuleList);
		}
		return BuildShapeOverlayFromText(baseEvent.EventHeader.Source.Name, baseEvent.EventHeader.Message);
	}

	public static Bitmap BuildOverlay(int width, int height, Alarm alarm)
	{
		if (alarm.ObjectList != null || alarm.RuleList != null)
		{
			return BuildOverlay(width, height, alarm.ObjectList, alarm.RuleList);
		}
		return BuildOverlayFromText(width, height, alarm.EventHeader.Source.Name, alarm.EventHeader.Message);
	}

	public static List<Shape> BuildShapeOverlay(int width, int height, Alarm alarm)
	{
		if (alarm.ObjectList != null || alarm.RuleList != null)
		{
			return BuildShapeOverlay(width, height, alarm.ObjectList, alarm.RuleList);
		}
		return BuildShapeOverlayFromText(alarm.EventHeader.Source.Name, alarm.EventHeader.Message);
	}

	public static Bitmap BuildOverlayFromText(int width, int height, string source, string message)
	{
		Bitmap bitmap = new Bitmap(width, height);
		using Graphics graphics = Graphics.FromImage(bitmap);
		graphics.DrawString(source, new Font(System.Drawing.FontFamily.GenericSerif, 11f), System.Drawing.Brushes.Black, new System.Drawing.Point(5, 5));
		graphics.DrawString(message, new Font(System.Drawing.FontFamily.GenericSerif, 11f), System.Drawing.Brushes.Black, new System.Drawing.Point(5, 20));
		return bitmap;
	}

	public static List<Shape> BuildShapeOverlayFromText(string source, string message)
	{
		List<Shape> list = new List<Shape>();
		Typeface typeface = new Typeface(new System.Windows.Media.FontFamily(System.Drawing.FontFamily.GenericSerif.Name), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
		Path path = new Path();
		FormattedText formattedText = new FormattedText(source, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 20.0, System.Windows.Media.Brushes.Black, VisualTreeHelper.GetDpi(path).PixelsPerDip);
		path.Data = formattedText.BuildGeometry(new System.Windows.Point(20.0, 15.0));
		path.Fill = System.Windows.Media.Brushes.Black;
		list.Add(path);
		Path path2 = new Path();
		FormattedText formattedText2 = new FormattedText(message, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 20.0, System.Windows.Media.Brushes.Black, VisualTreeHelper.GetDpi(path2).PixelsPerDip);
		path2.Data = formattedText2.BuildGeometry(new System.Windows.Point(20.0, 40.0));
		path2.Fill = System.Windows.Media.Brushes.Black;
		list.Add(path2);
		return list;
	}

	public static Bitmap BuildOverlay(int width, int height, AnalyticsObjectList analyticsObjectList, RuleList ruleList)
	{
		Bitmap bitmap = new Bitmap(width, height);
		using (Graphics g = Graphics.FromImage(bitmap))
		{
			if (ruleList != null)
			{
				DrawRules(g, null, width, height, ruleList);
			}
			if (analyticsObjectList != null)
			{
				DrawObjects(g, null, width, height, analyticsObjectList);
			}
		}
		return bitmap;
	}

	public static List<Shape> BuildShapeOverlay(int width, int height, AnalyticsObjectList analyticsObjectList, RuleList ruleList)
	{
		List<Shape> list = new List<Shape>();
		if (ruleList != null)
		{
			DrawRules(null, list, width, height, ruleList);
		}
		if (analyticsObjectList != null)
		{
			DrawObjects(null, list, width, height, analyticsObjectList);
		}
		return list;
	}

	private static void DrawBoundingBox(Graphics g, List<Shape> shapes, int width, int height, BoundingBox boundingBox)
	{
		if (boundingBox != null)
		{
			PointList pointList = new PointList();
			TPoint tPoint = new TPoint();
			tPoint.X = boundingBox.Left;
			tPoint.Y = boundingBox.Top;
			pointList.Add(tPoint);
			TPoint tPoint2 = new TPoint();
			tPoint2.X = boundingBox.Right;
			tPoint2.Y = boundingBox.Top;
			pointList.Add(tPoint2);
			TPoint tPoint3 = new TPoint();
			tPoint3.X = boundingBox.Right;
			tPoint3.Y = boundingBox.Bottom;
			pointList.Add(tPoint3);
			TPoint tPoint4 = new TPoint();
			tPoint4.X = boundingBox.Left;
			tPoint4.Y = boundingBox.Bottom;
			pointList.Add(tPoint4);
			TPolygon tPolygon = new TPolygon();
			tPolygon.PointList = pointList;
			tPolygon.Closed = true;
			tPolygon.FillColor = null;
			tPolygon.Color = boundingBox.Color;
			DrawPolygon(g, shapes, tPolygon, width, height, DrawType.BoundingBox);
		}
	}

	private static void DrawRules(Graphics g, List<Shape> shapes, int width, int height, RuleList ruleList)
	{
		foreach (Rule rule in ruleList)
		{
			if (rule.PolygonList == null)
			{
				continue;
			}
			foreach (TPolygon polygon in rule.PolygonList)
			{
				DrawPolygon(g, shapes, polygon, width, height, DrawType.Rule);
			}
		}
	}

	private static void DrawObjects(Graphics g, List<Shape> shapes, int width, int height, AnalyticsObjectList analyticsObjectList)
	{
		foreach (AnalyticsObject analyticsObject in analyticsObjectList)
		{
			if (analyticsObject.Motion != null)
			{
				DrawPolygon(g, shapes, analyticsObject.Motion.Path, width, height, DrawType.Motion);
			}
			if (analyticsObject.Polygon != null)
			{
				DrawPolygon(g, shapes, analyticsObject.Polygon, width, height, DrawType.Object);
			}
			else
			{
				DrawBoundingBox(g, shapes, width, height, analyticsObject.BoundingBox);
			}
		}
	}

	private static void DrawPolygon(Graphics g, List<Shape> shapes, TPolygon polygon, int width, int height, DrawType type)
	{
		if (polygon == null || polygon.PointList == null)
		{
			return;
		}
		List<System.Drawing.Point> list = new List<System.Drawing.Point>();
		bool flag = true;
		foreach (TPoint point in polygon.PointList)
		{
			if (point.X > 1.0 || point.Y > 1.0)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			foreach (TPoint point2 in polygon.PointList)
			{
				System.Drawing.Point item = new System.Drawing.Point((int)(point2.X * (double)(width - 1)), (int)(point2.Y * (double)(height - 1)));
				list.Add(item);
			}
		}
		else
		{
			foreach (TPoint point3 in polygon.PointList)
			{
				System.Drawing.Point item2 = new System.Drawing.Point((int)point3.X, (int)point3.Y);
				list.Add(item2);
			}
		}
		if (polygon.Closed && list.Count > 0)
		{
			list.Add(list[0]);
		}
		System.Drawing.Color pathColor = ((polygon.Color != null) ? System.Drawing.Color.FromArgb(polygon.Color.A, polygon.Color.R, polygon.Color.G, polygon.Color.B) : (type switch
		{
			DrawType.Rule => DefaultRulePathColor, 
			DrawType.Motion => DefaultMotionPathColor, 
			_ => DefaultObjectPathColor, 
		}));
		System.Drawing.Color fillColor = ((!polygon.Closed || polygon.FillColor == null) ? System.Drawing.Color.Transparent : System.Drawing.Color.FromArgb(polygon.FillColor.A, polygon.FillColor.R, polygon.FillColor.G, polygon.FillColor.B));
		if (g != null)
		{
			DrawPolygonToGraphics(g, list, fillColor, pathColor);
		}
		if (shapes != null)
		{
			DrawPolygonToShapes(shapes, list, fillColor, pathColor);
		}
	}

	private static void DrawPolygonToGraphics(Graphics g, List<System.Drawing.Point> points, System.Drawing.Color fillColor, System.Drawing.Color pathColor)
	{
		GraphicsPath graphicsPath = new GraphicsPath();
		graphicsPath.AddLines(points.ToArray());
		if (fillColor != System.Drawing.Color.Transparent)
		{
			using SolidBrush brush = new SolidBrush(fillColor);
			g.FillPath(brush, graphicsPath);
		}
		using SolidBrush brush2 = new SolidBrush(pathColor);
		using System.Drawing.Pen pen = new System.Drawing.Pen(brush2);
		g.DrawPath(pen, graphicsPath);
	}

	private static void DrawPolygonToShapes(List<Shape> shapes, List<System.Drawing.Point> points, System.Drawing.Color fillColor, System.Drawing.Color pathColor)
	{
		Polyline polyline = new Polyline();
		polyline.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(pathColor.A, pathColor.R, pathColor.G, pathColor.B));
		polyline.StrokeThickness = 2.0;
		polyline.StrokeEndLineCap = PenLineCap.Square;
		foreach (System.Drawing.Point point in points)
		{
			polyline.Points.Add(new System.Windows.Point(point.X, point.Y));
		}
		if (fillColor != System.Drawing.Color.Transparent)
		{
			polyline.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(fillColor.A, fillColor.R, fillColor.G, fillColor.B));
			polyline.FillRule = FillRule.EvenOdd;
		}
		shapes.Add(polyline);
	}
}
