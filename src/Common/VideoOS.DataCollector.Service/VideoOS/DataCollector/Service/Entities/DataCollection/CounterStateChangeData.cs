namespace VideoOS.DataCollector.Service.Entities.DataCollection;

public record CounterStateChangeData(int DataCollectorId, string CounterRuleId, string CounterPath, int State, double TriggerValue);
