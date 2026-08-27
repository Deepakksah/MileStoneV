using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization;

namespace VideoOS.Common.SharedTimeProfile.AppointmentCalendar;

public class CalendarInfo
{
	public bool AllowRecurringAppointments { get; set; }

	public int WeekRule { get; set; }

	public bool AllowAllDayEvents { get; set; }

	public FirstDayOfWeek FirstDayOfWeek{ get; set; }

	public List<Appointment> Appointments { get; } = new List<Appointment>();

	public IEnumerable<Appointment> GetAppointmentsInRange(DateTime startDateTime, DateTime endDateTime)
	{
		List<Appointment> list = new List<Appointment>();
		list.AddRange(Appointments.Where((Appointment appointment) => !appointment.IsVariance && appointment.Recurrence == null && ((appointment.AllDayEvent && appointment.StartDateTime.Date <= endDateTime.Date && appointment.EndDateTime.Date >= startDateTime.Date) || (!appointment.AllDayEvent && appointment.StartDateTime < endDateTime && appointment.EndDateTime > startDateTime))));
		foreach (Appointment item in Appointments.Where((Appointment appointment) => appointment.Recurrence != null && !appointment.IsVariance))
		{
			list.AddRange(item.Recurrence.GetAppointmentsInRange(startDateTime, endDateTime, item));
		}
		return list.ToList();
	}

	public void LoadFromXml(Stream stream, CalendarInfoCategories categories)
	{
		CalendarInfoEnvelope calendarInfoEnvelope = new CalendarInfoEnvelope();
		using (XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings
		{
			IgnoreWhitespace = true
		}))
		{
			calendarInfoEnvelope.ReadXml(reader);
		}
		FromSerializationObject(calendarInfoEnvelope);
	}

	public void SaveAsXml(Stream stream, CalendarInfoCategories categories)
	{
		CalendarInfoEnvelope calendarInfoEnvelope = ToSerializationObject();
		using XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings
		{
			Indent = true,
			OmitXmlDeclaration = true,
			Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)
		});
		calendarInfoEnvelope.Serialize(writer);
	}

	private void FromSerializationObject(CalendarInfoEnvelope calendarInfoEnvelope)
	{
		AllowRecurringAppointments = calendarInfoEnvelope.Body.CalendarInfoStreamerAllProps.Value.AllowRecurringAppointments;
		WeekRule = calendarInfoEnvelope.Body.CalendarInfoStreamerAllProps.Value.WeekRule.GetValueOrDefault();
		Appointments.Clear();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization.Appointment value in calendarInfoEnvelope.Body.Appointments.Values)
		{
			if (value.SubjectId != null)
			{
				dictionary[value.SubjectId] = value.Subject;
			}
		}
		foreach (VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization.Appointment value2 in calendarInfoEnvelope.Body.Appointments.Values)
		{
			Guid recurrenceId = (value2.RecurrenceId.HasValue ? value2.RecurrenceId.Value : Guid.Empty);
			AppointmentRecurrence appointmentRecurrenceFromBody = GetAppointmentRecurrenceFromBody(calendarInfoEnvelope, value2.RecurrenceRef);
			Appointment item = new Appointment(recurrenceId, value2.StartDateTime, value2.EndDateTime)
			{
				AllDayEvent = value2.AllDayEvent,
				Subject = ((value2.SubjectRef != null) ? dictionary[value2.SubjectRef.Substring(1)] : value2.Subject),
				OriginalStartDateTime = (value2.OriginalStartDateTime.HasValue ? value2.OriginalStartDateTime.Value : value2.StartDateTime),
				Recurrence = appointmentRecurrenceFromBody
			};
			Appointments.Add(item);
		}
	}

	private static AppointmentRecurrence GetAppointmentRecurrenceFromBody(CalendarInfoEnvelope calendarInfoEnvelope, string recurrenceRef)
	{
		if (recurrenceRef == null)
		{
			return null;
		}
		string key = (recurrenceRef.StartsWith("#", StringComparison.InvariantCulture) ? recurrenceRef.Substring(1) : recurrenceRef);
		if (calendarInfoEnvelope.Body.AppointmentRecurrences.TryGetValue(key, out var value))
		{
			return new AppointmentRecurrence(value.Id)
			{
				OccurrenceDuration = value.OccurrenceDuration,
				OccurrenceStartTime = value.OccurrenceStartTime,
				RangeLimit = (value.RangeLimit.HasValue ? ((RecurrenceRangeLimit)value.RangeLimit.Value) : RecurrenceRangeLimit.NoLimit),
				RangeMaxOccurrences = (value.RangeMaxOccurrences.HasValue ? value.RangeMaxOccurrences.Value : 10),
				PatternDayOfMonth = (value.PatternDayOfMonth.HasValue ? value.PatternDayOfMonth.Value : 0),
				PatternDaysOfWeek = (value.PatternDaysOfWeek.HasValue ? ((RecurrencePatternDaysOfWeek)value.PatternDaysOfWeek.Value) : RecurrencePatternDaysOfWeek.None),
				PatternFrequency = ((!value.PatternFrequency.HasValue) ? RecurrencePatternFrequency.Weekly : ((RecurrencePatternFrequency)value.PatternFrequency.Value)),
				PatternInterval = ((!value.PatternInterval.HasValue) ? 1 : value.PatternInterval.Value),
				PatternMonthOfYear = (value.PatternMonthOfYear.HasValue ? value.PatternMonthOfYear.Value : 0),
				PatternOccurrenceOfDayInMonth = (value.PatternOccurrenceOfDayInMonth.HasValue ? ((RecurrencePatternOccurrenceOfDayInMonth)value.PatternOccurrenceOfDayInMonth.Value) : RecurrencePatternOccurrenceOfDayInMonth.None),
				PatternType = (value.PatternType.HasValue ? ((RecurrencePatternType)value.PatternType.Value) : RecurrencePatternType.Explicit),
				RangeStartDate = value.RangeStartDate,
				RangeEndDate = value.RangeEndDate
			};
		}
		return null;
	}

	private CalendarInfoEnvelope ToSerializationObject()
	{
		CalendarInfoBody calendarInfoBody = new CalendarInfoBody
		{
			CalendarInfoStreamerAllProps = new KeyValuePair<string, CalendarInfoStreamerAllProps>("ref-1", new CalendarInfoStreamerAllProps
			{
				AllowRecurringAppointments = AllowRecurringAppointments,
				AppointmentsRef = ((Appointments.Count > 0) ? "#ref-3" : null),
				WeekRule = ((WeekRule != 0) ? new int?(WeekRule) : ((int?)null))
			}),
			AppointmentsCollection = ((Appointments.Count > 0) ? new KeyValuePair<string, AppointmentsCollection>("ref-3", new AppointmentsCollection()) : default(KeyValuePair<string, AppointmentsCollection>))
		};
		int num = 4;
		Dictionary<Appointment, string> dictionary = new Dictionary<Appointment, string>();
		foreach (Appointment appointment2 in Appointments)
		{
			string text = $"ref-{num++}";
			string text2 = appointment2.Subject ?? string.Empty;
			dictionary[appointment2] = text;
			calendarInfoBody.AppointmentsCollection.Value.AppointmentRefs.Add("#" + text);
			calendarInfoBody.Appointments.Add(text, new VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization.Appointment
			{
				AllDayEvent = appointment2.AllDayEvent,
				Subject = (appointment2.IsVariance ? null : text2),
				StartDateTime = appointment2.StartDateTime,
				EndDateTime = appointment2.EndDateTime,
				RecurrenceId = (appointment2.IsVariance ? new Guid?(appointment2.RecurrenceId) : ((Guid?)null)),
				OriginalStartDateTime = (appointment2.IsVariance ? new DateTime?(appointment2.OriginalStartDateTime) : ((DateTime?)null))
			});
		}
		foreach (VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization.Appointment item in calendarInfoBody.Appointments.Values.Where((VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization.Appointment a) => a.Subject != null))
		{
			item.SubjectId = $"ref-{num++}";
		}
		Dictionary<Guid, string> dictionary2 = new Dictionary<Guid, string>();
		Dictionary<Guid, VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization.Appointment> dictionary3 = new Dictionary<Guid, VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization.Appointment>();
		foreach (Appointment item2 in Appointments.Where((Appointment a) => a.IsRecurringAppointmentRoot))
		{
			AppointmentRecurrence recurrence = item2.Recurrence;
			string text3 = $"ref-{num++}";
			dictionary2[recurrence.Id] = text3;
			calendarInfoBody.AppointmentRecurrences.Add(text3, new VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization.AppointmentRecurrence
			{
				Id = recurrence.Id,
				OccurrenceDuration = recurrence.OccurrenceDuration,
				OccurrenceStartTime = recurrence.OccurrenceStartTime,
				RangeLimit = ((recurrence.RangeLimit != RecurrenceRangeLimit.NoLimit) ? new int?((int)recurrence.RangeLimit) : ((int?)null)),
				RangeMaxOccurrences = ((recurrence.RangeMaxOccurrences != 10) ? new int?(recurrence.RangeMaxOccurrences) : ((int?)null)),
				PatternDayOfMonth = ((recurrence.PatternDayOfMonth != 0) ? new int?(recurrence.PatternDayOfMonth) : ((int?)null)),
				PatternDaysOfWeek = ((recurrence.PatternDaysOfWeek != RecurrencePatternDaysOfWeek.None) ? new int?((int)recurrence.PatternDaysOfWeek) : ((int?)null)),
				PatternFrequency = ((recurrence.PatternFrequency != RecurrencePatternFrequency.Weekly) ? new int?((int)recurrence.PatternFrequency) : ((int?)null)),
				PatternInterval = ((recurrence.PatternInterval != 1) ? new int?(recurrence.PatternInterval) : ((int?)null)),
				PatternMonthOfYear = ((recurrence.PatternMonthOfYear != 0) ? new int?(recurrence.PatternMonthOfYear) : ((int?)null)),
				PatternOccurrenceOfDayInMonth = ((recurrence.PatternOccurrenceOfDayInMonth != RecurrencePatternOccurrenceOfDayInMonth.None) ? new int?((int)recurrence.PatternOccurrenceOfDayInMonth) : ((int?)null)),
				PatternType = ((recurrence.PatternType != RecurrencePatternType.Explicit) ? new int?((int)recurrence.PatternType) : ((int?)null)),
				RangeEndDate = recurrence.RangeEndDate,
				RangeStartDate = recurrence.RangeStartDate
			});
			VideoOS.Common.SharedTimeProfile.AppointmentCalendar.Serialization.Appointment appointment = calendarInfoBody.Appointments[dictionary[item2]];
			appointment.RecurrenceRef = "#" + text3;
			dictionary3[recurrence.Id] = appointment;
		}
		foreach (IGrouping<Guid, Appointment> item3 in from v in Appointments
			where v.IsVariance
			group v by v.RecurrenceId)
		{
			string text4 = $"ref-{num++}";
			VariancesCollection variancesCollection = new VariancesCollection
			{
				OwningRecurrenceRef = "#" + dictionary2[item3.Key]
			};
			foreach (Appointment item4 in item3)
			{
				variancesCollection.AppointmentVarianceRefs.Add("#" + dictionary[item4]);
				string subjectId = dictionary3[item3.Key].SubjectId;
				calendarInfoBody.Appointments[dictionary[item4]].SubjectRef = "#" + subjectId;
			}
			calendarInfoBody.VariancesCollections.Add(text4, variancesCollection);
			calendarInfoBody.AppointmentRecurrences[dictionary2[item3.Key]].VariancesRef = "#" + text4;
		}
		return new CalendarInfoEnvelope
		{
			Body = calendarInfoBody
		};
	}
}
