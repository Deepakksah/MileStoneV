using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class RecorderTaskFailedCommand : CommonCommandBase
{
	public long TaskId { get; set; }

	public string Description { get; set; }

	public bool IsFatal { get; set; }

	public string ExceptionMessage { get; set; }

	public RecorderTaskFailedCommand(long taskId, string description, bool isFatal, string exceptionMessage)
	{
		TaskId = taskId;
		Description = description;
		IsFatal = isFatal;
		ExceptionMessage = exceptionMessage;
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
