namespace VideoOS.Mobile.Portable.Utilities;

public interface IStateChangeInternal<T>
{
	T State { get; }

	void ChangeState(T state);
}
