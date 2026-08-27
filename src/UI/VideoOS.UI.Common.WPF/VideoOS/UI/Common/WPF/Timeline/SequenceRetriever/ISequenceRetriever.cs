using System;
using System.Collections.Generic;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;

namespace VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

public interface ISequenceRetriever : IDisposable
{
	bool IsShutdown { get; }

	event EventHandler<SequencesEventArgs> SequencesRetrieved;

	void GetSequences(IEnumerable<SourceQuery> queries);
}
