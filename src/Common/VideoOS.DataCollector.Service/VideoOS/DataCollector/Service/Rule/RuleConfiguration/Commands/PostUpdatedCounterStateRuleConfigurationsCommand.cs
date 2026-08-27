using MediatR;
using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Common.Models;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.Rule.RuleConfiguration.Commands;

public record PostUpdatedCounterStateRuleConfigurationsCommand(CounterStateRulesConfigurationUpdate[] RulesConfigurationUpdates) : ReceiveRequestBase<Unit>((LogSettings?)null);
