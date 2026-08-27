using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class ReportManualRecordingStateCommand : CommonCommandBase, ICommandWithSource
{
	public Guid DeviceId { get; private set; }

	public bool IsManualRecording { get; private set; }

	public Guid SourceId
	{
		[DebuggerStepThrough]
		get
		{
			return DeviceId;
		}
	}

	public ReportManualRecordingStateCommand(Guid deviceId, bool isManualRecording)
	{
		DeviceId = deviceId;
		IsManualRecording = isManualRecording;
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
