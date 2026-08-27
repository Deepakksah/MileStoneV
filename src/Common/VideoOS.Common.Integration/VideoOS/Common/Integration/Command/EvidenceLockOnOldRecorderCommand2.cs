using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class EvidenceLockOnOldRecorderCommand2 : CommonCommandBase
{
	public Version ServerVersion { get; set; }

	public EvidenceLockOperation Operation { get; set; }

	public Guid IDMarkedData { get; set; }

	public Guid IDDevice { get; set; }

	public Guid IDTrack { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime EndTime { get; set; }

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
