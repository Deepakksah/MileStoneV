using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientProfilePlaybackChildItem : IConfigurationChildItem
{
	public Dictionary<string, string> PanesPlaybackTabValues => base.Wrapper.GetEnumValues("PanesPlaybackTab");

	public string PanesPlaybackTab
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesPlaybackTab");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesPlaybackTab", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesPlaybackViewsValues => base.Wrapper.GetEnumValues("PanesPlaybackViews");

	public string PanesPlaybackViews
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesPlaybackViews");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesPlaybackViews", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesPlaybackViewsLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesPlaybackViewsLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesPlaybackViewsLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesPlaybackSystemOverviewValues => base.Wrapper.GetEnumValues("PanesPlaybackSystemOverview");

	public string PanesPlaybackSystemOverview
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesPlaybackSystemOverview");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesPlaybackSystemOverview", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesPlaybackSystemOverviewLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesPlaybackSystemOverviewLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesPlaybackSystemOverviewLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesPlaybackAudioValues => base.Wrapper.GetEnumValues("PanesPlaybackAudio");

	public string PanesPlaybackAudio
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesPlaybackAudio");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesPlaybackAudio", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesPlaybackAudioLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesPlaybackAudioLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesPlaybackAudioLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesPlaybackPrintValues => base.Wrapper.GetEnumValues("PanesPlaybackPrint");

	public string PanesPlaybackPrint
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesPlaybackPrint");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesPlaybackPrint", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesPlaybackPrintLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesPlaybackPrintLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesPlaybackPrintLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsPlaybackPrintReportHeadlineValues => base.Wrapper.GetEnumValues("FunctionsPlaybackPrintReportHeadline");

	public string FunctionsPlaybackPrintReportHeadline
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsPlaybackPrintReportHeadline");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsPlaybackPrintReportHeadline", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool FunctionsPlaybackPrintReportHeadlineLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FunctionsPlaybackPrintReportHeadlineLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsPlaybackPrintReportHeadlineLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string FunctionsPlaybackPrintReportHeadlineText
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsPlaybackPrintReportHeadlineText");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsPlaybackPrintReportHeadlineText", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsPlaybackPrintReportUserInformationValues => base.Wrapper.GetEnumValues("FunctionsPlaybackPrintReportUserInformation");

	public string FunctionsPlaybackPrintReportUserInformation
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsPlaybackPrintReportUserInformation");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsPlaybackPrintReportUserInformation", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsPlaybackPrintReportCopyRightsValues => base.Wrapper.GetEnumValues("FunctionsPlaybackPrintReportCopyRights");

	public string FunctionsPlaybackPrintReportCopyRights
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsPlaybackPrintReportCopyRights");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsPlaybackPrintReportCopyRights", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsPlaybackIndependentPlaybackValues => base.Wrapper.GetEnumValues("FunctionsPlaybackIndependentPlayback");

	public string FunctionsPlaybackIndependentPlayback
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsPlaybackIndependentPlayback");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsPlaybackIndependentPlayback", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsPlaybackBookmarkModeValues => base.Wrapper.GetEnumValues("FunctionsPlaybackBookmarkMode");

	public string FunctionsPlaybackBookmarkMode
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsPlaybackBookmarkMode");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsPlaybackBookmarkMode", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool FunctionsPlaybackBookmarkModeLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FunctionsPlaybackBookmarkModeLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsPlaybackBookmarkModeLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> FunctionsPlaybackBoundingBoxesValues => base.Wrapper.GetEnumValues("FunctionsPlaybackBoundingBoxes");

	public string FunctionsPlaybackBoundingBoxes
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FunctionsPlaybackBoundingBoxes");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsPlaybackBoundingBoxes", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool FunctionsPlaybackBoundingBoxesLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FunctionsPlaybackBoundingBoxesLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FunctionsPlaybackBoundingBoxesLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PanesPlaybackMIPPluginValues => base.Wrapper.GetEnumValues("PanesPlaybackMIPPlugin");

	public string PanesPlaybackMIPPlugin
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PanesPlaybackMIPPlugin");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesPlaybackMIPPlugin", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PanesPlaybackMIPPluginLocked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PanesPlaybackMIPPluginLocked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PanesPlaybackMIPPluginLocked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public ClientProfilePlaybackChildItem(ServerId serverId, ConfigurationItem clientProfilePlaybackChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, clientProfilePlaybackChildItem, parentWrapper)
	{
	}

	public ClientProfilePlaybackChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientProfilePlaybackChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
