#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using VideoOS.Common.SharedTimeProfile.AppointmentCalendar;
using VideoOS.Management.Common;

namespace VideoOS.Common.SharedTimeProfile;

public static class AppointmentConversions
{
	public static string ConvertFromInfragistics(string infragisticsXml)
	{
		AppointmentDefinitions appointmentDefinitions = ConvertToAppointments(infragisticsXml);
		string text = "";
		string assemblyQualifiedName = appointmentDefinitions.GetType().AssemblyQualifiedName;
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlTextWriter writer = new XmlTextWriter(stringWriter);
			stringWriter = null;
			new DataContractSerializer(appointmentDefinitions.GetType()).WriteObject(writer, appointmentDefinitions);
			return stringBuilder.ToString();
		}
		catch (Exception ex)
		{
			Trace.WriteLine("ExternalMessage.Serialize, Unable to serialize class:" + assemblyQualifiedName + ", Exception:" + ex.Message);
			throw new TimeProfileConversionException("Unable to convert from Infragistics to explicit classes", ex);
		}
		finally
		{
			stringWriter?.Dispose();
		}
	}

	public static string ConvertToInfragistics(string explicitXml)
	{
		string result = "";
		if (!string.IsNullOrEmpty(explicitXml))
		{
			StringReader stringReader = null;
			try
			{
				stringReader = new StringReader(explicitXml);
				XmlReader xmlReader = new XmlTextReader(stringReader);
				AppointmentDefinitions appointments = new DataContractSerializer(typeof(AppointmentDefinitions)).ReadObject(xmlReader) as AppointmentDefinitions;
				xmlReader.Close();
				stringReader = null;
				return Serialize(appointments);
			}
			catch (Exception ex)
			{
				Trace.WriteLine("ExtenalMessage.Deserialize: Unable to deserialize TimeProfileDefinition, Exception:" + ex.Message);
				throw new TimeProfileConversionException("Unable to convert from explicit TimeProfile xml to Infragistics", ex);
			}
			finally
			{
				stringReader?.Close();
			}
		}
		return result;
	}

	public static CalendarInfo ConvertToCalendarInfo(string definitionXml)
	{
		if (string.IsNullOrEmpty(definitionXml))
		{
			throw new ArgumentNullException("definitionXml");
		}
		CalendarInfo calendarInfo = new CalendarInfo();
		calendarInfo.AllowRecurringAppointments = true;
		string s = Regex.Replace(definitionXml, "0000000(\\u002B|-)(\\d{2})\\u003A00", "0000000");
		using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s));
		calendarInfo.LoadFromXml(stream, CalendarInfoCategories.All);
		return calendarInfo;
	}

	public static AppointmentDefinitions ConvertToAppointments(string definitionXml)
	{
		if (string.IsNullOrEmpty(definitionXml))
		{
			throw new ArgumentNullException("definitionXml");
		}
		return ConvertToAppointments(ConvertToCalendarInfo(definitionXml));
	}

	public static AppointmentDefinitions ConvertToAppointments(CalendarInfo calendarInfo)
	{
		if (calendarInfo == null)
		{
			throw new ArgumentNullException("calendarInfo");
		}
		try
		{
			AppointmentDefinitions appointmentDefinitions = new AppointmentDefinitions();
			foreach (Appointment appointment in calendarInfo.Appointments)
			{
				AppointmentRecurrenceDefinition appointmentRecurrence = null;
				if (appointment.IsRecurringAppointmentRoot)
				{
					AppointmentRecurrence recurrence = appointment.Recurrence;
					appointmentRecurrence = new AppointmentRecurrenceDefinition
					{
						RecurrenceId = recurrence.Id,
						PatternDayOfMonth = recurrence.PatternDayOfMonth,
						PatternMonthOfYear = recurrence.PatternMonthOfYear,
						PatternDaysOfWeek = ConvertToInt(recurrence.PatternDaysOfWeek),
						PatternOccurrenceOfDayInMonth = ConvertFromDayInMonth(recurrence.PatternOccurrenceOfDayInMonth),
						PatternFrequency = ConvertfromFrequency(recurrence.PatternFrequency),
						PatternInterval = recurrence.PatternInterval,
						PatternType = ConvertToPatternType(recurrence.PatternType),
						RangeStartDate = recurrence.RangeStartDate,
						RangeEndDate = recurrence.RangeEndDate,
						RangeLimit = ConvertToRangeLimit(recurrence.RangeLimit),
						RangeMaxOccurrences = recurrence.RangeMaxOccurrences,
						OccurrenceDuration = recurrence.OccurrenceDuration,
						OccurrenceStartTime = recurrence.OccurrenceStartTime,
						Description = recurrence.Description
					};
				}
				VarianceDefinition variance = null;
				if (appointment.IsVariance)
				{
					variance = new VarianceDefinition
					{
						RecurrenceId = appointment.RecurrenceId,
						Subject = appointment.Subject,
						StartDate = appointment.StartDateTime,
						EndDate = appointment.EndDateTime,
						OriginalStartDate = appointment.OriginalStartDateTime
					};
				}
				appointmentDefinitions.Appointments.Add(new AppointmentDefinition
				{
					AllDayEvent = appointment.AllDayEvent,
					Subject = appointment.Subject,
					StartDateTime = appointment.StartDateTime,
					EndDateTime = appointment.EndDateTime,
					AppointmentRecurrence = appointmentRecurrence,
					Variance = variance
				});
			}
			return appointmentDefinitions;
		}
		catch (Exception exception)
		{
			throw new TimeProfileConversionException("Unable to get information out of Infragistics classes", exception);
		}
	}

	public static CalendarInfo ConvertToCalendarInfo(AppointmentDefinitions appointments)
	{
		if (appointments == null)
		{
			throw new ArgumentNullException("appointments");
		}
		CalendarInfo calendarInfo = new CalendarInfo();
		calendarInfo.AllowRecurringAppointments = true;
		calendarInfo.AllowAllDayEvents = true;
		Dictionary<Guid, AppointmentRecurrence> dictionary = new Dictionary<Guid, AppointmentRecurrence>();
		HashSet<Guid> hashSet = new HashSet<Guid>();
		foreach (AppointmentDefinition appointment2 in appointments.Appointments)
		{
			Appointment appointment = new Appointment(appointment2.StartDateTime, appointment2.EndDateTime);
			appointment.Subject = appointment2.Subject;
			appointment.AllDayEvent = appointment2.AllDayEvent;
			if (appointment2.AppointmentRecurrence != null)
			{
				AppointmentRecurrence appointmentRecurrence = null;
				appointmentRecurrence = new AppointmentRecurrence();
				dictionary.Add(appointment2.AppointmentRecurrence.RecurrenceId, appointmentRecurrence);
				appointmentRecurrence.OccurrenceStartTime = appointment2.AppointmentRecurrence.OccurrenceStartTime;
				appointmentRecurrence.OccurrenceDuration = appointment2.AppointmentRecurrence.OccurrenceDuration;
				appointmentRecurrence.PatternDayOfMonth = appointment2.AppointmentRecurrence.PatternDayOfMonth;
				appointmentRecurrence.PatternMonthOfYear = appointment2.AppointmentRecurrence.PatternMonthOfYear;
				appointmentRecurrence.PatternDaysOfWeek = ConvertToDaysOfWeek(appointment2.AppointmentRecurrence.PatternDaysOfWeek);
				appointmentRecurrence.PatternOccurrenceOfDayInMonth = ConvertToDayInMonth(appointment2.AppointmentRecurrence.PatternOccurrenceOfDayInMonth);
				appointmentRecurrence.PatternFrequency = ConvertToFrequency(appointment2.AppointmentRecurrence.PatternFrequency);
				appointmentRecurrence.PatternInterval = appointment2.AppointmentRecurrence.PatternInterval;
				appointmentRecurrence.PatternType = ConvertFromPatternType(appointment2.AppointmentRecurrence.PatternType);
				appointmentRecurrence.RangeLimit = ConvertFromRangeLimit(appointment2.AppointmentRecurrence.RangeLimit);
				appointmentRecurrence.RangeMaxOccurrences = appointment2.AppointmentRecurrence.RangeMaxOccurrences;
				appointmentRecurrence.RangeStartDate = appointment2.AppointmentRecurrence.RangeStartDate;
				appointmentRecurrence.RangeEndDate = appointment2.AppointmentRecurrence.RangeEndDate;
				appointment.Recurrence = appointmentRecurrence;
				if (appointmentRecurrence.RangeStartDate != appointment2.AppointmentRecurrence.RangeStartDate)
				{
					hashSet.Add(appointmentRecurrence.Id);
					appointment2.AppointmentRecurrence.RecurrenceId = appointmentRecurrence.Id;
				}
			}
			if (appointment2.Variance != null)
			{
				if (dictionary.TryGetValue(appointment2.Variance.RecurrenceId, out var value))
				{
					Update(value, appointment2.Variance.OriginalStartDate, appointment2.Variance.StartDate, appointment2.Variance.EndDate, appointment2.Variance.Subject);
				}
			}
			else
			{
				calendarInfo.Appointments.Add(appointment);
			}
		}
		if (hashSet.Count != 0)
		{
			return FixRecurrenceStartTime(hashSet, calendarInfo, appointments);
		}
		return calendarInfo;
	}

	private static CalendarInfo FixRecurrenceStartTime(HashSet<Guid> recurToFix, CalendarInfo calendarInput, AppointmentDefinitions appointments)
	{
		MemoryStream memoryStream = new MemoryStream();
		calendarInput.SaveAsXml(memoryStream, CalendarInfoCategories.All);
		string xml = Encoding.UTF8.GetString(memoryStream.ToArray());
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
		xmlNamespaceManager.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/");
		xmlNamespaceManager.AddNamespace("a1", "http://schemas.microsoft.com/clr/nsassem/Infragistics.Win.UltraWinSchedule/Infragistics2.Win.UltraWinSchedule.v10.2");
		foreach (XmlNode item in xmlDocument.SelectSingleNode("SOAP-ENV:Envelope", xmlNamespaceManager).SelectSingleNode("SOAP-ENV:Body", xmlNamespaceManager).SelectNodes("a1:AppointmentRecurrence", xmlNamespaceManager))
		{
			Guid guid = FetchGuid(item["Id"]);
			if (!recurToFix.Contains(guid))
			{
				continue;
			}
			XmlElement xmlElement = item["RangeStartDate"];
			foreach (AppointmentDefinition appointment in appointments.Appointments)
			{
				if (appointment.AppointmentRecurrence != null && appointment.AppointmentRecurrence.RecurrenceId == guid)
				{
					string innerText = appointment.AppointmentRecurrence.RangeStartDate.ToString("yyyy-MM-ddTHH:mm:ss.0000000zzzzz", CultureInfo.InvariantCulture);
					xmlElement.InnerText = innerText;
					break;
				}
			}
		}
		string outerXml = xmlDocument.OuterXml;
		CalendarInfo calendarInfo = new CalendarInfo();
		calendarInfo.AllowRecurringAppointments = true;
		calendarInfo.AllowAllDayEvents = true;
		calendarInfo.FirstDayOfWeek = FirstDayOfWeek.Default;
		using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(outerXml));
		calendarInfo.LoadFromXml(stream, CalendarInfoCategories.All);
		return calendarInfo;
	}

	private static Guid FetchGuid(XmlNode node)
	{
		int a = GetInt(node, "_a");
		int num = GetInt(node, "_b");
		int num2 = GetInt(node, "_c");
		int num3 = GetInt(node, "_d");
		int num4 = GetInt(node, "_e");
		int num5 = GetInt(node, "_f");
		int num6 = GetInt(node, "_g");
		int num7 = GetInt(node, "_h");
		int num8 = GetInt(node, "_i");
		int num9 = GetInt(node, "_j");
		int num10 = GetInt(node, "_k");
		return new Guid((uint)a, (ushort)num, (ushort)num2, (byte)num3, (byte)num4, (byte)num5, (byte)num6, (byte)num7, (byte)num8, (byte)num9, (byte)num10);
	}

	private static int GetInt(XmlNode node, string subnodeName)
	{
		return int.Parse(node[subnodeName].InnerText, CultureInfo.InvariantCulture);
	}

	private static void Update(AppointmentRecurrence recurrence, DateTime originalstartdate, DateTime startdatetime, DateTime enddatetime, string subject)
	{
		IReadOnlyCollection<Appointment> appointmentsInRange = recurrence.GetAppointmentsInRange(originalstartdate, originalstartdate + TimeSpan.FromDays(1.0));
		if (appointmentsInRange.Count == 0)
		{
			throw new ArgumentException("Variance to non-existing appointment");
		}
		foreach (Appointment item in appointmentsInRange)
		{
			item.Subject = subject;
			item.StartDateTime = startdatetime;
			item.EndDateTime = enddatetime;
		}
	}

	public static string Serialize(AppointmentDefinitions appointments)
	{
		CalendarInfo calendarInfo = ConvertToCalendarInfo(appointments);
		using MemoryStream memoryStream = new MemoryStream();
		calendarInfo.SaveAsXml(memoryStream, CalendarInfoCategories.All);
		return Encoding.UTF8.GetString(memoryStream.ToArray());
	}

	private static PatternType ConvertToPatternType(RecurrencePatternType patternType)
	{
		return patternType switch
		{
			RecurrencePatternType.Calculated => PatternType.Calculated, 
			RecurrencePatternType.Explicit => PatternType.Explicit, 
			_ => throw new ArgumentException("RecurrencePatternType"), 
		};
	}

	private static RecurrencePatternType ConvertFromPatternType(PatternType patternType)
	{
		return patternType switch
		{
			PatternType.Calculated => RecurrencePatternType.Calculated, 
			PatternType.Explicit => RecurrencePatternType.Explicit, 
			_ => throw new ArgumentException("PatternType"), 
		};
	}

	private static RangeLimit ConvertToRangeLimit(RecurrenceRangeLimit limit)
	{
		return limit switch
		{
			RecurrenceRangeLimit.LimitByDate => RangeLimit.LimitByDate, 
			RecurrenceRangeLimit.LimitByNumberOfOccurrences => RangeLimit.LimitByNumberOfOccurrences, 
			RecurrenceRangeLimit.NoLimit => RangeLimit.NoLimit, 
			_ => throw new ArgumentException("RecurrenceRangeLimit"), 
		};
	}

	private static RecurrenceRangeLimit ConvertFromRangeLimit(RangeLimit limit)
	{
		return limit switch
		{
			RangeLimit.LimitByDate => RecurrenceRangeLimit.LimitByDate, 
			RangeLimit.LimitByNumberOfOccurrences => RecurrenceRangeLimit.LimitByNumberOfOccurrences, 
			RangeLimit.NoLimit => RecurrenceRangeLimit.NoLimit, 
			_ => throw new ArgumentException("RangeLimit"), 
		};
	}

	private static RecurrencePatternFrequencyType ConvertfromFrequency(RecurrencePatternFrequency freq)
	{
		return freq switch
		{
			RecurrencePatternFrequency.Daily => RecurrencePatternFrequencyType.Daily, 
			RecurrencePatternFrequency.Weekly => RecurrencePatternFrequencyType.Weekly, 
			RecurrencePatternFrequency.Monthly => RecurrencePatternFrequencyType.Monthly, 
			RecurrencePatternFrequency.Yearly => RecurrencePatternFrequencyType.Yearly, 
			_ => throw new ArgumentException("PatternFrequency"), 
		};
	}

	private static RecurrencePatternFrequency ConvertToFrequency(RecurrencePatternFrequencyType frequence)
	{
		return frequence switch
		{
			RecurrencePatternFrequencyType.Daily => RecurrencePatternFrequency.Daily, 
			RecurrencePatternFrequencyType.Weekly => RecurrencePatternFrequency.Weekly, 
			RecurrencePatternFrequencyType.Monthly => RecurrencePatternFrequency.Monthly, 
			RecurrencePatternFrequencyType.Yearly => RecurrencePatternFrequency.Yearly, 
			_ => RecurrencePatternFrequency.Daily, 
		};
	}

	private static PatternOccurrenceOfDayInMonthType ConvertFromDayInMonth(RecurrencePatternOccurrenceOfDayInMonth dayInMonth)
	{
		return dayInMonth switch
		{
			RecurrencePatternOccurrenceOfDayInMonth.None => PatternOccurrenceOfDayInMonthType.None, 
			RecurrencePatternOccurrenceOfDayInMonth.First => PatternOccurrenceOfDayInMonthType.First, 
			RecurrencePatternOccurrenceOfDayInMonth.Second => PatternOccurrenceOfDayInMonthType.Second, 
			RecurrencePatternOccurrenceOfDayInMonth.Third => PatternOccurrenceOfDayInMonthType.Third, 
			RecurrencePatternOccurrenceOfDayInMonth.Fourth => PatternOccurrenceOfDayInMonthType.Fourth, 
			RecurrencePatternOccurrenceOfDayInMonth.Last => PatternOccurrenceOfDayInMonthType.Last, 
			_ => PatternOccurrenceOfDayInMonthType.None, 
		};
	}

	private static RecurrencePatternOccurrenceOfDayInMonth ConvertToDayInMonth(PatternOccurrenceOfDayInMonthType dayInMonth)
	{
		return dayInMonth switch
		{
			PatternOccurrenceOfDayInMonthType.First => RecurrencePatternOccurrenceOfDayInMonth.First, 
			PatternOccurrenceOfDayInMonthType.Second => RecurrencePatternOccurrenceOfDayInMonth.Second, 
			PatternOccurrenceOfDayInMonthType.Third => RecurrencePatternOccurrenceOfDayInMonth.Third, 
			PatternOccurrenceOfDayInMonthType.Fourth => RecurrencePatternOccurrenceOfDayInMonth.Fourth, 
			PatternOccurrenceOfDayInMonthType.Last => RecurrencePatternOccurrenceOfDayInMonth.Last, 
			_ => RecurrencePatternOccurrenceOfDayInMonth.None, 
		};
	}

	private static int ConvertToInt(RecurrencePatternDaysOfWeek daysOfWeek)
	{
		return (int)daysOfWeek;
	}

	private static RecurrencePatternDaysOfWeek ConvertToDaysOfWeek(int daysOfWeek)
	{
		return (RecurrencePatternDaysOfWeek)daysOfWeek;
	}
}
