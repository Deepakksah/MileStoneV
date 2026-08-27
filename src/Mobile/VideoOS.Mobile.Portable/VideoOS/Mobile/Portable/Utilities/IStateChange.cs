namespace VideoOS.Mobile.Portable.Utilities;

public interface IStateChange<T>
{
	event StateEventHandler<T> OnStateChange;

	StateEventArgs<T> GetState();
}
