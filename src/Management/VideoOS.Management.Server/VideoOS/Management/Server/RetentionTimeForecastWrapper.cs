using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public class RetentionTimeForecastWrapper : IRetentionTimeForecastWrapper
{
	public TimeSpan CalculateRetentionTimeLimitHit(RetentionTimeHistoricalData[] historicalData, long storageSize, TimeSpan forecastPeriod)
	{
		return RetentionTimeForecast.CalculateRetentionTimeLimitHit(historicalData, storageSize, forecastPeriod);
	}

	public IEnumerable<Point> CalculateForecastLinear(RetentionTimeHistoricalData[] historicalData, TimeSpan forecastPeriod)
	{
		return RetentionTimeForecast.CalculateForecastLinear(historicalData, forecastPeriod);
	}

	public int GetForecastSampleCount(RetentionTimeHistoricalData[] historicalData, TimeSpan forecastPeriod)
	{
		return RetentionTimeForecast.GetForecastSampleCount(historicalData, forecastPeriod);
	}

	public IEnumerable<Point> GeneratingRobustData(RetentionTimeHistoricalData[] historicalData)
	{
		return RetentionTimeForecast.GeneratingRobustData(historicalData);
	}

	public double Interpolate(RetentionTimeHistoricalData current, RetentionTimeHistoricalData previousValue, RetentionTimeHistoricalData nextValue)
	{
		return RetentionTimeForecast.Interpolate(current, previousValue, nextValue);
	}

	public RetentionTimeHistoricalData FindNextNonzero(RetentionTimeHistoricalData[] historicalData, int from)
	{
		return RetentionTimeForecast.FindNextNonzero(historicalData, from);
	}

	public LinearParameters LinearRegression(IEnumerable<Point> points)
	{
		return RetentionTimeForecast.LinearRegression(points);
	}

	public double CalculateStandardError(LinearParameters linearEstimate, IEnumerable<Point> historicalData)
	{
		return RetentionTimeForecast.CalculateStandardError(linearEstimate, historicalData);
	}
}
