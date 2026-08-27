using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientProfileTimelineChildItem : IConfigurationChildItem
{
	public Dictionary<string, string> TimelineIncomingAudioValues => base.Wrapper.GetEnumValues("TimelineIncomingAudio");

	public string TimelineIncomingAudio
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TimelineIncomingAudio");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineIncomingAudio", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool TimelineIncomingAudioLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimelineIncomingAudioLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineIncomingAudioLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> TimelineOutgoingAudioValues => base.Wrapper.GetEnumValues("TimelineOutgoingAudio");

	public string TimelineOutgoingAudio
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TimelineOutgoingAudio");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineOutgoingAudio", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool TimelineOutgoingAudioLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimelineOutgoingAudioLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineOutgoingAudioLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> TimelineMipDataValues => base.Wrapper.GetEnumValues("TimelineMipData");

	public string TimelineMipData
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TimelineMipData");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineMipData", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool TimelineMipDataLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimelineMipDataLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineMipDataLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> TimelineBookmarksValues => base.Wrapper.GetEnumValues("TimelineBookmarks");

	public string TimelineBookmarks
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TimelineBookmarks");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineBookmarks", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool TimelineBookmarksLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimelineBookmarksLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineBookmarksLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> TimelineMipMarkerValues => base.Wrapper.GetEnumValues("TimelineMipMarker");

	public string TimelineMipMarker
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TimelineMipMarker");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineMipMarker", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool TimelineMipMarkerLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimelineMipMarkerLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineMipMarkerLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> TimelineMotionValues => base.Wrapper.GetEnumValues("TimelineMotion");

	public string TimelineMotion
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TimelineMotion");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineMotion", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool TimelineMotionLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimelineMotionLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineMotionLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> TimelineAllCamerasTimelineValues => base.Wrapper.GetEnumValues("TimelineAllCamerasTimeline");

	public string TimelineAllCamerasTimeline
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TimelineAllCamerasTimeline");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineAllCamerasTimeline", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool TimelineAllCamerasTimelineLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimelineAllCamerasTimelineLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineAllCamerasTimelineLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> TimelineSkipGapsValues => base.Wrapper.GetEnumValues("TimelineSkipGaps");

	public string TimelineSkipGaps
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TimelineSkipGaps");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineSkipGaps", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool TimelineSkipGapsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimelineSkipGapsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineSkipGapsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> TimelineHideForNormalViewsTimeoutValues => base.Wrapper.GetEnumValues("TimelineHideForNormalViewsTimeout");

	public string TimelineHideForNormalViewsTimeout
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TimelineHideForNormalViewsTimeout");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineHideForNormalViewsTimeout", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool TimelineHideForNormalViewsTimeoutLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimelineHideForNormalViewsTimeoutLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineHideForNormalViewsTimeoutLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> TimelineHideForSmartWallViewsTimeoutValues => base.Wrapper.GetEnumValues("TimelineHideForSmartWallViewsTimeout");

	public string TimelineHideForSmartWallViewsTimeout
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TimelineHideForSmartWallViewsTimeout");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineHideForSmartWallViewsTimeout", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool TimelineHideForSmartWallViewsTimeoutLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TimelineHideForSmartWallViewsTimeoutLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimelineHideForSmartWallViewsTimeoutLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public ClientProfileTimelineChildItem(ServerId serverId, ConfigurationItem clientProfileTimelineChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, clientProfileTimelineChildItem, parentWrapper)
	{
	}

	public ClientProfileTimelineChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientProfileTimelineChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
