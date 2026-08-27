using System;
using System.Collections.Generic;

namespace VideoOS.Rules;

public abstract class SourceCondition
{
	public abstract List<Guid> GetSources();
}
