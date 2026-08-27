using System;

namespace VideoOS.Management.VmoClient;

public sealed class PerformanceStateRuleConfigurationCollection : VmoObjectGuidBasedCollection<PerformanceStateRuleConfiguration>
{
	public PerformanceStateRuleConfigurationCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, PerformanceStateRuleConfiguration[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
