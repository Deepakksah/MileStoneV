using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using VideoOS.Common.Integration.Security;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public abstract class SecurityConfigurationApiBase : ConfigurationApiBase
{
	public enum SecurityValue
	{
		None,
		Allow
	}

	public const string UserPath = "UserPath";

	protected abstract Dictionary<string, List<string>> ItemTypeActionsToExclude { get; }

	public abstract bool SetSecurityOnRoleOnly();

	protected abstract Property[] CreateSecurityPermissionProperties(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, ConfigurationItemPath objectPath, string userId, bool isSettable);

	protected abstract ConfigurationItem ChangeSecurityPermissions(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath userPath);

	public ConfigurationItem InvokeSecurityMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		if (methodId.Equals("ChangeSecurityPermissions", StringComparison.Ordinal) && !path.IsFolder)
		{
			if (!flag)
			{
				return CreateInvokeInfoItemChangeSecurityPermissionsUserSelection(item, methodId);
			}
			string value = null;
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("UserPath", out value))
			{
				throw new PropertyInvalidException("User path property missing.");
			}
			if (string.IsNullOrEmpty(value))
			{
				throw new PropertyInvalidException("User not found", "UserPath");
			}
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(value);
			if (SetSecurityOnRoleOnly())
			{
				if (!Guid.TryParse(configurationItemPath.Id, out var result))
				{
					throw new PropertyInvalidException("Role not found", "UserPath");
				}
				if ((Role.QueryRoleById(requestContext, result.ToString()) ?? throw new PropertyInvalidException("Role not found", "UserPath")).Id.Equals(SecurityManager.AdministratorRoleId.ToString(), StringComparison.OrdinalIgnoreCase))
				{
					throw new NotSupportedException("Not possible to set security on administrator role.");
				}
			}
			else if (IdentityCache.ReadIdentityFromCache(requestContext, configurationItemPath.Id, QueryMembership.Expanded) == null)
			{
				throw new PropertyInvalidException("User not found", "UserPath");
			}
			if (item.Properties.Length == 1)
			{
				ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreatePath(item.ParentPath);
				IGeminiSecurityNamespace geminiSecurityNamespace;
				try
				{
					geminiSecurityNamespace = ((!configurationItemPath2.IsFolder || !(configurationItemPath2.ParentItemType == "MIPKind")) ? requestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(requestContext, path.Id) : requestContext.GetService<SecurityManagerService>().GetSecurityNamespace(requestContext, new Guid(configurationItemPath2.Id)));
				}
				catch (SecurityObjectDoesNotExistException)
				{
					throw new PathNotFoundException(path.Text);
				}
				if (geminiSecurityNamespace == null)
				{
					throw new PathNotFoundException(path.Text);
				}
				return CreateInvokeInfoItemChangeSecurityPermissions(requestContext, item, path, methodId, configurationItemPath, geminiSecurityNamespace);
			}
			return ChangeSecurityPermissions(requestContext, item, path, configurationItemPath);
		}
		throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
	}

	public static string[] AddSecurityMethods(string[] existingMethodIds)
	{
		List<string> list = new List<string>();
		if (existingMethodIds != null && existingMethodIds.Length != 0)
		{
			list.AddRange(existingMethodIds);
		}
		list.Add("ChangeSecurityPermissions");
		return list.ToArray();
	}

	private ConfigurationItem CreateInvokeInfoItemChangeSecurityPermissions(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId, ConfigurationItemPath userPath, IGeminiSecurityNamespace securityNamespace)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodChangeSecurityPermissions;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		list.Add(UserPathProperty(userPath.Text, isSettable: false));
		try
		{
			list.AddRange(CreateSecurityPermissionProperties(requestContext, securityNamespace, path, userPath.Id, isSettable: true));
			configurationItem.Properties = list.ToArray();
			configurationItem.MethodIds = new string[1] { methodId };
			return configurationItem;
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	private static ConfigurationItem CreateInvokeInfoItemChangeSecurityPermissionsUserSelection(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodChangeSecurityPermissions;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.Properties = new Property[1] { UserPathProperty(string.Empty, isSettable: true) };
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	protected ConfigurationItem ConstructPermissionChangeResult(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath userPath, IGeminiSecurityNamespace securityNamespace)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeResult";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodChangeSecurityPermissions;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		list.Add(UserPathProperty(userPath.Text, isSettable: false));
		list.AddRange(CreateSecurityPermissionProperties(requestContext, securityNamespace, path, userPath.Id, isSettable: false));
		foreach (Property item2 in list)
		{
			item2.IsSettable = false;
		}
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[0];
		return configurationItem;
	}

	protected static AccessControlEntry CreateAccessControlEntryFromProperty(Property property, string userId, out bool removed)
	{
		AccessControlEntry accessControlEntry = new AccessControlEntry();
		SecurityValue securityValue = (bool.Parse(property.Value) ? SecurityValue.Allow : SecurityValue.None);
		accessControlEntry.AccessDeny = false;
		accessControlEntry.ActionId = property.Key;
		accessControlEntry.Sid = userId;
		removed = securityValue == SecurityValue.None;
		return accessControlEntry;
	}

	public static Property UserPathProperty(string rolePath, bool isSettable)
	{
		Property property = new Property("UserPath", rolePath);
		property.ValueType = "Path";
		property.IsSettable = isSettable;
		property.DisplayName = ConfigurationApiResources.PropertySecurityUserPath;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertySecurityUserPath";
		property.ToolTipTranslationId = "PropertySecurityUserPathTooltip";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "Role"
			}
		};
		return property;
	}

	public static Property SecurityActionProperty(SecurityValue value, string securityNamespaceName, string actionId, bool isSettable, string configuredDisplayName)
	{
		FormatResourceIds(securityNamespaceName, actionId, out var resourceKeyAction, out var resourceKeyTooltip, out var displayName);
		bool value2 = value == SecurityValue.Allow;
		return new Property(actionId, PropertyTypesHelper.GetValue(value2))
		{
			ValueType = "Tick",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiService.InsertOemNames(displayName ?? configuredDisplayName),
			ItemReference = false,
			ServerValidation = false,
			TranslationId = resourceKeyAction,
			ToolTipTranslationId = resourceKeyTooltip,
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property SecurityActionTimeProfileProperty(string timeProfileId, string securityNamespaceName, string propertyKey, bool isSettable, ValueTypeInfo[] timeProfileEnum)
	{
		FormatResourceIds(securityNamespaceName, propertyKey, out var resourceKeyAction, out var resourceKeyTooltip, out var displayName);
		return new Property(propertyKey, timeProfileId)
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiService.InsertOemNames(displayName),
			ItemReference = false,
			ServerValidation = false,
			TranslationId = resourceKeyAction,
			ToolTipTranslationId = resourceKeyTooltip,
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = timeProfileEnum
		};
	}

	protected static void ValidateDeviceTimeProfileProperty(IGeminiRequestContext requestContext, Property property)
	{
		if (property.Value == "Always" || property.Value == "Default")
		{
			return;
		}
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(property.Value);
		if (configurationItemPath.ItemType != "TimeProfile")
		{
			throw new PropertyInvalidException("Property must be: 'Default', 'Always' or a time profile path.", property.Key);
		}
		using ITimeProfileComponent timeProfileComponent = requestContext.CreateTimeProfileComponent();
		if (timeProfileComponent.QueryTimeProfile(configurationItemPath.Id) == null)
		{
			throw new PropertyInvalidException("TimeProfile does not exist.", property.Key);
		}
	}

	protected static void FormatResourceIds(string securityNamespaceName, string actionId, out string resourceKeyAction, out string resourceKeyTooltip, out string displayName)
	{
		ResourceManager resourceManager = ConfigurationApiResources.ResourceManager;
		resourceKeyAction = string.Format(CultureInfo.InvariantCulture, "PropertySecurity{0}{1}", securityNamespaceName, actionId);
		resourceKeyTooltip = string.Format(CultureInfo.InvariantCulture, "PropertySecurity{0}{1}Tooltip", securityNamespaceName, actionId);
		displayName = actionId;
		try
		{
			displayName = resourceManager.GetString(resourceKeyAction);
			if (string.IsNullOrEmpty(displayName))
			{
				displayName = resourceManager.GetString(actionId);
				resourceKeyAction = string.Format(CultureInfo.InvariantCulture, "PropertySecurityGeneric{0}", actionId);
				resourceKeyTooltip = string.Format(CultureInfo.InvariantCulture, "PropertySecurityGeneric{0}Tooltip", actionId);
			}
		}
		catch (MissingManifestResourceException)
		{
			resourceKeyAction = string.Format(CultureInfo.InvariantCulture, "PropertySecurityGeneric{0}", actionId);
			resourceKeyTooltip = string.Format(CultureInfo.InvariantCulture, "PropertySecurityGeneric{0}Tooltip", actionId);
		}
		catch (MissingSatelliteAssemblyException)
		{
			resourceKeyAction = string.Format(CultureInfo.InvariantCulture, "PropertySecurityGeneric{0}", actionId);
			resourceKeyTooltip = string.Format(CultureInfo.InvariantCulture, "PropertySecurityGeneric{0}Tooltip", actionId);
		}
	}
}
