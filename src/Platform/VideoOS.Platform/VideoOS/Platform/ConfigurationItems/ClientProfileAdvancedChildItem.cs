using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientProfileAdvancedChildItem : IConfigurationChildItem
{
	public Dictionary<string, string> AdvancedMaxDecodingThreadsValues => base.Wrapper.GetEnumValues("AdvancedMaxDecodingThreads");

	public string AdvancedMaxDecodingThreads
	{
		get
		{
			return base.Wrapper.GetPropertyValue("AdvancedMaxDecodingThreads");
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedMaxDecodingThreads", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AdvancedMaxDecodingThreadsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AdvancedMaxDecodingThreadsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedMaxDecodingThreadsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> AdvancedDeinterlacingValues => base.Wrapper.GetEnumValues("AdvancedDeinterlacing");

	public string AdvancedDeinterlacing
	{
		get
		{
			return base.Wrapper.GetPropertyValue("AdvancedDeinterlacing");
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedDeinterlacing", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AdvancedDeinterlacingLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AdvancedDeinterlacingLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedDeinterlacingLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> AdvancedVideoPlayerDiagnosticLevelValues => base.Wrapper.GetEnumValues("AdvancedVideoPlayerDiagnosticLevel");

	public string AdvancedVideoPlayerDiagnosticLevel
	{
		get
		{
			return base.Wrapper.GetPropertyValue("AdvancedVideoPlayerDiagnosticLevel");
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedVideoPlayerDiagnosticLevel", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AdvancedVideoPlayerDiagnosticLevelLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AdvancedVideoPlayerDiagnosticLevelLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedVideoPlayerDiagnosticLevelLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> AdvancedVideoPlayerHardwareDecodingModeValues => base.Wrapper.GetEnumValues("AdvancedVideoPlayerHardwareDecodingMode");

	public string AdvancedVideoPlayerHardwareDecodingMode
	{
		get
		{
			return base.Wrapper.GetPropertyValue("AdvancedVideoPlayerHardwareDecodingMode");
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedVideoPlayerHardwareDecodingMode", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AdvancedVideoPlayerHardwareDecodingModeLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AdvancedVideoPlayerHardwareDecodingModeLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedVideoPlayerHardwareDecodingModeLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> AdvancedAdaptiveStreamingValues => base.Wrapper.GetEnumValues("AdvancedAdaptiveStreaming");

	public string AdvancedAdaptiveStreaming
	{
		get
		{
			return base.Wrapper.GetPropertyValue("AdvancedAdaptiveStreaming");
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedAdaptiveStreaming", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AdvancedAdaptiveStreamingLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AdvancedAdaptiveStreamingLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedAdaptiveStreamingLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> AdvancedTimeZoneValues => base.Wrapper.GetEnumValues("AdvancedTimeZone");

	public string AdvancedTimeZone
	{
		get
		{
			return base.Wrapper.GetPropertyValue("AdvancedTimeZone");
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedTimeZone", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AdvancedTimeZoneLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AdvancedTimeZoneLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedTimeZoneLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> AdvancedTimeZoneCustomValues => base.Wrapper.GetEnumValues("AdvancedTimeZoneCustom");

	public string AdvancedTimeZoneCustom
	{
		get
		{
			return base.Wrapper.GetPropertyValue("AdvancedTimeZoneCustom");
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedTimeZoneCustom", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AdvancedTimeZoneCustomLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AdvancedTimeZoneCustomLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedTimeZoneCustomLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> AdvancedMulticastValues => base.Wrapper.GetEnumValues("AdvancedMulticast");

	public string AdvancedMulticast
	{
		get
		{
			return base.Wrapper.GetPropertyValue("AdvancedMulticast");
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedMulticast", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AdvancedMulticastLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AdvancedMulticastLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AdvancedMulticastLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public ClientProfileAdvancedChildItem(ServerId serverId, ConfigurationItem clientProfileAdvancedChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, clientProfileAdvancedChildItem, parentWrapper)
	{
	}

	public ClientProfileAdvancedChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientProfileAdvancedChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
