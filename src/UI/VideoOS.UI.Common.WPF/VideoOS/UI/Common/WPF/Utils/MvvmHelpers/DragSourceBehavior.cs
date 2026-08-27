using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class DragSourceBehavior : Behavior<FrameworkElement>
{
	private static readonly Point _noMouseDown = new Point(double.NaN, double.NaN);

	private Point _mouseDownPoint = _noMouseDown;

	private bool _isDragging;

	public static readonly DependencyProperty DataObjectProperty = DependencyProperty.Register("DataObject", typeof(object), typeof(DragSourceBehavior));

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

	public ICommand DropCommand { get; set; }

	private static double DragTriggerLengthSquare => SystemParameters.MinimumHorizontalDragDistance * SystemParameters.MinimumVerticalDragDistance;

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.PreviewMouseLeftButtonDown += associatedObject_PreviewMouseLeftButtonDown;
		base.AssociatedObject.PreviewMouseLeftButtonUp += associatedObject_PreviewMouseLeftButtonUp;
		base.AssociatedObject.PreviewMouseMove += associatedObject_PreviewMouseMove;
	}

	private void associatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		_mouseDownPoint = e.GetPosition(base.AssociatedObject);
	}

	private void associatedObject_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		_mouseDownPoint = _noMouseDown;
	}

	private void associatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
	{
		if (DataObject == null || _isDragging || e.LeftButton != MouseButtonState.Pressed || !(_mouseDownPoint != _noMouseDown))
		{
			return;
		}
		Point position = e.GetPosition(base.AssociatedObject);
		if ((position.X - _mouseDownPoint.X) * (position.X - _mouseDownPoint.X) + (position.Y - _mouseDownPoint.Y) * (position.Y - _mouseDownPoint.Y) > DragTriggerLengthSquare)
		{
			_isDragging = true;
			DragDropEffects num = DragDrop.DoDragDrop(base.AssociatedObject, DataObject, DragDropEffects.Move);
			_isDragging = false;
			if (num != DragDropEffects.None && DropCommand != null && DropCommand.CanExecute(DataObject))
			{
				DropCommand.Execute(DataObject);
			}
		}
	}
}
