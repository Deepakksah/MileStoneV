using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;

namespace VideoOS.Management.Server.ConfigurationApi;

public abstract class NewSecurityConfigurationApi : SecurityConfigurationApiBase
{
	private static Dictionary<string, List<string>> _itemTypeActionsWithTimeProfile = new Dictionary<string, List<string>>
	{
		{
			"VideoWall",
			new List<string> { "OPERATE", "PLAYBACK" }
		},
		{
			"UserDefinedEvent",
			new List<string> { "TRIGGER" }
		}
	};

	protected override ConfigurationItem ChangeSecurityPermissions(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath userPath)
	{
		IGeminiSecurityNamespace securityNamespaceFromToken = requestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(requestContext, path.Id);
		if (securityNamespaceFromToken == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		List<AccessControlEntry> list = new List<AccessControlEntry>();
		List<AccessControlEntry> list2 = new List<AccessControlEntry>();
		Dictionary<string, AccessControlTimeProfile> dictionary = new Dictionary<string, AccessControlTimeProfile>();
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (property.Key.Equals("UserPath", StringComparison.Ordinal))
			{
				continue;
			}
			if (property.Key.EndsWith("TimeProfile", StringComparison.Ordinal))
			{
				SecurityConfigurationApiBase.ValidateDeviceTimeProfileProperty(requestContext, property);
				string key = property.Key.Substring(0, property.Key.Length - 11);
				string timeProfileId = null;
				bool isDefaultTimeProfile;
				if (property.Value == "Default")
				{
					isDefaultTimeProfile = true;
				}
				else if (property.Value == "Always")
				{
					isDefaultTimeProfile = false;
				}
				else
				{
					isDefaultTimeProfile = false;
					timeProfileId = ConfigurationItemPath.CreatePath(property.Value).Id;
				}
				dictionary.Add(key, new AccessControlTimeProfile
				{
					IsDefaultTimeProfile = isDefaultTimeProfile,
					TimeProfileId = timeProfileId
				});
				continue;
			}
			if (!securityNamespaceFromToken.NamespaceInfo.Actions.Any((ActionDefinition f) => f.Name.Equals(property.Key)))
			{
				throw new PropertyInvalidException("Security property not supported on configuration item.", property.Key);
			}
			try
			{
				bool removed;
				AccessControlEntry item2 = SecurityConfigurationApiBase.CreateAccessControlEntryFromProperty(property, userPath.Id, out removed);
				if (removed)
				{
					list2.Add(item2);
				}
				else
				{
					list.Add(item2);
				}
			}
			catch (FormatException)
			{
				throw new PropertyInvalidException("Invalid property", property.Key);
			}
		}
		string objectName = requestContext.GetNameLookupService().GetObjectName(requestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceId, path.Id);
		if (list.Count > 0)
		{
			requestContext.GetService<SecuredSecurityManager>().AddPermissions(requestContext, securityNamespaceFromToken, path.Id, objectName, list.ToArray());
		}
		if (list2.Count > 0)
		{
			foreach (AccessControlEntry item3 in list2)
			{
				dictionary.Remove(item3.ActionId);
			}
			requestContext.GetService<SecuredSecurityManager>().RemovePermissions(requestContext, securityNamespaceFromToken, path.Id, objectName, list2.ToArray());
		}
		if (dictionary.Any())
		{
			UpdateTimeProfiles(requestContext, dictionary, path, userPath, objectName);
		}
		securityNamespaceFromToken.ForceNamespaceReload(requestContext);
		return ConstructPermissionChangeResult(requestContext, item, path, userPath, securityNamespaceFromToken);
	}

	private static void UpdateTimeProfiles(IGeminiRequestContext requestContext, Dictionary<string, AccessControlTimeProfile> timeProfileChanges, ConfigurationItemPath path, ConfigurationItemPath userPath, string objectName)
	{
		SecuredSecurityManager service = requestContext.GetService<SecuredSecurityManager>();
		IGeminiSecurityNamespace securityNamespaceFromToken = requestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(requestContext, path.Id);
		foreach (string key in timeProfileChanges.Keys)
		{
			AccessControlTimeProfile accessControlTimeProfile = timeProfileChanges[key];
			service.AddUpdateAccessControlTimeProfileEntry(requestContext, securityNamespaceFromToken, path.Id, objectName, userPath.Id, key, accessControlTimeProfile.TimeProfileId, accessControlTimeProfile.IsDefaultTimeProfile);
		}
	}

	protected override Property[] CreateSecurityPermissionProperties(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, ConfigurationItemPath objectPath, string userId, bool isSettable)
	{
		string id = objectPath.Id;
		List<Tuple<AccessControlEntry, AccessControlTimeProfile>> list = requestContext.GetService<SecurityManagerService>().QueryAccessEntryAndTimeProfile(requestContext, securityNamespace.NamespaceInfo.NamespaceGuid, id, userId);
		ValueTypeInfo[] timeProfileEnum = BuildTimeProfileEnum(requestContext);
		List<Property> list2 = new List<Property>();
		foreach (Tuple<AccessControlEntry, AccessControlTimeProfile> item in list)
		{
			if (ItemTypeActionsToExclude[objectPath.ItemType].Contains(item.Item1.ActionId))
			{
				continue;
			}
			list2.Add(CreateAccessControlEntryProperty(item.Item1, securityNamespace.NamespaceInfo.Name, isSettable));
			if (_itemTypeActionsWithTimeProfile[objectPath.ItemType].Contains(item.Item1.ActionId))
			{
				string propertyKey = item.Item1.ActionId + "TimeProfile";
				if (item.Item2 == null)
				{
					list2.Add(SecurityConfigurationApiBase.SecurityActionTimeProfileProperty("Default", securityNamespace.NamespaceInfo.Name, propertyKey, isSettable: true, timeProfileEnum));
					continue;
				}
				string timeProfileId = (item.Item2.IsDefaultTimeProfile ? "Default" : ((item.Item2.TimeProfileId == null) ? "Always" : ConfigurationItemPath.CreateItemPath("TimeProfile", item.Item2.TimeProfileId).Text));
				list2.Add(SecurityConfigurationApiBase.SecurityActionTimeProfileProperty(timeProfileId, securityNamespace.NamespaceInfo.Name, propertyKey, isSettable: true, timeProfileEnum));
			}
		}
		foreach (ActionDefinition actionDefinition in securityNamespace.NamespaceInfo.Actions)
		{
			if (!ItemTypeActionsToExclude[objectPath.ItemType].Contains(actionDefinition.Name) && !list2.Any((Property f) => f.Key.Equals(actionDefinition.Name, StringComparison.OrdinalIgnoreCase)))
			{
				string configuredDisplayName = securityNamespace.NamespaceInfo.Actions.FirstOrDefault((ActionDefinition a) => a.Name == actionDefinition.Name)?.DisplayName;
				list2.Add(SecurityConfigurationApiBase.SecurityActionProperty(SecurityValue.None, securityNamespace.NamespaceInfo.Name, actionDefinition.Name, isSettable, configuredDisplayName));
			}
		}
		return list2.ToArray();
	}

	private static ValueTypeInfo[] BuildTimeProfileEnum(IGeminiRequestContext requestContext)
	{
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		list.Add(new ValueTypeInfo
		{
			Name = "Always",
			Value = "Always"
		});
		list.Add(new ValueTypeInfo
		{
			Name = "Default",
			Value = "Default"
		});
		TimeProfile[] array = requestContext.CreateTimeProfileComponent().QueryTimeProfiles();
		foreach (TimeProfile timeProfile in array)
		{
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("TimeProfile", timeProfile.Id.ToLower(CultureInfo.InvariantCulture));
			list.Add(new ValueTypeInfo
			{
				Name = timeProfile.Name,
				Value = configurationItemPath.Text
			});
		}
		return list.ToArray();
	}

	private static Property CreateAccessControlEntryProperty(AccessControlEntry accessControlEntry, string securityNamespaceName, bool isSettable)
	{
		SecurityValue value = SecurityValue.Allow;
		if (accessControlEntry.AccessDeny)
		{
			value = SecurityValue.None;
		}
		return SecurityConfigurationApiBase.SecurityActionProperty(value, securityNamespaceName, accessControlEntry.ActionId, isSettable, "");
	}
}
