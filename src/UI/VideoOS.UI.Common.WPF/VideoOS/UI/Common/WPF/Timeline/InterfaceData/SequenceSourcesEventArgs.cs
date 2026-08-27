using System;
using System.Collections.Generic;

namespace VideoOS.UI.Common.WPF.Timeline.InterfaceData;

public class SequenceSourcesEventArgs : EventArgs
{
	public new static readonly SequenceSourcesEventArgs Empty = new SequenceSourcesEventArgs(null);

	public IEnumerable<SourceQueryResult> Results { get; private set; }

	public SequenceSourcesEventArgs(IEnumerable<SourceQueryResult> results)
	{
		Results = results;
	}
}
