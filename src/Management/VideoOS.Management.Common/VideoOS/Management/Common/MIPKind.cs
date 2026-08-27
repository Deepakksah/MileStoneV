using System;

namespace VideoOS.Management.Common;

public class MIPKind
{
	public Guid Kind { get; private set; }

	public Guid[] SubFilters { get; private set; }

	public MIPKind(Guid kind, Guid[] subFilters)
	{
		Kind = kind;
		SubFilters = subFilters;
	}
}
