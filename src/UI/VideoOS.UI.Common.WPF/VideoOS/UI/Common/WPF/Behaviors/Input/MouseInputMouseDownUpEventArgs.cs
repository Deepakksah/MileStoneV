using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.Behaviors.Input;

public class MouseInputMouseDownUpEventArgs : InputEventArgs
{
	public MouseButton Button { get; }

	public MouseInputMouseDownUpEventArgs(MouseButton button)
	{
		Button = button;
	}
}
