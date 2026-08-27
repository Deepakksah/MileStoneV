using System;

namespace VideoOS.UI.Common.WPF.Behaviors.Input;

public interface IMouseInput
{
	event EventHandler<MouseInputMouseDownUpEventArgs> MouseDown;

	event EventHandler<MouseInputMouseDownUpEventArgs> MouseUp;

	event EventHandler<InputEventArgs> BeforeContextMenuShown;

	event EventHandler<InputEventArgs> AfterContextMenuClosed;

	event EventHandler<InputEventArgs> LongPressStarted;

	event EventHandler<InputEventArgs> LongPressStopped;

	event EventHandler<InputEventArgs> MouseLeftButtonClicked;

	event EventHandler<InputEventArgs> MouseLeftButtonDoubleClicked;

	event EventHandler<InputEventArgs> MouseRightButtonClicked;

	event EventHandler<InputEventArgs> DraggingStarted;

	event EventHandler<MouseInputDraggingEventArgs> Dragging;

	event EventHandler<InputEventArgs> DraggingStopped;

	event EventHandler<MouseInputMouseWheelEventArgs> MouseWheel;
}
