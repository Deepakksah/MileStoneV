using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class CheckMsFailoverStatusCommand : CommonCommandBase
{
	[DataMember]
	public bool ShouldInvokeNodeStartEvent { get; private set; }

	public CheckMsFailoverStatusCommand(bool shouldInvokeNodeStartEvent)
	{
		ShouldInvokeNodeStartEvent = shouldInvokeNodeStartEvent;
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
