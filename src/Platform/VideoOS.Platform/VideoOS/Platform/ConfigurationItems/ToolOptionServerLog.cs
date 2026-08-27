using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ToolOptionServerLog : IConfigurationItem
{
	public bool SystemLogEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("SystemLogEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public int SystemDaysToKeep
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("SystemDaysToKeep");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("SystemDaysToKeep", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> SystemLogLevelValues => base.Wrapper.GetEnumValues("SystemLogLevel");

	public string SystemLogLevel
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SystemLogLevel");
		}
		set
		{
			base.Wrapper.SetPropertyValue("SystemLogLevel", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AuditLogEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AuditLogEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public int AuditDaysToKeep
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AuditDaysToKeep");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AuditDaysToKeep", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AuditUserAccessLogging
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AuditUserAccessLogging");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AuditUserAccessLogging", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public int AuditPlaybackSequenceLogLength
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AuditPlaybackSequenceLogLength");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AuditPlaybackSequenceLogLength", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int AuditRecordsSeenBeforeLogging
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AuditRecordsSeenBeforeLogging");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AuditRecordsSeenBeforeLogging", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool RuleLogEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RuleLogEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RuleLogEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public int RuleDaysToKeep
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RuleDaysToKeep");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RuleDaysToKeep", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public ToolOptionServerLog(ServerId serverId, ConfigurationItem toolOptionServerLog)
		: base(serverId, toolOptionServerLog)
	{
	}

	public ToolOptionServerLog(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ToolOptionServerLog(FQID fqid)
		: base(fqid)
	{
	}
}
