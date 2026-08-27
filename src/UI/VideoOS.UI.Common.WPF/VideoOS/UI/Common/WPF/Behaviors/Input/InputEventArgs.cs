using System;

namespace VideoOS.UI.Common.WPF.Behaviors.Input;

public class InputEventArgs : EventArgs
{
	public new static InputEventArgs Empty { get; } = new InputEventArgs();

	public bool Handled { get; set; }
}
