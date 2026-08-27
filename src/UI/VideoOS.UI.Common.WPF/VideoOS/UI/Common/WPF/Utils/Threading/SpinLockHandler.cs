using System;
using System.Threading;

namespace VideoOS.UI.Common.WPF.Utils.Threading;

public class SpinLockHandler
{
	private class SpinLockHandle : IDisposable
	{
		private SpinLockHandler _parent;

		public SpinLockHandle(SpinLockHandler parent)
		{
			_parent = parent;
		}

		public void Dispose()
		{
			_parent.Exit();
		}
	}

	private SpinLock _spinLock;

	public bool IsHeldByCurrentThread => _spinLock.IsHeldByCurrentThread;

	public SpinLockHandler()
	{
		_spinLock = default(SpinLock);
	}

	public IDisposable AcquireWithBlock()
	{
		return Acquire(-1);
	}

	public IDisposable Acquire(int milliseconds)
	{
		bool lockTaken = false;
		_spinLock.TryEnter(milliseconds, ref lockTaken);
		if (lockTaken)
		{
			return new SpinLockHandle(this);
		}
		return null;
	}

	private void Exit()
	{
		_spinLock.Exit();
	}
}
