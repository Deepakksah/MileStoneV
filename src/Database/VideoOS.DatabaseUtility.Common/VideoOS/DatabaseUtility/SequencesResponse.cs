using System;

namespace VideoOS.DatabaseUtility;

public class SequencesResponse(DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd)
{
	private DateTime m_TimeBegin = timeBegin;

	private DateTime m_TimeTrigged = timeTrigged;

	private DateTime m_TimeEnd = timeEnd;

	public DateTime TimeEnd => m_TimeEnd;

	public DateTime TimeTrigged => m_TimeTrigged;

	public DateTime TimeBegin => m_TimeBegin;
}
