namespace VideoOS.UI.Common.WPF.Behaviors.Input;

public interface IMouseInputAttachable
{
	void Attached(IMouseInput input);

	void Detached(IMouseInput input);
}
