using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Common.Models;
using VideoOS.DataCollector.Service.Common.ResultResponse;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;

public record RegisterDataCollectorServerCommand(string HostName = "", int DataCollectorVersion = 0, string ApplicationId = "", string ClusterAddress = "", string ClusterApplicationId = "") : SendRequestBase<Result<int>>(new RetrySettings(int.MaxValue, 60000, 1000), (LogSettings?)null);
