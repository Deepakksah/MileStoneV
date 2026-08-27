using System.Collections.Generic;

namespace VideoOS.Management.VmoClient;

public class RuleSourceComparer : IEqualityComparer<RuleSource>
{
	public bool Equals(RuleSource x, RuleSource y)
	{
		if (x == y)
		{
			return true;
		}
		if (x == null || y == null)
		{
			return false;
		}
		return x.Id == y.Id;
	}

	public int GetHashCode(RuleSource obj)
	{
		return obj?.Id.GetHashCode() ?? 0;
	}
}
