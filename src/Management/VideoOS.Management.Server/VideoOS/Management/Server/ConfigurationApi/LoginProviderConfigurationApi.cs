using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPILoginProvider")]
public class LoginProviderConfigurationApi : ConfigurationApiEntityBase
{
	public override ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		LoginProviderData loginProvider;
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, parentPath, out loginProvider);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		LoginProviderData loginProvider2 = LoginProvider.Update(requestContext, loginProvider.Id, loginProvider.Name, loginProvider.ClientId, loginProvider.ClientSecret, loginProvider.ClientSecretType, loginProvider.CallbackPath, loginProvider.Authority, loginProvider.Scopes, loginProvider.UserNameClaimType, loginProvider.PromptForLogin, loginProvider.Enabled);
		return new ValidateResult
		{
			ValidatedOk = true,
			ResultItem = ConvertToConfigurationItem(loginProvider2)
		};
	}

	public override ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		LoginProviderData loginProvider;
		return ValidateItemInternal(requestContext, item, path, parentPath, out loginProvider);
	}

	private ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, out LoginProviderData loginProvider)
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
		ThrowIfParentPathInvalid(parentPath);
		loginProvider = ConvertToLoginProvider(path.Id, item.EnableProperty?.Enabled, item.Properties);
		try
		{
			LoginProvider.UpdateValidation(requestContext, loginProvider.Id, loginProvider.Name, loginProvider.ClientId, loginProvider.ClientSecret, loginProvider.ClientSecretType, loginProvider.CallbackPath, loginProvider.Authority, loginProvider.Scopes);
			return new ValidateResult
			{
				ValidatedOk = true,
				ResultItem = item
			};
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
		return new ConfigurationItem[1] { GroupConfigurationItemFactory.CreateFolderItem("RegisteredClaimFolder", path) };
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
		return ConvertToConfigurationItem(LoginProvider.QueryById(requestContext, path.Id));
	}

	protected override ConfigurationItem GetMethodInputParameters(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		throw new NotSupportedException();
	}

	protected override ConfigurationItem ExecuteMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		throw new NotSupportedException();
	}

	protected override void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		ThrowIfItemPathInvalid(path);
	}

	public static void ThrowIfItemPathInvalid(ConfigurationItemPath path)
	{
		if (string.Equals(path?.ItemType, "LoginProvider", StringComparison.Ordinal) && path.Id.IsGuid() && !path.IsFolder)
		{
			return;
		}
		throw new PathInvalidException("Invalid path. Path: " + path.Text);
	}

	protected override void ThrowIfParentPathInvalid(ConfigurationItemPath path)
	{
		LoginProviderFolderConfigurationApi.ThrowIfFolderPathInvalid(path);
	}

	public static LoginProviderData ConvertToLoginProvider(string id, bool? enabled, IEnumerable<Property> properties)
	{
		LoginProviderData loginProviderData = new LoginProviderData();
		loginProviderData.Id = id;
		loginProviderData.Enabled = enabled;
		if (properties != null)
		{
			loginProviderData.Name = (properties.TryGetPropertyValue("Name", out string value) ? value : null);
			loginProviderData.ClientId = (properties.TryGetPropertyValue("ClientId", out string value2) ? value2 : null);
			loginProviderData.ClientSecret = (properties.TryGetPropertyValue("ClientSecret", out string value3) ? value3 : null);
			loginProviderData.ClientSecretType = (properties.TryGetPropertyValue("ClientSecretType", out string value4) ? value4 : null);
			loginProviderData.CallbackPath = (properties.TryGetPropertyValue("CallbackPath", out string value5) ? value5 : null);
			loginProviderData.Authority = (properties.TryGetPropertyValue("Authority", out string value6) ? value6 : null);
			loginProviderData.UserNameClaimType = (properties.TryGetPropertyValue("UserNameClaimType", out string value7) ? value7 : null);
			loginProviderData.Scopes = ((properties.TryGetPropertyValue("Scopes", out string[] values) && values != null) ? new Collection<string>(values.ToList()) : null);
			loginProviderData.PromptForLogin = (properties.TryGetPropertyValue("PromptForLogin", out bool? value8) ? value8 : ((bool?)null));
		}
		return loginProviderData;
	}

	public static ConfigurationItem ConvertToConfigurationItem(LoginProviderData loginProvider)
	{
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			ItemType = "LoginProvider",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = loginProvider.Name,
			Path = ConfigurationItemPath.CreateItemPath("LoginProvider", loginProvider.Id).Text,
			ParentPath = LoginProviderFolderConfigurationApi.Path().Text,
			EnableProperty = (loginProvider.Enabled.HasValue ? GeneralPropertyDefinitions.GetEnabledProperty(loginProvider.Enabled.Value, uiFollowsEnabled: true) : null),
			Children = Array.Empty<ConfigurationItem>(),
			ChildrenFilled = false,
			MethodIds = Array.Empty<string>()
		};
		configurationItem.Properties = new Property[11]
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(loginProvider.Id),
			GeneralPropertyDefinitions.GetNameProperty(loginProvider.Name),
			LoginProviderPropertyDefinition.ClientId(loginProvider.ClientId),
			LoginProviderPropertyDefinition.ClientSecret(null),
			LoginProviderPropertyDefinition.ClientSecretType(loginProvider.ClientSecretType),
			LoginProviderPropertyDefinition.ClientSecretHasValue(!string.IsNullOrEmpty(loginProvider.ClientSecret)),
			LoginProviderPropertyDefinition.Authority(loginProvider.Authority),
			LoginProviderPropertyDefinition.CallbackPath(loginProvider.CallbackPath),
			LoginProviderPropertyDefinition.Scopes(loginProvider.Scopes),
			LoginProviderPropertyDefinition.UserNameClaimType(loginProvider.UserNameClaimType),
			LoginProviderPropertyDefinition.PromptForLogin(loginProvider.PromptForLogin.Value)
		};
		return configurationItem;
	}
}
