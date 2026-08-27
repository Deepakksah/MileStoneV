using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class SetUserSecurityTokenCommand : CommonCommandBase, ICommandWithSource
{
	private SecurityTokenCollectionDto _securityTokensDto;

	[DataMember]
	public SecurityTokenCollectionDto SecurityTokensDto
	{
		[DebuggerStepThrough]
		get
		{
			return _securityTokensDto;
		}
		[DebuggerStepThrough]
		set
		{
			_securityTokensDto = value;
		}
	}

	Guid ICommandWithSource.SourceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid("56CAC3AA-5407-4159-8099-1D9196D8D364");
		}
	}

	public SetUserSecurityTokenCommand(SecurityTokenCollectionDto securityTokensDto)
	{
		_securityTokensDto = securityTokensDto;
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
