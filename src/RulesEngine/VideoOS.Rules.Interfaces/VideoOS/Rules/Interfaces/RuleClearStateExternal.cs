using System;

namespace VideoOS.Rules.Interfaces;

public sealed class RuleClearStateExternal
{
	public Guid RuleId { get; private set; }

	public RuleClearStateExternal(Guid ruleId)
	{
		RuleId = ruleId;
	}
}
