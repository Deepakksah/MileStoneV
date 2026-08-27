using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class GetHardwareNetworkSettingsResponse : CommonResponseBase
{
	[DataMember]
	public Guid HardwareId { get; set; }

	[DataMember]
	public string HardwareAddress { get; private set; }

	[DataMember]
	public int? HardwarePort { get; private set; }

	[DataMember]
	public string DefaultGateway { get; private set; }

	[DataMember]
	public string SubnetMask { get; private set; }

	public GetHardwareNetworkSettingsResponse()
	{
	}

	public GetHardwareNetworkSettingsResponse(Guid hardwareId, string hardwareAddress, int? hardwarePort, string defaultGateway, string subnetMask)
	{
		HardwareId = hardwareId;
		HardwareAddress = hardwareAddress;
		HardwarePort = hardwarePort;
		DefaultGateway = defaultGateway;
		SubnetMask = subnetMask;
	}

	public override void Accept(ICommonResponseVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override Task AcceptAsync(ICommonResponseVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
