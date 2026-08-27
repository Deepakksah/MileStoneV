using System;
using System.Threading.Tasks;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public class ReplaceHardwareUpdateSystemConfigurationCommand : UpdateSystemConfigurationCommand
{
	public ReplaceHardwareUpdateSystemConfigurationCommand(SystemConfigurationDto systemConfigurationDto)
		: base(systemConfigurationDto)
	{
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
