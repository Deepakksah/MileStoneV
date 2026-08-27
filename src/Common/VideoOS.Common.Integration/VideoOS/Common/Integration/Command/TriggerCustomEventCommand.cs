using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public class TriggerCustomEventCommand : CommonCommandBase, ICanBulk<ICommandBulk>
{
	private TriggerEvent _triggerEvent;

	public TriggerEvent TriggerEvent
	{
		[DebuggerStepThrough]
		get
		{
			return _triggerEvent;
		}
	}

	public ICommandBulk BulkCommand => new TriggerCustomEventBulkCommand();

	public TriggerCustomEventCommand(TriggerEvent triggerEvent)
	{
		_triggerEvent = triggerEvent;
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
