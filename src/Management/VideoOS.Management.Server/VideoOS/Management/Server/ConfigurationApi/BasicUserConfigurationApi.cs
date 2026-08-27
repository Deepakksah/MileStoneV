using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIBasicUser")]
[EntityName("ConfigAPIBasicUserFolder")]
public class BasicUserConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, parentPath, out var description);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		BasicUser basicUser;
		try
		{
			basicUser = BasicUser.QueryBasicUserById(requestContext, path.Id);
		}
		catch (BasicUserNotExistException)
		{
			throw new PathNotFoundException(path.Text);
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CanChangePassword", out bool? value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ForcePasswordChange", out bool? value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Status", out BasicUserStatus? value3);
		basicUser = BasicUser.UpdateBasicUser(requestContext, basicUser.Id, description, value ?? basicUser.CanChangePassword, value2 ?? basicUser.ForcePasswordChange, value3 ?? basicUser.Status);
		validateResult = new ValidateResult();
		validateResult.ValidatedOk = true;
		validateResult.ResultItem = ConvertToBasicUserConfigurationItem(parentPath, basicUser);
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		string description;
		return ValidateItemInternal(requestContext, item, path, parentPath, out description);
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, out string description)
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
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			try
			{
				BasicUser.QueryBasicUserById(requestContext, path.Id);
			}
			catch (BasicUserNotExistException)
			{
				throw new PathNotFoundException(path.Text);
			}
			string id = path.Id;
			description = null;
			if (item.Properties != null)
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
			}
			try
			{
				BasicUser.UpdateBasicUserValidation(requestContext, ref id, ref description);
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
		if (path.IsFolder)
		{
			BasicUser[] array = BasicUser.QueryBasicUsers(requestContext);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			BasicUser[] array2 = array;
			foreach (BasicUser user in array2)
			{
				list.Add(ConvertToBasicUserConfigurationItem(path, user));
			}
			return list.ToArray();
		}
		ConfigurationItem configurationItem = GroupConfigurationItemFactory.CreateFolderItem("ClaimFolder", path);
		return new ConfigurationItem[1] { configurationItem };
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
			return GroupConfigurationItemFactory.CreateFolderItem("BasicUserFolder", ConfigurationItemPath.CreateSystemPath());
		}
		BasicUser user;
		try
		{
			user = BasicUser.QueryBasicUserById(requestContext, path.Id);
		}
		catch (BasicUserNotExistException)
		{
			throw new PathNotFoundException(path.Text);
		}
		ConfigurationItemPath parentPath = new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "BasicUserFolder").CreatePath();
		bool ownUser = string.Equals(requestContext.UserContext.Identifier, path.Id, StringComparison.OrdinalIgnoreCase);
		return ConvertToBasicUserConfigurationItem(parentPath, user, ownUser);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		if (!item.MethodIds.Contains(methodId))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", methodId, item.ItemType), "methodId");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (methodId)
		{
		case "AddBasicUser":
			if (!flag)
			{
				return CreateInvokeInfoAddBasicUser(item, methodId);
			}
			return AddBasicUser(requestContext, item, path);
		case "RemoveBasicUser":
			if (!flag)
			{
				return CreateInvokeInfoRemoveBasicUser(item, GetChildItems(requestContext, path), methodId);
			}
			return RemoveBasicUser(requestContext, item, path);
		case "ChangePasswordBasicUser":
			if (!flag)
			{
				return CreateInvokeInfoChangePasswordBasicUser(item, methodId);
			}
			return ChangePasswordBasicUser(requestContext, item, path);
		default:
			throw new ArgumentException("Unknown methodId:" + methodId);
		}
	}

	private static ConfigurationItem CreateInvokeInfoRemoveBasicUser(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveBasicUser, "PropertyRemoveBasicUser", ConfigurationApiResources.PropertyRemoveBasicUser, item, members);
	}

	private static ConfigurationItem CreateInvokeInfoAddBasicUser(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddBasicUser;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[8]
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(Guid.Empty),
			GeneralPropertyDefinitions.GetNameProperty("", isSettable: true),
			GeneralPropertyDefinitions.GetDescriptionProperty(""),
			BasicUserPropertyDefinition.CanChangePasswordProperty(canChangePassword: true),
			BasicUserPropertyDefinition.ForcePasswordChangeProperty(forceChangePassword: false),
			BasicUserPropertyDefinition.PasswordProperty(""),
			BasicUserPropertyDefinition.Status(BasicUserStatus.Enabled),
			BasicUserPropertyDefinition.LockoutEnd(null)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoChangePasswordBasicUser(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardChangePasswordBasicUser;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { BasicUserPropertyDefinition.PasswordProperty("") };
		return configurationItem;
	}

	private static ConfigurationItem ChangePasswordBasicUser(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
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
		ThrowIfPathInvalid(path);
		if (!string.Equals(path.ItemType, "BasicUser", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid path. Path: " + path.Text);
		}
		if (path.IsFolder)
		{
			throw new PathInvalidException("Invalid path. Path: " + path.Text);
		}
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Password", out string value))
		{
			throw new PropertyInvalidException("No new password selected.", "Password");
		}
		try
		{
			BasicUser.QueryBasicUserById(requestContext, path.Id);
		}
		catch (BasicUserNotExistException)
		{
			throw new PathNotFoundException(path.Text);
		}
		BasicUser.ChangePasswordBasicUser(requestContext, path.Id, value);
		item.Properties = Array.Empty<Property>();
		return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
	}

	private static ConfigurationItem RemoveBasicUser(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
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
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			BasicUser.DeleteBasicUser(requestContext, invokeMemberItemSelectionPath.Id);
		}
		catch (BasicUserDeleteException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem AddBasicUser(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
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
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Unsupported item type. " + path.Text);
		}
		string value = null;
		string value2 = null;
		string value3 = null;
		string value4 = null;
		bool? value5 = null;
		bool? value6 = null;
		BasicUserStatus? value7 = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out value3);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CanChangePassword", out value5);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ForcePasswordChange", out value6);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Password", out value4);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Status", out value7);
		}
		if (value.IsGuid() && value.Equals(Guid.Empty.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			value = null;
		}
		try
		{
			BasicUser user = BasicUser.CreateBasicUser(requestContext, value, value2, value3, value5 ?? true, value4, value6 == true, value7.GetValueOrDefault());
			ConfigurationItem configurationItem = ConvertToBasicUserConfigurationItem(path, user);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (BasicUserCreationException)
		{
			throw new PropertyInvalidException("User already exists");
		}
		catch (BasicUserPasswordException)
		{
			throw new PropertyInvalidException("Password not complex enough");
		}
	}

	private static ConfigurationItem ConvertToBasicUserConfigurationItem(ConfigurationItemPath parentPath, BasicUser user, bool ownUser = false)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "BasicUser";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = user.Name;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.ChildrenFilled = false;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, user.Id).Text;
		List<Property> list = new List<Property>();
		bool isSettable = !ownUser;
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(user.Id));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(user.Name, isSettable: false));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(user.Description, isSettable));
		list.Add(GeneralPropertyDefinitions.GetCreatedDateProperty(user.CreatedDate));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(user.LastModified));
		list.Add(BasicUserPropertyDefinition.SidProperty(user.Sid));
		list.Add(BasicUserPropertyDefinition.CanChangePasswordProperty(user.CanChangePassword));
		list.Add(BasicUserPropertyDefinition.ForcePasswordChangeProperty(user.ForcePasswordChange));
		list.Add(BasicUserPropertyDefinition.Status(user.Status));
		list.Add(BasicUserPropertyDefinition.LockoutEnd(user.LockoutEnd));
		list.Add(BasicUserPropertyDefinition.IsExternal(user.IsExternal));
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[1] { "ChangePasswordBasicUser" };
		return configurationItem;
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
}
