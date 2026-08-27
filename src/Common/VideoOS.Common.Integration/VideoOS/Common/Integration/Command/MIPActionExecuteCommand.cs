using System;
using System.Threading.Tasks;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Command;

public class MIPActionExecuteCommand : CommonCommandBase
{
	public Guid ActionId { get; private set; }

	public string ActionName { get; private set; }

	public Guid[] Targets { get; private set; }

	public TriggerEvent TriggerEvent { get; private set; }

	public MIPActionExecuteCommand(Guid actionId, string actionName, Guid[] targets, TriggerEvent triggerEvent)
	{
		ActionId = actionId;
		ActionName = actionName;
		Targets = targets;
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
