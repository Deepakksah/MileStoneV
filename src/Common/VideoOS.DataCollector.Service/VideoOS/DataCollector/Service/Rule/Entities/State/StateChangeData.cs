namespace VideoOS.DataCollector.Service.Rule.Entities.State;

public record StateChangeData(int DataCollectorId, string CounterRuleId, string CounterPath, int State, double TriggerValue);
