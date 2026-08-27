using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class EdgeJobUpdatedCommand : CommonCommandBase
{
	public Guid DeviceId { get; private set; }

	public DateTime StartTime { get; private set; }

	public DateTime EndTime { get; private set; }

	public EdgeJobUpdatedCommand(Guid deviceId, DateTime startTime, DateTime endTime)
	{
		DeviceId = deviceId;
		StartTime = startTime;
		EndTime = endTime;
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
