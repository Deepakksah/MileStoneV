using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class ReportMediaCommittedToDatabaseCommand : CommonCommandBase, ICommandWithSource
{
	public Guid DeviceId { get; private set; }

	public DateTime StartTime { get; private set; }

	public DateTime EndTime { get; private set; }

	public Guid SourceId
	{
		[DebuggerStepThrough]
		get
		{
			return DeviceId;
		}
	}

	public ReportMediaCommittedToDatabaseCommand(Guid deviceId, DateTime startTime, DateTime endTime)
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
