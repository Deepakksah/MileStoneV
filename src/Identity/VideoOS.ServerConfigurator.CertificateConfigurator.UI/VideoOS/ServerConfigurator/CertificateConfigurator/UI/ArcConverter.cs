using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class ArcConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		if (values.Any((object v) => v == DependencyProperty.UnsetValue))
		{
			return null;
		}
		switch (parameter as string)
		{
		case "PointFromAngle":
		{
			double num3 = (double)values[0] * Math.PI / 180.0;
			double width2 = (double)values[1];
			double height = (double)values[2];
			double thickness = (double)values[3];
			Size size = ArcSize(width2, height, thickness);
			double num4 = thickness / 2.0;
			return new Point((1.0 + Math.Sin(num3)) * size.Width + num4, (1.0 - Math.Cos(num3)) * size.Height + num4);
		}
		case "ArcSize":
		{
			double width = (double)values[0];
			double height = (double)values[1];
			double thickness = (double)values[2];
			return ArcSize(width, height, thickness);
		}
		case "IsLargeArc":
		{
			double num = (double)values[0];
			double num2 = ((double)values[1] - num) % 360.0;
			return ((num2 < 0.0) ? (num2 + 360.0) : num2) > 180.0;
		}
		default:
			throw new ArgumentException("Invalid parameter", "parameter");
		}
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	private static Size ArcSize(double width, double height, double thickness)
	{
		double width2 = (width - thickness) / 2.0;
		double height2 = (height - thickness) / 2.0;
		return new Size(width2, height2);
	}
}
