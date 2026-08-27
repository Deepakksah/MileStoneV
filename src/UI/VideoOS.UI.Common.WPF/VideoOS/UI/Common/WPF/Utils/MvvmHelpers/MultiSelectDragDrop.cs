using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class MultiSelectDragDrop : Behavior<ListBox>
{
	public static readonly DependencyProperty DataObjectProperty = DependencyProperty.Register("DataObject", typeof(object), typeof(MultiSelectDragDrop));

	private Point? _mouseDownPosition;

	private int _mouseDownTimestamp;

	private ListBoxItem _mouseDownListBoxItem;

	private MouseButtonEventArgs _mouseDownEventArgs;

	public object DataObject
	{
		get
		{
			return ((DependencyObject)(object)this).GetValue(DataObjectProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(DataObjectProperty, value);
		}
	}

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
		base.AssociatedObject.PreviewMouseLeftButtonUp += AssociatedObject_PreviewMouseLeftButtonUp;
		base.AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
	}

	private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
		DependencyObject dependencyObject = (DependencyObject)base.AssociatedObject.InputHitTest(e.GetPosition(base.AssociatedObject));
		_mouseDownEventArgs = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left)
		{
			RoutedEvent = UIElement.MouseLeftButtonDownEvent,
			Source = dependencyObject
		};
		while (!_mouseDownEventArgs.Handled && !(dependencyObject is ListBoxItem) && dependencyObject != base.AssociatedObject)
		{
			if (dependencyObject is IInputElement inputElement)
			{
				inputElement.RaiseEvent(_mouseDownEventArgs);
			}
			dependencyObject = ((!(dependencyObject is FrameworkContentElement frameworkContentElement)) ? VisualTreeHelper.GetParent(dependencyObject) : frameworkContentElement.Parent);
			_mouseDownListBoxItem = dependencyObject as ListBoxItem;
		}
		if (!_mouseDownEventArgs.Handled)
		{
			base.AssociatedObject.CaptureMouse();
			_mouseDownPosition = e.GetPosition(base.AssociatedObject);
			_mouseDownTimestamp = e.Timestamp;
		}
	}

	private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
	{
		if (!base.AssociatedObject.IsMouseCaptured || !_mouseDownPosition.HasValue)
		{
			return;
		}
		Point value = _mouseDownPosition.Value;
		e.Handled = true;
		Point position = e.GetPosition(base.AssociatedObject);
		if (Math.Abs(position.X - value.X) >= SystemParameters.MinimumHorizontalDragDistance || Math.Abs(position.Y - value.Y) >= SystemParameters.MinimumVerticalDragDistance)
		{
			base.AssociatedObject.ReleaseMouseCapture();
			_mouseDownPosition = null;
			if (_mouseDownListBoxItem != null && !_mouseDownListBoxItem.IsSelected)
			{
				_mouseDownListBoxItem.RaiseEvent(_mouseDownEventArgs);
			}
			Task.Factory.StartNew(delegate
			{
				StartDragDropCore();
			}, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.FromCurrentSynchronizationContext());
		}
	}

	private void StartDragDropCore()
	{
		DragDrop.DoDragDrop(base.AssociatedObject, DataObject, (DataObject != null) ? DragDropEffects.Copy : DragDropEffects.None);
	}

	private void AssociatedObject_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (base.AssociatedObject.IsMouseCaptured && _mouseDownPosition.HasValue)
		{
			base.AssociatedObject.ReleaseMouseCapture();
			if (_mouseDownPosition.HasValue)
			{
				IInputElement inputElement = base.AssociatedObject.InputHitTest(_mouseDownPosition.Value);
				inputElement.RaiseEvent(new MouseButtonEventArgs(e.MouseDevice, _mouseDownTimestamp, MouseButton.Left)
				{
					RoutedEvent = Mouse.MouseDownEvent,
					Source = inputElement
				});
				_mouseDownPosition = null;
			}
		}
	}
}
