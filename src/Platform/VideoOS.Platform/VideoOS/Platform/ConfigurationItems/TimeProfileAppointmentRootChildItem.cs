using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class TimeProfileAppointmentRootChildItem : IConfigurationChildItem
{
	public bool AllDayEvent
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AllDayEvent");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AllDayEvent", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public DateTime OriginalStartDateTime
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("OriginalStartDateTime"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public DateTime StartDateTime
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("StartDateTime"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
		set
		{
			base.Wrapper.SetPropertyValue("StartDateTime", TimeConvert.ToGmtString(value));
			base.Wrapper.IsDirty = true;
		}
	}

	public DateTime EndDateTime
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("EndDateTime"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
		set
		{
			base.Wrapper.SetPropertyValue("EndDateTime", TimeConvert.ToGmtString(value));
			base.Wrapper.IsDirty = true;
		}
	}

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

	public TimeProfileAppointmentRootChildItem(ServerId serverId, ConfigurationItem timeProfileAppointmentRootChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, timeProfileAppointmentRootChildItem, parentWrapper)
	{
	}

	public TimeProfileAppointmentRootChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public TimeProfileAppointmentRootChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
