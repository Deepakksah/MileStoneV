using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public class UpdateSecurityConfigurationCommand : CommonCommandBase, ICommandWithSource
{
	private SecurityConfigurationDto securityConfigurationDto;

	public SecurityConfigurationDto SecurityConfigurationDto
	{
		[DebuggerStepThrough]
		get
		{
			return securityConfigurationDto;
		}
		[DebuggerStepThrough]
		set
		{
			securityConfigurationDto = value;
		}
	}

	Guid ICommandWithSource.SourceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid("C9F7ED1D-ABF4-40CB-A874-331FBB6DCE8C");
		}
	}

	public UpdateSecurityConfigurationCommand(SecurityConfigurationDto securityConfigurationDto)
	{
		this.securityConfigurationDto = securityConfigurationDto;
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
