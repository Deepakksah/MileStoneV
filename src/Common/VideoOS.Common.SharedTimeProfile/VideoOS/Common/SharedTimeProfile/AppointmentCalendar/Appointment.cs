using System;

namespace VideoOS.Common.SharedTimeProfile.AppointmentCalendar;

public class Appointment
{
	public bool AllDayEvent { get; set; }

	public string Subject { get; set; }

	public DateTime StartDateTime { get; set; }

	public DateTime EndDateTime { get; set; }

	public AppointmentRecurrence Recurrence{ get; set; }

	public Guid RecurrenceId { get; set; }

	public DateTime OriginalStartDateTime { get; set; }

	public bool IsVariance => RecurrenceId != Guid.Empty;

	public bool IsRecurringAppointmentRoot => Recurrence != null;

	public Appointment(DateTime startDateTime, DateTime endDateTime)
	{
		StartDateTime = startDateTime;
		EndDateTime = endDateTime;
	}

	public Appointment(Guid recurrenceId, DateTime startDateTime, DateTime endDateTime)
	{
		RecurrenceId = recurrenceId;
		StartDateTime = startDateTime;
		EndDateTime = endDateTime;
	}
}
