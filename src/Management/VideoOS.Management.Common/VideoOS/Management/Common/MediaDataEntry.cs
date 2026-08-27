using System;

namespace VideoOS.Management.Common;

[Serializable]
public class MediaDataEntry
{
	public DateTime StartTime { get; set; }

	public DateTime EndTime { get; set; }

	public MediaDataEntry(DateTime startTime, DateTime endTime)
	{
		StartTime = startTime;
		EndTime = endTime;
	}
}
