using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public partial class ArcControl : UserControl, IComponentConnector
{
	public static readonly DependencyProperty Angle1Property = DependencyProperty.Register("Angle1", typeof(double), typeof(ArcControl), new PropertyMetadata(0.0));

	public static readonly DependencyProperty Angle2Property = DependencyProperty.Register("Angle2", typeof(double), typeof(ArcControl), new PropertyMetadata(270.0));

	public static readonly DependencyProperty BrushProperty = DependencyProperty.Register("Brush", typeof(Brush), typeof(ArcControl), new PropertyMetadata(Brushes.Blue));

	public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(ArcControl), new PropertyMetadata(4.0));

	public double Angle1
	{
		get
		{
			return (double)GetValue(Angle1Property);
		}
		set
		{
			SetValue(Angle1Property, value);
		}
	}

	public double Angle2
	{
		get
		{
			return (double)GetValue(Angle2Property);
		}
		set
		{
			SetValue(Angle2Property, value);
		}
	}

	public Brush Brush
	{
		get
		{
			return (Brush)GetValue(BrushProperty);
		}
		set
		{
			SetValue(BrushProperty, value);
		}
	}

	public double StrokeThickness
	{
		get
		{
			return (double)GetValue(StrokeThicknessProperty);
		}
		set
		{
			SetValue(StrokeThicknessProperty, value);
		}
	}

	public ArcControl()
	{
		InitializeComponent();
	}
}
