using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public class ExecuteRuleActionsCommand : CommonCommandBase
{
	private Guid sourceId;

	private RuleActionExecutionRequest[] ruleActionExecutionRequests;

	private DateTime senderUtcTime;

	public Guid SourceId
	{
		[DebuggerStepThrough]
		get
		{
			return sourceId;
		}
		[DebuggerStepThrough]
		set
		{
			sourceId = value;
		}
	}

	public RuleActionExecutionRequest[] RuleActionExecutionRequests
	{
		[DebuggerStepThrough]
		get
		{
			return ruleActionExecutionRequests;
		}
		[DebuggerStepThrough]
		set
		{
			ruleActionExecutionRequests = value;
		}
	}

	public DateTime SenderUtcTime
	{
		[DebuggerStepThrough]
		get
		{
			return senderUtcTime;
		}
		[DebuggerStepThrough]
		set
		{
			senderUtcTime = value;
		}
	}

	public ExecuteRuleActionsCommand(Guid sourceId, RuleActionExecutionRequest[] ruleActionExecutionRequests)
	{
		this.sourceId = sourceId;
		this.ruleActionExecutionRequests = ruleActionExecutionRequests;
		senderUtcTime = DateTime.UtcNow;
	}

	public ExecuteRuleActionsCommand(Guid sourceId, RuleActionExecutionRequest[] ruleActionExecutionRequests, DateTime senderTimeUtc)
	{
		this.sourceId = sourceId;
		this.ruleActionExecutionRequests = ruleActionExecutionRequests;
		senderUtcTime = senderTimeUtc;
	}

	public override void Accept(ICommandVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonCommandVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
