using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class SetupInitialHardwareUserCommand : CommonCommandBase
{
	[DataMember]
	public Guid RecorderId { get; private set; }

	[DataMember]
	public Uri HardwareUri { get; private set; }

	[DataMember]
	public int DriverNumber { get; private set; }

	[DataMember]
	public HardwareUser HardwareUser { get; private set; }

	[DataMember]
	public int? SecurePort { get; private set; }

	public SetupInitialHardwareUserCommand(Guid recorderId, Uri hardwareUri, int driverNumber, HardwareUser hardwareUser, int? httpsPort)
	{
		RecorderId = recorderId;
		HardwareUri = hardwareUri;
		DriverNumber = driverNumber;
		HardwareUser = hardwareUser;
		SecurePort = httpsPort;
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
