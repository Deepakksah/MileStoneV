using System;

namespace VideoOS.Management.VmoClient;

public sealed class RuleSourceCollection : VmoObjectGuidBasedCollection<RuleSource>
{
	public RuleSourceCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, RuleSource[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
