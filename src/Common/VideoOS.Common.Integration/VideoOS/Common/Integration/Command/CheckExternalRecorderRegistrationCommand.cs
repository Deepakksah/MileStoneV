using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class CheckExternalRecorderRegistrationCommand : CommonCommandBase
{
	[DataMember]
	public Guid HardwareId { get; private set; }

	[DataMember]
	public string Address { get; private set; }

	public CheckExternalRecorderRegistrationCommand(Guid hardwareId, string address)
	{
		HardwareId = hardwareId;
		Address = address;
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
