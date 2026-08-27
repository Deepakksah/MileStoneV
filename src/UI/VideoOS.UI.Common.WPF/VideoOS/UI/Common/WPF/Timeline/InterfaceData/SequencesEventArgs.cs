using System;
using System.Collections.Generic;

namespace VideoOS.UI.Common.WPF.Timeline.InterfaceData;

public class SequencesEventArgs : EventArgs
{
	public new static readonly SequencesEventArgs Empty = new SequencesEventArgs(new SourceQueryResult[0]);

	public IEnumerable<SourceQueryResult> Results { get; private set; }

	public SequencesEventArgs(IEnumerable<SourceQueryResult> results)
	{
		Results = results;
	}

	public SequencesEventArgs(SourceQueryResult result)
	{
		Results = new SourceQueryResult[1] { result };
	}
}
