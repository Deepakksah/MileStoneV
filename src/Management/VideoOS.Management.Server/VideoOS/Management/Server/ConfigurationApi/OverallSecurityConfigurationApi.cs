using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration.Security;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class OverallSecurityConfigurationApi
{
	public enum OverallSecurityValue
	{
		None,
		Allow,
		Deny
	}

	public const string SecurityNamespaceKey = "SecurityNamespace";

	public static ICollection<Guid> PluginToExclude = new List<Guid>
	{
		new Guid("E145597A-ECAF-40A1-99E6-14BB9D45A09E"),
		new Guid("E125597A-ECAF-40A1-99E6-24BB9D45A09E"),
		new Guid("6f714690-e09f-4d6a-a0ca-9f913ca125e8"),
		new Guid("31272566-417E-45B7-B17D-8F591B1E9461"),
		new Guid("bb78769d-79c7-4a12-bd6e-491f61327f3b"),
		new Guid("32c415c6-6ddb-4931-ae35-e971590dd8b6"),
		new Guid("9947f340-397c-483b-9e03-f27253f7f403"),
		new Guid("32DC3EC5-FD0F-4C12-899B-EC62E077D101"),
		new Guid("24a01849-a322-4ad3-8284-0ebceb1a8036"),
		new Guid("61768c74-942c-4125-af52-577d23ac7a74"),
		new Guid("DBB3A246-122E-498D-A804-6363A154A127"),
		new Guid("bb529de8-608e-4ebb-b796-889504e54670")
	};

	public static ConfigurationItem InvokeSecurityMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
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
		if (methodId.Equals("ChangeOverallSecurityPermissions", StringComparison.Ordinal) && !path.IsFolder)
		{
			if ((Role.QueryRoleById(requestContext, path.Id) ?? throw new PathNotFoundException(path.Text)).Id.Equals(SecurityManager.AdministratorRoleId.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				throw new NotSupportedException("Not possible to set security on administrator role.");
			}
			if (!flag)
			{
				return CreateInvokeInfoChangeOverallSecurityPermissionsNamespaceSelection(requestContext, item, methodId);
			}
			string value = null;
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SecurityNamespace", out value))
			{
				throw new PropertyInvalidException("Namespace property missing.", "SecurityNamespace");
			}
			if (string.IsNullOrEmpty(value))
			{
				throw new PropertyInvalidException("Namespace not found", "SecurityNamespace");
			}
			if (item.Properties.Length == 1)
			{
				return CreateInvokeInfoItemChangeOverallSecurityPermissions(requestContext, item, path, methodId, value);
			}
			return ChangeOverallSecurityPermissions(requestContext, item, path, value);
		}
		throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
	}

	private static ConfigurationItem ChangeOverallSecurityPermissions(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string namespaceId)
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
		if (namespaceId == null)
		{
			throw new ArgumentNullException("namespaceId");
		}
		if (!Guid.TryParse(namespaceId, out var result))
		{
			throw new PropertyInvalidException("Security namespace is invalid", namespaceId);
		}
		IGeminiSecurityNamespace securityNamespace = requestContext.GetService<SecurityManagerService>().GetSecurityNamespace(requestContext, result);
		if (securityNamespace == null)
		{
			throw new PropertyInvalidException("Security namespace not found", namespaceId);
		}
		SecurityNamespaceInfo namespaceInfo = securityNamespace.NamespaceInfo;
		List<AccessControlEntry> list = new List<AccessControlEntry>();
		List<AccessControlEntry> list2 = new List<AccessControlEntry>();
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (property.Key.Equals("SecurityNamespace", StringComparison.Ordinal))
			{
				continue;
			}
			if (!namespaceInfo.Actions.Any((ActionDefinition f) => f.Name.Equals(property.Key)))
			{
				throw new PropertyInvalidException("Security property not supported on configuration item.", property.Key);
			}
			try
			{
				bool removed;
				AccessControlEntry item2 = CreateAccessControlEntryFromProperty(property, path.Id, out removed);
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
		List<AccessControlEntry> list3 = new List<AccessControlEntry>();
		foreach (AccessControlEntry item5 in list)
		{
			AccessControlEntry item3 = new AccessControlEntry
			{
				AccessDeny = !item5.AccessDeny,
				ActionId = item5.ActionId,
				Sid = item5.Sid
			};
			list3.Add(item3);
		}
		foreach (AccessControlEntry item6 in list2)
		{
			AccessControlEntry item4 = new AccessControlEntry
			{
				AccessDeny = !item6.AccessDeny,
				ActionId = item6.ActionId,
				Sid = item6.Sid
			};
			list3.Add(item4);
		}
		list2.AddRange(list3);
		list2.AddRange(list);
		requestContext.GetService<SecuredSecurityManager>().AddRemovePermissions(requestContext, securityNamespace, "/", "", list.ToArray(), list2.ToArray());
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeResult";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodChangeOverallSecurityPermissions;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list4 = new List<Property>();
		IEnumerable<ValueTypeInfo> source = item.Properties.Single((Property p) => p.Key.Equals("SecurityNamespace", StringComparison.OrdinalIgnoreCase)).ValueTypeInfos.Where((ValueTypeInfo v) => v.Value.Equals(namespaceId, StringComparison.OrdinalIgnoreCase));
		list4.Add(RolesPropertyDefinition.NamespaceProperty(namespaceId, isSettable: false, source.ToArray()));
		list4.AddRange(CreateOverallSecurityPermissionProperties(requestContext, path, namespaceId, isSettable: false));
		configurationItem.Properties = list4.ToArray();
		configurationItem.MethodIds = new string[0];
		return configurationItem;
	}

	private static AccessControlEntry CreateAccessControlEntryFromProperty(Property property, string roleId, out bool removed)
	{
		AccessControlEntry accessControlEntry = new AccessControlEntry();
		removed = property.Value == OverallSecurityValue.None.ToString();
		accessControlEntry.AccessDeny = property.Value == OverallSecurityValue.Deny.ToString();
		accessControlEntry.ActionId = property.Key;
		accessControlEntry.Sid = roleId;
		return accessControlEntry;
	}

	private static ConfigurationItem CreateInvokeInfoItemChangeOverallSecurityPermissions(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId, string namespacePropertyValue)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodChangeOverallSecurityPermissions;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		IEnumerable<ValueTypeInfo> source = item.Properties.Single((Property p) => p.Key.Equals("SecurityNamespace", StringComparison.OrdinalIgnoreCase)).ValueTypeInfos.Where((ValueTypeInfo v) => v.Value.Equals(namespacePropertyValue, StringComparison.OrdinalIgnoreCase));
		list.Add(RolesPropertyDefinition.NamespaceProperty(namespacePropertyValue, isSettable: false, source.ToArray()));
		list.AddRange(CreateOverallSecurityPermissionProperties(requestContext, path, namespacePropertyValue, isSettable: true));
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	private static Property[] CreateOverallSecurityPermissionProperties(IGeminiRequestContext requestContext, ConfigurationItemPath rolePath, string namespaceId, bool isSettable)
	{
		if (!Guid.TryParse(namespaceId, out var _))
		{
			throw new PropertyInvalidException(string.Format(CultureInfo.InvariantCulture, "Security namespace is invalid: {0}", namespaceId), "SecurityNamespace");
		}
		SecuredSecurityManager service = requestContext.GetService<SecuredSecurityManager>();
		List<SecurityNamespaceInfo> list = service.QuerySecurityNamespaces(requestContext, namespaceId);
		if (list == null || list.Count != 1)
		{
			throw new PropertyInvalidException(string.Format(CultureInfo.InvariantCulture, "Security namespace is invalid: {0}", namespaceId), "SecurityNamespace");
		}
		SecurityNamespaceInfo securityNamespaceInfo = list[0];
		AccessControlEntry[] array = service.QueryPermissions(requestContext, namespaceId, "/", rolePath.Id);
		List<Property> list2 = new List<Property>();
		AccessControlEntry[] array2 = array;
		foreach (AccessControlEntry accessControlEntry in array2)
		{
			list2.Add(CreateAccessControlEntryProperty(accessControlEntry, securityNamespaceInfo.Name, isSettable));
		}
		foreach (ActionDefinition actionDefinition in securityNamespaceInfo.Actions)
		{
			if (!list2.Any((Property f) => f.Key.Equals(actionDefinition.Name, StringComparison.OrdinalIgnoreCase)))
			{
				list2.Add(RolesPropertyDefinition.OverallSecurityActionProperty(OverallSecurityValue.None, securityNamespaceInfo.Name, actionDefinition.Name, isSettable));
			}
		}
		return list2.ToArray();
	}

	private static Property CreateAccessControlEntryProperty(AccessControlEntry accessControlEntry, string namespaceName, bool isSettable)
	{
		OverallSecurityValue value = OverallSecurityValue.Allow;
		if (accessControlEntry.AccessDeny)
		{
			value = OverallSecurityValue.Deny;
		}
		return RolesPropertyDefinition.OverallSecurityActionProperty(value, namespaceName, accessControlEntry.ActionId, isSettable);
	}

	private static ConfigurationItem CreateInvokeInfoChangeOverallSecurityPermissionsNamespaceSelection(IGeminiRequestContext requestContext, ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodChangeOverallSecurityPermissions;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		ValueTypeInfo[] namespaces = GetNamespaces(requestContext);
		string value = namespaces.First().Value;
		configurationItem.Properties = new Property[1] { RolesPropertyDefinition.NamespaceProperty(value, isSettable: true, namespaces) };
		configurationItem.MethodIds = new string[1] { methodId };
		return configurationItem;
	}

	public static ValueTypeInfo[] GetNamespaces(IGeminiRequestContext requestContext)
	{
		List<SecurityNamespaceInfo> list = requestContext.GetService<SecuredSecurityManager>().QuerySecurityNamespaces(requestContext, null);
		List<ValueTypeInfo> list2 = new List<ValueTypeInfo>();
		foreach (SecurityNamespaceInfo item in list)
		{
			if (!PluginToExclude.Contains(item.NamespaceGuid))
			{
				string text = string.Format(CultureInfo.InvariantCulture, "PropertySecurityNamespace{0}", item.Name);
				if (string.IsNullOrEmpty(ConfigurationApiResources.ResourceManager.GetString(text)))
				{
					text = "";
				}
				list2.Add(new ValueTypeInfo
				{
					Name = item.DisplayName,
					TranslationId = text,
					Value = item.NamespaceId
				});
			}
		}
		return list2.ToArray();
	}
}
