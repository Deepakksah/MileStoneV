using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class HardwareSetupInformationCommand : CommonCommandBase
{
	[DataMember]
	public Guid RecorderId { get; private set; }

	[DataMember]
	public Uri Uri { get; private set; }

	[DataMember]
	public string UserName { get; private set; }

	[DataMember]
	public string Password { get; set; }

	[DataMember]
	public string CustomDriverData { get; private set; }

	[DataMember]
	public int DriverNumber { get; private set; }

	[DataMember]
	public string AcceptMacAddress { get; private set; }

	[DataMember]
	public bool AacSupported { get; set; }

	[DataMember]
	public int? HttpsPort { get; set; }

	public HardwareSetupInformationCommand(Guid recorderId, Uri uri, string userName, string password, string customDriverData, int driverNumber, string acceptMacAddress)
		: this(recorderId, uri, userName, password, customDriverData, driverNumber, acceptMacAddress, aacSupported: false, null)
	{
	}

	public HardwareSetupInformationCommand(Guid recorderId, Uri uri, string userName, string password, string customDriverData, int driverNumber, string acceptMacAddress, bool aacSupported)
		: this(recorderId, uri, userName, password, customDriverData, driverNumber, acceptMacAddress, aacSupported, null)
	{
	}

	public HardwareSetupInformationCommand(Guid recorderId, Uri uri, string userName, string password, string customDriverData, int driverNumber, string acceptMacAddress, bool aacSupported, int? httpsPort)
	{
		RecorderId = recorderId;
		Uri = uri;
		UserName = userName;
		Password = password;
		CustomDriverData = customDriverData;
		DriverNumber = driverNumber;
		AcceptMacAddress = acceptMacAddress;
		AacSupported = aacSupported;
		HttpsPort = httpsPort;
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
