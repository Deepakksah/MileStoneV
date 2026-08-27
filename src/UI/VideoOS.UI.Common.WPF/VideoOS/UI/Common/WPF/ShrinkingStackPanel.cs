using System;
using System.Windows;
using System.Windows.Controls;

namespace VideoOS.UI.Common.WPF;

public class ShrinkingStackPanel : Panel
{
	public static readonly DependencyPropertyKey IsItemShrunkPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsItemShrunk", typeof(bool), typeof(ShrinkingStackPanel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

	private double _itemsDesiredPaddingLeft;

	private double _itemsDesiredPaddingRight;

	private bool[] _isItemShrunk = new bool[0];

	public Thickness ItemsDesiredPadding { get; set; }

	public ShrinkingStackPanel()
	{
		ItemsDesiredPadding = new Thickness(3.0);
	}

	private static void SetIsItemShrunk(DependencyObject item, bool value)
	{
		item.SetValue(IsItemShrunkPropertyKey, value);
	}

	public static bool GetIsItemShrunk(DependencyObject item)
	{
		return (bool)item.GetValue(IsItemShrunkPropertyKey.DependencyProperty);
	}

	protected override Size MeasureOverride(Size availableSize)
	{
		UIElementCollection internalChildren = base.InternalChildren;
		int count = internalChildren.Count;
		Size result = default(Size);
		_itemsDesiredPaddingLeft = ItemsDesiredPadding.Left;
		_itemsDesiredPaddingRight = ItemsDesiredPadding.Right;
		double num = 1.0;
		Size availableSize2 = new Size(double.PositiveInfinity, double.PositiveInfinity);
		if (count != _isItemShrunk.Length)
		{
			_isItemShrunk = new bool[count];
		}
		for (int i = 0; i < count; i++)
		{
			UIElement uIElement = internalChildren[i];
			if (uIElement != null)
			{
				uIElement.Measure(availableSize2);
				Size desiredSize = uIElement.DesiredSize;
				result.Width += desiredSize.Width + _itemsDesiredPaddingLeft + _itemsDesiredPaddingRight;
				result.Height = Math.Max(result.Height, desiredSize.Height + ItemsDesiredPadding.Top + ItemsDesiredPadding.Bottom);
			}
			_isItemShrunk[i] = false;
		}
		if (result.Width > availableSize.Width)
		{
			if (result.Width - availableSize.Width < (_itemsDesiredPaddingLeft + _itemsDesiredPaddingRight) * (double)count)
			{
				double num2 = (result.Width - availableSize.Width) / (double)count;
				_itemsDesiredPaddingLeft -= num2 / 2.0;
				_itemsDesiredPaddingRight -= num2 / 2.0;
			}
			else
			{
				_itemsDesiredPaddingLeft = 0.0;
				_itemsDesiredPaddingRight = 0.0;
				double num3 = (double)count * (ItemsDesiredPadding.Left + ItemsDesiredPadding.Right);
				bool flag = true;
				while (flag && result.Width - num3 > availableSize.Width)
				{
					flag = false;
					num = num * availableSize.Width / (result.Width - num3);
					int i = count;
					while (i > 0 && result.Width - num3 > availableSize.Width)
					{
						i--;
						UIElement uIElement2 = internalChildren[i];
						if (uIElement2 != null)
						{
							FrameworkElement frameworkElement = uIElement2 as FrameworkElement;
							availableSize2.Width = ((frameworkElement != null) ? Math.Max(frameworkElement.MinWidth, uIElement2.DesiredSize.Width * num) : (uIElement2.DesiredSize.Width * num));
							double width = uIElement2.DesiredSize.Width;
							uIElement2.Measure(availableSize2);
							if (width != uIElement2.DesiredSize.Width)
							{
								num3 += width - uIElement2.DesiredSize.Width;
								_isItemShrunk[i] = true;
								flag = true;
							}
						}
					}
				}
			}
			result = default(Size);
			for (int i = 0; i < count; i++)
			{
				UIElement uIElement3 = internalChildren[i];
				if (uIElement3 != null)
				{
					availableSize2.Width = uIElement3.DesiredSize.Width;
					Size desiredSize2 = uIElement3.DesiredSize;
					result.Width += desiredSize2.Width + _itemsDesiredPaddingLeft + _itemsDesiredPaddingRight;
					result.Height = Math.Max(result.Height, desiredSize2.Height + ItemsDesiredPadding.Top + ItemsDesiredPadding.Bottom);
				}
			}
		}
		for (int i = 0; i < count; i++)
		{
			UIElement uIElement4 = internalChildren[i];
			if (uIElement4 != null)
			{
				SetIsItemShrunk(uIElement4, _isItemShrunk[i]);
			}
		}
		result.Width = Math.Min(result.Width, availableSize.Width);
		return result;
	}

	protected override Size ArrangeOverride(Size finalSize)
	{
		UIElementCollection children = base.Children;
		int count = children.Count;
		Rect finalRect = new Rect(finalSize);
		double num = 0.0;
		for (int i = 0; i < count; i++)
		{
			UIElement uIElement = children[i];
			if (uIElement != null)
			{
				finalRect.X += num;
				num = (finalRect.Width = uIElement.DesiredSize.Width + _itemsDesiredPaddingLeft + _itemsDesiredPaddingRight);
				finalRect.Height = Math.Max(finalSize.Height, uIElement.DesiredSize.Height);
				finalRect.Y = Math.Max(0.0, (base.DesiredSize.Height - finalSize.Height) / 2.0);
				uIElement.Arrange(finalRect);
			}
		}
		return finalSize;
	}
}
