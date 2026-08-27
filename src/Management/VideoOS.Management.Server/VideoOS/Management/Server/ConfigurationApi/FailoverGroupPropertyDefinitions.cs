using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public class FailoverGroupPropertyDefinitions
{
	public const string FailoverServerKey = "FailoverServer";

	public const string FailoverGroupKey = "FailoverGroup";

	public static Property SelectFailoverServerProperty()
	{
		Property property = new Property("FailoverServer", "");
		property.ValueType = "Path";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.PropertyFailoverServer;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyFailoverServer";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 1;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "FailoverRecorder"
			}
		};
		return property;
	}

	public static Property SelectFailoverGroupProperty()
	{
		Property property = new Property("FailoverGroup", "");
		property.ValueType = "Path";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.PropertyMoveFailoverGroup;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyMoveFailoverGroup";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 1;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "FailoverGroup"
			}
		};
		return property;
	}
}
