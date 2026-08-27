namespace VideoOS.DataCollector.Service.Entities.DataCollection;

public record CounterStateRuleConfiguration(string CounterPath, float WarningThreshold, float CriticalThreshold, int TimeWindow, string CounterRuleId, string Name, string CategoryName, string CounterName);
