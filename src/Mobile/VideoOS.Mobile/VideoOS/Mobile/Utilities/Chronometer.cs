using System;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Utilities;

public class Chronometer
{
	private DateTime _startTime;

	public void Reset()
	{
		_startTime = DateTime.UtcNow;
	}

	public void MeasureAndPrintTime(string origin, string timeDescription)
	{
		Log.Info(origin, $"{timeDescription}: {DateTime.UtcNow.Subtract(_startTime).ToString()}");
	}
}
