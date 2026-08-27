using System;
using System.Globalization;

namespace VideoOS.Rules.Interfaces;

public class DateTimeRange
{
	public DateTime StartTime { get; set; }

	public DateTime EndTime { get; set; }

	public override string ToString()
	{
		return StartTime.ToString("t", CultureInfo.InvariantCulture) + "," + EndTime.ToString("t", CultureInfo.InvariantCulture);
	}
}
