using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class UpdateSystemConfigurationCommand : CommonCommandBase, ICommandWithSource
{
	[DataMember]
	public SystemConfigurationDto SystemConfigurationDto { get; set; }

	Guid ICommandWithSource.SourceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid("A6C7242A-AB2B-4FC7-8F78-D465FFD7BFC5");
		}
	}

	public UpdateSystemConfigurationCommand(SystemConfigurationDto systemConfigurationDto)
	{
		SystemConfigurationDto = systemConfigurationDto;
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
