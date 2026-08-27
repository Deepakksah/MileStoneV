using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class UpdateRecorderTaskProgressCommand : CommonCommandBase
{
	public long TaskId { get; set; }

	public int PercentComplete { get; set; }

	public string Details { get; set; }

	public int Status { get; set; }

	public DateTime EndTime { get; set; }

	public UpdateRecorderTaskProgressCommand(long taskId, int percentComplete, string details, int status, DateTime endTime)
	{
		TaskId = taskId;
		PercentComplete = percentComplete;
		Details = details;
		Status = status;
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
