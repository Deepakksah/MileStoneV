using System;

namespace VideoOS.DatabaseUtility.MediaStorage;

public struct DatabaseTimeSlice(DateTime beginTime, DateTime endTime)
{
	private DateTime m_BeginTime = beginTime;

	private DateTime m_EndTime = endTime;

	public DateTime EndTime => m_EndTime;

	public DateTime BeginTime => m_BeginTime;
}
