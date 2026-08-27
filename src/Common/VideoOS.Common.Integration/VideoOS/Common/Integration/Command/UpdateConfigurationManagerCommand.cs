using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class UpdateConfigurationManagerCommand : CommonCommandBase, ICommandWithSource
{
	[DataMember]
	public bool PersistChanges { get; set; }

	[DataMember]
	public bool SendHeartbeat { get; set; }

	[DataMember]
	public ConfigurationManagerDataTransferObject ConfigurationManagerDto { get; set; }

	Guid ICommandWithSource.SourceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid("4988FA65-5520-4559-9295-87EBB932D033");
		}
	}

	public UpdateConfigurationManagerCommand(ConfigurationManagerDataTransferObject configurationManagerDto)
	{
		ConfigurationManagerDto = configurationManagerDto;
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
