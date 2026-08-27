using System;
using System.Diagnostics;

namespace VideoOS.Common.SharedTimeProfile;

[DebuggerDisplay("Start {StartDateTime}, End {EndDateTime}")]
public class TimeProfileAppointment
{
	public DateTime StartDateTime { get; private set; }

	public DateTime EndDateTime { get; private set; }

	public TimeProfileAppointment(DateTime startTime, DateTime endTime)
	{
		StartDateTime = startTime;
		EndDateTime = endTime;
	}
}
