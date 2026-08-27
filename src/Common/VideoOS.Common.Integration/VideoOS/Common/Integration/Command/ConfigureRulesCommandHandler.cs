using System;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Command;

public class ConfigureRulesCommandHandler : ICommandHandler
{
	private IRuleEngine _ruleEngine;

	public ConfigureRulesCommandHandler(IRuleEngine ruleEngine)
	{
		_ruleEngine = ruleEngine;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is ConfigureRulesCommand configureRulesCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		_ruleEngine.ConfigureRules(configureRulesCommand.RulesInfo);
		responseSink.AddResponse(new SuccessResponse());
	}
}
