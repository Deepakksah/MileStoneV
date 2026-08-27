using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class HardwareSetupInformationResponse : CommonResponseBase
{
	[DataMember]
	public HardwareSetupInformation HardwareSetupInformation { get; private set; }

	public HardwareSetupInformationResponse(HardwareSetupInformation hardwareSetupInformation)
	{
		HardwareSetupInformation = hardwareSetupInformation;
	}

	public override void Accept(ICommonResponseVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonResponseVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
