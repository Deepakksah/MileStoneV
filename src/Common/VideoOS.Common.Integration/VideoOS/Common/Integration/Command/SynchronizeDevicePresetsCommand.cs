using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class SynchronizeDevicePresetsCommand : CommonCommandBase
{
	[DataMember]
	public Guid RecorderId { get; private set; }

	[DataMember]
	public Guid DeviceId { get; private set; }

	public SynchronizeDevicePresetsCommand(Guid recorderId, Guid deviceId)
	{
		RecorderId = recorderId;
		DeviceId = deviceId;
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
