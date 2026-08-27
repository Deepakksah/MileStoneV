using System;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class SitePropertyDefinition
{
	public const string PropertySiteConnectionState = "ConnectionState";

	public const string PropertySiteAddressList = "AddressList";

	public static Property ConnectionState(SiteState state)
	{
		return new Property("ConnectionState", state.ToString())
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertySiteConnectionState,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertySiteConnectionState",
			ToolTipTranslationId = "",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property AddressList(ServerAddress[] serverAddresses)
	{
		if (serverAddresses == null)
		{
			throw new ArgumentNullException("serverAddresses");
		}
		string text = "";
		foreach (ServerAddress serverAddress in serverAddresses)
		{
			text = text + serverAddress.URL + ",";
		}
		text = text.Trim(',');
		return new Property("ConnectionState", text)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertySiteAddressList,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertySiteAddressList",
			ToolTipTranslationId = "",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
