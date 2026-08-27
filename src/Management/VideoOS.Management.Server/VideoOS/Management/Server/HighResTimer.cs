using System.Diagnostics;

namespace VideoOS.Management.Server;

public class HighResTimer
{
	private long _startTime;

	private long _stopTime;

	private static double _defaultConversionFactor = 1000000.0;

	private double _conversionFactor = _defaultConversionFactor;

	public double ConversionFactor
	{
		get
		{
			return _conversionFactor;
		}
		set
		{
			_conversionFactor = value;
		}
	}

	public static long Timestamp => Stopwatch.GetTimestamp();

	public long Duration => DiffTime(_startTime, _stopTime, _conversionFactor);

	public void Start()
	{
		_startTime = Stopwatch.GetTimestamp();
	}

	public void Stop()
	{
		_stopTime = Stopwatch.GetTimestamp();
	}

	public void Reset()
	{
		_startTime = 0L;
		_stopTime = 0L;
		_conversionFactor = _defaultConversionFactor;
	}

	public static long ElapsedTime(long start, long stop)
	{
		return DiffTime(start, stop, _defaultConversionFactor);
	}

	private static long DiffTime(long start, long stop, double conversionFactor)
	{
		if (stop < start)
		{
			stop = start;
		}
		double num = (double)(stop - start) / (double)Stopwatch.Frequency;
		return (long)(conversionFactor * num);
	}
}
