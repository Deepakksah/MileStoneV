using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPILoginProviderFolder")]
public class LoginProviderFolderConfigurationApi : ConfigurationApiEntityBase
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
		LoginProviderData[] array = LoginProvider.QueryAll(requestContext);
		ConfigurationItem[] array2 = new ConfigurationItem[array.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = LoginProviderConfigurationApi.ConvertToConfigurationItem(array[i]);
		}
		return array2;
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
		return GroupConfigurationItemFactory.CreateFolderItem("LoginProviderFolder", ConfigurationItemPath.CreateSystemPath());
	}

	protected override ConfigurationItem GetMethodInputParameters(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		if (!(methodId == "AddLoginProvider"))
		{
			if (methodId == "RemoveLoginProvider")
			{
				ConfigurationItem[] childItems = GetChildItems(requestContext, path);
				return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.MethodRemoveLoginProvider, "MethodRemoveLoginProvider", ConfigurationApiResources.MethodRemoveLoginProvider, item, childItems);
			}
			throw new ArgumentException("Unknown methodId: " + methodId, "methodId");
		}
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodAddLoginProvider;
		configurationItem.ParentPath = "/";
		configurationItem.Path = Path().Text;
		configurationItem.MethodIds = new string[1] { "AddLoginProvider" };
		configurationItem.Properties = new Property[11]
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(Guid.Empty, uiVisible: false, isSettable: true),
			GeneralPropertyDefinitions.GetNameProperty("", isSettable: true),
			LoginProviderPropertyDefinition.ClientId(""),
			LoginProviderPropertyDefinition.ClientSecret(""),
			LoginProviderPropertyDefinition.ClientSecretType("SharedSecret"),
			LoginProviderPropertyDefinition.CallbackPath("/signin-oidc"),
			LoginProviderPropertyDefinition.Authority("https://"),
			LoginProviderPropertyDefinition.UserNameClaimType(""),
			LoginProviderPropertyDefinition.Scopes(Enumerable.Empty<string>()),
			LoginProviderPropertyDefinition.PromptForLogin(promptForLogin: true),
			LoginProviderPropertyDefinition.EnableCreationProperty(enabled: true)
		};
		return configurationItem;
	}

	protected override ConfigurationItem ExecuteMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		ThrowIfPathInvalid(path);
		if (!(methodId == "AddLoginProvider"))
		{
			if (methodId == "RemoveLoginProvider")
			{
				return DeleteLoginProvider(requestContext, item);
			}
			throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
		}
		return AddLoginProvider(requestContext, item);
	}

	protected override void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		ThrowIfFolderPathInvalid(path);
	}

	protected override void ThrowIfParentPathInvalid(ConfigurationItemPath path)
	{
		if (path != null && path.ParentItemType != null && path.ParentItemType.Length == 0 && path.Id != null && path.Id.Length == 0 && path.ItemType == "System")
		{
			return;
		}
		throw new PathInvalidException("Invalid login provider folder path: " + path?.Text);
	}

	public static ConfigurationItemPath Path()
	{
		return ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), "LoginProviderFolder");
	}

	public static void ThrowIfFolderPathInvalid(ConfigurationItemPath path)
	{
		if (path != null && path.IsFolder && string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			return;
		}
		throw new PathInvalidException("Invalid login provider folder path: " + path?.Text);
	}

	private static ConfigurationItem AddLoginProvider(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		LoginProviderData loginProviderData = new LoginProviderData();
		if (item.Properties != null)
		{
			loginProviderData.Id = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out string value) ? value : null);
			loginProviderData.ClientId = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClientId", out string value2) ? value2 : null);
			loginProviderData.ClientSecret = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClientSecret", out string value3) ? value3 : null);
			loginProviderData.ClientSecretType = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClientSecretType", out string value4) ? value4 : null);
			loginProviderData.CallbackPath = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CallbackPath", out string value5) ? value5 : null);
			loginProviderData.Authority = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Authority", out string value6) ? value6 : null);
			loginProviderData.UserNameClaimType = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("UserNameClaimType", out string value7) ? value7 : null);
			loginProviderData.Scopes = ((((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Scopes", out string[] values) && values != null) ? new Collection<string>(values.ToList()) : new Collection<string>());
			loginProviderData.PromptForLogin = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PromptForLogin", out bool? value8) ? value8 : new bool?(true));
			loginProviderData.Name = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out string value9) ? value9 : null);
			loginProviderData.Enabled = (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Enabled", out bool? value10) ? value10 : new bool?(true));
		}
		if (loginProviderData.Id != null && loginProviderData.Id.IsGuid() && loginProviderData.Id.Equals(Guid.Empty.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			loginProviderData.Id = null;
		}
		try
		{
			LoginProviderData loginProviderData2 = LoginProvider.Create(requestContext, loginProviderData.Id, loginProviderData.Name, loginProviderData.ClientId, loginProviderData.ClientSecret, loginProviderData.ClientSecretType, loginProviderData.CallbackPath, loginProviderData.Authority, loginProviderData.Scopes, loginProviderData.UserNameClaimType, loginProviderData.PromptForLogin.Value, loginProviderData.Enabled.Value);
			ConfigurationItem configurationItem = LoginProviderConfigurationApi.ConvertToConfigurationItem(loginProviderData2);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true, LoginProviderPropertyDefinition.EnableCreationProperty(loginProviderData2.Enabled.Value));
		}
		catch (VmoServerException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
	}

	private static ConfigurationItem DeleteLoginProvider(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		LoginProviderConfigurationApi.ThrowIfItemPathInvalid(invokeMemberItemSelectionPath);
		try
		{
			LoginProvider.Delete(requestContext, invokeMemberItemSelectionPath.Id);
		}
		catch (VmoServerException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(new ConfigurationItem
		{
			Path = ConfigurationItemPath.CreateItemPath("LoginProvider", invokeMemberItemSelectionPath.Id).Text,
			ParentPath = Path().Text
		});
	}
}
