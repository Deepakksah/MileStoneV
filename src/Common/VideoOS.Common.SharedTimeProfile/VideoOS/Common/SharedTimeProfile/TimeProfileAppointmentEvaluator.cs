using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using VideoOS.Common.SharedTimeProfile.AppointmentCalendar;

namespace VideoOS.Common.SharedTimeProfile;

public class TimeProfileAppointmentEvaluator : IAppointmentDay
{
	public static class TimeProfileDefinitionUtilities
	{
		public static string StripTimeZone(string definitionXml)
		{
			return Regex.Replace(definitionXml, "0000000(\\u002B|-)(\\d{2})\\u003A00", "0000000");
		}
	}

	private CalendarInfo calendarInfo;

	public TimeProfileAppointmentEvaluator(CalendarInfo calendarInfo)
	{
		this.calendarInfo = calendarInfo;
	}

	public TimeProfileAppointmentEvaluator(string definitionXml)
	{
		if (definitionXml == null)
		{
			throw new ArgumentNullException("definitionXml");
		}
		calendarInfo = new CalendarInfo();
		calendarInfo.AllowRecurringAppointments = true;
		string s = TimeProfileDefinitionUtilities.StripTimeZone(definitionXml);
		MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s));
		try
		{
			calendarInfo.LoadFromXml(stream, CalendarInfoCategories.All);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			throw new ArgumentException("Invalid definitionXml");
		}
	}

	public TimeProfileAppointmentDay GetAppointments(DateTime day)
	{
		List<TimeProfileAppointment> list = new List<TimeProfileAppointment>();
		foreach (Appointment item in calendarInfo.GetAppointmentsInRange(day.Date, day.Date.AddDays(1.0).AddMilliseconds(-1.0)))
		{
			TimeProfileAppointment timeProfileAppointment = null;
			timeProfileAppointment = ((!item.AllDayEvent) ? new TimeProfileAppointment(item.StartDateTime, item.EndDateTime) : new TimeProfileAppointment(day.Date, day.Date.AddDays(1.0).AddMilliseconds(-1.0)));
			if (!(timeProfileAppointment.StartDateTime == timeProfileAppointment.EndDateTime))
			{
				if (timeProfileAppointment.StartDateTime.Date == day.Date && item.EndDateTime.Date == day.Date)
				{
					list.Add(timeProfileAppointment);
				}
				else if (timeProfileAppointment.StartDateTime.Date == day.Date)
				{
					list.Add(new TimeProfileAppointment(timeProfileAppointment.StartDateTime, day.Date.AddDays(1.0).AddMilliseconds(-1.0)));
				}
				else if (timeProfileAppointment.EndDateTime.Date == day.Date)
				{
					list.Add(new TimeProfileAppointment(day.Date, timeProfileAppointment.EndDateTime));
				}
				else if (timeProfileAppointment.StartDateTime.Date < day.Date && timeProfileAppointment.EndDateTime.Date > day.Date)
				{
					list.Add(new TimeProfileAppointment(day.Date, day.Date.AddDays(1.0).AddMilliseconds(-1.0)));
				}
			}
		}
		return new TimeProfileAppointmentDay(list.ToArray(), day.Date);
	}
}
