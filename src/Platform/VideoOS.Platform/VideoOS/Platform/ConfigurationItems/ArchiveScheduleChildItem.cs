using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ArchiveScheduleChildItem : IConfigurationChildItem
{
	public Dictionary<string, string> FrequencyTypeValues => base.Wrapper.GetEnumValues("FrequencyType");

	public string FrequencyType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FrequencyType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FrequencyType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int FrequencyRecurrenceFactor
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FrequencyRecurrenceFactor");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FrequencyRecurrenceFactor", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int FrequencyInterval
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FrequencyInterval");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FrequencyInterval", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FrequencyRelativeIntervalValues => base.Wrapper.GetEnumValues("FrequencyRelativeInterval");

	public string FrequencyRelativeInterval
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FrequencyRelativeInterval");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FrequencyRelativeInterval", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FrequencySubDayTypeValues => base.Wrapper.GetEnumValues("FrequencySubDayType");

	public string FrequencySubDayType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FrequencySubDayType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FrequencySubDayType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int FrequencySubDayInterval
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FrequencySubDayInterval");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FrequencySubDayInterval", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double ActiveStartTimeOfDay
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ActiveStartTimeOfDay");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ActiveStartTimeOfDay", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double ActiveEndTimeOfDay
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ActiveEndTimeOfDay");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ActiveEndTimeOfDay", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public ArchiveScheduleChildItem(ServerId serverId, ConfigurationItem archiveScheduleChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, archiveScheduleChildItem, parentWrapper)
	{
	}

	public ArchiveScheduleChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ArchiveScheduleChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
