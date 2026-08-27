using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class SetHardwareNetworkSettingsCommand : CommonCommandBase
{
	[DataMember]
	public Guid HardwareId { get; private set; }

	[DataMember]
	public string HardwareAddress { get; private set; }

	[DataMember]
	public int? HardwarePort { get; private set; }

	[DataMember]
	public string DefaultGateway { get; private set; }

	[DataMember]
	public string SubnetMask { get; private set; }

	public SetHardwareNetworkSettingsCommand(Guid hardwareId, string hardwareAddress, int? hardwarePort, string defaultGateway, string subnetMask)
	{
		HardwareId = hardwareId;
		HardwareAddress = hardwareAddress;
		HardwarePort = hardwarePort;
		DefaultGateway = defaultGateway;
		SubnetMask = subnetMask;
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
