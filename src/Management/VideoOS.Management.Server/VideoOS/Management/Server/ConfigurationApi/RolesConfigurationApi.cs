using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Security;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Platform;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIRole")]
[EntityName("ConfigAPIRoleFolder")]
public class RolesConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public const string MakeUsersAnonymousDuringPTZSession = "MakeUsersAnonymousDuringPTZSession";

	public const string TimeProfilePropertyName = "ClientLoginTimeProfile";

	public static readonly Guid DefaultTimeProfileId = new Guid("00000000-0000-0000-0000-000000000000");

	public static readonly Guid AlwaysTimeProfileId = new Guid("11111111-1111-1111-1111-111111111111");

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			Role[] array = Role.QueryRoles(requestContext);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			Role[] array2 = array;
			foreach (Role role in array2)
			{
				list.Add(ConvertToRoleConfigurationItem(requestContext, path, role, includeProfiles: false));
			}
			return list.ToArray();
		}
		ConfigurationItem configurationItem = GroupConfigurationItemFactory.CreateFolderItem("UserFolder", path);
		ConfigurationItem configurationItem2 = GroupConfigurationItemFactory.CreateFolderItem("ClaimFolder", path);
		return new ConfigurationItem[2] { configurationItem, configurationItem2 };
	}

	private static ConfigurationItem[] GetChildItemsInternal(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			Role[] array = Role.QueryRoles(requestContext);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			Role[] array2 = array;
			foreach (Role role in array2)
			{
				list.Add(ConvertToRoleConfigurationItem(requestContext, path, role, includeProfiles: false));
			}
			return list.ToArray();
		}
		ConfigurationItem configurationItem = GroupConfigurationItemFactory.CreateFolderItem("UserFolder", path);
		return new ConfigurationItem[1] { configurationItem };
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("RoleFolder", ConfigurationItemPath.CreateSystemPath());
		}
		Role role = Role.QueryRoleById(requestContext, path.Id);
		if (role == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		ConfigurationItemPath parentPath = new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "RoleFolder").CreatePath();
		return ConvertToRoleConfigurationItem(requestContext, parentPath, role);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (method)
		{
		case "AddRole":
			if (!flag)
			{
				return CreateInvokeInfoAddRole(item, method, GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "TimeControlledAccessRights"), GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EvidenceLock"));
			}
			return AddRole(requestContext, item, path);
		case "RemoveRole":
			if (!flag)
			{
				return CreateInvokeInfoRemoveRole(item, GetChildItemsInternal(requestContext, path), method);
			}
			return RemoveRole(requestContext, item, path);
		case "CopyRole":
			if (!flag)
			{
				return CreateInvokeInfoCopyRole(item, GetChildItemsInternal(requestContext, path), method);
			}
			return CopyRole(requestContext, item, path);
		case "GetClientTimeProfile":
			if (!flag)
			{
				return CreateInvokeInfoGetSmartClientTimeProfile(item, method);
			}
			return GetClientLoginTime(requestContext, item, path);
		case "SetClientTimeProfile":
			if (!flag)
			{
				return CreateInvokeInfoSetSmartClientTimeProfile(requestContext, item, path, method);
			}
			return SetClientLoginTime(requestContext, item, path);
		case "ChangeOverallSecurityPermissions":
			return OverallSecurityConfigurationApi.InvokeSecurityMethod(requestContext, item, path, method);
		case "SetClientProfile":
			if (!flag)
			{
				return CreateInvokeInfoSetClientProfile(requestContext, item, method);
			}
			return SetClientProfile(requestContext, item, path);
		case "QueryUsersWithAccess":
			if (!flag)
			{
				return CreateInvokeInfoQueryUsersWithAccess(item);
			}
			return QueryUsersWithAccess(requestContext, item, path);
		case "QueryIsPermittedTimeBasedAction":
			if (!flag)
			{
				return CreateInvokeInfoQueryIsPermittedTimeBasedAction(item);
			}
			return QueryIsPermittedTimeBasedAction(requestContext, item, path);
		default:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
	}

	private static ConfigurationItem AddRole(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		string value2 = null;
		bool? value3 = null;
		bool? value4 = null;
		bool? value5 = null;
		bool? value6 = null;
		bool? value7 = null;
		bool dualAuthorizationRequired = false;
		bool showPtzReservedNameAsAnonymous = false;
		bool allowMobileClientLogOn = false;
		bool allowSmartClientLogOn = true;
		bool allowWebClientLogOn = false;
		string value8 = null;
		string value9 = null;
		string value10 = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DualAuthorizationRequired", out value3);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MakeUsersAnonymousDuringPTZSession", out value4);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AllowMobileClientLogOn", out value5);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AllowSmartClientLogOn", out value6);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AllowWebClientLogOn", out value7);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RoleDefaultTimeProfile", out value8);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RoleClientLogOnTimeProfile", out value9);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EvidenceLockProfile", out value10);
		}
		if (value3.HasValue)
		{
			dualAuthorizationRequired = value3.Value;
		}
		if (value4.HasValue)
		{
			showPtzReservedNameAsAnonymous = value4.Value;
		}
		if (value5.HasValue)
		{
			allowMobileClientLogOn = value5.Value;
		}
		if (value6.HasValue)
		{
			allowSmartClientLogOn = value6.Value;
		}
		if (value7.HasValue)
		{
			allowWebClientLogOn = value7.Value;
		}
		try
		{
			Role role = Role.CreateRole(requestContext, value, value2, dualAuthorizationRequired, showPtzReservedNameAsAnonymous, allowMobileClientLogOn, allowSmartClientLogOn, allowWebClientLogOn);
			CreateRoleServerGroup(requestContext, role);
			UpdateRoleProfiles(requestContext, role, value8, value9, value10);
			ConfigurationItem configurationItem = ConvertToRoleConfigurationItem(requestContext, path, role);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (RoleCreationException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
	}

	private static void CreateRoleServerGroup(IGeminiRequestContext requestContext, Role role)
	{
		try
		{
			if (ServerGroup.QueryServerGroupByName(requestContext, "430C2FE2-B233-42DA-B508-1A22EF03383D", role.Name) != null)
			{
				return;
			}
		}
		catch (ServerGroupNotFoundException)
		{
		}
		ServerGroup serverGroup = new ServerGroup
		{
			Id = string.Empty,
			ParentId = "430C2FE2-B233-42DA-B508-1A22EF03383D",
			Name = role.Name,
			Description = "",
			CreatedDate = DateTime.UtcNow,
			StructureType = ServerGroup.EnumStructureType.ViewGroup
		};
		ServerGroup.CreateServerGroup(requestContext, serverGroup, checkPermissions: false);
	}

	private static ConfigurationItem CopyRole(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		string value2 = null;
		ConfigurationItemPath configurationItemPath = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out value2);
			configurationItemPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		}
		try
		{
			Role role = Role.CopyRole(requestContext, configurationItemPath.Id, value, value2);
			ConfigurationItem configurationItem = ConvertToRoleConfigurationItem(requestContext, path, role);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (RoleCreationException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
	}

	private static ConfigurationItem RemoveRole(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			Role.DeleteRole(requestContext, invokeMemberItemSelectionPath.Id);
			RemoveRoleProfiles(requestContext, invokeMemberItemSelectionPath.Id);
		}
		catch (RemoveNonExistingRoleException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem CreateInvokeInfoAddRole(ConfigurationItem item, string methodId, bool includeTimeProfileProperties, bool includeEvidenceLockProfile)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddRole;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		List<Property> list = new List<Property>
		{
			GeneralPropertyDefinitions.GetNameProperty(""),
			GeneralPropertyDefinitions.GetDescriptionProperty(""),
			RolesPropertyDefinition.DualAuthorizationRequiredProperty(dualAuthorizationRequired: false),
			RolesPropertyDefinition.MakeUsersAnonymousDuringPTZSessionProperty(makeUsersAnonymousDuringPTZSession: false),
			RolesPropertyDefinition.AllowMobileClientLogOnProperty(allowMobileClientLogOn: true),
			RolesPropertyDefinition.AllowSmartClientLogOnProperty(allowSmartClientLogOn: true),
			RolesPropertyDefinition.AllowWebClientLogOnProperty(allowWebClientLogOn: true)
		};
		if (includeTimeProfileProperties)
		{
			list.Add(RolesPropertyDefinition.TimeProfileProperty("RoleDefaultTimeProfile", string.Format(CultureInfo.InvariantCulture, "TimeProfile[{0}]", AlwaysTimeProfileId)));
			list.Add(RolesPropertyDefinition.TimeProfileProperty("RoleClientLogOnTimeProfile", string.Format(CultureInfo.InvariantCulture, "TimeProfile[{0}]", DefaultTimeProfileId)));
		}
		if (includeEvidenceLockProfile)
		{
			string id = EvidenceLockProfileConfigurationApi.DefaultEvidenceLockProfileId.ToString();
			string text = ConfigurationItemPath.CreateItemPath("EvidenceLockProfile", id).Text;
			list.Add(RolesPropertyDefinition.EvidenceLockProfileProperty(text));
		}
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoCopyRole(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		ConfigurationItem configurationItem = MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardCopyRole, "PropertyCopyRole", ConfigurationApiResources.PropertyCopyRole, item, members);
		configurationItem.Properties = new List<Property>(configurationItem.Properties)
		{
			GeneralPropertyDefinitions.GetNameProperty(""),
			GeneralPropertyDefinitions.GetDescriptionProperty("")
		}.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveRole(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveRole, "PropertyRemoveRole", ConfigurationApiResources.PropertyRemoveRole, item, members);
	}

	private static ConfigurationItem CreateInvokeInfoSetSmartClientTimeProfile(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		ConfigurationItem[] timeProfileItems = GetTimeProfileItems(requestContext, path);
		ConfigurationItem configurationItem = MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardSetClientLoginTimeProfile, "PropertyClientLoginTimeProfile", ConfigurationApiResources.PropertyClientLoginTimeProfile, item, timeProfileItems, (ConfigurationItem configurationItem2) => configurationItem2.Properties.Where((Property p) => p.Key == "Id").First().Value);
		try
		{
			string value = Role.QueryClientLoginLegacyPermission(requestContext, path.Id);
			configurationItem.Properties.SetValue("ItemSelection", value);
			return configurationItem;
		}
		catch (FeatureNotLicensedException)
		{
			throw new LicenseException();
		}
	}

	private static ConfigurationItem CreateInvokeInfoGetSmartClientTimeProfile(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardGetClientLoginTimeProfile;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[0];
		return configurationItem;
	}

	private static ConfigurationItem GetClientLoginTime(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "Role", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		return MethodTypesHelper.CreateInvokeResult(CreateInvokeInfoSetSmartClientTimeProfile(requestContext, item, path, "GetClientTimeProfile"), null, "Result", "Success", true);
	}

	private static ConfigurationItem SetClientLoginTime(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "Role", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ItemSelection", out value))
		{
			throw new PropertyInvalidException("No selection.", "ItemSelection");
		}
		string text = value;
		if (text == default(Guid).ToString())
		{
			text = null;
		}
		try
		{
			Role.UpdateClientLoginLegacyPermission(requestContext, path.Id, text, isDefaultTimeProfile: false);
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (FeatureNotLicensedException)
		{
			throw new LicenseException();
		}
	}

	private static ConfigurationItem CreateInvokeInfoSetClientProfile(IGeminiRequestContext requestContext, ConfigurationItem item, string methodId)
	{
		Dictionary<string, string> clientProfileItems = GetClientProfileItems(requestContext);
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardSetClientProfile, "PropertyClientProfile", ConfigurationApiResources.PropertyClientProfile, item, clientProfileItems);
	}

	private static ConfigurationItem SetClientProfile(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "Role", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ItemSelection", out value))
		{
			throw new PropertyInvalidException("No selection.", "ItemSelection");
		}
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(value);
		string id = configurationItemPath.Id;
		if (configurationItemPath.ItemType != "ClientProfile")
		{
			throw new PathInvalidException("Invalid path");
		}
		try
		{
			ClientProfile.QueryClientProfileById(requestContext, id);
			ClientProfile.AttachClientProfileToRole(requestContext, id, path.Id);
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (ClientProfileNotExistException)
		{
			throw new PropertyInvalidException("Client profile not found", "ItemSelection");
		}
		catch (FeatureNotLicensedException)
		{
			throw new LicenseException();
		}
	}

	private static ConfigurationItem CreateInvokeInfoQueryIsPermittedTimeBasedAction(ConfigurationItem item)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardQueryIsPermittedTimeBasedAction;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { "QueryIsPermittedTimeBasedAction" };
		configurationItem.Properties = new Property[2]
		{
			RolesPropertyDefinition.RelatedObjectPath(),
			RolesPropertyDefinition.ActionId()
		};
		return configurationItem;
	}

	private static ConfigurationItem QueryIsPermittedTimeBasedAction(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RelatedObjectPath", out string value))
		{
			throw new PropertyInvalidException("No selection", "RelatedObjectPath");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ActionId", out string value2))
		{
			throw new PropertyInvalidException("Action id missing", "ActionId");
		}
		ConfigurationItemPath configurationItemPath;
		try
		{
			configurationItemPath = ConfigurationItemPath.CreatePath(value);
		}
		catch (ArgumentException)
		{
			throw new PropertyInvalidException("Invalid path", "RelatedObjectPath");
		}
		SecurityManagerService service = requestContext.GetService<SecurityManagerService>();
		IGeminiSecurityNamespace securityNamespaceFromToken = service.GetSecurityNamespaceFromToken(requestContext, configurationItemPath.Id);
		string id = configurationItemPath.Id;
		Guid namespaceId = new Guid(securityNamespaceFromToken.NamespaceInfo.NamespaceId);
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, namespaceId, "GENERIC_READ", configurationItemPath.Id))
		{
			throw new UnauthorizedAccessException();
		}
		bool isPermitted = service.TimeBasedSecurityIsPermitted(requestContext, namespaceId, id, value2);
		return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true, RolesPropertyDefinition.IsPermitted(isPermitted));
	}

	private static ConfigurationItem CreateInvokeInfoQueryUsersWithAccess(ConfigurationItem item)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardQueryUsersWithAccess;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { "QueryUsersWithAccess" };
		configurationItem.Properties = new Property[4]
		{
			RolesPropertyDefinition.QueryUsersWithAccessProperty(),
			RolesPropertyDefinition.IncludeAdministratorsProperty(),
			RolesPropertyDefinition.ExpandGroupsProperty(),
			RolesPropertyDefinition.IncludeGroupsProperty()
		};
		return configurationItem;
	}

	private static ConfigurationItem QueryUsersWithAccess(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("QueryUsersWithAccessPath", out string value))
		{
			throw new PropertyInvalidException("No selection", "QueryUsersWithAccessPath");
		}
		bool result = true;
		bool result2 = true;
		bool result3 = false;
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("IncludeAdministrators", out string value2))
		{
			bool.TryParse(value2, out result);
		}
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ExpandGroups", out string value3))
		{
			bool.TryParse(value3, out result2);
		}
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("IncludeGroups", out string value4))
		{
			bool.TryParse(value4, out result3);
		}
		ConfigurationItemPath configurationItemPath;
		try
		{
			configurationItemPath = ConfigurationItemPath.CreatePath(value);
		}
		catch (ArgumentException)
		{
			throw new PropertyInvalidException("Invalid path", "QueryUsersWithAccessPath");
		}
		SecurityManagerService service = requestContext.GetService<SecurityManagerService>();
		IGeminiSecurityNamespace securityNamespaceFromToken = service.GetSecurityNamespaceFromToken(requestContext, configurationItemPath.Id);
		string id = configurationItemPath.Id;
		Guid namespaceId;
		if (securityNamespaceFromToken.NamespaceInfo.UserDefined)
		{
			Item item2 = MIPItem.GetItem(requestContext, new Guid(configurationItemPath.Id));
			securityNamespaceFromToken = service.GetSecurityNamespace(requestContext, item2.FQID.Kind);
			namespaceId = new Guid(securityNamespaceFromToken.NamespaceInfo.NamespaceId);
		}
		else
		{
			namespaceId = new Guid(securityNamespaceFromToken.NamespaceInfo.NamespaceId);
			if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, namespaceId, "GENERIC_READ", configurationItemPath.Id))
			{
				throw new UnauthorizedAccessException();
			}
		}
		HashSet<Guid> hashSet = new HashSet<Guid>();
		HashSet<string> hashSet2 = new HashSet<string>();
		AccessControlEntry[] array = service.QueryPermissions(requestContext, namespaceId, id);
		foreach (AccessControlEntry accessControlEntry in array)
		{
			if (Guid.TryParse(accessControlEntry.Sid, out var result4))
			{
				hashSet.Add(result4);
			}
			else
			{
				hashSet2.Add(accessControlEntry.Sid);
			}
		}
		if (result)
		{
			hashSet.Add(SecurityManager.AdministratorRoleId);
		}
		HashSet<string> hashSet3 = new HashSet<string>();
		foreach (Guid item5 in hashSet)
		{
			using IRoleComponent roleComponent = requestContext.CreateRoleComponent();
			string[] array2 = roleComponent.QueryRoleMembers(item5.ToString(), includeDynamicallyAddedIdentities: true);
			foreach (string sid in array2)
			{
				Identity identity = IdentityCache.ReadIdentityFromCache(requestContext, sid, (!result2) ? QueryMembership.Direct : QueryMembership.Expanded);
				if (identity == null || !(identity.Type != IdentityType.WindowsGroup || result3))
				{
					continue;
				}
				hashSet3.Add(identity.Sid);
				if (identity.Members != null)
				{
					string[] members = identity.Members;
					foreach (string item3 in members)
					{
						hashSet3.Add(item3);
					}
				}
			}
		}
		Identity[] array3 = IdentityCache.ReadIdentitiesFromCache(requestContext, hashSet3.ToArray(), QueryMembership.None);
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		Identity[] array4 = array3;
		for (int i = 0; i < array4.Length; i++)
		{
			ConfigurationItem item4 = RoleMembersConfigurationApi.ConvertToIdentityConfigurationItem(array4[i]);
			list.Add(item4);
		}
		ConfigurationItem configurationItem = MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		configurationItem.Children = list.ToArray();
		configurationItem.ChildrenFilled = true;
		return configurationItem;
	}

	private static void UpdateRoleProfiles(IGeminiRequestContext requestContext, Role role, string defaultTimeProfilePathText, string clientLogOnTimeProfileText, string evidenceLockProfileText)
	{
		if (!string.IsNullOrEmpty(defaultTimeProfilePathText))
		{
			string text = null;
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(defaultTimeProfilePathText);
			if (new Guid(configurationItemPath.Id) != AlwaysTimeProfileId)
			{
				text = configurationItemPath.Id;
			}
			if (!string.Equals(Role.QueryDefaultTimeProfileId(requestContext, role.Id), text, StringComparison.OrdinalIgnoreCase))
			{
				Role.UpdateDefaultTimeProfileId(requestContext, role.Id, text);
			}
		}
		if (!string.IsNullOrEmpty(clientLogOnTimeProfileText))
		{
			string text2 = null;
			bool flag = false;
			ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreatePath(clientLogOnTimeProfileText);
			Guid guid = new Guid(configurationItemPath2.Id);
			if (guid == DefaultTimeProfileId)
			{
				flag = true;
			}
			else if (guid == AlwaysTimeProfileId)
			{
				flag = false;
			}
			else
			{
				text2 = guid.ToString();
			}
			Tuple<string, bool> tuple = Role.QueryClientLoginLegacyPermissionId(requestContext, role.Id);
			if (tuple.Item1 != text2 || tuple.Item2 != flag)
			{
				Role.UpdateClientLoginLegacyPermission(requestContext, role.Id, text2, flag);
			}
		}
		if (string.IsNullOrEmpty(evidenceLockProfileText))
		{
			return;
		}
		ConfigurationItemPath configurationItemPath3 = ConfigurationItemPath.CreatePath(evidenceLockProfileText);
		Guid profileId = new Guid(configurationItemPath3.Id);
		string text3 = RetentionOptionsXmlManager.Deserialize(SystemSetting.GetSystemSetting(requestContext, "RetentionOptions")).RetentionOptions.FirstOrDefault((RetentionTimeOption r) => r.Id == profileId)?.Name;
		if (text3 == null)
		{
			throw new PropertyInvalidException("EvidenceLockProfile not found");
		}
		try
		{
			RetentionOptionRoleRelation.QueryRetentionOptionRoleRelationByRoleId(requestContext, role.Id);
			RetentionOptionRoleRelation.UpdateRetentionOptionRoleRelation(requestContext, role.Id, configurationItemPath3.Id, text3);
		}
		catch (RetentionOptionRoleRelationNotExistException)
		{
			if (new Guid(configurationItemPath3.Id) != EvidenceLockProfileConfigurationApi.DefaultEvidenceLockProfileId)
			{
				RetentionOptionRoleRelation.CreateRetentionOptionRoleRelation(requestContext, role.Id, configurationItemPath3.Id);
			}
		}
	}

	private static void RemoveRoleProfiles(IGeminiRequestContext requestContext, string roleId)
	{
		if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EvidenceLock"))
		{
			try
			{
				RetentionOptionRoleRelation.QueryRetentionOptionRoleRelationByRoleId(requestContext, roleId);
				RetentionOptionRoleRelation.DeleteRetentionOptionRoleRelation(requestContext, roleId);
			}
			catch (RetentionOptionRoleRelationNotExistException)
			{
			}
		}
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, parentPath);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		bool? value = null;
		bool? value2 = null;
		bool? value3 = null;
		bool? value4 = null;
		bool? value5 = null;
		string value6 = null;
		string value7 = null;
		string value8 = null;
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out string value9);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out string value10);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DualAuthorizationRequired", out value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MakeUsersAnonymousDuringPTZSession", out value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AllowMobileClientLogOn", out value3);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AllowSmartClientLogOn", out value4);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AllowWebClientLogOn", out value5);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RoleDefaultTimeProfile", out value6);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RoleClientLogOnTimeProfile", out value7);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("EvidenceLockProfile", out value8);
		Role role;
		try
		{
			role = Role.UpdateRole(requestContext, path.Id, value9, value10, value, value2, value3, value4, value5);
			UpdateRoleProfiles(requestContext, role, value6, value7, value8);
		}
		catch (RoleUpdateException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (RoleRenameException ex2)
		{
			throw new PropertyInvalidException(ex2.Message, "Name");
		}
		validateResult = new ValidateResult();
		validateResult.ValidatedOk = true;
		validateResult.ResultItem = ConvertToRoleConfigurationItem(requestContext, parentPath, role);
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		return ValidateItemInternal(requestContext, item, path, parentPath);
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			if (Role.QueryRoleById(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			string value = null;
			string value2 = null;
			bool? value3 = null;
			bool? value4 = null;
			bool? value5 = null;
			bool? value6 = null;
			bool? value7 = null;
			string value8 = null;
			string value9 = null;
			if (item.Properties != null)
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out value);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out value2);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DualAuthorizationRequired", out value3);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MakeUsersAnonymousDuringPTZSession", out value4);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AllowMobileClientLogOn", out value5);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AllowSmartClientLogOn", out value6);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AllowWebClientLogOn", out value7);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RoleDefaultTimeProfile", out value8);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RoleClientLogOnTimeProfile", out value9);
			}
			try
			{
				Role.UpdateRoleValidation(requestContext, path.Id, value, value2, value3, value4, value5, value6, value7);
				if (!string.IsNullOrEmpty(value8) && ConfigurationItemPath.CreatePath(value8).ItemType != "TimeProfile")
				{
					throw new PropertyInvalidException("ItemType must be TimeProfile", "RoleDefaultTimeProfile");
				}
				if (!string.IsNullOrEmpty(value9) && ConfigurationItemPath.CreatePath(value9).ItemType != "TimeProfile")
				{
					throw new PropertyInvalidException("ItemType must be TimeProfile", "RoleClientLogOnTimeProfile");
				}
				return new ValidateResult
				{
					ValidatedOk = true,
					ResultItem = item
				};
			}
			catch (ArgumentNullException exception)
			{
				ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
				if (validateResult != null)
				{
					return validateResult;
				}
				throw;
			}
			catch (ArgumentException exception2)
			{
				ValidateResult validateResult2 = ErrorHelper.CreateValidateResult(exception2, item.Properties);
				if (validateResult2 != null)
				{
					return validateResult2;
				}
				throw;
			}
		}
		throw new NotSupportedException();
	}

	private static ConfigurationItem[] GetTimeProfileItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		string[] array = TimeProfile.QueryTimeProfileIds(requestContext);
		if (array == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		if (array.Length != 0)
		{
			string[] array2 = array;
			foreach (string timeProfileId in array2)
			{
				TimeProfile profile = TimeProfile.QueryTimeProfile(requestContext, timeProfileId);
				list.Add(ConvertToTimeProfileConfigurationItem(path, profile));
			}
		}
		TimeProfile profile2 = new TimeProfile
		{
			Id = default(Guid).ToString(),
			Name = ConfigurationApiResources.PropertyDefaultClientLoginTimeProfileName,
			Description = ConfigurationApiResources.PropertyDefaultClientLoginTimeProfileDescription
		};
		list.Add(ConvertToTimeProfileConfigurationItem(path, profile2));
		return list.ToArray();
	}

	private static Dictionary<string, string> GetClientProfileItems(IGeminiRequestContext requestContext)
	{
		string[] ids = ClientProfile.QueryClientProfileIds(requestContext, ClientProfileType.SmartClientProfile);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		ClientProfile[] array = ClientProfile.QueryClientProfilesByIds(requestContext, ids);
		foreach (ClientProfile clientProfile in array)
		{
			dictionary.Add(string.Format(CultureInfo.InvariantCulture, "ClientProfile[{0}]", clientProfile.Id), clientProfile.Name);
		}
		return dictionary;
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static ConfigurationItem ConvertToRoleConfigurationItem(IGeminiRequestContext requestContext, ConfigurationItemPath parentPath, Role role, bool includeProfiles = true)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "Role";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = role.Name;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, role.Id).Text;
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(role.Id));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(role.Name));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(role.Description));
		list.Add(RolesPropertyDefinition.RoleTypeProperty(role.Type));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(role.LastModified));
		list.Add(RolesPropertyDefinition.DualAuthorizationRequiredProperty(role.DualAuthorizationRequired));
		list.Add(RolesPropertyDefinition.MakeUsersAnonymousDuringPTZSessionProperty(role.ShowPtzReservedNameAsAnonymous));
		list.Add(RolesPropertyDefinition.AllowMobileClientLogOnProperty(role.AllowMobileClientLogOn));
		list.Add(RolesPropertyDefinition.AllowSmartClientLogOnProperty(role.AllowSmartClientLogOn));
		list.Add(RolesPropertyDefinition.AllowWebClientLogOnProperty(role.AllowWebClientLogOn));
		if (includeProfiles)
		{
			string id = ClientProfile.QueryClientProfileIdByRoleId(requestContext, ClientProfileType.SmartClientProfile, role.Id);
			string text = ConfigurationItemPath.CreateItemPath("ClientProfile", id).Text;
			list.Add(RolesPropertyDefinition.ClientProfileProperty(text));
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EvidenceLock"))
			{
				string id2;
				try
				{
					id2 = RetentionOptionRoleRelation.QueryRetentionOptionRoleRelationByRoleId(requestContext, role.Id).IdRetention;
				}
				catch (RetentionOptionRoleRelationNotExistException)
				{
					id2 = EvidenceLockProfileConfigurationApi.DefaultEvidenceLockProfileId.ToString();
				}
				string text2 = ConfigurationItemPath.CreateItemPath("EvidenceLockProfile", id2).Text;
				list.Add(RolesPropertyDefinition.EvidenceLockProfileProperty(text2));
			}
		}
		if (includeProfiles && GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "TimeControlledAccessRights") && !role.Id.Equals(SecurityManager.AdministratorRoleId.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			string text3 = Role.QueryDefaultTimeProfileId(requestContext, role.Id);
			string text4 = ConfigurationItemPath.CreateItemPath("TimeProfile", string.IsNullOrEmpty(text3) ? AlwaysTimeProfileId.ToString() : text3).Text;
			Tuple<string, bool> tuple = Role.QueryClientLoginLegacyPermissionId(requestContext, role.Id);
			string pathValue = ((tuple == null) ? ConfigurationItemPath.CreateItemPath("TimeProfile", AlwaysTimeProfileId.ToString()).Text : ((tuple.Item1 != null) ? ConfigurationItemPath.CreateItemPath("TimeProfile", tuple.Item1).Text : ConfigurationItemPath.CreateItemPath("TimeProfile", (tuple.Item2 ? DefaultTimeProfileId : AlwaysTimeProfileId).ToString()).Text));
			list.Add(RolesPropertyDefinition.TimeProfileProperty("RoleDefaultTimeProfile", text4));
			list.Add(RolesPropertyDefinition.TimeProfileProperty("RoleClientLogOnTimeProfile", pathValue));
		}
		configurationItem.Properties = list.ToArray();
		List<string> list2 = new List<string> { "SetClientTimeProfile", "GetClientTimeProfile", "SetClientProfile" };
		if (!role.Id.Equals(SecurityManager.AdministratorRoleId.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			list2.Add("ChangeOverallSecurityPermissions");
		}
		configurationItem.MethodIds = list2.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem ConvertToTimeProfileConfigurationItem(ConfigurationItemPath parentPath, TimeProfile profile)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "TimeProfile",
			ItemCategory = ItemCategories.Item,
			DisplayName = profile.Name,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, profile.Id).Text;
		obj.Properties = new List<Property> { GeneralPropertyDefinitions.GetIdPropertyGuid(profile.Id) }.ToArray();
		return obj;
	}
}
