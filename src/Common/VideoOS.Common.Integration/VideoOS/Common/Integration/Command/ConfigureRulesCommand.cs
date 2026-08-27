using System;
using System.Threading.Tasks;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Command;

public class ConfigureRulesCommand : CommonCommandBase
{
	private IRuleConfiguration _rulesInfo;

	public IRuleConfiguration RulesInfo => _rulesInfo;

	public ConfigureRulesCommand(IRuleConfiguration rulesInfo)
	{
		_rulesInfo = rulesInfo;
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
