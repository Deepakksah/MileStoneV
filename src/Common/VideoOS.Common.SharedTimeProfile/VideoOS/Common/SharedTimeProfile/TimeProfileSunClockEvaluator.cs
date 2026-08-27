using System;
using System.Collections.Generic;

namespace VideoOS.Common.SharedTimeProfile;

public class TimeProfileSunClockEvaluator : IAppointmentDay
{
	public static class AppointmentCalculator
	{
		public static TimeProfileAppointment GetSunriseSunsetAppointment(DateTime day, TimeProfileSunClockDefinition definition, TimeZoneInfo timeZone)
		{
			int num = ((!(definition.GPSCoordinate.Longitude >= 0.0)) ? ((int)((definition.GPSCoordinate.Longitude - 7.5) / 15.0)) : ((int)((definition.GPSCoordinate.Longitude + 7.5) / 15.0)));
			DateTimeKind kind = DateTimeKind.Unspecified;
			if (timeZone.Id == TimeZoneInfo.Local.Id)
			{
				kind = DateTimeKind.Local;
			}
			DateTime dateTime = CalculateSunriseSunsetWilliam(definition.GPSCoordinate.Longitude, definition.GPSCoordinate.Latitude, sunrise: true, day.Date);
			DateTime dateTime2 = CalculateSunriseSunsetWilliam(definition.GPSCoordinate.Longitude, definition.GPSCoordinate.Latitude, sunrise: false, day.Date);
			if (dateTime == DateTime.MinValue || dateTime2 == DateTime.MinValue)
			{
				dateTime = day.Date.AddHours(-num) + timeZone.GetUtcOffset(day);
				dateTime2 = day.Date.AddHours(-num) + timeZone.GetUtcOffset(day);
				return new TimeProfileAppointment(new DateTime(dateTime.Ticks, kind), new DateTime(dateTime2.Ticks, kind));
			}
			if (dateTime == DateTime.MaxValue || dateTime2 == DateTime.MaxValue)
			{
				dateTime = day.Date.AddHours(-num) + timeZone.GetUtcOffset(day);
				dateTime2 = day.Date.AddDays(1.0).AddHours(-num) + timeZone.GetUtcOffset(day);
				return new TimeProfileAppointment(new DateTime(dateTime.Ticks, kind), new DateTime(dateTime2.Ticks, kind));
			}
			if (dateTime > dateTime2)
			{
				if (definition.GPSCoordinate.Longitude < 0.0)
				{
					dateTime2 = dateTime2.AddDays(1.0);
				}
				else
				{
					dateTime = dateTime.AddDays(-1.0);
				}
			}
			dateTime = dateTime.AddMinutes(definition.SunRiseOffset);
			dateTime2 = dateTime2.AddMinutes(definition.SunSetOffset);
			if (dateTime > dateTime2)
			{
				dateTime = day.Date.AddHours(-num) + timeZone.GetUtcOffset(day);
				dateTime2 = day.Date.AddHours(-num) + timeZone.GetUtcOffset(day);
				return new TimeProfileAppointment(new DateTime(dateTime.Ticks, kind), new DateTime(dateTime2.Ticks, kind));
			}
			dateTime += timeZone.GetUtcOffset(dateTime);
			dateTime2 += timeZone.GetUtcOffset(dateTime2);
			return new TimeProfileAppointment(new DateTime(dateTime.Ticks, kind), new DateTime(dateTime2.Ticks, kind));
		}
	}

	private TimeProfileSunClockDefinition definition;

	public TimeProfileSunClockEvaluator(string definitionXml)
	{
		definition = TimeProfileSunClockDefinition.FromXml(definitionXml);
	}

	public TimeProfileSunClockEvaluator(TimeProfileSunClockDefinition sunclockDefinition)
	{
		if (sunclockDefinition == null)
		{
			throw new ArgumentNullException("sunclockDefinition");
		}
		definition = sunclockDefinition;
	}

	public TimeProfileAppointmentDay GetAppointments(DateTime day)
	{
		return GetAppointments(day, TimeZoneInfo.Local);
	}

	public TimeProfileAppointmentDay GetAppointments(DateTime day, TimeZoneInfo timeZone)
	{
		List<TimeProfileAppointment> list = new List<TimeProfileAppointment>();
		TimeProfileAppointment timeProfileAppointment;
		foreach (TimeProfileAppointment item in new List<TimeProfileAppointment>
		{
			AppointmentCalculator.GetSunriseSunsetAppointment(day.Date.AddDays(-1.0), definition, timeZone),
			AppointmentCalculator.GetSunriseSunsetAppointment(day.Date, definition, timeZone),
			AppointmentCalculator.GetSunriseSunsetAppointment(day.Date.AddDays(1.0), definition, timeZone)
		})
		{
			if (item.StartDateTime >= item.EndDateTime)
			{
				continue;
			}
			timeProfileAppointment = null;
			if (item.StartDateTime >= day.Date && item.EndDateTime <= day.Date.AddDays(1.0))
			{
				timeProfileAppointment = new TimeProfileAppointment(item.StartDateTime, item.EndDateTime);
			}
			else if (item.StartDateTime >= day.Date)
			{
				if (item.StartDateTime < day.Date.AddDays(1.0))
				{
					timeProfileAppointment = new TimeProfileAppointment(item.StartDateTime, day.Date.AddDays(1.0));
				}
			}
			else if (item.EndDateTime >= day.Date && item.EndDateTime > day.Date)
			{
				timeProfileAppointment = new TimeProfileAppointment(day.Date, item.EndDateTime);
			}
			if (timeProfileAppointment == null)
			{
				continue;
			}
			foreach (TimeProfileAppointment item2 in list)
			{
				if ((!(timeProfileAppointment.StartDateTime < item2.StartDateTime) || !(timeProfileAppointment.EndDateTime > item2.StartDateTime)) && (!(timeProfileAppointment.StartDateTime >= item2.StartDateTime) || !(timeProfileAppointment.EndDateTime <= item2.EndDateTime)) && timeProfileAppointment.StartDateTime < item2.EndDateTime && timeProfileAppointment.EndDateTime > item2.EndDateTime)
				{
					timeProfileAppointment = new TimeProfileAppointment(item2.EndDateTime, timeProfileAppointment.EndDateTime);
				}
			}
			if (timeProfileAppointment.StartDateTime < timeProfileAppointment.EndDateTime)
			{
				list.Add(timeProfileAppointment);
			}
		}
		timeProfileAppointment = null;
		if (list.Count >= 2 && list[1].StartDateTime == list[0].EndDateTime)
		{
			timeProfileAppointment = new TimeProfileAppointment(list[0].StartDateTime, list[1].EndDateTime);
		}
		if (timeProfileAppointment != null)
		{
			list.Clear();
			list.Add(timeProfileAppointment);
		}
		return new TimeProfileAppointmentDay(list.ToArray(), day.Date);
	}

	public static DateTime CalculateSunriseSunsetWilliam(double longitude, double latitude, bool sunrise, DateTime time)
	{
		double num = 180.0 / Math.PI;
		double num2 = Math.PI / 180.0;
		double num3 = 90.83333333333333;
		int num4 = 275 * time.Month / 9;
		int num5 = (time.Month + 9) / 12;
		int num6 = 1 + (time.Year - 4 * (time.Year / 4) + 2) / 3;
		int num7 = num4 - num5 * num6 + time.Day - 30;
		double num8 = longitude / 15.0;
		double num9 = ((!sunrise) ? ((double)num7 + (18.0 - num8) / 24.0) : ((double)num7 + (6.0 - num8) / 24.0));
		double num10 = 0.9856 * num9 - 3.289;
		double num11;
		for (num11 = num10 + 1.916 * Math.Sin(num10 * num2) + 0.02 * Math.Sin(2.0 * num10 * num2) + 282.634; num11 > 360.0; num11 -= 360.0)
		{
		}
		for (; num11 < 0.0; num11 += 360.0)
		{
		}
		double num12;
		for (num12 = Math.Atan(0.91764 * Math.Tan(num11 * num2)) * num; num12 > 360.0; num12 -= 360.0)
		{
		}
		for (; num12 < 0.0; num12 += 360.0)
		{
		}
		int num13 = (int)(num11 / 90.0) * 90;
		int num14 = (int)(num12 / 90.0) * 90;
		num12 += (double)(num13 - num14);
		num12 /= 15.0;
		double num15 = 0.39782 * Math.Sin(num11 * num2);
		double num16 = Math.Cos(Math.Asin(num15));
		double num17 = (Math.Cos(num3 * num2) - num15 * Math.Sin(latitude * num2)) / (num16 * Math.Cos(latitude * num2));
		DateTime dateTime = new DateTime(time.Year, 3, 20);
		DateTime dateTime2 = new DateTime(time.Year, 9, 23);
		if (num17 > 1.0 || num17 < -1.0)
		{
			if (latitude > 0.0 && dateTime < time && time < dateTime2)
			{
				return DateTime.MaxValue;
			}
			if (latitude < 0.0 && (dateTime > time || time > dateTime2))
			{
				return DateTime.MaxValue;
			}
			return DateTime.MinValue;
		}
		double num18 = ((!sunrise) ? (Math.Acos(num17) * num) : (360.0 - Math.Acos(num17) * num));
		num18 /= 15.0;
		double num19;
		for (num19 = num18 + num12 - 0.06571 * num9 - 6.622 - num8; num19 > 24.0; num19 -= 24.0)
		{
		}
		for (; num19 < 0.0; num19 += 24.0)
		{
		}
		int num20 = (int)num19;
		num19 -= (double)num20;
		int num21 = (int)(num19 * 60.0);
		num19 -= (double)num21 / 60.0;
		int second = (int)(num19 * 3600.0);
		return new DateTime(time.Year, time.Month, time.Day, num20, num21, second, DateTimeKind.Utc);
	}
}
