using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;

namespace VideoOS.Management.Server.ConfigurationApi;

public abstract class SecurityConfigurationApi : SecurityConfigurationApiBase
{
	private const string TimeBackward_PostFix = "TimeBackward";

	private const string Priority_PostFix = "Priority";

	public const string PtzPriority_PropertyKey = "DEVICEPTZPRIORITY";

	private const string PtzPriority_PriorityValueKey = "DEVICEPTZPRIORITYPriority";

	private static readonly Dictionary<string, List<string>> _deviceTimeProfileProperties = new Dictionary<string, List<string>>
	{
		{
			"Camera",
			new List<string> { "DEVICELIVEVIEW", "DEVICERECORDEDBROWSE", "DEVICEPTZPRIORITY", "DEVICEPTZCONTROL", "DEVICEPTZPRESETS" }
		},
		{
			"Microphone",
			new List<string> { "DEVICEMICLIVEVIEW", "DEVICEMICRECORDEDBROWSE" }
		},
		{
			"Speaker",
			new List<string> { "DEVICESPEAKERLIVEVIEW", "DEVICESPEAKERRECORDEDBROWSE" }
		},
		{
			"Metadata",
			new List<string> { "DEVICEMETADATALIVEVIEW", "DEVICEMETADATARECORDEDBROWSE" }
		},
		{
			"Output",
			new List<string> { "DEVICEACTIVEOUTPUT" }
		}
	};

	protected override ConfigurationItem ChangeSecurityPermissions(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath userPath)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(item.ParentPath);
		IGeminiSecurityNamespace geminiSecurityNamespace = ((!configurationItemPath.IsFolder || !(configurationItemPath.ParentItemType == "MIPKind")) ? requestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(requestContext, path.Id) : requestContext.GetService<SecurityManagerService>().GetSecurityNamespace(requestContext, new Guid(configurationItemPath.Id)));
		if (geminiSecurityNamespace == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		string objectName = requestContext.GetNameLookupService().GetObjectName(requestContext, geminiSecurityNamespace.NamespaceInfo.NamespaceId, path.Id);
		List<string> list = (_deviceTimeProfileProperties.ContainsKey(path.ItemType) ? _deviceTimeProfileProperties[path.ItemType] : new List<string>());
		List<AccessControlEntry> list2 = new List<AccessControlEntry>();
		List<AccessControlEntry> list3 = new List<AccessControlEntry>();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		int num = 0;
		bool? flag = null;
		bool flag2 = geminiSecurityNamespace.NamespaceInfo.Actions.FirstOrDefault((ActionDefinition a) => a.Name == "PTZ_CONTROL") != null;
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (property.Key.Equals("UserPath", StringComparison.Ordinal))
			{
				continue;
			}
			if (property.Key == "DEVICEPTZPRIORITY")
			{
				if (bool.Parse(property.Value))
				{
					num = 3000;
					flag = true;
				}
				else
				{
					flag = false;
				}
				continue;
			}
			if (property.Key == "DEVICEPTZPRIORITYPriority" && !flag.HasValue)
			{
				flag = true;
			}
			string text = (property.Key.EndsWith("TimeBackward", StringComparison.Ordinal) ? property.Key.Substring(0, property.Key.Length - 12) : property.Key);
			if (text.EndsWith("Priority", StringComparison.Ordinal))
			{
				text = text.Substring(0, text.Length - 8);
			}
			if (list.Contains(text))
			{
				ValidateDeviceExtendedTimeProfileProperty(requestContext, property);
				dictionary.Add(property.Key, property.Value);
				continue;
			}
			if (!geminiSecurityNamespace.NamespaceInfo.Actions.Any((ActionDefinition f) => f.Name.Equals(property.Key)))
			{
				throw new PropertyInvalidException("Security property not supported on configuration item.", property.Key);
			}
			try
			{
				bool removed;
				AccessControlEntry item2 = SecurityConfigurationApiBase.CreateAccessControlEntryFromProperty(property, userPath.Id, out removed);
				if (removed)
				{
					list3.Add(item2);
				}
				else
				{
					list2.Add(item2);
				}
			}
			catch (FormatException)
			{
				throw new PropertyInvalidException("Invalid property", property.Key);
			}
		}
		if (list2.Count > 0 && list3.Count > 0)
		{
			requestContext.GetService<SecuredSecurityManager>().AddRemovePermissions(requestContext, geminiSecurityNamespace, path.Id, objectName, list2.ToArray(), list3.ToArray());
		}
		else if (list2.Count > 0)
		{
			requestContext.GetService<SecuredSecurityManager>().AddPermissions(requestContext, geminiSecurityNamespace, path.Id, objectName, list2.ToArray());
		}
		else if (list3.Count > 0)
		{
			requestContext.GetService<SecuredSecurityManager>().RemovePermissions(requestContext, geminiSecurityNamespace, path.Id, objectName, list3.ToArray());
		}
		if (flag.HasValue && flag.Value)
		{
			if (!dictionary.ContainsKey("DEVICEPTZPRIORITYPriority"))
			{
				dictionary.Add("DEVICEPTZPRIORITYPriority", num.ToString(CultureInfo.InvariantCulture));
			}
		}
		else if (flag2)
		{
			if (!dictionary.ContainsKey("DEVICEPTZPRIORITYPriority"))
			{
				dictionary.Add("DEVICEPTZPRIORITYPriority", "0");
			}
			else
			{
				dictionary["DEVICEPTZPRIORITYPriority"] = "0";
			}
		}
		if (dictionary.Any())
		{
			UpdateTimeBasedDeviceSecurity(requestContext, dictionary, path, userPath);
		}
		geminiSecurityNamespace.ForceNamespaceReload(requestContext);
		return ConstructPermissionChangeResult(requestContext, item, path, userPath, geminiSecurityNamespace);
	}

	private static void ValidateDeviceExtendedTimeProfileProperty(IGeminiRequestContext requestContext, Property property)
	{
		if (property.Key.EndsWith("TimeBackward", StringComparison.Ordinal) || property.Key.EndsWith("Priority", StringComparison.Ordinal))
		{
			if (!int.TryParse(property.Value, out var _))
			{
				throw new PropertyInvalidException("Property must be a number", property.Key);
			}
		}
		else
		{
			SecurityConfigurationApiBase.ValidateDeviceTimeProfileProperty(requestContext, property);
		}
	}

	private static void UpdateTimeBasedDeviceSecurity(IGeminiRequestContext requestContext, Dictionary<string, string> timeProfileChanges, ConfigurationItemPath path, ConfigurationItemPath userPath)
	{
		List<AccessControlDeviceTimeProfileEntry> source = requestContext.GetSecurityManagerService().QueryDeviceTimeProfile(requestContext, userPath.Id, path.Id);
		List<AccessControlDeviceTimeProfileEntry> list = new List<AccessControlDeviceTimeProfileEntry>();
		foreach (string key in timeProfileChanges.Keys)
		{
			string text = null;
			string value = "True";
			bool flag = false;
			bool flag2 = false;
			string baseKey = key;
			bool flag3 = false;
			bool flag4 = false;
			if (key == "DEVICEPTZPRIORITY")
			{
				flag2 = true;
				baseKey = key;
				value = timeProfileChanges[key];
			}
			else if (key.EndsWith("Priority", StringComparison.Ordinal))
			{
				flag2 = true;
				baseKey = key.Substring(0, key.Length - 8);
				value = timeProfileChanges[key];
			}
			else if (key.EndsWith("TimeBackward", StringComparison.Ordinal))
			{
				flag4 = true;
				baseKey = key.Substring(0, key.Length - 12);
			}
			else
			{
				flag3 = true;
				if (timeProfileChanges[key] == "Always")
				{
					text = null;
					flag = false;
				}
				else if (timeProfileChanges[key] == "Default")
				{
					text = null;
					flag = true;
				}
				else
				{
					text = ConfigurationItemPath.CreatePath(timeProfileChanges[key]).Id;
				}
			}
			AccessControlDeviceTimeProfileEntry accessControlDeviceTimeProfileEntry = list.FirstOrDefault((AccessControlDeviceTimeProfileEntry e) => e.ActionId == baseKey);
			bool flag5 = true;
			if (accessControlDeviceTimeProfileEntry == null)
			{
				flag5 = false;
				accessControlDeviceTimeProfileEntry = source.FirstOrDefault((AccessControlDeviceTimeProfileEntry e) => e.ActionId == baseKey);
			}
			bool flag6 = false;
			if (accessControlDeviceTimeProfileEntry == null && baseKey == "DEVICEPTZPRIORITY")
			{
				accessControlDeviceTimeProfileEntry = new AccessControlDeviceTimeProfileEntry(baseKey, new Guid(userPath.Id), new Guid(path.Id), value, null, 0, isDefaultTimeProfile: false);
			}
			if (accessControlDeviceTimeProfileEntry == null)
			{
				continue;
			}
			if (flag3)
			{
				if (accessControlDeviceTimeProfileEntry.TimeProfileId != text || accessControlDeviceTimeProfileEntry.IsDefaultTimeProfile != flag)
				{
					flag6 = true;
					accessControlDeviceTimeProfileEntry.TimeProfileId = text;
					accessControlDeviceTimeProfileEntry.IsDefaultTimeProfile = flag;
				}
			}
			else if (flag4)
			{
				if (int.TryParse(timeProfileChanges[key], out var result) && accessControlDeviceTimeProfileEntry.TimeBackward != result)
				{
					flag6 = true;
					accessControlDeviceTimeProfileEntry.TimeBackward = result;
				}
			}
			else if (flag2)
			{
				accessControlDeviceTimeProfileEntry.Value = value;
				flag6 = true;
			}
			if (flag6 && !flag5)
			{
				list.Add(accessControlDeviceTimeProfileEntry);
			}
		}
		if (list.Any())
		{
			IGeminiSecurityNamespace securityNamespaceFromToken = requestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(requestContext, path.Id);
			requestContext.GetService<SecuredSecurityManager>().UpdateDeviceTimeProfilePermissions(requestContext, securityNamespaceFromToken, list);
		}
	}

	protected override Property[] CreateSecurityPermissionProperties(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, ConfigurationItemPath objectPath, string userId, bool isSettable)
	{
		string id = objectPath.Id;
		AccessControlEntry[] array = requestContext.GetService<SecurityManagerService>().QueryPermissions(requestContext, securityNamespace.NamespaceInfo.NamespaceGuid, id, userId);
		List<Property> list = new List<Property>();
		AccessControlEntry[] array2 = array;
		foreach (AccessControlEntry accessControlEntry in array2)
		{
			if (!ItemTypeActionsToExclude[objectPath.ItemType].Contains(accessControlEntry.ActionId))
			{
				string configuredDisplayName = securityNamespace.NamespaceInfo.Actions.FirstOrDefault((ActionDefinition a) => a.Name == accessControlEntry.ActionId)?.DisplayName;
				list.Add(CreateAccessControlEntryProperty(accessControlEntry, securityNamespace.NamespaceInfo.Name, isSettable, configuredDisplayName));
			}
		}
		foreach (ActionDefinition actionDefinition in securityNamespace.NamespaceInfo.Actions)
		{
			if (!ItemTypeActionsToExclude[objectPath.ItemType].Contains(actionDefinition.Name) && !list.Any((Property f) => f.Key.Equals(actionDefinition.Name, StringComparison.OrdinalIgnoreCase)))
			{
				string configuredDisplayName2 = securityNamespace.NamespaceInfo.Actions.FirstOrDefault((ActionDefinition a) => a.Name == actionDefinition.Name)?.DisplayName;
				list.Add(SecurityConfigurationApiBase.SecurityActionProperty(SecurityValue.None, securityNamespace.NamespaceInfo.Name, actionDefinition.Name, isSettable, configuredDisplayName2));
			}
		}
		if (_deviceTimeProfileProperties.ContainsKey(objectPath.ItemType))
		{
			ValueTypeInfo[] timeProfileEnum = BuildTimeProfileEnum(requestContext);
			List<string> list2 = _deviceTimeProfileProperties[objectPath.ItemType];
			List<AccessControlDeviceTimeProfileEntry> list3 = requestContext.GetSecurityManagerService().QueryDeviceTimeProfile(requestContext, userId, objectPath.Id);
			bool flag = securityNamespace.NamespaceInfo.Actions.FirstOrDefault((ActionDefinition a) => a.Name == "PTZ_CONTROL") != null;
			bool flag2 = false;
			foreach (AccessControlDeviceTimeProfileEntry item in list3)
			{
				if (!list2.Contains(item.ActionId))
				{
					continue;
				}
				string timeProfileId = (item.IsDefaultTimeProfile ? "Default" : (string.IsNullOrEmpty(item.TimeProfileId) ? "Always" : ConfigurationItemPath.CreateItemPath("TimeProfile", item.TimeProfileId.ToLower(CultureInfo.InvariantCulture)).Text));
				if (item.ActionId == "DEVICEPTZPRIORITY")
				{
					list.Add(SecurityConfigurationApiBase.SecurityActionProperty(SecurityValue.Allow, securityNamespace.NamespaceInfo.Name, "DEVICEPTZPRIORITY", isSettable, ""));
					list.Add(SecurityActionIntProperty(item.Value, securityNamespace.NamespaceInfo.Name, item.ActionId + "Priority", isSettable: true));
					flag2 = true;
					continue;
				}
				list.Add(SecurityConfigurationApiBase.SecurityActionTimeProfileProperty(timeProfileId, securityNamespace.NamespaceInfo.Name, item.ActionId, isSettable: true, timeProfileEnum));
				if (item.ActionId.EndsWith("BROWSE", StringComparison.Ordinal))
				{
					list.Add(SecurityActionIntProperty(item.TimeBackward.ToString(CultureInfo.InvariantCulture), securityNamespace.NamespaceInfo.Name, item.ActionId + "TimeBackward", isSettable: true));
				}
			}
			if (flag && !flag2)
			{
				list.Add(SecurityConfigurationApiBase.SecurityActionProperty(SecurityValue.None, securityNamespace.NamespaceInfo.Name, "DEVICEPTZPRIORITY", isSettable, ""));
			}
		}
		return list.ToArray();
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

	private static Property CreateAccessControlEntryProperty(AccessControlEntry accessControlEntry, string securityNamespaceName, bool isSettable, string configuredDisplayName)
	{
		SecurityValue value = SecurityValue.Allow;
		if (accessControlEntry.AccessDeny)
		{
			value = SecurityValue.None;
		}
		return SecurityConfigurationApiBase.SecurityActionProperty(value, securityNamespaceName, accessControlEntry.ActionId, isSettable, configuredDisplayName);
	}

	private static Property SecurityActionIntProperty(string val, string securityNamespaceName, string actionId, bool isSettable)
	{
		SecurityConfigurationApiBase.FormatResourceIds(securityNamespaceName, actionId, out var resourceKeyAction, out var resourceKeyTooltip, out var displayName);
		return new Property(actionId, val)
		{
			ValueType = "Int",
			IsSettable = isSettable,
			DisplayName = displayName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = resourceKeyAction,
			ToolTipTranslationId = resourceKeyTooltip,
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
