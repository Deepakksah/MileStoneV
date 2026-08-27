using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public class FailoverRecorderPropertyDefinitions
{
	public static Property UdpPortNumberProperty(int portNumber)
	{
		return new Property("UdpPort", PropertyTypesHelper.GetValue(portNumber))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyFailoverServerUdpPortNumber,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyFailoverServerUdpPortNumber",
			ToolTipTranslationId = "PropertyFailoverServerUdpPortNumberToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GroupPositionProperty(int position, bool isSettable)
	{
		return new Property("Position", PropertyTypesHelper.GetValue(position))
		{
			ValueType = "Int",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyFailoverServerPosition,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyFailoverServerPosition",
			ToolTipTranslationId = "PropertyFailoverServerPositionToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property DatabasePathProperty(string path)
	{
		return new Property("DatabasePath", path)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyFailoverServerDatabasePath,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyFailoverServerDatabasePath",
			ToolTipTranslationId = "PropertyFailoverServerDatabasePathToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
