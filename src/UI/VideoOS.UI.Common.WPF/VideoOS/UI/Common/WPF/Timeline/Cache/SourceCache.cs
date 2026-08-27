using System;
using System.Threading;
using C5;

namespace VideoOS.UI.Common.WPF.Timeline.Cache;

public class SourceCache : SortedArray<CacheEntry>, IDisposable
{
	private class ReaderLockHandle : IDisposable
	{
		private ReaderWriterLockSlim _lock;

		public ReaderLockHandle(ReaderWriterLockSlim theLock)
		{
			_lock = theLock;
			_lock.EnterReadLock();
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			_lock.ExitReadLock();
		}
	}

	private class WriterLockHandle : IDisposable
	{
		private ReaderWriterLockSlim _lock;

		public WriterLockHandle(ReaderWriterLockSlim theLock)
		{
			_lock = theLock;
			_lock.EnterWriteLock();
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			_lock.ExitWriteLock();
		}
	}

	private class UpgradeableReadLockHandle : IDisposable
	{
		private ReaderWriterLockSlim _lock;

		public UpgradeableReadLockHandle(ReaderWriterLockSlim theLock)
		{
			_lock = theLock;
			_lock.EnterUpgradeableReadLock();
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			_lock.ExitUpgradeableReadLock();
		}
	}

	private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

	private DateTime _outOfUseTime = DateTime.MaxValue;

	public DateTime OutOfUseTime
	{
		get
		{
			return _outOfUseTime;
		}
		set
		{
			_outOfUseTime = value;
		}
	}

	public bool AnyLockActivityExists => _lock.CurrentReadCount + _lock.WaitingReadCount + _lock.WaitingUpgradeCount + _lock.WaitingWriteCount != 0;

	public SourceCache()
		: base((MemoryType)0)
	{
	}

	public IDisposable AcquireReaderLock()
	{
		return new ReaderLockHandle(_lock);
	}

	public IDisposable AcquireWriterLock()
	{
		return new WriterLockHandle(_lock);
	}

	public IDisposable AcquireUpgradeableReadLock()
	{
		return new UpgradeableReadLockHandle(_lock);
	}

	public void Dispose()
	{
		((ArrayBase<CacheEntry>)(object)this).Clear();
		if (_lock != null)
		{
			_lock.Dispose();
			_lock = null;
		}
	}
}
