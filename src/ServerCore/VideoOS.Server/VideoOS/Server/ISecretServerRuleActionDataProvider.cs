using VideoOS.Rules.Interfaces;

namespace VideoOS.Server;

public interface ISecretServerRuleActionDataProvider
{
	Server Server { get; }

	IRuleEngine RuleEngine { get; }
}
