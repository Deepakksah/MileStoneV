#define TRACE
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using C5;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;
using VideoOS.UI.Common.WPF.Timeline.QueryDispatcher;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;
using VideoOS.UI.Common.WPF.Utils.Threading;

namespace VideoOS.UI.Common.WPF.Timeline.Cache;

public class C5TimelineCache : ITimelineSequenceCache
{
	protected class AccessHandle : ITimelineSequenceCache, IDisposable
	{
		private C5TimelineCache _parentCache;

		private TimeInterval _interval = new TimeInterval();

		private IEnumerable<ISequenceSource> _sequenceSources = new ISequenceSource[0];

		private ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

		public TimeInterval Interval
		{
			get
			{
				try
				{
					_readerWriterLock.EnterReadLock();
					return _interval;
				}
				finally
				{
					_readerWriterLock.ExitReadLock();
				}
			}
			private set
			{
				try
				{
					_readerWriterLock.EnterWriteLock();
					_interval = value;
				}
				finally
				{
					_readerWriterLock.ExitWriteLock();
				}
			}
		}

		public IEnumerable<ISequenceSource> SequenceSources
		{
			get
			{
				try
				{
					_readerWriterLock.EnterReadLock();
					return _sequenceSources.ToList();
				}
				finally
				{
					_readerWriterLock.ExitReadLock();
				}
			}
			private set
			{
				try
				{
					_readerWriterLock.EnterWriteLock();
					IEnumerable<ISequenceSource> sequenceSources;
					if (value == null)
					{
						sequenceSources = value;
					}
					else
					{
						IEnumerable<ISequenceSource> enumerable = value.ToList();
						sequenceSources = enumerable;
					}
					_sequenceSources = sequenceSources;
				}
				finally
				{
					_readerWriterLock.ExitWriteLock();
				}
			}
		}

		public string Name => "C5TimelineCache.AccessHandle";

		public bool SupportsMultithreading => true;

		public event EventHandler<SequenceSourcesEventArgs> NewSequencesAvailable;

		public AccessHandle(C5TimelineCache parentCache)
		{
			_parentCache = parentCache;
		}

		public void FireNewSequencesAvailable(SequenceSourcesEventArgs e)
		{
			this.NewSequencesAvailable?.Invoke(this, e);
		}

		public void SetCurrentTimeInterval(TimeInterval interval)
		{
			Interval = interval;
			_parentCache.TimeIntervalUpdated();
		}

		public ISequenceSource[] GetTimelineData(IEnumerable<ISequenceSource> sequenceSources, DateTime beginTime, TimeSpan interval, int count)
		{
			return _parentCache.GetTimelineData(sequenceSources, beginTime, interval, count);
		}

		public IEnumerable<Sequence> GetSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime fromTime, DateTime toTime)
		{
			return _parentCache.GetSequences(sequenceSources, fromTime, toTime);
		}

		public void SetSequenceSources(IEnumerable<ISequenceSource> sequenceSources)
		{
			SequenceSources = sequenceSources.ToList();
			_parentCache.SequenceSourcesUpdated();
		}

		public void InvalidateSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime fromTime, DateTime toTime)
		{
			_parentCache.InvalidateSequences(sequenceSources, fromTime, toTime);
		}

		public void Dispose()
		{
			_readerWriterLock.Dispose();
		}

		public void InitConfig(TimelineCacheSettings timelineCacheSettings)
		{
			_parentCache.InitConfig(timelineCacheSettings);
		}

		public void InvalidateSequences(IEnumerable<Guid> sourceIds, DateTime fromTime, DateTime toTime)
		{
			_parentCache.InvalidateSequences(sourceIds, fromTime, toTime);
		}

		public void InvalidateSequences(DateTime fromTime, DateTime toTime)
		{
			_parentCache.InvalidateSequences(fromTime, toTime);
		}
	}

	public class CacheDebugObject
	{
		private C5TimelineCache _owner;

		public ISourceQueryDispatcher SourceQueryDispatcher => _owner._queryDispatcher;

		public event EventHandler CacheEntryRemoved;

		public CacheDebugObject(C5TimelineCache owner)
		{
			_owner = owner;
		}

		public void FireCacheEntryRemoved()
		{
			this.CacheEntryRemoved?.Invoke(_owner, EventArgs.Empty);
		}
	}

	private ConcurrentDictionary<ISequenceSource, SourceCache> _sourceCacheDictionary = new ConcurrentDictionary<ISequenceSource, SourceCache>();

	private ConcurrentQueue<SourceQueryResult> _availableResultQueue = new ConcurrentQueue<SourceQueryResult>();

	protected ISourceQueryDispatcher _queryDispatcher;

	private Dispatcher _dispatcher;

	private Dispatcher _backgroundDispatcher;

	private DispatcherTimer _cleanupTimer;

	private IList<WeakReference> _accessHandles = new List<WeakReference>();

	private CacheDebugObject _debugObject;

	private IList<SourceQuery> _sourceQueryList = new List<SourceQuery>();

	private bool _isInitialized;

	private static readonly DateTime _blockReferenceTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	private List<SourceCache> _disposableSourceCacheList = new List<SourceCache>();

	private TimelineCacheSettings _timelineCacheSettings { get; set; }

	public bool SupportsMultithreading => true;

	public string Name => "C5TimelineCache";

	public CacheDebugObject DebugObject{
		get
		{
			Trace.TraceError("C5TimelineCache.DebugObject should only be used in the unit tests.");
			if (_debugObject == null)
			{
				_debugObject = new CacheDebugObject(this);
			}
			return _debugObject;
		}
	}

	public event EventHandler<SequenceSourcesEventArgs> NewSequencesAvailable
	{
		add
		{
			throw new NotSupportedException("Event only present to satisfy the cache interface. Do not add handler.");
		}
		remove
		{
		}
	}

	public C5TimelineCache(ISourceQueryDispatcher queryDispatcher)
	{
		if (queryDispatcher == null)
		{
			throw new ArgumentNullException("queryDispatcher", "An 'ISourceQueryDispatcher' instance must be specified.");
		}
		_queryDispatcher = queryDispatcher;
		_queryDispatcher.SequencesRetrieved += _queryDispatcher_SequencesRetrievedEvent;
		_dispatcher = Dispatcher.CurrentDispatcher;
		_backgroundDispatcher = DispatcherExtension.Create("Timeline cache background thread.");
		_cleanupTimer = new DispatcherTimer(DispatcherPriority.Background);
		_cleanupTimer.Interval = TimeSpan.FromMilliseconds(1000.0);
	}

	public ITimelineSequenceCache CreateAccessHandle()
	{
		AccessHandle accessHandle = new AccessHandle(this);
		EnsureAccessHandleRegistered(accessHandle);
		return accessHandle;
	}

	public void InitConfig(TimelineCacheSettings timelineCacheSettings)
	{
		_timelineCacheSettings = timelineCacheSettings;
		_isInitialized = true;
		TimeIntervalUpdated();
	}

	public ISequenceSource[] GetTimelineData(IEnumerable<ISequenceSource> sequenceSources, DateTime beginTime, TimeSpan interval, int count)
	{
		ISequenceSource[] timelineData = new ISequenceSource[count];
		foreach (ISequenceSource sequenceSource in sequenceSources)
		{
			FetchDataFromCache(sequenceSource, beginTime, interval, ref timelineData);
		}
		return timelineData;
	}

	private bool FetchDataFromCache(ISequenceSource source, DateTime beginTime, TimeSpan interval, ref ISequenceSource[] timelineData)
	{
		DateTime utcNow = DateTime.UtcNow;
		if (_sourceCacheDictionary.TryGetValue(source, out var value))
		{
			using (value.AcquireReaderLock())
			{
				for (int i = 0; i < timelineData.Length; i++)
				{
					if (timelineData[i] != null)
					{
						continue;
					}
					TimeInterval timeInterval = new TimeInterval(beginTime, interval, interval.TotalMilliseconds * (double)i).IntersectWith(new TimeInterval(DateTime.MinValue, utcNow));
					if (timeInterval.BeginTime > utcNow)
					{
						break;
					}
					foreach (CacheEntry item in ((IEnumerable<CacheEntry>)value).GetAllIntersecting(timeInterval))
					{
						if (item.Sequences.GetAllIntersecting(timeInterval).Any())
						{
							timelineData[i] = source;
							break;
						}
					}
				}
			}
		}
		return true;
	}

	public IEnumerable<Sequence> GetSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime beginTime, DateTime endTime)
	{
		new List<Sequence>();
		foreach (ISequenceSource sequenceSource in sequenceSources)
		{
			if (!_sourceCacheDictionary.TryGetValue(sequenceSource, out var value))
			{
				continue;
			}
			using (value.AcquireReaderLock())
			{
				foreach (CacheEntry item in ((IEnumerable<CacheEntry>)value).GetAllIntersecting(beginTime, endTime))
				{
					foreach (Sequence item2 in item.Sequences.GetAllIntersecting(beginTime, endTime))
					{
						yield return item2;
					}
				}
			}
		}
	}

	public void InvalidateSequences(DateTime fromTime, DateTime toTime)
	{
		InvalidateSequences((ISequenceSource s) => true, fromTime, toTime);
	}

	public void InvalidateSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime fromTime, DateTime toTime)
	{
		InvalidateSequences((ISequenceSource s) => sequenceSources.Contains(s), fromTime, toTime);
	}

	public void InvalidateSequences(IEnumerable<Guid> sourceIds, DateTime fromTime, DateTime toTime)
	{
		InvalidateSequences((ISequenceSource s) => sourceIds.Contains(s.SourceId), fromTime, toTime);
	}

	private void InvalidateSequences(Predicate<ISequenceSource> filter, DateTime fromTime, DateTime toTime)
	{
		List<ISequenceSource> list = new List<ISequenceSource>();
		foreach (KeyValuePair<ISequenceSource, SourceCache> item in _sourceCacheDictionary)
		{
			ISequenceSource key = item.Key;
			if (!filter(key))
			{
				continue;
			}
			list.Add(key);
			SourceCache value = item.Value;
			using (value.AcquireWriterLock())
			{
				foreach (CacheEntry item2 in ((IEnumerable<CacheEntry>)value).GetAllIntersecting(fromTime, toTime))
				{
					((SortedArray<CacheEntry>)value).Remove(item2);
				}
			}
		}
		foreach (AccessHandle activeHandle in GetActiveHandles())
		{
			IList<ISequenceSource> list2 = activeHandle.SequenceSources.Intersect(list).ToList();
			if (list2.Count <= 0)
			{
				continue;
			}
			TimeInterval interval = new TimeInterval(fromTime, toTime).IntersectWith(activeHandle.Interval);
			IList<TimeInterval> loadBlocks = GetLoadBlocks(interval, _timelineCacheSettings.BlockLength, 0.0).ToList();
			IEnumerable<SourceQuery> arg = list2.Convert((ISequenceSource source) => loadBlocks.Convert((TimeInterval block) => new SourceQuery(block)
			{
				Source = source,
				Priority = loadBlocks.IndexOf(block)
			})).SelectMany((IEnumerable<SourceQuery> result) => result).ToList();
			_backgroundDispatcher.BeginInvoke(delegate(IEnumerable<SourceQuery> ql)
			{
				_queryDispatcher.GetSequences(ql);
			}, arg);
		}
	}

	public void SetCurrentTimeInterval(TimeInterval interval)
	{
		throw new NotSupportedException("Do not call SetCurrentTimeInterval directly. Use the access handles.");
	}

	protected void TimeIntervalUpdated()
	{
		UpdateQueries(GetActiveHandles().ToList());
	}

	public void SetSequenceSources(IEnumerable<ISequenceSource> sequenceSources)
	{
		throw new NotSupportedException("Do not call SetSequenceSources directly. Use the access handles.");
	}

	protected void SequenceSourcesUpdated()
	{
		if (!_cleanupTimer.IsEnabled)
		{
			_cleanupTimer.Tick += delegate
			{
				_backgroundDispatcher.BeginInvoke(delegate(List<AccessHandle> activeHandles)
				{
					MaintenanceStrategy(activeHandles);
				}, GetActiveHandles().ToList());
			};
			_cleanupTimer.Start();
		}
		UpdateQueries(GetActiveHandles().ToList());
	}

	protected virtual void OnNewSequencesAvailable(SequenceSourcesEventArgs e)
	{
		if (Dispatcher.CurrentDispatcher != _dispatcher)
		{
			_dispatcher.BeginInvoke(delegate(SequenceSourcesEventArgs ev)
			{
				OnNewSequencesAvailable(ev);
			}, e);
			return;
		}
		foreach (AccessHandle activeHandle in GetActiveHandles())
		{
			activeHandle.FireNewSequencesAvailable(e);
		}
	}

	private void _queryDispatcher_SequencesRetrievedEvent(object sender, SequencesEventArgs e)
	{
		foreach (SourceQueryResult result in e.Results)
		{
			_availableResultQueue.Enqueue(result);
		}
		_backgroundDispatcher.BeginInvoke(HandleAvailableSequences);
	}

	private void StoreResult(SourceQueryResult result)
	{
		IDisposable lockObject = null;
		try
		{
			SourceCache orAdd = _sourceCacheDictionary.GetOrAdd(result.Source, delegate
			{
				SourceCache sourceCache = new SourceCache();
				lockObject = sourceCache.AcquireWriterLock();
				return sourceCache;
			});
			if (lockObject == null)
			{
				lockObject = orAdd.AcquireWriterLock();
			}
			CacheEntry cacheEntry = new CacheEntry(result);
			if (((SortedArray<CacheEntry>)orAdd).Contains(cacheEntry))
			{
				((SortedArray<CacheEntry>)orAdd).RemoveAllCopies(cacheEntry);
			}
			((SortedArray<CacheEntry>)orAdd).Add(cacheEntry);
		}
		finally
		{
			lockObject.Dispose();
		}
	}

	private void EnsureAccessHandleRegistered(AccessHandle accessHandle)
	{
		bool flag = false;
		for (int i = 0; i < _accessHandles.Count; i++)
		{
			if (!_accessHandles[i].IsAlive)
			{
				_accessHandles.RemoveAt(i);
				i--;
			}
			else if (accessHandle == _accessHandles[i].Target)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			_accessHandles.Add(new WeakReference(accessHandle));
		}
	}

	private IEnumerable<AccessHandle> GetActiveHandles()
	{
		for (int index = 0; index < _accessHandles.Count; index++)
		{
			if (!(_accessHandles[index].Target is AccessHandle accessHandle))
			{
				_accessHandles.RemoveAt(index);
				index--;
			}
			else
			{
				yield return accessHandle;
			}
		}
	}

	private void UpdateQueries(IList<AccessHandle> accessHandles)
	{
		if (!_isInitialized)
		{
			return;
		}
		if (Thread.CurrentThread != _backgroundDispatcher.Thread)
		{
			_backgroundDispatcher.BeginInvoke(delegate(IList<AccessHandle> ahs)
			{
				UpdateQueries(ahs);
			}, accessHandles);
			return;
		}
		List<SourceQuery> list = new List<SourceQuery>(_sourceQueryList);
		List<SourceQuery> list2 = new List<SourceQuery>();
		_sourceQueryList.Clear();
		foreach (AccessHandle accessHandle in accessHandles)
		{
			long num = 0L;
			foreach (TimeInterval loadBlock in GetLoadBlocks(accessHandle.Interval, _timelineCacheSettings.BlockLength, _timelineCacheSettings.PrefetchRatio))
			{
				foreach (ISequenceSource sequenceSource in accessHandle.SequenceSources)
				{
					bool flag = false;
					if (_sourceCacheDictionary.TryGetValue(sequenceSource, out var value) && _sourceCacheDictionary.TryGetValue(sequenceSource, out value))
					{
						using (value.AcquireReaderLock())
						{
							if (((IEnumerable<CacheEntry>)value).GetAllIntersecting(loadBlock).FirstOrDefault() != null)
							{
								flag = true;
							}
						}
					}
					if (flag)
					{
						continue;
					}
					SourceQuery sourceQuery = new SourceQuery(loadBlock)
					{
						Source = sequenceSource,
						Priority = num
					};
					if (list2.Contains(sourceQuery))
					{
						sourceQuery = list2[list2.IndexOf(sourceQuery)];
						if (num < sourceQuery.Priority)
						{
							sourceQuery.Priority = num;
						}
						list2.Remove(sourceQuery);
					}
					if (list.Contains(sourceQuery))
					{
						SourceQuery sourceQuery2 = list[list.IndexOf(sourceQuery)];
						if (!sourceQuery2.Source.SequenceRetriever.IsShutdown && sourceQuery2.Priority != SourceQuery.PriorityIgnore)
						{
							sourceQuery = sourceQuery2;
							list.Remove(sourceQuery);
							sourceQuery.Priority = num;
						}
						else
						{
							list2.Add(sourceQuery);
						}
					}
					else
					{
						list2.Add(sourceQuery);
					}
					_sourceQueryList.Add(sourceQuery);
				}
				num++;
			}
		}
		foreach (SourceQuery item in list)
		{
			item.Priority = SourceQuery.PriorityIgnore;
		}
		if (list2.Count > 0)
		{
			_queryDispatcher.GetSequences(list2);
		}
	}

	private void AddRefreshQueries(IEnumerable<SourceQuery> queries)
	{
		if (Thread.CurrentThread != _backgroundDispatcher.Thread)
		{
			_backgroundDispatcher.BeginInvoke(delegate(IEnumerable<SourceQuery> ql)
			{
				AddRefreshQueries(ql);
			}, queries);
			return;
		}
		List<SourceQuery> list = new List<SourceQuery>();
		foreach (SourceQuery query in queries)
		{
			SourceQuery item = query;
			if (list.Contains(query))
			{
				item = list[list.IndexOf(query)];
				if (query.Priority < item.Priority)
				{
					item.Priority = query.Priority;
				}
			}
			else
			{
				list.Add(item);
			}
		}
		if (list.Count > 0)
		{
			_queryDispatcher.GetSequences(list);
		}
	}

	private static IEnumerable<TimeInterval> GetLoadBlocks(TimeInterval interval, TimeSpan blockLength, double prefetchRatio)
	{
		if (interval == null || interval.TimeSpan.Ticks == 0L)
		{
			yield break;
		}
		TimeInterval other = new TimeInterval(GetBlockTime(interval.MiddleTime, blockLength), blockLength);
		yield return new TimeInterval(other);
		IEnumerator<TimeInterval> leftEnumerator = GetLeftLoadBlocks(interval, blockLength, prefetchRatio).GetEnumerator();
		IEnumerator<TimeInterval> rightEnumerator = GetRightLoadBlocks(interval, blockLength, prefetchRatio).GetEnumerator();
		bool availableOnLeft = leftEnumerator.MoveNext();
		bool availableOnRight = rightEnumerator.MoveNext();
		while (availableOnLeft || availableOnRight)
		{
			if (!availableOnLeft && availableOnRight)
			{
				yield return rightEnumerator.Current;
				availableOnRight = rightEnumerator.MoveNext();
			}
			else if (!availableOnRight && availableOnLeft)
			{
				yield return leftEnumerator.Current;
				availableOnLeft = leftEnumerator.MoveNext();
			}
			else if (interval.MiddleTime - leftEnumerator.Current.MiddleTime < rightEnumerator.Current.MiddleTime - interval.MiddleTime)
			{
				yield return leftEnumerator.Current;
				availableOnLeft = leftEnumerator.MoveNext();
			}
			else
			{
				yield return rightEnumerator.Current;
				availableOnRight = rightEnumerator.MoveNext();
			}
		}
	}

	private static IEnumerable<TimeInterval> GetLeftLoadBlocks(TimeInterval interval, TimeSpan blockLength, double prefetchRatio)
	{
		DateTime currentTime = GetBlockTime(interval.MiddleTime, blockLength);
		while (currentTime > interval.BeginTime - TimeSpan.FromTicks((long)((double)interval.TimeSpan.Ticks * prefetchRatio)))
		{
			currentTime -= blockLength;
			yield return new TimeInterval(currentTime, blockLength);
		}
	}

	private static IEnumerable<TimeInterval> GetRightLoadBlocks(TimeInterval interval, TimeSpan blockLength, double prefetchRatio)
	{
		for (DateTime currentTime = GetBlockTime(interval.MiddleTime, blockLength) + blockLength; currentTime < interval.EndTime + TimeSpan.FromTicks((long)((double)interval.TimeSpan.Ticks * prefetchRatio)); currentTime += blockLength)
		{
			yield return new TimeInterval(currentTime, blockLength);
		}
	}

	private static DateTime GetBlockTime(DateTime time, TimeSpan blockLength)
	{
		return new DateTime(_blockReferenceTime.Ticks + blockLength.Ticks * ((time.Ticks - _blockReferenceTime.Ticks) / blockLength.Ticks), DateTimeKind.Utc);
	}

	private void HandleAvailableSequences()
	{
		List<SourceQueryResult> list = new List<SourceQueryResult>();
		SourceQueryResult result;
		while (_availableResultQueue.TryDequeue(out result))
		{
			StoreResult(result);
			list.Add(result);
		}
		if (list.Count > 0)
		{
			_dispatcher.BeginInvoke(delegate(SequenceSourcesEventArgs eventArgs)
			{
				OnNewSequencesAvailable(eventArgs);
			}, new SequenceSourcesEventArgs(list));
		}
	}

	private void MaintenanceStrategy(IList<AccessHandle> accessHandles)
	{
		for (int i = 0; i < _disposableSourceCacheList.Count; i++)
		{
			SourceCache sourceCache = _disposableSourceCacheList[i];
			if (!sourceCache.AnyLockActivityExists)
			{
				sourceCache.Dispose();
				_disposableSourceCacheList.RemoveAt(i);
				i--;
			}
		}
		IEnumerator<ISequenceSource> enumerator = _sourceCacheDictionary.Keys.GetEnumerator();
		IList<ISequenceSource> list = accessHandles.SelectMany((AccessHandle ah) => ah.SequenceSources).ToList();
		DateTime utcNow = DateTime.UtcNow;
		IList<CacheEntry> list2 = new List<CacheEntry>();
		while (enumerator.MoveNext())
		{
			ISequenceSource current = enumerator.Current;
			if (!_sourceCacheDictionary.TryGetValue(current, out var value))
			{
				continue;
			}
			using (value.AcquireUpgradeableReadLock())
			{
				if (current.SequenceRetriever.IsShutdown)
				{
					_sourceCacheDictionary.TryRemove(current, out value);
					_disposableSourceCacheList.Add(value);
					continue;
				}
				if (!list.Contains(current))
				{
					if (value.OutOfUseTime == DateTime.MaxValue)
					{
						value.OutOfUseTime = utcNow;
					}
				}
				else
				{
					value.OutOfUseTime = DateTime.MaxValue;
					IList<CacheEntry> list3 = new List<CacheEntry>();
					bool flag = false;
					foreach (CacheEntry item in (EnumerableBase<CacheEntry>)(object)value)
					{
						bool flag2 = false;
						foreach (AccessHandle accessHandle in accessHandles)
						{
							if (accessHandle.SequenceSources.Contains(current))
							{
								TimeInterval interval = new TimeInterval(accessHandle.Interval.BeginTime.Ticks - (long)((double)accessHandle.Interval.TimeSpan.Ticks * _timelineCacheSettings.KeepRatio), accessHandle.Interval.TimeSpan.TotalMilliseconds * (1.0 + 2.0 * _timelineCacheSettings.KeepRatio));
								if (item.IntersectsWith(interval))
								{
									flag2 = true;
									break;
								}
							}
						}
						if (!flag2)
						{
							if (item.OutOfUseTime == DateTime.MaxValue)
							{
								item.OutOfUseTime = utcNow;
							}
						}
						else
						{
							item.OutOfUseTime = DateTime.MaxValue;
						}
						if (utcNow - item.OutOfUseTime >= _timelineCacheSettings.UnusedDataKeepSpan)
						{
							list3.Add(item);
						}
						if (!flag && item.Sequences.FirstOrDefault() != null)
						{
							flag = true;
							if (utcNow - item.FetchTime > _timelineCacheSettings.DatabaseStartRefreshSpan)
							{
								list2.Add(item);
								item.FetchTime = utcNow;
							}
						}
						if (item.IntersectsWith(new TimeInterval(item.FetchTime.AddMinutes(-1.0), DateTime.MaxValue)) && utcNow - item.FetchTime > _timelineCacheSettings.LiveViewsRefreshSpan)
						{
							list2.Add(item);
							item.FetchTime = utcNow;
						}
					}
					if (list3.Count > 0)
					{
						using (value.AcquireWriterLock())
						{
							foreach (CacheEntry item2 in list3)
							{
								((SortedArray<CacheEntry>)value).Remove(item2);
							}
						}
						if (_debugObject != null)
						{
							_debugObject.FireCacheEntryRemoved();
						}
					}
				}
				if (utcNow - value.OutOfUseTime > _timelineCacheSettings.UnusedDataKeepSpan)
				{
					_sourceCacheDictionary.TryRemove(current, out value);
					_disposableSourceCacheList.Add(value);
				}
			}
		}
		if (list2.Count > 0)
		{
			AddRefreshQueries(list2.Convert((CacheEntry t) => new SourceQuery(t)
			{
				Priority = SourceQuery.PriorityLowest
			}));
		}
	}
}
