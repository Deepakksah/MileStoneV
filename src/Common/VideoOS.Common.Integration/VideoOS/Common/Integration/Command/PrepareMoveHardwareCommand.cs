using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class PrepareMoveHardwareCommand : CommonCommandBase
{
	public Guid HardwareId { get; private set; }

	public int TimeoutSeconds { get; private set; }

	public PrepareMoveHardwareCommand(Guid hardwareId, int timeoutSeconds)
	{
		HardwareId = hardwareId;
		TimeoutSeconds = timeoutSeconds;
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
