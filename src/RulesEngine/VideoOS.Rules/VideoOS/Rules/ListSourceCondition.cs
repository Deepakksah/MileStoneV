using System;
using System.Collections.Generic;

namespace VideoOS.Rules;

public class ListSourceCondition : SourceCondition
{
	private List<Guid> _sourceList;

	public ListSourceCondition(List<Guid> sourceList)
	{
		_sourceList = sourceList;
	}

	public override List<Guid> GetSources()
	{
		return _sourceList;
	}
}
