using System;
using System.Diagnostics;

namespace VideoOS.Common.SharedTimeProfile;

[DebuggerDisplay("Date {ActiveDate}, Appointments {Appointments.Length}")]
public class TimeProfileAppointmentDay
{
	public TimeProfileAppointment[] Appointments { get; private set; }

	public DateTime ActiveDate { get; private set; }

	public TimeProfileAppointmentDay(TimeProfileAppointment[] appointments, DateTime day)
	{
		Appointments = appointments;
		ActiveDate = day.Date;
	}
}
