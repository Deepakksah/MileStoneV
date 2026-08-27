using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public sealed class UpdateHardwareDriverCommand : CommonCommandBase
{
	private SystemConfigurationDto systemConfigurationDto;

	public SystemConfigurationDto SystemConfigurationDto
	{
		[DebuggerStepThrough]
		get
		{
			return systemConfigurationDto;
		}
		[DebuggerStepThrough]
		set
		{
			systemConfigurationDto = value;
		}
	}

	public UpdateHardwareDriverCommand(SystemConfigurationDto systemConfigurationDto)
	{
		this.systemConfigurationDto = systemConfigurationDto;
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
