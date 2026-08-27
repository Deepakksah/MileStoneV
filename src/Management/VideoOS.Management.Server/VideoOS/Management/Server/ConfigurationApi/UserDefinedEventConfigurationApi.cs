using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIUserDefinedEvent")]
[EntityName("ConfigAPIUserDefinedEventFolder")]
public class UserDefinedEventConfigurationApi : NewSecurityConfigurationApi, IConfigurationApiSupport
{
	protected override Dictionary<string, List<string>> ItemTypeActionsToExclude => new Dictionary<string, List<string>> { 
	{
		"UserDefinedEvent",
		new List<string> { "CREATE_USER_DEFINED_EVENT", "ADMIN_SECURITY" }
	} };

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		ValidateResult validateResult = ValidateItemInternal(item, path, out var name, allowNameIsNull: true);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		UserDefinedEvent userDefinedEvent = UserDefinedEvent.QueryUserDefinedEvent(requestContext, path.Id);
		if (userDefinedEvent == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		if (userDefinedEvent.Subtype != "UserDefined")
		{
			throw new ArgumentException("Only event types with subtype='UserDefined' can be updated");
		}
		if (name != null)
		{
			userDefinedEvent.Name = name;
		}
		userDefinedEvent = UserDefinedEvent.UpdateUserDefinedEvent(requestContext, userDefinedEvent);
		ConfigurationItem resultItem = ConvertToUserDefinedEventConfigurationItem(parentPath, userDefinedEvent);
		validateResult = new ValidateResult();
		validateResult.ValidatedOk = true;
		validateResult.ResultItem = resultItem;
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			if (UserDefinedEvent.QueryUserDefinedEvent(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			string name;
			return ValidateItemInternal(item, path, out name, allowNameIsNull: false);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(ConfigurationItem item, ConfigurationItemPath path, out string name, bool allowNameIsNull)
	{
		name = null;
		try
		{
			if (!path.IsFolder)
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
				}
				Validation.CheckName(name, "Name", allowNameIsNull);
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				return validateResult;
			}
			throw new NotSupportedException();
		}
		catch (ArgumentException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			string[] array = UserDefinedEvent.QueryUserDefinedEventIds(requestContext);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			if (array != null && array.Length != 0)
			{
				UserDefinedEvent[] array2 = UserDefinedEvent.QueryUserDefinedEvents(requestContext, array);
				foreach (UserDefinedEvent userDefinedEvent in array2)
				{
					if (userDefinedEvent.Subtype == "System" || userDefinedEvent.Subtype == "UserDefined")
					{
						list.Add(ConvertToUserDefinedEventConfigurationItem(path, userDefinedEvent));
					}
				}
			}
			return list.ToArray();
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("UserDefinedEventFolder", ConfigurationItemPath.CreateSystemPath());
		}
		UserDefinedEvent userDefinedEvent = UserDefinedEvent.QueryUserDefinedEvent(requestContext, path.Id);
		if (userDefinedEvent == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToUserDefinedEventConfigurationItem(new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "UserDefinedEventFolder").CreatePath(), userDefinedEvent);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!item.MethodIds.Contains(method))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", method, item.ItemType), "method");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (method)
		{
		case "AddUserDefinedEvent":
			if (!flag)
			{
				return CreateInvokeInfoAddUserDefinedEvent(item, method);
			}
			return AddUserDefinedEvent(requestContext, item, path);
		case "RemoveUserDefinedEvent":
			if (!flag)
			{
				return CreateInvokeInfoRemoveUserDefinedEvent(item, GetChildItems(requestContext, path), method);
			}
			return RemoveUserDefinedEvent(requestContext, item, path);
		case "ChangeSecurityPermissions":
			return InvokeSecurityMethod(requestContext, item, path, method);
		default:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
	}

	private static ConfigurationItem CreateInvokeInfoAddUserDefinedEvent(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddUserDefinedEvent;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { GeneralPropertyDefinitions.GetNameProperty("") };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveUserDefinedEvent(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveUserDefinedEvent, "PropertyRemoveUserDefinedEvent", ConfigurationApiResources.PropertyRemoveUserDefinedEvent, item, members);
	}

	private static ConfigurationItem AddUserDefinedEvent(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out value);
		}
		try
		{
			Validation.CheckName(value, "Name", allowNull: false);
		}
		catch (ArgumentException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
		try
		{
			UserDefinedEvent userDefinedEvent = UserDefinedEvent.CreateUserDefinedEvent(requestContext, new UserDefinedEvent
			{
				Name = value,
				Subtype = "UserDefined"
			});
			ConfigurationItem configurationItem = ConvertToUserDefinedEventConfigurationItem(path, userDefinedEvent);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (Exception ex2)
		{
			throw new PropertyInvalidException(ex2.Message);
		}
	}

	private static ConfigurationItem RemoveUserDefinedEvent(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			UserDefinedEvent userDefinedEvent = UserDefinedEvent.QueryUserDefinedEvent(requestContext, invokeMemberItemSelectionPath.Id);
			if (userDefinedEvent != null && userDefinedEvent.Subtype != "UserDefined")
			{
				throw new ArgumentException("Cannot remove this type of event");
			}
			UserDefinedEvent.DeleteUserDefinedEvent(requestContext, invokeMemberItemSelectionPath.Id);
		}
		catch (RemoveNonExistingUserDefinedEventException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
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

	private static ConfigurationItem ConvertToUserDefinedEventConfigurationItem(ConfigurationItemPath parentPath, UserDefinedEvent userDefinedEvent)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "UserDefinedEvent",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = userDefinedEvent.Name,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, userDefinedEvent.Id).Text;
		List<Property> list = new List<Property> { GeneralPropertyDefinitions.GetIdPropertyGuid(userDefinedEvent.Id) };
		if (userDefinedEvent.Subtype == "UserDefined")
		{
			list.Add(GeneralPropertyDefinitions.GetNameProperty(userDefinedEvent.Name, isSettable: true));
		}
		else
		{
			list.Add(GeneralPropertyDefinitions.GetNameProperty(userDefinedEvent.Name, isSettable: false));
		}
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(userDefinedEvent.LastModified));
		list.Add(GeneralPropertyDefinitions.GetCreatedDateProperty(userDefinedEvent.CreatedDate));
		list.Add(SubtypeProperty(userDefinedEvent.Subtype));
		obj.Properties = list.ToArray();
		List<string> list2 = new List<string>();
		obj.MethodIds = list2.ToArray();
		obj.MethodIds = SecurityConfigurationApiBase.AddSecurityMethods(obj.MethodIds);
		return obj;
	}

	private static Property SubtypeProperty(string val)
	{
		return new Property("Subtype", val)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyUserDefinedEventSubtype,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyUserDefinedEventSubtype",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}

	public override bool SetSecurityOnRoleOnly()
	{
		return false;
	}
}
