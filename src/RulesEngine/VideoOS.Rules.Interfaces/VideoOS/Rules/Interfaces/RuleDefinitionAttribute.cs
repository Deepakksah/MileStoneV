using System;

namespace VideoOS.Rules.Interfaces;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RuleDefinitionAttribute : Attribute
{
	private RuleType ruleTypes;

	private DefinitionType definitionTypes;

	private bool timeCondition;

	public bool TimeCondition
	{
		get
		{
			return timeCondition;
		}
		set
		{
			timeCondition = value;
		}
	}

	public RuleDefinitionAttribute(RuleType ruleTypes, DefinitionType definitionTypes)
	{
		this.ruleTypes = ruleTypes;
		this.definitionTypes = definitionTypes;
	}

	public bool IsInRuleType(RuleType ruleType)
	{
		return (ruleTypes & ruleType) > RuleType.None;
	}

	public bool IsInDefinitionType(DefinitionType definitionType)
	{
		return (definitionTypes & definitionType) > (DefinitionType)0;
	}
}
