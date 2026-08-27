using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.CustomControls;

[ContentProperty("Content")]
[DefaultProperty("Content")]
public class RelativeSizePresenter : ContentPresenter
{
	private FrameworkElement _child;

	public static readonly DependencyProperty HeightRatioProperty = DependencyProperty.Register("HeightRatio", typeof(double), typeof(RelativeSizePresenter), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure));

	public static readonly DependencyProperty WidthRatioProperty = DependencyProperty.Register("WidthRatio", typeof(double), typeof(RelativeSizePresenter), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure));

	public double WidthRatio
	{
		get
		{
			return (double)GetValue(WidthRatioProperty);
		}
		set
		{
			SetValue(WidthRatioProperty, value);
		}
	}

	public double HeightRatio
	{
		get
		{
			return (double)GetValue(HeightRatioProperty);
		}
		set
		{
			SetValue(HeightRatioProperty, value);
		}
	}

	protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
	{
		base.OnVisualChildrenChanged(visualAdded, visualRemoved);
		_child = ((VisualChildrenCount > 0) ? ((FrameworkElement)GetVisualChild(0)) : null);
	}

	protected override Size MeasureOverride(Size constraint)
	{
		if (_child != null && !_child.IsMeasureValid)
		{
			new Size(double.IsNaN(WidthRatio) ? constraint.Width : double.PositiveInfinity, double.IsNaN(HeightRatio) ? constraint.Height : double.PositiveInfinity);
			_child.Measure(constraint);
		}
		return new Size(_child.DesiredSize.Width * (double.IsNaN(WidthRatio) ? 1.0 : WidthRatio), _child.DesiredSize.Height * (double.IsNaN(HeightRatio) ? 1.0 : HeightRatio));
	}

	protected override Size ArrangeOverride(Size arrangeSize)
	{
		return base.ArrangeOverride(arrangeSize);
	}
}
