using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AlarmDefinition : IConfigurationItem
{
	public bool Enabled
	{
		get
		{
			return base.Wrapper.ConfigurationItem.EnableProperty.Enabled;
		}
		set
		{
			base.Wrapper.ConfigurationItem.EnableProperty.Enabled = value;
			base.Wrapper.IsDirty = true;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public Dictionary<string, string> EventTypeGroupValues => base.Wrapper.GetEnumValues("EventTypeGroup");

	public string EventTypeGroup
	{
		get
		{
			return base.Wrapper.GetPropertyValue("EventTypeGroup");
		}
		set
		{
			base.Wrapper.SetPropertyValue("EventTypeGroup", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> EventTypeValues => base.Wrapper.GetEnumValues("EventType");

	public string EventType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("EventType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("EventType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string SourceList
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SourceList");
		}
		set
		{
			base.Wrapper.SetPropertyValue("SourceList", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> EnableRuleValues => base.Wrapper.GetEnumValues("EnableRule");

	public string EnableRule
	{
		get
		{
			return base.Wrapper.GetPropertyValue("EnableRule");
		}
		set
		{
			base.Wrapper.SetPropertyValue("EnableRule", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string TimeProfile
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TimeProfile");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimeProfile", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string EnableEventList
	{
		get
		{
			return base.Wrapper.GetPropertyValue("EnableEventList");
		}
		set
		{
			base.Wrapper.SetPropertyValue("EnableEventList", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string DisableEventList
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DisableEventList");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DisableEventList", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ManagementTimeoutTime
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ManagementTimeoutTime");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ManagementTimeoutTime", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ManagementTimeoutEventList
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ManagementTimeoutEventList");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ManagementTimeoutEventList", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string RelatedCameraList
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RelatedCameraList");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RelatedCameraList", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> MapTypeValues => base.Wrapper.GetEnumValues("MapType");

	public string MapType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("MapType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("MapType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> RelatedMapValues => base.Wrapper.GetEnumValues("RelatedMap");

	public string RelatedMap
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RelatedMap");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RelatedMap", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string Owner
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Owner");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Owner", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PriorityValues => base.Wrapper.GetEnumValues("Priority");

	public string Priority
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Priority");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Priority", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> CategoryValues => base.Wrapper.GetEnumValues("Category");

	public string Category
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Category");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Category", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string TriggerEventlist
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TriggerEventlist");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TriggerEventlist", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AutoClose
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AutoClose");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AutoClose", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AssignableToAdmins
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AssignableToAdmins");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AssignableToAdmins", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public AlarmDefinition(ServerId serverId, ConfigurationItem alarmDefinition)
		: base(serverId, alarmDefinition)
	{
	}

	public AlarmDefinition(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public AlarmDefinition(FQID fqid)
		: base(fqid)
	{
	}
}
