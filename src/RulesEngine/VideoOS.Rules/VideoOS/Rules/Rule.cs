using System;
using System.Diagnostics;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

[DebuggerDisplay("Rule. Name: {Name}; Enabled: {Enabled}; Id: {ID}")]
public sealed class Rule : IRule
{
	private ISubRule startRule;

	private ISubRule stopRule;

	public Guid ID { get; private set; }

	public string Name { get; private set; }

	public bool Enabled { get; set; }

	public ISubRule StartRule
	{
		[DebuggerStepThrough]
		get
		{
			return startRule;
		}
		set
		{
			startRule = value;
			if (startRule != null)
			{
				startRule.ParentRule = this;
				startRule.IsStartRule = true;
			}
		}
	}

	public ISubRule StopRule
	{
		[DebuggerStepThrough]
		get
		{
			return stopRule;
		}
		set
		{
			stopRule = value;
			if (stopRule != null)
			{
				stopRule.ParentRule = this;
				stopRule.IsStopRule = true;
			}
		}
	}

	public Rule(Guid ruleId, string name)
	{
		ID = ruleId;
		Name = name;
		Enabled = false;
		startRule = null;
		stopRule = null;
	}
}
