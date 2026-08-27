using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Management.Server;

public static class RetentionTimeForecast
{
	public static TimeSpan CalculateRetentionTimeLimitHit(RetentionTimeHistoricalData[] historicalData, long storageSize, TimeSpan forecastPeriod)
	{
		IEnumerable<Point> enumerable = CalculateForecastLinear(historicalData, forecastPeriod);
		if (enumerable != null)
		{
			TimeSpan timeSpan = historicalData[1].Time - historicalData[0].Time;
			TimeSpan zero = TimeSpan.Zero;
			foreach (Point item in enumerable)
			{
				zero += timeSpan;
				if (item.Y >= (double)storageSize)
				{
					return zero;
				}
			}
			return TimeSpan.MaxValue;
		}
		GeminiTrace.Info("Retention Time Rule: Insufficient data.");
		return TimeSpan.MaxValue;
	}

	public static IEnumerable<Point> CalculateForecastLinear(RetentionTimeHistoricalData[] historicalData, TimeSpan forecastPeriod)
	{
		if (historicalData.Length > 2)
		{
			IEnumerable<Point> enumerable = GeneratingRobustData(historicalData);
			if (enumerable != null)
			{
				LinearParameters fit = LinearRegression(enumerable);
				Func<double, double> func = (double x) => x * fit.A + fit.B;
				double num = CalculateStandardError(fit, enumerable);
				int forecastSampleCount = GetForecastSampleCount(historicalData, forecastPeriod);
				List<Point> list = new List<Point>();
				for (int num2 = 0; num2 < forecastSampleCount + historicalData.Length; num2++)
				{
					list.Add(new Point(num2, func(num2) + num));
				}
				return list.Skip(historicalData.Length);
			}
		}
		return null;
	}

	public static int GetForecastSampleCount(RetentionTimeHistoricalData[] historicalData, TimeSpan forecastPeriod)
	{
		TimeSpan timeSpan = historicalData[1].Time - historicalData[0].Time;
		if (timeSpan.TotalMilliseconds == 0.0)
		{
			throw new ArgumentException("historical data from same point in time", "historicalData");
		}
		return (int)Math.Ceiling(forecastPeriod.TotalMilliseconds / timeSpan.TotalMilliseconds);
	}

	public static IEnumerable<Point> GeneratingRobustData(RetentionTimeHistoricalData[] historicalData)
	{
		RetentionTimeHistoricalData retentionTimeHistoricalData = null;
		RetentionTimeHistoricalData retentionTimeHistoricalData2 = null;
		bool flag = false;
		List<Point> list = new List<Point>();
		for (int i = 0; i < historicalData.Length; i++)
		{
			if (historicalData[i].Value == 0.0)
			{
				if (retentionTimeHistoricalData2 == null && !flag)
				{
					retentionTimeHistoricalData2 = FindNextNonzero(historicalData, i);
				}
				if (retentionTimeHistoricalData2 != null)
				{
					if (retentionTimeHistoricalData != null)
					{
						list.Add(new Point(i, Interpolate(historicalData[i], retentionTimeHistoricalData, retentionTimeHistoricalData2)));
					}
					else
					{
						list.Add(new Point(i, retentionTimeHistoricalData2.Value));
					}
					continue;
				}
				flag = true;
				if (retentionTimeHistoricalData == null)
				{
					return null;
				}
				list.Add(new Point(i, retentionTimeHistoricalData.Value));
			}
			else
			{
				list.Add(new Point(i, historicalData[i].Value));
				retentionTimeHistoricalData = historicalData[i];
				retentionTimeHistoricalData2 = null;
			}
		}
		return list;
	}

	public static double Interpolate(RetentionTimeHistoricalData current, RetentionTimeHistoricalData previousValue, RetentionTimeHistoricalData nextValue)
	{
		double value = previousValue.Value;
		double value2 = nextValue.Value;
		double num = previousValue.Time.Ticks;
		double num2 = nextValue.Time.Ticks;
		double num3 = current.Time.Ticks;
		if (num == num2)
		{
			return value + (value2 - value) / 2.0;
		}
		return value + (value2 - value) * ((num3 - num) / (num2 - num));
	}

	public static RetentionTimeHistoricalData FindNextNonzero(RetentionTimeHistoricalData[] historicalData, int from)
	{
		for (int i = from + 1; i < historicalData.Length; i++)
		{
			if (historicalData[i].Value != 0.0)
			{
				return historicalData[i];
			}
		}
		return null;
	}

	public static LinearParameters LinearRegression(IEnumerable<Point> points)
	{
		Point[] array = points.ToArray();
		double num = 0.0;
		double num2 = 0.0;
		for (int i = 0; i < array.Length; i++)
		{
			num += array[i].X;
			num2 += array[i].Y;
		}
		num /= (double)array.Length;
		num2 /= (double)array.Length;
		double num3 = 0.0;
		double num4 = 0.0;
		for (int j = 0; j < array.Length; j++)
		{
			num3 += (array[j].X - num) * (array[j].Y - num2);
			num4 += Math.Pow(array[j].X - num, 2.0);
		}
		double num5 = num3 / num4;
		double b = num2 - num5 * num;
		return new LinearParameters(num5, b);
	}

	public static double CalculateStandardError(LinearParameters linearEstimate, IEnumerable<Point> historicalData)
	{
		Func<double, double> func = (double x) => x * linearEstimate.A + linearEstimate.B;
		double num = 0.0;
		int num2 = 0;
		foreach (Point historicalDatum in historicalData)
		{
			num += Math.Pow(historicalDatum.Y - func(num2), 2.0);
			num2++;
		}
		return Math.Sqrt(num / (double)historicalData.Count());
	}
}
