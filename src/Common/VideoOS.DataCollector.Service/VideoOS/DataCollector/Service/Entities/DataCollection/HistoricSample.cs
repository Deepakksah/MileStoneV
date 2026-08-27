using System;

namespace VideoOS.DataCollector.Service.Entities.DataCollection;

public record HistoricSample(DateTime Time, float Value);
