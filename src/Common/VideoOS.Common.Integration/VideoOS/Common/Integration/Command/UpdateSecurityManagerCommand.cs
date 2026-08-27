using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class UpdateSecurityManagerCommand : CommonCommandBase, ICommandWithSource
{
	[DataMember]
	public bool PersistChanges { get; set; }

	[DataMember]
	public SecurityManagerDataTransferObject SecurityManagerDto { get; set; }

	Guid ICommandWithSource.SourceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid("EB937612-44A6-49f0-ADE1-2573039D65F8");
		}
	}

	public UpdateSecurityManagerCommand(SecurityManagerDataTransferObject securityManagerDto)
	{
		SecurityManagerDto = securityManagerDto;
		PersistChanges = true;
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
