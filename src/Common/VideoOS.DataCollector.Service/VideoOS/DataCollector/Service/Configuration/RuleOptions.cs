using System.ComponentModel.DataAnnotations;

namespace VideoOS.DataCollector.Service.Configuration;

public class RuleOptions
{
	public const string Rule = "RuleSettings";

	[Range(1, int.MaxValue, ErrorMessage = "The value should be greater than 1")]
	public int CounterStateSampleCount { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "The value should be greater than 1")]
	public int StateChangeLatency { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "The value should be greater than 1")]
	public int CounterStateMinSampleIntervalMS { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "The value should be greater than 1")]
	public int ConfigurationRefreshIntervalInSec { get; set; }
}
