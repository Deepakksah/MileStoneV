namespace VideoOS.DataCollector.Service.Entities.DataCollection;

public record PerformanceCounterData(int DataCollectorId, PerformanceCounterValue[] Values);
