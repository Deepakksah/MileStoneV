using MediatR;
using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Common.Models;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service.Rule.RuleConfiguration.Commands;

public record DeleteStateServerCommand(DeleteStateChangeData StateChange) : SendRequestBase<Result<Unit>>((RetrySettings?)null, (LogSettings?)null);
