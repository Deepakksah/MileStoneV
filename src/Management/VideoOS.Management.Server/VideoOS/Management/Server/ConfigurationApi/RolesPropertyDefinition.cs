using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class RolesPropertyDefinition
{
	public static Property NamespaceProperty(string value, bool isSettable, ValueTypeInfo[] valueTypes)
	{
		return new Property("SecurityNamespace", value)
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertySecurityNamespace,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertySecurityNamespace",
			ToolTipTranslationId = "PropertySecurityNamespaceTooltip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = valueTypes
		};
	}

	public static Property OverallSecurityActionProperty(OverallSecurityConfigurationApi.OverallSecurityValue value, string namespaceName, string actionId, bool isSettable)
	{
		ResourceManager resourceManager = ConfigurationApiResources.ResourceManager;
		string text = string.Format(CultureInfo.InvariantCulture, "PropertySecurity{0}{1}", namespaceName, actionId);
		string toolTipTranslationId = string.Format(CultureInfo.InvariantCulture, "PropertySecurity{0}{1}Tooltip", namespaceName, actionId);
		try
		{
			if (string.IsNullOrEmpty(resourceManager.GetString(text)))
			{
				text = string.Format(CultureInfo.InvariantCulture, "PropertySecurityGeneric{0}", actionId);
				toolTipTranslationId = string.Format(CultureInfo.InvariantCulture, "PropertySecurityGeneric{0}Tooltip", actionId);
			}
		}
		catch (MissingManifestResourceException)
		{
			text = string.Format(CultureInfo.InvariantCulture, "PropertySecurityGeneric{0}", actionId);
			toolTipTranslationId = string.Format(CultureInfo.InvariantCulture, "PropertySecurityGeneric{0}Tooltip", actionId);
		}
		catch (MissingSatelliteAssemblyException)
		{
			text = string.Format(CultureInfo.InvariantCulture, "PropertySecurityGeneric{0}", actionId);
			toolTipTranslationId = string.Format(CultureInfo.InvariantCulture, "PropertySecurityGeneric{0}Tooltip", actionId);
		}
		return new Property(actionId, PropertyTypesHelper.GetValue(value))
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiService.InsertOemNames(resourceManager.GetString(text)),
			ItemReference = false,
			ServerValidation = false,
			TranslationId = text,
			ToolTipTranslationId = toolTipTranslationId,
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(OverallSecurityConfigurationApi.OverallSecurityValue))
		};
	}

	public static Property DualAuthorizationRequiredProperty(bool dualAuthorizationRequired)
	{
		return new Property("DualAuthorizationRequired", dualAuthorizationRequired.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRoleDualAuthorizationRequired,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleDualAuthorizationRequired",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property MakeUsersAnonymousDuringPTZSessionProperty(bool makeUsersAnonymousDuringPTZSession)
	{
		return new Property("MakeUsersAnonymousDuringPTZSession", makeUsersAnonymousDuringPTZSession.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRoleMakeUsersAnonymousDuringPTZSession,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleMakeUsersAnonymousDuringPTZSession",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property RoleTypeProperty(RoleType roleType)
	{
		return new Property("RoleType", roleType.ToString())
		{
			ValueType = "Enum",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRoleType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleType",
			UIImportance = UIImportance.Hidden,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(RoleType))
		};
	}

	public static Property AllowMobileClientLogOnProperty(bool allowMobileClientLogOn)
	{
		return new Property("AllowMobileClientLogOn", allowMobileClientLogOn.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRoleAllowMobileClientLogOn.Replace("{MobileClientNameShort}", OemResources.Instance.MobileServer.MobileClientNameShort),
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleAllowMobileClientLogOn",
			ToolTipTranslationId = "PropertyRoleAllowMobileClientLogOnToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property AllowSmartClientLogOnProperty(bool allowSmartClientLogOn)
	{
		return new Property("AllowSmartClientLogOn", allowSmartClientLogOn.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRoleAllowSmartClientLogOn.Replace("{OemClientName}", OemResources.Instance.SmartClient.ClientName),
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleAllowSmartClientLogOn",
			ToolTipTranslationId = "PropertyRoleAllowSmartClientLogOnToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property AllowWebClientLogOnProperty(bool allowWebClientLogOn)
	{
		return new Property("AllowWebClientLogOn", allowWebClientLogOn.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRoleAllowWebClientLogOn.Replace("{WebClientNameShort}", OemResources.Instance.MobileServer.WebClientNameShort),
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleAllowWebClientLogOn",
			ToolTipTranslationId = "PropertyRoleAllowWebClientLogOnToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property TimeProfileProperty(string propertyName, string pathValue)
	{
		string displayName = ConfigurationApiResources.ResourceManager.GetString(propertyName);
		Property pathProperty = GeneralPropertyDefinitions.GetPathProperty(propertyName, displayName, pathValue, new List<string> { "TimeProfile" }, propertyName, isSettable: true);
		pathProperty.ToolTipTranslationId = propertyName + "ToolTip";
		return pathProperty;
	}

	public static Property ClientProfileProperty(string pathValue)
	{
		string propertyClientProfile = ConfigurationApiResources.PropertyClientProfile;
		Property pathProperty = GeneralPropertyDefinitions.GetPathProperty("ClientProfile", propertyClientProfile, pathValue, new List<string> { "ClientProfile" }, "PropertyClientProfile", isSettable: false);
		pathProperty.ToolTipTranslationId = "PropertyClientProfileToolTip";
		return pathProperty;
	}

	public static Property EvidenceLockProfileProperty(string pathValue)
	{
		string propertyEvidenceLockProfile = ConfigurationApiResources.PropertyEvidenceLockProfile;
		return GeneralPropertyDefinitions.GetPathProperty("EvidenceLockProfile", propertyEvidenceLockProfile, pathValue, new List<string> { "EvidenceLockProfile" }, "PropertyEvidenceLockProfile", isSettable: true);
	}

	public static Property QueryUsersWithAccessProperty()
	{
		Property property = new Property("QueryUsersWithAccessPath", "");
		property.ValueType = "Path";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.QueryUsersWithAccessPath;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathAllowAllItemType",
				Value = ""
			}
		};
		return property;
	}

	public static Property IncludeAdministratorsProperty()
	{
		return new Property("IncludeAdministrators", "true")
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyIncludeAdministrators,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyIncludeAdministrators",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property ExpandGroupsProperty()
	{
		return new Property("ExpandGroups", "true")
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyExpandGroups,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyExpandGroups",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property IncludeGroupsProperty()
	{
		return new Property("IncludeGroups", "false")
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyIncludeGroups,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyIncludeGroups",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property RelatedObjectPath()
	{
		Property property = new Property("RelatedObjectPath", "");
		property.ValueType = "Path";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.RelatedObjectPath;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "RelatedObjectPath";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathAllowAllItemType",
				Value = ""
			}
		};
		return property;
	}

	public static Property ActionId()
	{
		return new Property("ActionId", "")
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyActionId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyActionId",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property IsPermitted(bool isPermitted)
	{
		return new Property("IsPermitted", isPermitted.ToString())
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyIsPermitted,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyIsPermitted",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
