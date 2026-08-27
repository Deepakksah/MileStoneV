using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public class UpdateRecorderStatusCommand : CommonCommandBase
{
	private RecorderStatuses _recorderStatus;

	public Guid ServiceId { get; private set; }

	public RecorderStatuses Status
	{
		[DebuggerStepThrough]
		get
		{
			return _recorderStatus;
		}
	}

	public UpdateRecorderStatusCommand(Guid serviceId, RecorderStatuses recorderStatus)
	{
		ServiceId = serviceId;
		_recorderStatus = recorderStatus;
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
