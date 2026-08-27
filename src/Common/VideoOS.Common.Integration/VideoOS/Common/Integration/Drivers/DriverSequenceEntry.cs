using System;
using System.Globalization;

namespace VideoOS.Common.Integration.Drivers;

public class DriverSequenceEntry
{
	public DateTime StartTime { get; private set; }

	public DateTime EndTime { get; private set; }

	public DriverSequenceEntry(DateTime startTime, DateTime endTime)
	{
		StartTime = startTime;
		EndTime = endTime;
	}

	public override string ToString()
	{
		return StartTime.ToString("dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture) + " => " + EndTime.ToString("dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
	}
}
