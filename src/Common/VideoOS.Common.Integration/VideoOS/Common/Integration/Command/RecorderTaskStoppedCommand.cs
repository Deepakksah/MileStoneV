using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class RecorderTaskStoppedCommand : CommonCommandBase
{
	public long TaskId { get; set; }

	public RecorderTaskStoppedCommand(long taskId)
	{
		TaskId = taskId;
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
