using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddRecurringAppointmentServerTask : ServerTask
{
	public string AppointmentRootId => base.Wrapper.GetPropertyValue("AppointmentRootId");

	public string Subject
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Subject");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Subject", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string RecurrenceOccurrenceStartTime
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RecurrenceOccurrenceStartTime");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecurrenceOccurrenceStartTime", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string RecurrenceOccurrenceDuration
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RecurrenceOccurrenceDuration");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecurrenceOccurrenceDuration", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> RecurrencePatternFrequencyValues => base.Wrapper.GetEnumValues("RecurrencePatternFrequency");

	public string RecurrencePatternFrequency
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RecurrencePatternFrequency");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecurrencePatternFrequency", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int RecurrencePatternInterval
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RecurrencePatternInterval");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecurrencePatternInterval", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int RecurrencePatternDaysOfWeek
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RecurrencePatternDaysOfWeek");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecurrencePatternDaysOfWeek", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int RecurrencePatternDayOfMonth
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RecurrencePatternDayOfMonth");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecurrencePatternDayOfMonth", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int RecurrencePatternMonthOfYear
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RecurrencePatternMonthOfYear");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecurrencePatternMonthOfYear", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> RecurrencePatternOccurrenceOfDayInMonthValues => base.Wrapper.GetEnumValues("RecurrencePatternOccurrenceOfDayInMonth");

	public string RecurrencePatternOccurrenceOfDayInMonth
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RecurrencePatternOccurrenceOfDayInMonth");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecurrencePatternOccurrenceOfDayInMonth", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> RecurrencePatternTypeValues => base.Wrapper.GetEnumValues("RecurrencePatternType");

	public string RecurrencePatternType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RecurrencePatternType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecurrencePatternType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public DateTime RecurrenceRangeStartDate
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("RecurrenceRangeStartDate"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecurrenceRangeStartDate", TimeConvert.ToGmtString(value));
			base.Wrapper.IsDirty = true;
		}
	}

	public DateTime RecurrenceRangeEndDate
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("RecurrenceRangeEndDate"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecurrenceRangeEndDate", TimeConvert.ToGmtString(value));
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> RecurrenceRangeLimitValues => base.Wrapper.GetEnumValues("RecurrenceRangeLimit");

	public string RecurrenceRangeLimit
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RecurrenceRangeLimit");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecurrenceRangeLimit", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int RecurrenceRangeMaxOccurrences
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RecurrenceRangeMaxOccurrences");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecurrenceRangeMaxOccurrences", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public AddRecurringAppointmentServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddRecurringAppointment"));
	}
}
