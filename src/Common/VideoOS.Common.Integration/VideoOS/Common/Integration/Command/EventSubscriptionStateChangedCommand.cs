using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class EventSubscriptionStateChangedCommand : CommonCommandBase
{
	[DataMember]
	public bool EventSubscriptionState { get; private set; }

	[DataMember]
	public Guid ServiceId { get; private set; }

	public EventSubscriptionStateChangedCommand(bool eventSubscriptionState, Guid serviceId)
	{
		EventSubscriptionState = eventSubscriptionState;
		ServiceId = serviceId;
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
