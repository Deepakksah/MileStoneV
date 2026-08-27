namespace VideoOS.DataCollector.Service.Common.Models;

public record RetrySettings(int MaxRetryAttempts = 1, int MaxRetryDelayMilliseconds = 0, int RetryDelayMilliseconds = 0);
