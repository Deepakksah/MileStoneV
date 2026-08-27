using VideoOS.Mobile.Portable.Interfaces;

namespace VideoOS.Mobile.MetaChannel;

public interface IListenerControl : IControl
{
	IControl InnerContext { get; }

	void Activate();
}
