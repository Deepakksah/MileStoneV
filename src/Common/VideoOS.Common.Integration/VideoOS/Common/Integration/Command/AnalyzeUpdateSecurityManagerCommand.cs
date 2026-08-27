using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Common.Integration.Command;

[DataContract]
public class AnalyzeUpdateSecurityManagerCommand : CommonCommandBase
{
	[DataMember]
	public bool DoAuditLog { get; set; }

	[DataMember]
	public SecurityManagerDataTransferObject SecurityManagerDto { get; set; }

	public AnalyzeUpdateSecurityManagerCommand(SecurityManagerDataTransferObject securityManagerDto, bool doAuditLog)
	{
		SecurityManagerDto = securityManagerDto;
		DoAuditLog = doAuditLog;
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
