using System;

namespace VideoOS.DataCollector.Service.Entities.DataCollection;

public record HistoricSampleRequestData(int DataCollectorId, string CounterPath, DateTime FromTime, int MaxSampleCount);
