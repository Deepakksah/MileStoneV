using System.Collections.Generic;
using MediatR;
using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Common.Models;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.Sample.EventHandlers.Historical;

public record SendBatchToServerServerCommand(int DataCollectorId, IEnumerable<PerformanceCounterValue> CounterValues) : SendRequestBase<Result<Unit>>(new RetrySettings(int.MaxValue, 60000, 1000), (LogSettings?)null);
