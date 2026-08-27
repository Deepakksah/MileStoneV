using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIRegisteredClaimFolder")]
public class RegisteredClaimFolderConfigurationApi : ConfigurationApiEntityBase
{
	public override ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public override ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public override ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
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
		string[] array = RegisteredClaim.QueryIdsByLoginProvider(requestContext, path.Id);
		RegisteredClaimData[] array2 = (array.Any() ? RegisteredClaim.QueryByIds(requestContext, array) : Array.Empty<RegisteredClaimData>());
		ConfigurationItem[] array3 = new ConfigurationItem[array2.Length];
		for (int i = 0; i < array3.Length; i++)
		{
			array3[i] = RegisteredClaimConfigurationApi.ConvertToConfigurationItem(array2[i]);
		}
		return array3;
	}

	public override ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
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
		return GroupConfigurationItemFactory.CreateFolderItem("RegisteredClaimFolder", ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id));
	}

	protected override ConfigurationItem GetMethodInputParameters(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		if (!(methodId == "AddRegisteredClaim"))
		{
			if (methodId == "RemoveRegisteredClaim")
			{
				ConfigurationItem[] childItems = GetChildItems(requestContext, path);
				return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.MethodRemoveRegisteredClaim, "MethodRemoveRegisteredClaim", ConfigurationApiResources.MethodRemoveRegisteredClaim, item, childItems);
			}
			throw new ArgumentException("Unknown methodId: " + methodId, "methodId");
		}
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodAddRegisteredClaim;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { "AddRegisteredClaim" };
		configurationItem.Properties = new Property[3]
		{
			GeneralPropertyDefinitions.GetNameProperty("", isSettable: true),
			RegisteredClaimPropertyDefinition.DisplayNameCreationProperty(""),
			RegisteredClaimPropertyDefinition.CaseSensitive(caseSensitive: true, isSettable: true)
		};
		return configurationItem;
	}

	protected override ConfigurationItem ExecuteMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		ThrowIfPathInvalid(path);
		if (!(methodId == "AddRegisteredClaim"))
		{
			if (methodId == "RemoveRegisteredClaim")
			{
				return DeleteRegisteredClaim(requestContext, item);
			}
			throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
		}
		return AddRegisteredClaim(requestContext, item, path);
	}

	protected override void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		ThrowIfFolderPathInvalid(path);
	}

	protected override void ThrowIfParentPathInvalid(ConfigurationItemPath path)
	{
		if (path != null && path.ParentItemType != null && path.ParentItemType.Length == 0 && path.Id != null && path.Id.IsGuid() && path.ItemType == "LoginProvider")
		{
			return;
		}
		throw new PathInvalidException("Invalid claim info folder parent path: " + path?.Text);
	}

	public static ConfigurationItemPath Path(string loginProviderId)
	{
		return ConfigurationItemPath.CreateFolderPath("LoginProvider", loginProviderId, "RegisteredClaimFolder");
	}

	public static void ThrowIfFolderPathInvalid(ConfigurationItemPath path)
	{
		if (path != null && path.IsFolder && string.Equals(path.ParentItemType, "LoginProvider", StringComparison.Ordinal) && path.Id != null && path.Id.IsGuid() && string.Equals(path.ItemType, "RegisteredClaimFolder", StringComparison.Ordinal))
		{
			return;
		}
		throw new PathInvalidException("Invalid claim info folder path: " + path?.Text);
	}

	private static ConfigurationItem AddRegisteredClaim(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		RegisteredClaimNotValidated registeredClaimNotValidated = new RegisteredClaimNotValidated();
		if (item.Properties != null)
		{
			registeredClaimNotValidated.LoginProviderId = path.Id;
			registeredClaimNotValidated.Name = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out string value) ? value : null);
			registeredClaimNotValidated.DisplayName = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DisplayName", out string value2) ? value2 : null);
			registeredClaimNotValidated.CaseSensitive = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CaseSensitive", out bool? value3) ? value3 : ((bool?)null));
		}
		try
		{
			RegisteredClaimData registeredClaimData = RegisteredClaim.Create(requestContext, registeredClaimNotValidated.LoginProviderId, registeredClaimNotValidated.Name, registeredClaimNotValidated.DisplayName, registeredClaimNotValidated.CaseSensitive);
			ConfigurationItem configurationItem = RegisteredClaimConfigurationApi.ConvertToConfigurationItem(registeredClaimData);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true, RegisteredClaimPropertyDefinition.DisplayNameCreationProperty(registeredClaimData.DisplayName));
		}
		catch (VmoServerException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
	}

	private static ConfigurationItem DeleteRegisteredClaim(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		RegisteredClaimConfigurationApi.ThrowIfItemPathInvalid(invokeMemberItemSelectionPath);
		try
		{
			RegisteredClaim.Delete(requestContext, invokeMemberItemSelectionPath.Id);
		}
		catch (VmoServerException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(new ConfigurationItem
		{
			Path = invokeMemberItemSelectionPath.Text,
			ParentPath = item.Path
		});
	}
}
