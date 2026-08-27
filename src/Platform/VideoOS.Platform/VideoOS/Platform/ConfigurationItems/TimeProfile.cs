using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class TimeProfile : IConfigurationItem
{
	public ICollection<TimeProfileAppointmentRecurChildItem> TimeProfileAppointmentRecurChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<TimeProfileAppointmentRecurChildItem> list = new List<TimeProfileAppointmentRecurChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "TimeProfileAppointmentRecur")
				{
					list.Add(new TimeProfileAppointmentRecurChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public ICollection<TimeProfileAppointmentRootChildItem> TimeProfileAppointmentRootChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<TimeProfileAppointmentRootChildItem> list = new List<TimeProfileAppointmentRootChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "TimeProfileAppointmentRoot")
				{
					list.Add(new TimeProfileAppointmentRootChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public Dictionary<string, string> TimeProfileTypeValues => base.Wrapper.GetEnumValues("TimeProfileType");

	public string TimeProfileType => base.Wrapper.GetPropertyValue("TimeProfileType");

	public TimeProfile(ServerId serverId, ConfigurationItem timeProfile)
		: base(serverId, timeProfile)
	{
	}

	public TimeProfile(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public TimeProfile(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddAppointment(bool allDayEvent, DateTime startDateTime, DateTime endDateTime, string subject)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddAppointment");
			configurationApiItemWrapper.SetProperty("AllDayEvent", allDayEvent.ToString());
			configurationApiItemWrapper.SetProperty("StartDateTime", TimeConvert.ToGmtString(startDateTime));
			configurationApiItemWrapper.SetProperty("EndDateTime", TimeConvert.ToGmtString(endDateTime));
			configurationApiItemWrapper.SetProperty("Subject", subject);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddAppointment"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "TimeProfile", "Unable to call method 'AddAppointment':" + ex.Message);
		}
		return null;
	}

	public AddAppointmentServerTask AddAppointment()
	{
		try
		{
			return new AddAppointmentServerTask(base.Wrapper.ExecuteMethod("AddAppointment"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "TimeProfile", "Unable to call method 'AddAppointment':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveAppointment(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveAppointment");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveAppointment"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "TimeProfile", "Unable to call method 'RemoveAppointment':" + ex.Message);
		}
		return null;
	}

	public RemoveAppointmentServerTask RemoveAppointment()
	{
		try
		{
			return new RemoveAppointmentServerTask(base.Wrapper.ExecuteMethod("RemoveAppointment"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "TimeProfile", "Unable to call method 'RemoveAppointment':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddRecurringAppointment(string subject, string recurrenceOccurrenceStartTime, string recurrenceOccurrenceDuration, string recurrencePatternFrequency, int recurrencePatternInterval, int recurrencePatternDaysOfWeek, int recurrencePatternDayOfMonth, int recurrencePatternMonthOfYear, string recurrencePatternOccurrenceOfDayInMonth, string recurrencePatternType, DateTime recurrenceRangeStartDate, DateTime recurrenceRangeEndDate, string recurrenceRangeLimit, int recurrenceRangeMaxOccurrences)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddRecurringAppointment");
			configurationApiItemWrapper.SetProperty("Subject", subject);
			configurationApiItemWrapper.SetProperty("RecurrenceOccurrenceStartTime", recurrenceOccurrenceStartTime);
			configurationApiItemWrapper.SetProperty("RecurrenceOccurrenceDuration", recurrenceOccurrenceDuration);
			configurationApiItemWrapper.SetProperty("RecurrencePatternFrequency", recurrencePatternFrequency);
			configurationApiItemWrapper.SetProperty("RecurrencePatternInterval", recurrencePatternInterval.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("RecurrencePatternDaysOfWeek", recurrencePatternDaysOfWeek.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("RecurrencePatternDayOfMonth", recurrencePatternDayOfMonth.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("RecurrencePatternMonthOfYear", recurrencePatternMonthOfYear.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("RecurrencePatternOccurrenceOfDayInMonth", recurrencePatternOccurrenceOfDayInMonth);
			configurationApiItemWrapper.SetProperty("RecurrencePatternType", recurrencePatternType);
			configurationApiItemWrapper.SetProperty("RecurrenceRangeStartDate", TimeConvert.ToGmtString(recurrenceRangeStartDate));
			configurationApiItemWrapper.SetProperty("RecurrenceRangeEndDate", TimeConvert.ToGmtString(recurrenceRangeEndDate));
			configurationApiItemWrapper.SetProperty("RecurrenceRangeLimit", recurrenceRangeLimit);
			configurationApiItemWrapper.SetProperty("RecurrenceRangeMaxOccurrences", recurrenceRangeMaxOccurrences.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddRecurringAppointment"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "TimeProfile", "Unable to call method 'AddRecurringAppointment':" + ex.Message);
		}
		return null;
	}

	public AddRecurringAppointmentServerTask AddRecurringAppointment()
	{
		try
		{
			return new AddRecurringAppointmentServerTask(base.Wrapper.ExecuteMethod("AddRecurringAppointment"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "TimeProfile", "Unable to call method 'AddRecurringAppointment':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveRecurringAppointment(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveRecurringAppointment");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveRecurringAppointment"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "TimeProfile", "Unable to call method 'RemoveRecurringAppointment':" + ex.Message);
		}
		return null;
	}

	public RemoveRecurringAppointmentServerTask RemoveRecurringAppointment()
	{
		try
		{
			return new RemoveRecurringAppointmentServerTask(base.Wrapper.ExecuteMethod("RemoveRecurringAppointment"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "TimeProfile", "Unable to call method 'RemoveRecurringAppointment':" + ex.Message);
		}
		return null;
	}
}
