using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;
using VideoOS.UI.Common.WPF.Timeline.TestData;

namespace VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

public class DummySequenceRetriever : SequenceRetrieverBase
{
	private class DummyRetriever : RetrieverOperation
	{
		public DummyRetriever(SourceQuery query)
			: base(query)
		{
		}

		protected override void OnExecute()
		{
			AddSortedSequences(SourceData.GetSourceData(base.Queries.First().Source).GetSequences(base.Queries.First()));
			FlushSequences(base.Queries.First().Source, clearCache: true);
		}
	}

	public DummySequenceRetriever()
		: base(Guid.Empty, "Dummy API retriever background thread", 5)
	{
	}

	protected override RetrieverOperation CreateRetrieverOperation(SourceQuery selectedQuery, IEnumerable<SourceQuery> availableQueryList, out IEnumerable<SourceQuery> selectedQueryList)
	{
		selectedQueryList = null;
		return new DummyRetriever(selectedQuery);
	}
}
