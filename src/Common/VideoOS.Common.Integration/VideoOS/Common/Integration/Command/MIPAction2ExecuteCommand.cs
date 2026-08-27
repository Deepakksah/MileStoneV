using System;
using System.Threading.Tasks;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Command;

public class MIPAction2ExecuteCommand : CommonCommandBase
{
	public Guid ActionId { get; private set; }

	public string ActionName { get; private set; }

	public Guid[] Targets { get; private set; }

	public Guid[] Targets2 { get; private set; }

	public TriggerEvent TriggerEvent { get; private set; }

	public MIPAction2ExecuteCommand(Guid actionId, string actionName, Guid[] targets, Guid[] targets2, TriggerEvent triggerEvent)
	{
		ActionId = actionId;
		ActionName = actionName;
		Targets = targets;
		Targets2 = targets2;
		TriggerEvent = triggerEvent;
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
