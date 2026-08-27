using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class ListBoxScrollBehavior : Behavior<ListBox>
{
	private static ScrollViewer _scrollViewer = null;

	public static readonly DependencyProperty ScrollPositionProperty = DependencyProperty.Register("ScrollPosition", typeof(double), typeof(ListBoxScrollBehavior), new PropertyMetadata(ScrollPositionChanged));

	public double ScrollPosition
	{
		get
		{
			return (double)((DependencyObject)(object)this).GetValue(ScrollPositionProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(ScrollPositionProperty, (object)value);
		}
	}

	private static void ScrollPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (_scrollViewer != null && Math.Abs((double)e.NewValue - _scrollViewer.VerticalOffset) > 0.1)
		{
			_scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
		}
	}

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.Loaded += AssociatedObject_Loaded;
	}

	private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
	{
		_scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild((Border)VisualTreeHelper.GetChild(base.AssociatedObject, 0), 0);
		if (_scrollViewer != null)
		{
			_scrollViewer.ScrollChanged += _scrollViewer_ScrollChanged;
		}
	}

	private void _scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
	{
		if (_scrollViewer != null)
		{
			ScrollPosition = _scrollViewer.VerticalOffset;
		}
	}
}
