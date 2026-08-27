using System.Collections.Generic;
using MediatR;
using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Common.Models;
using VideoOS.DataCollector.Service.Common.ResultResponse;

namespace VideoOS.DataCollector.Service.Rule.RuleConfiguration.Commands;

public record DeleteNonActiveStatesServerCommand(int DataCollectorId, IEnumerable<string> ActiveRules) : SendRequestBase<Result<Unit>>((RetrySettings?)null, (LogSettings?)null);
