using MediatR;
using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Common.Models;
using VideoOS.DataCollector.Service.Common.ResultResponse;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.ReportGpuCounterInstances;

public record ReportGpuCounterInstancesServerCommand(int DataCollectorId) : SendRequestBase<Result<Unit>>(new RetrySettings(int.MaxValue, 60000, 1000), (LogSettings?)null);
