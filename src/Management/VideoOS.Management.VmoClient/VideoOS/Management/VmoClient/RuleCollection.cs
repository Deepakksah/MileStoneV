using System;

namespace VideoOS.Management.VmoClient;

public class RuleCollection : VmoObjectGuidBasedCollection<Rule>
{
	public RuleCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, Rule[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
