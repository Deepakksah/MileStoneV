using MediatR;
using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Common.Models;
using VideoOS.DataCollector.Service.Common.ResultResponse;

namespace VideoOS.DataCollector.Service.Rule.RuleConfiguration.Queries.GetConfiguration;

public record GetRuleConfigurationServerQuery(int DataCollectorId) : SendRequestBase<Result<Unit>>((RetrySettings?)null, (LogSettings?)null);
