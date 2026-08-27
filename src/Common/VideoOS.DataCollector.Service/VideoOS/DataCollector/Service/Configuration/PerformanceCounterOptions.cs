using System.ComponentModel.DataAnnotations;

namespace VideoOS.DataCollector.Service.Configuration;

public class PerformanceCounterOptions
{
	public const string PerformanceCounter = "PerformanceCounterSettings";

	[Range(1, int.MaxValue, ErrorMessage = "The value should be greater than 1")]
	public int LiveCounterInactivityTimeoutInSeconds { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "The value should be greater than 1")]
	public int MaxQueueSize { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "The value should be greater than 1")]
	public int BatchSize { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "The value should be greater than 1")]
	public int ConfigurationRefreshIntervalInSec { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "The value should be greater than 1")]
	public int BatchSenderTimeInSeconds { get; set; }
}
