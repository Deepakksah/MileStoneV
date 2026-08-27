using System;

namespace VideoOS.Rules.Interfaces;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RuleActionFactoryAttribute : Attribute
{
	public string RuleActionName { get; }

	public Guid RuleActionFactoryId { get; }

	public bool Replacement { get; }

	public ActionTargetType ActionTargetType { get; }

	public string TargetItemsAttributeName { get; }

	public string TargetGroupItemsAttributeName { get; }

	public RuleActionFactoryAttribute(string ruleActionName, string ruleActionFactoryId, ActionTargetType actionTargetType)
		: this(ruleActionName, ruleActionFactoryId, actionTargetType, string.Empty)
	{
	}

	public RuleActionFactoryAttribute(string ruleActionName, string ruleActionFactoryId, ActionTargetType actionTargetType, string targetItemsAttributeName)
		: this(ruleActionName, ruleActionFactoryId, actionTargetType, targetItemsAttributeName, string.Empty)
	{
	}

	public RuleActionFactoryAttribute(string ruleActionName, string ruleActionFactoryId, ActionTargetType actionTargetType, string targetItemsAttributeName, string targetGroupItemsAttributeName)
		: this(ruleActionName, ruleActionFactoryId, actionTargetType, targetItemsAttributeName, targetGroupItemsAttributeName, replacement: false)
	{
	}

	public RuleActionFactoryAttribute(string ruleActionName, string ruleActionFactoryId, ActionTargetType actionTargetType, string targetItemsAttributeName, string targetGroupItemsAttributeName, bool replacement)
	{
		ActionTargetType = actionTargetType;
		TargetItemsAttributeName = targetItemsAttributeName;
		TargetGroupItemsAttributeName = targetGroupItemsAttributeName;
		RuleActionName = ruleActionName;
		RuleActionFactoryId = new Guid(ruleActionFactoryId);
		Replacement = replacement;
	}
}
