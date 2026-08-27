using System.Windows;
using System.Windows.Controls;

namespace VideoOS.UI.Common.WPF.SmoothScroller;

public class Slice : Canvas
{
	public int BeginIndex { get; set; }

	public int EndIndex { get; set; }

	public Size ItemSize { get; set; }

	public bool HasItems { get; set; }

	public int EmptyPreItems { get; set; }

	public Orientation Orientation { get; set; }

	public Slice()
	{
	}

	public void SetItemRenderer(FrameworkElement itemRenderer, int index)
	{
		itemRenderer.Width = ItemSize.Width;
		itemRenderer.Height = ItemSize.Height;
		if (Orientation == Orientation.Vertical)
		{
			Canvas.SetLeft(itemRenderer, 0.0);
			Canvas.SetTop(itemRenderer, (double)(index - BeginIndex + EmptyPreItems) * ItemSize.Height);
		}
		else
		{
			Canvas.SetLeft(itemRenderer, (double)(index - BeginIndex + EmptyPreItems) * ItemSize.Width);
			Canvas.SetTop(itemRenderer, 0.0);
		}
		base.Children.Add(itemRenderer);
	}

	public FrameworkElement[] ClearItems()
	{
		FrameworkElement[] array = new FrameworkElement[base.Children.Count];
		for (int i = 0; i < base.Children.Count; i++)
		{
			array[i] = (FrameworkElement)base.Children[i];
		}
		base.Children.Clear();
		return array;
	}

	public FrameworkElement GetItemRendererAtPoint(double x, double y)
	{
		for (int i = 0; i < base.Children.Count; i++)
		{
			FrameworkElement frameworkElement = (FrameworkElement)base.Children[i];
			if (new Rect(Canvas.GetLeft(frameworkElement), Canvas.GetTop(frameworkElement), frameworkElement.ActualWidth, frameworkElement.ActualHeight).Contains(x, y))
			{
				return frameworkElement;
			}
		}
		return null;
	}

	public FrameworkElement GetItemRenderer(int index)
	{
		if (index >= BeginIndex && index <= EndIndex)
		{
			return (FrameworkElement)base.Children[index - BeginIndex];
		}
		return null;
	}
}
