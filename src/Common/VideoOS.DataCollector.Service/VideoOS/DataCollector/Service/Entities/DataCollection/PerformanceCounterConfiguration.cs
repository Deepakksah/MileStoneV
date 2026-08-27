namespace VideoOS.DataCollector.Service.Entities.DataCollection;

public record PerformanceCounterConfiguration(PerformanceCounterInstance CounterInstance, int SampleFrequency);
