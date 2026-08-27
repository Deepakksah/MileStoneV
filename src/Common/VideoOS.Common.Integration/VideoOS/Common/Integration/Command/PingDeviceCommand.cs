using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class PingDeviceCommand : CommonCommandBase
{
	[DataMember]
	public string DeviceAddress { get; private set; }

	[DataMember]
	public Guid RecorderId { get; private set; }

	[DataMember]
	public TimeSpan Timeout { get; private set; }

	public PingDeviceCommand(string deviceAddress, Guid recorderId, TimeSpan timeout)
	{
		DeviceAddress = deviceAddress;
		RecorderId = recorderId;
		Timeout = timeout;
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
