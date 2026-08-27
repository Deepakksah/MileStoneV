using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class FirmwareUpgradeSuccessResponse : CommonResponseBase
{
	[DataMember]
	public string NewFirmwareVersion { get; set; }

	public FirmwareUpgradeSuccessResponse(string newFirmwareVersion)
	{
		NewFirmwareVersion = newFirmwareVersion;
	}

	public override string ToString()
	{
		return GetType().Name;
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
