using System;
using System.Collections.Generic;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;

namespace VideoOS.UI.Common.WPF.Timeline.QueryDispatcher;

public interface ISourceQueryDispatcher
{
	event EventHandler<SequencesEventArgs> SequencesRetrieved;

	void GetSequences(IEnumerable<SourceQuery> queries);
}
