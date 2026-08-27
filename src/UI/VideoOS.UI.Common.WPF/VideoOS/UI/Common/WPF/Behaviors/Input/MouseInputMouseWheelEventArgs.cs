namespace VideoOS.UI.Common.WPF.Behaviors.Input;

public class MouseInputMouseWheelEventArgs : InputEventArgs
{
	public int Delta { get; }

	public MouseInputMouseWheelEventArgs(int delta)
	{
		Delta = delta;
	}
}
