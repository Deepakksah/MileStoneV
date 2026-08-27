using System;
using System.Threading.Tasks;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public class UpdateUserSecurityTokensCommand : CommonCommandBase
{
	private SecurityTokenCollectionDto2 _securityTokensDto;

	public SecurityTokenCollectionDto2 SecurityTokensDto
	{
		get
		{
			return _securityTokensDto;
		}
		set
		{
			_securityTokensDto = value;
		}
	}

	public UpdateUserSecurityTokensCommand(SecurityTokenCollectionDto2 securityTokensDto)
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
