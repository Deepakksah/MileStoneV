#define TRACE
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;
using VideoOS.UI.Common.WPF.Timeline.TestData;

namespace VideoOS.UI.Common.WPF.Timeline.QueryDispatcher;

public class UnitTestQueryDispatcher : ISourceQueryDispatcher, IDisposable
{
	private ConcurrentQueue<SourceQueryResult> _sequenceListQueue = new ConcurrentQueue<SourceQueryResult>();

	private DispatcherTimer _eventTimer;

	private IList<SourceQuery> _queries = new List<SourceQuery>();

	private ManualResetEventSlim _waitHandle;

	private ManualResetEventSlim _sourceDataAccessSync = new ManualResetEventSlim(initialState: true);

	private long _activeQueryCount;

	public bool NoDataRetrieval { get; set; }

	public IList<SourceQuery> Queries => _queries;

	public long ActiveQueryCount => Interlocked.Read(ref _activeQueryCount);

	public event EventHandler<SequencesEventArgs> SequencesRetrieved;

	public event EventHandler QueryReceived;

	public UnitTestQueryDispatcher(ManualResetEventSlim waitHandler)
	{
		_waitHandle = waitHandler;
		_eventTimer = new DispatcherTimer();
		_eventTimer.Interval = TimeSpan.FromMilliseconds(100.0);
		_eventTimer.Tick += _eventTimer_Tick;
		_eventTimer.Start();
	}

	private long IncrementActiveQueryCount()
	{
		return Interlocked.Increment(ref _activeQueryCount);
	}

	private long DecrementActiveQueryCount()
	{
		return Interlocked.Decrement(ref _activeQueryCount);
	}

	public void GetSequences(IEnumerable<SourceQuery> queries)
	{
		try
		{
			foreach (SourceQuery item2 in queries.OrderBy((SourceQuery q) => q.Priority))
			{
				IncrementActiveQueryCount();
				Trace.WriteLine($"Query {item2}");
				_sourceDataAccessSync.Wait();
				_sourceDataAccessSync.Reset();
				if (NoDataRetrieval)
				{
					_queries.Add(item2);
					_sourceDataAccessSync.Set();
				}
				else
				{
					SourceData sourceData = SourceData.GetSourceData(item2.Source);
					sourceData.AverageDelay = TimeSpan.FromTicks(0L);
					sourceData.MaxDelayDeviation = TimeSpan.FromTicks(0L);
					sourceData.BeginGetSequences(item2, delegate(SourceQuery q, IEnumerable<Sequence> sequences)
					{
						SourceQueryResult item = new SourceQueryResult(q)
						{
							Sequences = sequences.GetAllIntersecting(DateTime.UtcNow, (Sequence s) => new Sequence(s))
						};
						_sequenceListQueue.Enqueue(item);
						DecrementActiveQueryCount();
						_sourceDataAccessSync.Set();
					});
				}
				OnQueryReceived();
			}
		}
		catch (ObjectDisposedException)
		{
		}
	}

	private void OnQueryReceived()
	{
		this.QueryReceived?.Invoke(this, EventArgs.Empty);
	}

	protected virtual void OnSequencesRetrieved(SequencesEventArgs e)
	{
		this.SequencesRetrieved?.Invoke(this, e);
	}

	private void _eventTimer_Tick(object sender, EventArgs e)
	{
		if (_waitHandle.Wait(1))
		{
			List<SourceQueryResult> list = new List<SourceQueryResult>();
			if (_sequenceListQueue.TryDequeue(out var result))
			{
				Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "[UnitTestQueryDispatcher] One event dequeued, {0} left.", _sequenceListQueue.Count));
				list.Add(result);
				SequencesEventArgs e2 = new SequencesEventArgs(list);
				OnSequencesRetrieved(e2);
			}
		}
	}

	public void Dispose()
	{
		_sourceDataAccessSync.Dispose();
	}
}
