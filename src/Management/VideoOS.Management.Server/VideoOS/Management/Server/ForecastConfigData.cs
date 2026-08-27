using System;

namespace VideoOS.Management.Server;

public class ForecastConfigData
{
	public TimeSpan ForecastPeriod { get; private set; }

	public TimeSpan CriticalDays { get; private set; }

	public TimeSpan WarningDays { get; private set; }

	public TimeSpan HistoricalDataPeriod { get; private set; }

	public int HistoricalSampleCount { get; private set; }

	public ForecastConfigData(TimeSpan forecastPeriod, TimeSpan criticalDays, TimeSpan warningDays, TimeSpan historicalDataPeriod, int historicalSampleCount)
	{
		ForecastPeriod = forecastPeriod;
		CriticalDays = criticalDays;
		WarningDays = warningDays;
		HistoricalDataPeriod = historicalDataPeriod;
		HistoricalSampleCount = historicalSampleCount;
	}
}
