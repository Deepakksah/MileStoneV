namespace VideoOS.Rules.Interfaces;

public interface IRuleConfigurationProcessing : IRuleConfiguration
{
	void ProcessConfiguration(IRuleConfiguration ruleConfiguration);
}
