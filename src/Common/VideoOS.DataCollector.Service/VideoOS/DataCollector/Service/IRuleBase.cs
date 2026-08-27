namespace VideoOS.DataCollector.Service;

public interface IRuleBase
{
	string RuleId { get; set; }

	string CounterPath { get; set; }
}
