using MediatR;
using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Common.Models;
using VideoOS.DataCollector.Service.Common.ResultResponse;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.EventHandlers.RefreshActivePerformanceCounters;

public record MarkPerformanceCounterDataAsObsoleteServerCommand(string[] ObsoleteCounterData) : SendRequestBase<Result<Unit>>(new RetrySettings(int.MaxValue, 60000, 1000), (LogSettings?)null);
