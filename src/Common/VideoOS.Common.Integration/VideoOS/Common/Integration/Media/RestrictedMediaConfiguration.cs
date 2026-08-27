using System;

namespace VideoOS.Common.Integration.Media;

public class RestrictedMediaConfiguration
{
	public DateTime StartTime { get; set; }

	public DateTime? EndTime { get; set; }

	public RestrictedMediaConfiguration(DateTime startTime, DateTime? endTime)
	{
		StartTime = startTime;
		EndTime = endTime;
	}
}
