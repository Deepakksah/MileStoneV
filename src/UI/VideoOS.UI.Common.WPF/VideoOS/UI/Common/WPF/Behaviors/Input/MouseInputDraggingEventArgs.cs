using System.Windows;

namespace VideoOS.UI.Common.WPF.Behaviors.Input;

public class MouseInputDraggingEventArgs : InputEventArgs
{
	public Point OriginalOffsetPoint { get; }

	public Point Point { get; }

	public FrameworkElement ParentContainer { get; }

	public MouseInputDraggingEventArgs(Point originalOffsetPoint, Point point, FrameworkElement parentContainer)
	{
		OriginalOffsetPoint = originalOffsetPoint;
		Point = point;
		ParentContainer = parentContainer;
	}
}
