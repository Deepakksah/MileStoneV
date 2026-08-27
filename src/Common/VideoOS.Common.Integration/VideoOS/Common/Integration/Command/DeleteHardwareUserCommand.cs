using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class DeleteHardwareUserCommand : CommonCommandBase
{
	[DataMember]
	public Guid HardwareId { get; private set; }

	[DataMember]
	public string UserName { get; private set; }

	public DeleteHardwareUserCommand(Guid hardwareId, string userName)
	{
		HardwareId = hardwareId;
		UserName = userName;
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
