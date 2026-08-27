using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace VideoOS.Platform.Search.Results;

public sealed class PolygonBoundingShape : BoundingShape
{
	public ICollection<Point> Points { get; } = new Collection<Point>();

	public PolygonBoundingShape()
	{
	}

	public PolygonBoundingShape(IEnumerable<Point> points)
	{
		if (points == null)
		{
			throw new ArgumentNullException("points");
		}
		foreach (Point point in points)
		{
			Points.Add(point);
		}
	}
}
