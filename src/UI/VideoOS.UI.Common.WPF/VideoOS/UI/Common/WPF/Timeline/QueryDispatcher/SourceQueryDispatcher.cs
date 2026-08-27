using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

namespace VideoOS.UI.Common.WPF.Timeline.QueryDispatcher;

public class SourceQueryDispatcher : ISourceQueryDispatcher
{
	private ConcurrentDictionary<ISequenceRetriever, int> _retrievers = new ConcurrentDictionary<ISequenceRetriever, int>();

	private int _controlNumber;

	public event EventHandler<SequencesEventArgs> SequencesRetrieved;

	public void GetSequences(IEnumerable<SourceQuery> queries)
	{
		foreach (ISequenceRetriever key in _retrievers.Keys)
		{
			if (key.IsShutdown)
			{
				_retrievers.TryRemove(key, out var _);
			}
		}
		Dictionary<ISequenceRetriever, IList<SourceQuery>> dictionary = new Dictionary<ISequenceRetriever, IList<SourceQuery>>();
		foreach (SourceQuery query in queries)
		{
			if (!dictionary.TryGetValue(query.Source.SequenceRetriever, out var value2))
			{
				value2 = new List<SourceQuery>();
				dictionary.Add(query.Source.SequenceRetriever, value2);
			}
			value2.Add(query);
		}
		foreach (ISequenceRetriever key2 in dictionary.Keys)
		{
			dictionary.TryGetValue(key2, out var value3);
			if (!_retrievers.ContainsKey(key2))
			{
				int num = Interlocked.Increment(ref _controlNumber);
				if (num == _retrievers.GetOrAdd(key2, num))
				{
					key2.SequencesRetrieved += retriever_SequencesRetrieved;
				}
			}
			key2.GetSequences(value3);
		}
	}

	private void retriever_SequencesRetrieved(object sender, SequencesEventArgs e)
	{
		this.SequencesRetrieved?.Invoke(this, e);
	}
}
