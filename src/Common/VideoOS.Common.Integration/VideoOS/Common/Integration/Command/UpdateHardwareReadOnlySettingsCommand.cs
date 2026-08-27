using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public sealed class UpdateHardwareReadOnlySettingsCommand : CommonCommandBase
{
	public Guid HardwareId { get; private set; }

	public string UpdatedSettingsXml { get; private set; }

	public UpdateHardwareReadOnlySettingsCommand(Guid hardwareId, string updatedSettingsXml)
	{
		HardwareId = hardwareId;
		UpdatedSettingsXml = updatedSettingsXml;
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
