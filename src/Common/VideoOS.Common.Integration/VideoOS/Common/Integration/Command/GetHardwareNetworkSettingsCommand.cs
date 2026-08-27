using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class GetHardwareNetworkSettingsCommand : CommonCommandBase
{
	[DataMember]
	public Guid HardwareId { get; private set; }

	[DataMember]
	public int RescheduledTimes { get; set; }

	public GetHardwareNetworkSettingsCommand(Guid hardwareId)
	{
		HardwareId = hardwareId;
		RescheduledTimes = 0;
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
