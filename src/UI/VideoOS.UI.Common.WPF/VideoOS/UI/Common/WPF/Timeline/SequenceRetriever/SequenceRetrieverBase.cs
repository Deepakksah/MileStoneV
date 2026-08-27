using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using C5;
using Microsoft.Extensions.Logging;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

public abstract class SequenceRetrieverBase : ISequenceRetriever, IDisposable
{
	protected abstract class RetrieverOperation
	{
		private Task _task;

		private ISorted<Sequence> _retrievedSequences = (ISorted<Sequence>)(object)new SortedArray<Sequence>((MemoryType)0);

		private ISortedEnumerable<SourceQuery> _queries;

		protected CancellationToken CancellationToken { get; private set; }

		protected ISortedEnumerable<SourceQuery> Queries => _queries;

		public event EventHandler<SequencesEventArgs> SequencesRetrieved;

		public event EventHandler Finished;

		protected RetrieverOperation(ISortedEnumerable<SourceQuery> sortedQueries)
		{
			_queries = sortedQueries;
		}

		protected RetrieverOperation(SourceQuery sourceQuery)
			: this(new SourceQuery[1] { sourceQuery }.ToSortedEnumerable())
		{
		}

		public void Start(CancellationToken cancellationToken)
		{
			CancellationToken = cancellationToken;
			_task = Task.Factory.StartNew(Run, cancellationToken);
		}

		protected void OnDataRetrieved(IEnumerable<SourceQueryResult> results)
		{
			if (!CancellationToken.IsCancellationRequested)
			{
				this.SequencesRetrieved?.Invoke(this, new SequencesEventArgs(results));
			}
		}

		private void Run()
		{
			try
			{
				OnExecute();
			}
			catch (Exception ex)
			{
				if (!CancellationToken.IsCancellationRequested)
				{
					LogException(ex);
				}
			}
			finally
			{
				OnFinished();
			}
		}

		protected void AddSequence(Sequence sequence)
		{
			if (!CancellationToken.IsCancellationRequested)
			{
				((ICollection<Sequence>)(object)_retrievedSequences).Add(sequence);
			}
		}

		protected void AddSortedSequences(ISortedEnumerable<Sequence> sequences)
		{
			if (!CancellationToken.IsCancellationRequested)
			{
				_retrievedSequences.AddSorted((IEnumerable<Sequence>)sequences);
			}
		}

		protected void FlushSequences(bool clearCache)
		{
			if (!CancellationToken.IsCancellationRequested)
			{
				FlushSequences(Guid.Empty, Guid.Empty, clearCache);
			}
		}

		protected void FlushSequences(ISequenceSource source, bool clearCache)
		{
			if (!CancellationToken.IsCancellationRequested)
			{
				FlushSequences(source.SourceId, source.InformationId, clearCache);
			}
		}

		protected void FlushSequences(Guid sourceId, Guid informationType, bool clearCache)
		{
			List<SourceQueryResult> list = new List<SourceQueryResult>();
			foreach (SourceQuery query in _queries)
			{
				if (CancellationToken.IsCancellationRequested)
				{
					break;
				}
				if ((Guid.Empty == sourceId || query.Source.SourceId == sourceId) && (Guid.Empty == informationType || query.Source.InformationId == informationType))
				{
					ISortedEnumerable<Sequence> allIntersecting = ((IEnumerable<Sequence>)_retrievedSequences).GetAllIntersecting(query);
					list.Add(new SourceQueryResult(query)
					{
						Sequences = allIntersecting
					});
				}
			}
			if (clearCache)
			{
				((ICollection<Sequence>)(object)_retrievedSequences).Clear();
			}
			if (list.Count > 0)
			{
				OnDataRetrieved(list);
			}
		}

		protected abstract void OnExecute();

		private void OnFinished()
		{
			this.Finished?.Invoke(this, EventArgs.Empty);
		}
	}

	private Guid _serverId;

	private int _maxConcurrentQueries;

	private AutoResetEvent _activityEvent = new AutoResetEvent(initialState: false);

	private object _backgroundThreadHandlingLock = new object();

	private CancellationTokenSource _finalShutDownTokenSource = new CancellationTokenSource();

	private Thread _backgroundThread;

	private long _runningOperationCount;

	private bool _suspendRequested;

	private string _backgroundThreadName;

	private DateTime _lastActivityTime;

	private ConcurrentDictionary<SourceQuery, short> _incomingQuerySet;

	private static readonly TimeSpan _backgroundThreadShutdownTimeout = TimeSpan.FromMinutes(3.0);

	private static ILogger _logger;

	protected Guid ServerId => _serverId;

	public bool IsShutdown
	{
		get
		{
			if (_finalShutDownTokenSource != null)
			{
				return _finalShutDownTokenSource.IsCancellationRequested;
			}
			return true;
		}
	}

	public event EventHandler<SequencesEventArgs> SequencesRetrieved;

	public SequenceRetrieverBase(Guid serverId, string backgroundThreadName, int maxConcurrentQueries)
	{
		_backgroundThreadName = backgroundThreadName;
		_incomingQuerySet = new ConcurrentDictionary<SourceQuery, short>();
		_serverId = serverId;
		_maxConcurrentQueries = maxConcurrentQueries;
		if (_logger == null)
		{
			_logger = Telemetry.GetILogger("ClientLogger");
		}
	}

	protected static ISortedEnumerable<SourceQuery> BundleQueriesSingleSource(SourceQuery referenceQuery, IEnumerable<SourceQuery> availableQueryList, TimeSpan timeSpanOptimizationHint)
	{
		return DoBundleQueriesSingleSource(referenceQuery, availableQueryList, timeSpanOptimizationHint).Values.ToSortedEnumerable();
	}

	private static SortedList<DateTime, SourceQuery> DoBundleQueriesSingleSource(SourceQuery referenceQuery, IEnumerable<SourceQuery> availableQueryList, TimeSpan timeSpanOptimizationHint)
	{
		SortedList<DateTime, SourceQuery> sortedList = new SortedList<DateTime, SourceQuery>();
		SortedList<DateTime, SourceQuery> sortedList2 = new SortedList<DateTime, SourceQuery>();
		sortedList2.Add(referenceQuery.BeginTime, referenceQuery);
		foreach (SourceQuery availableQuery in availableQueryList)
		{
			if (availableQuery != referenceQuery && availableQuery.Source == referenceQuery.Source && availableQuery.BeginTime - referenceQuery.BeginTime <= timeSpanOptimizationHint && referenceQuery.EndTime - availableQuery.EndTime <= timeSpanOptimizationHint)
			{
				sortedList2.Add(availableQuery.BeginTime, availableQuery);
			}
		}
		int num;
		int i = (num = sortedList2.IndexOfValue(referenceQuery));
		while (num > 0 && sortedList2.Values[num].BeginTime == sortedList2.Values[num - 1].EndTime)
		{
			num--;
		}
		for (; i < sortedList2.Count - 1 && sortedList2.Values[i].EndTime == sortedList2.Values[i + 1].BeginTime; i++)
		{
		}
		for (int j = num; j <= i; j++)
		{
			sortedList.Add(sortedList2.Keys[j], sortedList2.Values[j]);
		}
		return sortedList;
	}

	protected static ISortedEnumerable<SourceQuery> BundleQueriesMultipleSources(SourceQuery referenceQuery, IEnumerable<SourceQuery> availableQueryList, TimeSpan timeSpanOptimizationHint, bool sameSourceId, out List<ISequenceSource> selectedSequenceSources)
	{
		SortedList<DateTime, SourceQuery> sortedList = DoBundleQueriesSingleSource(referenceQuery, availableQueryList, timeSpanOptimizationHint);
		ISorted<SourceQuery> val = (ISorted<SourceQuery>)(object)new SortedArray<SourceQuery>((MemoryType)0);
		List<ISequenceSource> list = new List<ISequenceSource>();
		List<SourceQuery[]> list2 = new List<SourceQuery[]>();
		foreach (SourceQuery availableQuery in availableQueryList)
		{
			if (!sameSourceId || referenceQuery.Source.SourceId == availableQuery.Source.SourceId)
			{
				int num = list.IndexOf(availableQuery.Source);
				if (num < 0)
				{
					num = list.Count;
					list.Add(availableQuery.Source);
					list2.Add(new SourceQuery[sortedList.Keys.Count]);
				}
				int num2 = sortedList.Keys.IndexOf(availableQuery.BeginTime);
				if (num2 >= 0)
				{
					list2[num][num2] = availableQuery;
				}
			}
		}
		selectedSequenceSources = new List<ISequenceSource>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list2[i].Count((SourceQuery sq) => sq != null) < sortedList.Keys.Count / 2)
			{
				continue;
			}
			ISequenceSource item = list[i];
			selectedSequenceSources.Add(item);
			for (int num3 = 0; num3 < sortedList.Keys.Count; num3++)
			{
				SourceQuery sourceQuery = list2[i][num3];
				if (sourceQuery != null)
				{
					((ICollection<SourceQuery>)(object)val).Add(sourceQuery);
				}
			}
		}
		return val.ToSortedEnumerable<SourceQuery>();
	}

	private void ProcessIncomingQueries()
	{
		long num = Interlocked.Read(ref _runningOperationCount);
		bool flag = true;
		while (num < _maxConcurrentQueries && flag)
		{
			flag = TryDequeueAndProcessOneQuery();
			num = Interlocked.Read(ref _runningOperationCount);
		}
		if (num != 0L || _incomingQuerySet.Count != 0 || !(DateTime.UtcNow - _lastActivityTime > _backgroundThreadShutdownTimeout))
		{
			return;
		}
		lock (_backgroundThreadHandlingLock)
		{
			if (num == 0L && _incomingQuerySet.Count == 0 && DateTime.UtcNow - _lastActivityTime > _backgroundThreadShutdownTimeout)
			{
				SuspendBackgroundThread(join: false);
			}
		}
	}

	private void SuspendBackgroundThread(bool join)
	{
		_suspendRequested = true;
		WakeUpBackgroundThread(createIfNeeded: false);
		if (join && _backgroundThread != null)
		{
			_backgroundThread.Join(10000);
		}
	}

	private bool TryDequeueAndProcessOneQuery()
	{
		bool result = false;
		SourceQuery[] array = _incomingQuerySet.Keys.ToArray();
		if (array.Length != 0)
		{
			_lastActivityTime = DateTime.UtcNow;
			long num = long.MaxValue;
			SourceQuery sourceQuery = null;
			SourceQuery[] array2 = array;
			foreach (SourceQuery sourceQuery2 in array2)
			{
				if (sourceQuery2.Priority < num)
				{
					num = sourceQuery2.Priority;
					sourceQuery = sourceQuery2;
				}
			}
			if (num == SourceQuery.PriorityIgnore)
			{
				array2 = array;
				foreach (SourceQuery key in array2)
				{
					_incomingQuerySet.TryRemove(key, out var _);
				}
			}
			else
			{
				IEnumerable<SourceQuery> selectedQueryList;
				RetrieverOperation retrieverOperation = CreateRetrieverOperation(sourceQuery, array, out selectedQueryList);
				if (selectedQueryList == null)
				{
					selectedQueryList = new SourceQuery[1] { sourceQuery };
				}
				foreach (SourceQuery item in selectedQueryList)
				{
					_incomingQuerySet.TryRemove(item, out var _);
				}
				Interlocked.Increment(ref _runningOperationCount);
				retrieverOperation.SequencesRetrieved += operation_SequencesRetrieved;
				retrieverOperation.Finished += operation_Finished;
				retrieverOperation.Start(_finalShutDownTokenSource.Token);
				result = true;
			}
		}
		return result;
	}

	public static void LogException(Exception ex)
	{
		try
		{
			if (_logger != null)
			{
				string text = "Timeline Sequence Retriever: Exception occured.";
				text = text + Environment.NewLine + "Message: " + ex.Message;
				text = text + Environment.NewLine + "Source: " + ex.Source;
				text = text + Environment.NewLine + "ExceptionType: " + ex.GetType().FullName;
				text = text + Environment.NewLine + "Source: " + ex.StackTrace;
				for (Exception innerException = ex.InnerException; innerException != null; innerException = innerException.InnerException)
				{
					text += "-->";
					text = text + Environment.NewLine + "Message: " + innerException.Message;
					text = text + Environment.NewLine + "Source: " + innerException.Source;
					text = text + Environment.NewLine + "Source: " + innerException.StackTrace;
				}
				LoggerExtensions.LogError(_logger, text, Array.Empty<object>());
			}
		}
		catch (InvalidOperationException)
		{
		}
	}

	public void GetSequences(IEnumerable<SourceQuery> queries)
	{
		if (IsShutdown)
		{
			return;
		}
		foreach (SourceQuery query in queries)
		{
			_incomingQuerySet.TryRemove(query, out var _);
			_incomingQuerySet.AddOrUpdate(query, 0, (SourceQuery q, short s) => (short)(s + 1));
		}
		WakeUpBackgroundThread(createIfNeeded: true);
	}

	private void WakeUpBackgroundThread(bool createIfNeeded)
	{
		if ((_backgroundThread == null || !_backgroundThread.IsAlive) && createIfNeeded)
		{
			lock (_backgroundThreadHandlingLock)
			{
				if (_backgroundThread == null || !_backgroundThread.IsAlive)
				{
					ThreadStart start = BackgroundThreadLoop;
					_backgroundThread = new Thread(start);
					_backgroundThread.IsBackground = true;
					_backgroundThread.Name = _backgroundThreadName;
					_backgroundThread.Start();
					_lastActivityTime = DateTime.UtcNow;
				}
			}
		}
		_activityEvent.Set();
	}

	private void BackgroundThreadLoop()
	{
		while (!IsShutdown && !_suspendRequested)
		{
			ProcessIncomingQueries();
			_activityEvent.WaitOne((_incomingQuerySet.Count == 0) ? ((int)_backgroundThreadShutdownTimeout.TotalMilliseconds) : (-1));
		}
		_suspendRequested = false;
	}

	private void operation_SequencesRetrieved(object sender, SequencesEventArgs e)
	{
		OnSequencesRetrieved(e.Results);
	}

	private void operation_Finished(object sender, EventArgs e)
	{
		Interlocked.Decrement(ref _runningOperationCount);
		if (!IsShutdown)
		{
			WakeUpBackgroundThread(createIfNeeded: true);
		}
	}

	protected void OnSequencesRetrieved(IEnumerable<SourceQueryResult> results)
	{
		EventHandler<SequencesEventArgs> eventHandler = this.SequencesRetrieved;
		if (eventHandler != null && !IsShutdown)
		{
			SequencesEventArgs e = new SequencesEventArgs(results);
			eventHandler(this, e);
		}
	}

	protected abstract RetrieverOperation CreateRetrieverOperation(SourceQuery selectedQuery, IEnumerable<SourceQuery> availableQueryList, out IEnumerable<SourceQuery> selectedQueryList);

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_finalShutDownTokenSource.Cancel();
			SuspendBackgroundThread(join: true);
			_incomingQuerySet.Clear();
			_finalShutDownTokenSource.Dispose();
			_finalShutDownTokenSource = null;
			_activityEvent.Dispose();
			_activityEvent = null;
		}
	}
}
