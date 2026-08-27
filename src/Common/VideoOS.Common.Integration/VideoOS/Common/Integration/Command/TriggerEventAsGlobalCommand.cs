using System;
using System.Threading.Tasks;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public class TriggerEventAsGlobalCommand : CommonCommandBase
{
	public Guid SourceId { get; set; }

	public TriggerEvent TriggerEvent { get; private set; }

	public TriggerEventAsGlobalCommand(Guid sourceId, TriggerEvent triggerEvent)
	{
		SourceId = sourceId;
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
