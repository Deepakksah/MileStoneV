using System;
using System.Linq;
using System.Windows;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.Utils;

public static class RectangleTools
{
	public static Rect BoundingBox(Quadrangle quadrangle)
	{
		float[] source = new float[4]
		{
			quadrangle.UpperLeft.X,
			quadrangle.UpperRight.X,
			quadrangle.LowerRight.X,
			quadrangle.LowerLeft.X
		};
		float[] source2 = new float[4]
		{
			quadrangle.UpperLeft.Y,
			quadrangle.UpperRight.Y,
			quadrangle.LowerRight.Y,
			quadrangle.LowerLeft.Y
		};
		float num = source.Min();
		float num2 = source.Max();
		float num3 = source2.Min();
		float num4 = source2.Max();
		return new Rect(num, num3, num2 - num, num4 - num3);
	}

	public static Rect Denormalize(Rect rect, Size size)
	{
		double x = rect.X * size.Width;
		double y = rect.Y * size.Height;
		double width = rect.Width * size.Width;
		double height = rect.Height * size.Height;
		return new Rect(x, y, width, height);
	}

	public static Rect ApplyMargin(Rect rect, double margin, double maxX, double maxY)
	{
		double num = Math.Max(0.0, rect.Left - margin);
		double num2 = Math.Max(0.0, rect.Top - margin);
		double num3 = Math.Min(maxX, rect.Right + margin);
		double num4 = Math.Min(maxY, rect.Bottom + margin);
		return new Rect(num, num2, num3 - num, num4 - num2);
	}

	public static Rect ExpandToSize(Rect rect, Size size, double maxX, double maxY)
	{
		double num;
		double num2;
		if (rect.Width < size.Width)
		{
			num = Math.Max(0.0, rect.Left - (size.Width - rect.Width) / 2.0);
			num2 = num + size.Width;
			if (num2 > maxX)
			{
				num = Math.Max(0.0, num - (num2 - maxX));
				num2 = maxX;
			}
		}
		else
		{
			num = rect.Left;
			num2 = rect.Right;
		}
		double num3;
		double num4;
		if (rect.Height < size.Height)
		{
			num3 = Math.Max(0.0, rect.Top - (size.Height - rect.Height) / 2.0);
			num4 = num3 + size.Height;
			if (num4 > maxY)
			{
				num3 = Math.Max(0.0, num3 - (num4 - maxY));
				num4 = maxY;
			}
		}
		else
		{
			num3 = rect.Top;
			num4 = rect.Bottom;
		}
		return new Rect(num, num3, num2 - num, num4 - num3);
	}
}
