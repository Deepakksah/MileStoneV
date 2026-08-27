#define TRACE
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Threading;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;
using VideoOS.UI.Common.WPF.Timeline.TestData;

namespace VideoOS.UI.Common.WPF.Timeline.QueryDispatcher;

public class DummyQueryDispatcher : ISourceQueryDispatcher
{
	private DispatcherTimer _eventTimer;

	private ConcurrentQueue<SourceQueryResult> _sequenceListQueue = new ConcurrentQueue<SourceQueryResult>();

	public event EventHandler<SequencesEventArgs> SequencesRetrieved;

	public DummyQueryDispatcher()
	{
		_eventTimer = new DispatcherTimer
		{
			Interval = TimeSpan.FromMilliseconds(100.0),
			IsEnabled = true
		};
		_eventTimer.Tick += _eventTimer_Tick;
	}

	public void GetSequences(IEnumerable<SourceQuery> queries)
	{
		Trace.WriteLine("=== DummyQueryDispatcher.GetSequences ===");
		foreach (SourceQuery query in queries)
		{
			Trace.WriteLine($"Query {query}");
			SourceData.GetSourceData(query.Source).BeginGetSequences(query, delegate(SourceQuery q, IEnumerable<Sequence> sequences)
			{
				SourceQueryResult result = new SourceQueryResult(q)
				{
					Sequences = sequences.GetAllIntersecting(DateTime.UtcNow, (Sequence s) => new Sequence(s))
				};
				OnSequencesRetrieved(new SequencesEventArgs(result));
			});
		}
	}

	protected virtual void OnSequencesRetrieved(SequencesEventArgs e)
	{
		this.SequencesRetrieved?.Invoke(this, e);
	}

	private void _eventTimer_Tick(object sender, EventArgs e)
	{
		List<SourceQueryResult> list = new List<SourceQueryResult>();
		list.AddRange(GetEnqueuedResults(5));
		if (list.Count > 0)
		{
			SequencesEventArgs e2 = new SequencesEventArgs(list);
			OnSequencesRetrieved(e2);
		}
	}

	private IEnumerable<SourceQueryResult> GetEnqueuedResults(int maxCount)
	{
		SourceQueryResult result;
		while (_sequenceListQueue.TryDequeue(out result))
		{
			yield return result;
			int num = maxCount - 1;
			maxCount = num;
			if (num == 0)
			{
				break;
			}
		}
	}
}
