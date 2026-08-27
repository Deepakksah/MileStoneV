using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using VideoOS.UI.Common.WPF.Annotations;
using VideoOS.Utilities.Validation;

namespace VideoOS.UI.Common.WPF.Utils.RecyclingFactory;

public class RecyclingFactory<TItem> : IRecyclingFactory<TItem>, IDisposable where TItem : IRecyclableItem
{
	private readonly Func<TItem> _itemFactory;

	private readonly List<TItem> _recycledItems = new List<TItem>();

	private DateTime _lastActivity;

	private bool _disposed;

	private readonly Timer _timer;

	private readonly TimeSpan _maxIdleTimeBeforeCleanup;

	public RecyclingFactory([NotNull] Func<TItem> itemFactory, TimeSpan maxIdleTimeBeforeCleanup)
	{
		itemFactory.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\SC_CommonFiles\\VideoOS.UI.Common.WPF\\Utils\\RecyclingFactory\\RecyclingFactory.cs", 27);
		if (maxIdleTimeBeforeCleanup.TotalMilliseconds < 0.0)
		{
			throw new ArgumentException("Must be a positive timespan", "maxIdleTimeBeforeCleanup");
		}
		_itemFactory = itemFactory;
		_maxIdleTimeBeforeCleanup = maxIdleTimeBeforeCleanup;
		_lastActivity = DateTime.UtcNow;
		if (_maxIdleTimeBeforeCleanup.TotalDays <= 365.0)
		{
			_timer = new Timer(TimerOnElapsed, null, _maxIdleTimeBeforeCleanup, TimeSpan.FromMilliseconds(-1.0));
		}
	}

	public void TimerOnElapsed(object state)
	{
		CleanupIfNeeded();
		_timer.Change(_maxIdleTimeBeforeCleanup, TimeSpan.FromMilliseconds(-1.0));
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	private void CleanupIfNeeded()
	{
		if (_lastActivity.Add(_maxIdleTimeBeforeCleanup) <= DateTime.UtcNow)
		{
			Clear();
		}
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public TItem Create()
	{
		bool created;
		return Create(out created);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public TItem Create(out bool created)
	{
		_lastActivity = DateTime.UtcNow;
		created = false;
		TItem val;
		if (_recycledItems.Count == 0)
		{
			val = _itemFactory();
			created = true;
		}
		else
		{
			val = _recycledItems[0];
			_recycledItems.Remove(val);
		}
		val.ReadyToBeRecycled += ItemOnReadyToBeRecycled;
		return val;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public void Clear()
	{
		_recycledItems.Clear();
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	private void ItemOnReadyToBeRecycled(object sender, EventArgs e)
	{
		_lastActivity = DateTime.UtcNow;
		TItem item = (TItem)sender;
		item.ReadyToBeRecycled -= ItemOnReadyToBeRecycled;
		_recycledItems.Add(item);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	protected void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			_disposed = true;
			Clear();
			_timer.Dispose();
		}
	}
}
