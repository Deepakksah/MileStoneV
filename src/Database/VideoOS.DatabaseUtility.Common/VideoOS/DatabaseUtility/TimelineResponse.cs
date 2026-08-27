using System;

namespace VideoOS.DatabaseUtility;

public class TimelineResponse(TimeLineInformationType activityType, DateTime beginTime, int count, string Data, TimeSpan interval)
{
	private TimeLineInformationType m_ActivityType = activityType;

	private DateTime m_BeginTime = beginTime;

	private int m_Count = count;

	private string m_Data = Data;

	private TimeSpan m_IntervalMicroSecs = interval;

	public TimeSpan IntervalMicroSecs => m_IntervalMicroSecs;

	public string Data => m_Data;

	public int Count => m_Count;

	public DateTime BeginTime => m_BeginTime;

	public TimeLineInformationType ActivityType => m_ActivityType;
}
