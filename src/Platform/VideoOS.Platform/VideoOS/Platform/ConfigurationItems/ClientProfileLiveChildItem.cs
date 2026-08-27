using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientProfileLiveChildItem : IConfigurationChildItem
{
	public Dictionary<string, string> PanesLiveTabValues => base.Wrapper.GetEnumValues("PanesLiveTab");

	public string PanesLiveTab
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesLiveTab");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesLiveTab", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesLiveViewsValues => base.Wrapper.GetEnumValues("PanesLiveViews");

	public string PanesLiveViews
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesLiveViews");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesLiveViews", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesLiveViewsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesLiveViewsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesLiveViewsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesLiveSystemOverviewValues => base.Wrapper.GetEnumValues("PanesLiveSystemOverview");

	public string PanesLiveSystemOverview
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesLiveSystemOverview");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesLiveSystemOverview", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesLiveSystemOverviewLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesLiveSystemOverviewLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesLiveSystemOverviewLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesLiveAudioValues => base.Wrapper.GetEnumValues("PanesLiveAudio");

	public string PanesLiveAudio
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesLiveAudio");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesLiveAudio", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesLiveAudioLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesLiveAudioLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesLiveAudioLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesLiveOutputsValues => base.Wrapper.GetEnumValues("PanesLiveOutputs");

	public string PanesLiveOutputs
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesLiveOutputs");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesLiveOutputs", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesLiveOutputsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesLiveOutputsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesLiveOutputsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesLiveEventsValues => base.Wrapper.GetEnumValues("PanesLiveEvents");

	public string PanesLiveEvents
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesLiveEvents");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesLiveEvents", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesLiveEventsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesLiveEventsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesLiveEventsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsLiveBookmarkModeValues => base.Wrapper.GetEnumValues("FunctionsLiveBookmarkMode");

	public string FunctionsLiveBookmarkMode
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsLiveBookmarkMode");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsLiveBookmarkMode", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool FunctionsLiveBookmarkModeLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FunctionsLiveBookmarkModeLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsLiveBookmarkModeLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsLivePrintAvailabilityValues => base.Wrapper.GetEnumValues("FunctionsLivePrintAvailability");

	public string FunctionsLivePrintAvailability
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsLivePrintAvailability");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsLivePrintAvailability", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool FunctionsLivePrintAvailabilityLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FunctionsLivePrintAvailabilityLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsLivePrintAvailabilityLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsLiveCameraPlaybackValues => base.Wrapper.GetEnumValues("FunctionsLiveCameraPlayback");

	public string FunctionsLiveCameraPlayback
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsLiveCameraPlayback");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsLiveCameraPlayback", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool FunctionsLiveCameraPlaybackLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FunctionsLiveCameraPlaybackLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsLiveCameraPlaybackLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsLiveOverlayButtonsValues => base.Wrapper.GetEnumValues("FunctionsLiveOverlayButtons");

	public string FunctionsLiveOverlayButtons
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsLiveOverlayButtons");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsLiveOverlayButtons", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool FunctionsLiveOverlayButtonsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FunctionsLiveOverlayButtonsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsLiveOverlayButtonsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsLiveBoundingBoxesValues => base.Wrapper.GetEnumValues("FunctionsLiveBoundingBoxes");

	public string FunctionsLiveBoundingBoxes
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsLiveBoundingBoxes");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsLiveBoundingBoxes", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool FunctionsLiveBoundingBoxesLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FunctionsLiveBoundingBoxesLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsLiveBoundingBoxesLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesLiveMIPPluginValues => base.Wrapper.GetEnumValues("PanesLiveMIPPlugin");

	public string PanesLiveMIPPlugin
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesLiveMIPPlugin");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesLiveMIPPlugin", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesLiveMIPPluginLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesLiveMIPPluginLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesLiveMIPPluginLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public ClientProfileLiveChildItem(ServerId serverId, ConfigurationItem clientProfileLiveChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, clientProfileLiveChildItem, parentWrapper)
	{
	}

	public ClientProfileLiveChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientProfileLiveChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
