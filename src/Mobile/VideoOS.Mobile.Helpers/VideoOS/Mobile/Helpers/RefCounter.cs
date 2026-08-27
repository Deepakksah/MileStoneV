using System.Threading;

namespace VideoOS.Mobile.Helpers;

public class RefCounter
{
	private int _refCount;

	public int RefCount => _refCount;

	public void AddRef()
	{
		Interlocked.Increment(ref _refCount);
	}

	public void RemoveRef()
	{
		Interlocked.Decrement(ref _refCount);
	}
}
