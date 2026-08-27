using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class RecorderTaskSuccessCommand : CommonCommandBase
{
	public long TaskId { get; set; }

	public string Result { get; set; }

	public RecorderTaskSuccessCommand(long taskId, string result)
	{
		TaskId = taskId;
		Result = result;
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
