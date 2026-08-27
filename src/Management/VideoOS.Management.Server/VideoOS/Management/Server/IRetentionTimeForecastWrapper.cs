using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IRetentionTimeForecastWrapper
{
	TimeSpan CalculateRetentionTimeLimitHit(RetentionTimeHistoricalData[] historicalData, long storageSize, TimeSpan forecastPeriod);

	IEnumerable<Point> CalculateForecastLinear(RetentionTimeHistoricalData[] historicalData, TimeSpan forecastPeriod);

	int GetForecastSampleCount(RetentionTimeHistoricalData[] historicalData, TimeSpan forecastPeriod);

	IEnumerable<Point> GeneratingRobustData(RetentionTimeHistoricalData[] historicalData);

	double Interpolate(RetentionTimeHistoricalData current, RetentionTimeHistoricalData previousValue, RetentionTimeHistoricalData nextValue);

	RetentionTimeHistoricalData FindNextNonzero(RetentionTimeHistoricalData[] historicalData, int from);

	LinearParameters LinearRegression(IEnumerable<Point> points);

	double CalculateStandardError(LinearParameters linearEstimate, IEnumerable<Point> historicalData);
}
