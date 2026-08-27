using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class ChangeHardwarePasswordCommand : CommonCommandBase
{
	[DataMember]
	public Guid HardwareId { get; private set; }

	[DataMember]
	public string TargetUserName { get; private set; }

	[DataMember]
	public string NewPassword { get; private set; }

	public ChangeHardwarePasswordCommand(Guid hardwareId, string newPassword, string targetUserName)
	{
		HardwareId = hardwareId;
		TargetUserName = targetUserName;
		NewPassword = newPassword;
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
