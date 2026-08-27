using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIUser")]
[EntityName("ConfigAPIUserFolder")]
public class RoleMembersConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		if (string.Equals(path.ItemType, "UserFolder", StringComparison.OrdinalIgnoreCase))
		{
			string id = path.Id;
			Identity[] array = Role.QueryRoleMembersExtended(requestContext, id, QueryMembership.None, includeMappedExternalUsers: true);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			Identity[] array2 = array;
			foreach (Identity identity in array2)
			{
				list.Add(ConvertToIdentityConfigurationItem(identity));
			}
			return list.ToArray();
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			if (string.Equals(path.ParentItemType, "Role", StringComparison.Ordinal) && string.Equals(path.ItemType, "UserFolder", StringComparison.OrdinalIgnoreCase))
			{
				string id = path.Id;
				if (!Role.RoleExists(requestContext, id))
				{
					throw new PathNotFoundException(path.Text);
				}
				ConfigurationItemPath parentPath = new ConfigurationItemPathBuilder
				{
					ItemType = "Role",
					Id = id
				}.CreatePath();
				return GroupConfigurationItemFactory.CreateFolderItem("UserFolder", parentPath);
			}
		}
		else if (string.Equals(path.ItemType, "User", StringComparison.OrdinalIgnoreCase))
		{
			string id2 = path.Id;
			return ConvertToIdentityConfigurationItem(IdentityCache.ReadIdentityFromCache(requestContext, id2, QueryMembership.Expanded) ?? throw new PathNotFoundException(path.Text));
		}
		throw new NotSupportedException();
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
		if (!item.MethodIds.Contains(method))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", method, item.ItemType), "method");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		if (!(method == "AddRoleMember"))
		{
			if (method == "RemoveRoleMember")
			{
				if (!flag)
				{
					return CreateInvokeInfoRemoveRoleMember(item, GetChildItems(requestContext, path), method);
				}
				return RemoveRoleMember(requestContext, item, path);
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
		if (!flag)
		{
			return CreateInvokeInfoAddRoleMember(item, method);
		}
		return AddRoleMember(requestContext, item, path);
	}

	private static ConfigurationItem AddRoleMember(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
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
		if (!string.Equals(path.ParentItemType, "Role", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Sid", out value);
		}
		try
		{
			Role.AddMemberToRole(requestContext, path.Id, value);
			Identity identity = IdentityCache.ReadIdentityFromCache(requestContext, value, QueryMembership.Expanded);
			ServerGroup.QueryPrivateViewGroup(requestContext, identity.Sid);
			ConfigurationItem configurationItem = ConvertToIdentityConfigurationItem(identity);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (AddMemberRoleMissingException)
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		catch (AddMemberNoRoleException)
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		catch (AddMemberIdentityMissingException)
		{
			throw new ArgumentException("User does not exist", "Sid");
		}
		catch (BasicUserNotExistException)
		{
			throw new ArgumentException("User does not exist", "Sid");
		}
	}

	private static ConfigurationItem RemoveRoleMember(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
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
		if (!string.Equals(path.ParentItemType, "Role", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ItemSelection", out value))
		{
			throw new PropertyInvalidException("No selection.", "ItemSelection");
		}
		string value2 = Regex.Match(value, "User\\[(?<sid>(.*?))\\]", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled).Groups["sid"].Value;
		try
		{
			Role.RemoveMemberFromRole(requestContext, path.Id, value2);
		}
		catch (RemoveMemberRoleMissingException)
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		catch (RemoveRoleMemberException)
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		catch (RemoveMemberIdentityMissingException)
		{
			throw new ArgumentException("User does not exist", "Sid");
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem CreateInvokeInfoAddRoleMember(ConfigurationItem item, string methodId)
	{
		Property property = BasicUserPropertyDefinition.SidProperty("");
		property.IsSettable = true;
		property.UIImportance = UIImportance.Default;
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddRoleMember;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { property };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveRoleMember(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveRoleMember, "PropertyRemoveRoleMember", ConfigurationApiResources.PropertyRemoveRoleMember, item, members);
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "Role", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!string.Equals(path.ItemType, "User", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
	}

	public static ConfigurationItem ConvertToIdentityConfigurationItem(Identity identity)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "User",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = identity.DomainAccountName
		};
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPathBuilder(obj.ItemType, identity.Sid).CreatePath();
		obj.Path = configurationItemPath.Text;
		obj.ParentPath = configurationItemPath.Text;
		obj.Children = Array.Empty<ConfigurationItem>();
		obj.ChildrenFilled = true;
		obj.Properties = new List<Property>
		{
			BasicUserPropertyDefinition.SidProperty(identity.Sid),
			RoleMembersPropertyDefinition.IdentityTypeProperty(identity.Type),
			RoleMembersPropertyDefinition.AccountNameProperty(identity.AccountName),
			RoleMembersPropertyDefinition.DisplayNameProperty(identity.DisplayName),
			RoleMembersPropertyDefinition.DomainProperty(identity.Domain),
			GeneralPropertyDefinitions.GetDescriptionProperty(Identity.GetFilteredDescription(identity)),
			RoleMembersPropertyDefinition.MemberOfGroupsProperty(identity.MemberOf),
			RoleMembersPropertyDefinition.MemberOfRolesProperty(identity.MemberOfRoles),
			RoleMembersPropertyDefinition.MembersProperty(identity.Members)
		}.ToArray();
		return obj;
	}
}
