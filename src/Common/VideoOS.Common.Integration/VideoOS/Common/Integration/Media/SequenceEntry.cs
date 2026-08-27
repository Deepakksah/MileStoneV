using System;
using System.Globalization;

namespace VideoOS.Common.Integration.Media;

public class SequenceEntry
{
	public DateTime StartTime { get; set; }

	public DateTime TriggerTime { get; set; }

	public DateTime EndTime { get; set; }

	public SequenceEntry(DateTime startTime, DateTime endTime)
	{
		StartTime = startTime;
		TriggerTime = startTime;
		EndTime = endTime;
	}

	public override string ToString()
	{
		return StartTime.ToString("dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture) + " => " + EndTime.ToString("dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
	}
}
