using System;
using System.Linq;
using System.Threading;

namespace VideoOS.Common.SharedTimeProfile;

public class TimeProfileEvaluator
{
	public class TestAccessor
	{
		public static TimeProfileAppointmentDay GetDailyAppointments(TimeProfileEvaluator evaluator, DateTime date)
		{
			return evaluator.GetDailyAppointments(date);
		}
	}

	private ITimeProfileAppointmentDayCache dayCache;

	private ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

	private IAppointmentDay timeProfileEvaluator;

	public Guid Id { get; private set; }

	public TimeProfileType TimeProfileType { get; private set; }

	public TimeProfileEvaluator(Guid id, string definitionXml, TimeProfileType timeProfileType)
		: this(id, definitionXml, timeProfileType, new TimeProfileAppointmentDayCache(10))
	{
	}

	public TimeProfileEvaluator(Guid id, string definitionXml, TimeProfileType timeProfileType, ITimeProfileAppointmentDayCache dayCache)
	{
		this.dayCache = dayCache;
		Id = id;
		TimeProfileType = timeProfileType;
		switch (timeProfileType)
		{
		case TimeProfileType.CalendarProfile:
			timeProfileEvaluator = new TimeProfileAppointmentEvaluator(definitionXml);
			break;
		case TimeProfileType.SunclockProfile:
			timeProfileEvaluator = new TimeProfileSunClockEvaluator(definitionXml);
			break;
		default:
			throw new ArgumentException("Invalid Time Profile Type");
		}
	}

	public bool EvaluateTime(DateTime utcTime)
	{
		DateTime dateTime = utcTime.ToLocalTime();
		TimeProfileAppointmentDay dailyAppointments = GetDailyAppointments(dateTime);
		if (dailyAppointments != null)
		{
			TimeProfileAppointment[] appointments = dailyAppointments.Appointments;
			foreach (TimeProfileAppointment timeProfileAppointment in appointments)
			{
				if (timeProfileAppointment.StartDateTime <= dateTime && dateTime <= timeProfileAppointment.EndDateTime)
				{
					return true;
				}
			}
		}
		return false;
	}

	public TimeSpan GetAppointmentDuration(DateTime utcStartTime, TimeSpan searchInterval)
	{
		TimeSpan timeSpan = new TimeSpan(0L);
		DateTime dateTime = utcStartTime.ToLocalTime();
		DateTime dateTime2 = dateTime;
		DateTime dateTime3 = dateTime.Add(searchInterval);
		while (dateTime2 < dateTime3)
		{
			DateTime dateTime4 = dateTime2;
			TimeProfileAppointmentDay dailyAppointments = GetDailyAppointments(dateTime2);
			if (dailyAppointments == null || dailyAppointments.Appointments.Length == 0)
			{
				if (timeSpan > searchInterval)
				{
					timeSpan = searchInterval;
				}
				return timeSpan;
			}
			DateTime dateTime5 = dateTime2;
			TimeProfileAppointment[] appointments = dailyAppointments.Appointments;
			foreach (TimeProfileAppointment timeProfileAppointment in appointments)
			{
				if (dateTime2 >= timeProfileAppointment.StartDateTime && dateTime2 <= timeProfileAppointment.EndDateTime && dateTime5 < timeProfileAppointment.EndDateTime)
				{
					dateTime5 = ((!(timeProfileAppointment.EndDateTime < dateTime2.Date.AddDays(1.0))) ? dateTime2.Date.AddDays(1.0) : timeProfileAppointment.EndDateTime);
				}
			}
			dateTime2 = dateTime2.Date.AddDays(1.0);
			if (dateTime5 == dateTime2)
			{
				timeSpan += dateTime2 - dateTime4;
				continue;
			}
			timeSpan += dateTime5 - dateTime4;
			if (timeSpan > searchInterval)
			{
				timeSpan = searchInterval;
			}
			return timeSpan;
		}
		if (timeSpan > searchInterval)
		{
			timeSpan = searchInterval;
		}
		return timeSpan;
	}

	public void ClearAppointmentCache()
	{
		rwLock.EnterWriteLock();
		try
		{
			dayCache.Clear();
		}
		finally
		{
			rwLock.ExitWriteLock();
		}
	}

	private TimeProfileAppointmentDay GetDailyAppointments(DateTime date)
	{
		rwLock.EnterReadLock();
		try
		{
			foreach (TimeProfileAppointmentDay item in dayCache)
			{
				if (item.ActiveDate == date.Date)
				{
					return item;
				}
			}
		}
		finally
		{
			rwLock.ExitReadLock();
		}
		rwLock.EnterWriteLock();
		try
		{
			foreach (TimeProfileAppointmentDay item2 in dayCache)
			{
				if (item2.ActiveDate.Date == date.Date)
				{
					return item2;
				}
			}
			TimeProfileAppointmentDay appointments = timeProfileEvaluator.GetAppointments(date);
			if (dayCache.Count() == 0)
			{
				dayCache.Add(appointments);
			}
			else
			{
				if (dayCache.Count() == dayCache.MaxCacheEntries)
				{
					dayCache.RemoveAt(0);
				}
				if (dayCache[dayCache.Count() - 1].ActiveDate == DateTime.Now.Date)
				{
					dayCache.Insert(dayCache.Count() - 1, appointments);
				}
				else
				{
					dayCache.Add(appointments);
				}
			}
			return appointments;
		}
		finally
		{
			rwLock.ExitWriteLock();
		}
	}
}
