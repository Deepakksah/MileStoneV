using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ToolOptionRecordingServer : IConfigurationItem
{
	public int PtzManualSessionTimeoutSec
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PtzManualSessionTimeoutSec");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PtzManualSessionTimeoutSec", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int PausePatrollingTimeoutSec
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PausePatrollingTimeoutSec");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PausePatrollingTimeoutSec", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int PtzReservedSessionTimeoutSec
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PtzReservedSessionTimeoutSec");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PtzReservedSessionTimeoutSec", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PtzUseDefaultPresetAsPtzHome
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PtzUseDefaultPresetAsPtzHome");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PtzUseDefaultPresetAsPtzHome", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> DeviceErrorTimeoutSecValues => base.Wrapper.GetEnumValues("DeviceErrorTimeoutSec");

	public string DeviceErrorTimeoutSec
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DeviceErrorTimeoutSec");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DeviceErrorTimeoutSec", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ToolOptionRecordingServer(ServerId serverId, ConfigurationItem toolOptionRecordingServer)
		: base(serverId, toolOptionRecordingServer)
	{
	}

	public ToolOptionRecordingServer(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ToolOptionRecordingServer(FQID fqid)
		: base(fqid)
	{
	}
}
