using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VideoOS.UI.Common.WPF.Controls.Internal;

public class VideoOSHorizontalPanel : Panel
{
	[DebuggerDisplay("{DesiredWidth} {CurrentWidth}")]
	private class WidthDescr
	{
		public readonly UIElement UiElement;

		public readonly double DesiredWidth;

		public double CurrentWidth;

		public WidthDescr(UIElement ctl, double desiredWidth, double currentWidth)
		{
			if (ctl == null)
			{
				throw new ArgumentNullException("ctl");
			}
			UiElement = ctl;
			DesiredWidth = desiredWidth;
			CurrentWidth = currentWidth;
		}
	}

	private class DesiredWidthComparer : IComparer<WidthDescr>
	{
		public int Compare(WidthDescr x, WidthDescr y)
		{
			if (x == y)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			int num = x.UiElement.DesiredSize.Width.CompareTo(y.UiElement.DesiredSize.Width);
			if (num == 0)
			{
				num = x.UiElement.GetHashCode().CompareTo(y.UiElement.GetHashCode());
			}
			return num;
		}
	}

	private const double gapHorizontal = 10.0;

	private const double minChildWidth = 10.0;

	public static readonly DependencyProperty MinChildWidthProperty = DependencyProperty.Register("MinChildWidth", typeof(double), typeof(VideoOSHorizontalPanel), new PropertyMetadata(10.0));

	public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register("Spacing", typeof(double), typeof(VideoOSHorizontalPanel), new PropertyMetadata(10.0));

	private readonly Dictionary<UIElement, WidthDescr> _child2Width = new Dictionary<UIElement, WidthDescr>();

	private List<WidthDescr> _childrenOrderedByWidth = new List<WidthDescr>();

	public double MinChildWidth
	{
		get
		{
			return (double)GetValue(MinChildWidthProperty);
		}
		set
		{
			SetValue(MinChildWidthProperty, value);
		}
	}

	public double Spacing
	{
		get
		{
			return (double)GetValue(SpacingProperty);
		}
		set
		{
			SetValue(SpacingProperty, value);
		}
	}

	protected override Size MeasureOverride(Size availableSize)
	{
		double num = 0.0;
		double num2 = 0.0;
		int num3 = 0;
		foreach (UIElement internalChild in base.InternalChildren)
		{
			if (internalChild == null)
			{
				continue;
			}
			internalChild.Measure(availableSize);
			if (!(internalChild.DesiredSize.Width <= 0.0))
			{
				double num4 = Math.Ceiling(Math.Max(internalChild.DesiredSize.Width + 1.0, 10.0));
				if (_child2Width.ContainsKey(internalChild) && _child2Width[internalChild].DesiredWidth > num4)
				{
					num4 = _child2Width[internalChild].DesiredWidth;
				}
				num += Math.Max(num4, 10.0);
				if (internalChild.DesiredSize.Height > num2)
				{
					num2 = internalChild.DesiredSize.Height;
				}
				num3++;
			}
		}
		double num5 = 10.0 * (double)num3;
		if (num3 > 1)
		{
			num += (double)(num3 - 1) * 10.0;
			num5 += (double)(num3 - 1) * 10.0;
		}
		if (!double.IsInfinity(availableSize.Width) && availableSize.Width < num)
		{
			num = availableSize.Width;
		}
		if (num < num5)
		{
			num = num5;
		}
		if (!_childrenOrderedByWidth.Any())
		{
			foreach (UIElement item in base.InternalChildren.OfType<UIElement>().ToList())
			{
				WidthDescr widthDescr = new WidthDescr(item, Math.Ceiling(item.DesiredSize.Width + 1.0), 0.0);
				_child2Width.Add(item, widthDescr);
				_childrenOrderedByWidth.Add(widthDescr);
			}
			_childrenOrderedByWidth.Sort(new DesiredWidthComparer());
		}
		return new Size(num, num2);
	}

	protected override Size ArrangeOverride(Size finalSize)
	{
		List<UIElement> list = base.InternalChildren.OfType<UIElement>().ToList();
		if (list.Count < 1)
		{
			return finalSize;
		}
		double width = finalSize.Width;
		_child2Width.Values.ToList().ForEach(delegate(WidthDescr i)
		{
			i.CurrentWidth = 0.0;
		});
		CalculateArrange(_childrenOrderedByWidth.ToList(), width);
		ArrangeControls(list, finalSize, _child2Width);
		return finalSize;
	}

	private void CalculateArrange(List<WidthDescr> slice, double availWidth)
	{
		bool flag = true;
		while (slice.Any())
		{
			UIElement ctl = slice[0].UiElement;
			if (ctl.DesiredSize.Width > 0.0)
			{
				double estimateWidth = GetEstimateWidth(_child2Width[ctl].DesiredWidth - _child2Width[ctl].CurrentWidth, slice.Count, flag);
				if (!(estimateWidth <= availWidth))
				{
					double spacePerControl = Math.Max((availWidth - ((slice.Count < 2 || !flag) ? 0.0 : ((double)(slice.Count - 1) * 10.0))) / (double)slice.Count, 0.0);
					slice.ForEach(delegate(WidthDescr i)
					{
						_child2Width[i.UiElement].CurrentWidth += spacePerControl;
					});
					break;
				}
				slice.ForEach(delegate(WidthDescr k)
				{
					_child2Width[k.UiElement].CurrentWidth = _child2Width[ctl].DesiredWidth;
				});
				availWidth -= estimateWidth;
				flag = false;
			}
			slice = slice.Skip(1).Take(slice.Count - 1).ToList();
		}
	}

	private static void ArrangeControls(List<UIElement> children, Size finalSize, Dictionary<UIElement, WidthDescr> c2w)
	{
		double num = 0.0;
		foreach (UIElement child in children)
		{
			child.Arrange(new Rect(new Point(num, 0.0), new Size(c2w[child].CurrentWidth, finalSize.Height)));
			if (c2w[child].CurrentWidth != 0.0)
			{
				if (child is FrameworkElement frameworkElement)
				{
					frameworkElement.Width = c2w[child].CurrentWidth;
				}
				num += c2w[child].CurrentWidth + 10.0;
			}
		}
	}

	private static double GetEstimateWidth(double width, int count, bool includeGaps)
	{
		double num = width * (double)count;
		if (includeGaps && count > 1)
		{
			num += 10.0 * (double)(count - 1);
		}
		return num;
	}
}
