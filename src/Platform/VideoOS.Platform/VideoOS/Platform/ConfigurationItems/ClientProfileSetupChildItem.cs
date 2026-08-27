using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientProfileSetupChildItem : IConfigurationChildItem
{
	public Dictionary<string, string> PanesSetupTabValues => base.Wrapper.GetEnumValues("PanesSetupTab");

	public string PanesSetupTab
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesSetupTab");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesSetupTab", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesSetupViewsValues => base.Wrapper.GetEnumValues("PanesSetupViews");

	public string PanesSetupViews
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesSetupViews");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesSetupViews", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesSetupViewsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesSetupViewsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesSetupViewsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesSetupSystemOverviewValues => base.Wrapper.GetEnumValues("PanesSetupSystemOverview");

	public string PanesSetupSystemOverview
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesSetupSystemOverview");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesSetupSystemOverview", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesSetupSystemOverviewLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesSetupSystemOverviewLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesSetupSystemOverviewLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesSetupOverlayButtonsValues => base.Wrapper.GetEnumValues("PanesSetupOverlayButtons");

	public string PanesSetupOverlayButtons
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesSetupOverlayButtons");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesSetupOverlayButtons", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesSetupOverlayButtonsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesSetupOverlayButtonsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesSetupOverlayButtonsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesSetupPropertiesValues => base.Wrapper.GetEnumValues("PanesSetupProperties");

	public string PanesSetupProperties
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesSetupProperties");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesSetupProperties", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesSetupPropertiesLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesSetupPropertiesLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesSetupPropertiesLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsSetupOverlayButtonsValues => base.Wrapper.GetEnumValues("FunctionsSetupOverlayButtons");

	public string FunctionsSetupOverlayButtons
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsSetupOverlayButtons");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsSetupOverlayButtons", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool FunctionsSetupOverlayButtonsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FunctionsSetupOverlayButtonsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsSetupOverlayButtonsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsSetupEnableVideoBufferingOptionValues => base.Wrapper.GetEnumValues("FunctionsSetupEnableVideoBufferingOption");

	public string FunctionsSetupEnableVideoBufferingOption
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsSetupEnableVideoBufferingOption");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsSetupEnableVideoBufferingOption", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool FunctionsSetupEnableVideoBufferingOptionLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FunctionsSetupEnableVideoBufferingOptionLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsSetupEnableVideoBufferingOptionLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesSetupMIPPluginValues => base.Wrapper.GetEnumValues("PanesSetupMIPPlugin");

	public string PanesSetupMIPPlugin
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesSetupMIPPlugin");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesSetupMIPPlugin", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesSetupMIPPluginLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesSetupMIPPluginLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesSetupMIPPluginLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> MapEditMapsValues => base.Wrapper.GetEnumValues("MapEditMaps");

	public string MapEditMaps
	{
		get
		{
			return base.Wrapper.GetPropertyValue("MapEditMaps");
		}
		set
		{
			base.Wrapper.SetPropertyValue("MapEditMaps", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> MapEditGisMapValues => base.Wrapper.GetEnumValues("MapEditGisMap");

	public string MapEditGisMap
	{
		get
		{
			return base.Wrapper.GetPropertyValue("MapEditGisMap");
		}
		set
		{
			base.Wrapper.SetPropertyValue("MapEditGisMap", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ClientProfileSetupChildItem(ServerId serverId, ConfigurationItem clientProfileSetupChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, clientProfileSetupChildItem, parentWrapper)
	{
	}

	public ClientProfileSetupChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientProfileSetupChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
