using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("LoginProvider")]
public class LoginProviderGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(LoginProvider.QueryById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(LoginProvider.QuerysByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer")
		{
			return LoginProvider.QueryIds(requestContext);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		LoginProviderData loginProviderData = ConvertToLoginProvider(properties);
		loginProviderData = LoginProvider.Create(requestContext, loginProviderData.Id, loginProviderData.Name, loginProviderData.ClientId, loginProviderData.ClientSecret, loginProviderData.ClientSecretType, loginProviderData.CallbackPath, loginProviderData.Authority, loginProviderData.Scopes, loginProviderData.UserNameClaimType, loginProviderData.PromptForLogin.Value, loginProviderData.Enabled.Value);
		return ConvertToGenericObject2(loginProviderData);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		LoginProviderData loginProviderData = ConvertToLoginProvider(genericObject);
		loginProviderData = LoginProvider.Update(requestContext, loginProviderData.Id, loginProviderData.Name, loginProviderData.ClientId, loginProviderData.ClientSecret, loginProviderData.ClientSecretType, loginProviderData.CallbackPath, loginProviderData.Authority, loginProviderData.Scopes, loginProviderData.UserNameClaimType, loginProviderData.PromptForLogin, loginProviderData.Enabled);
		return ConvertToGenericObject2(loginProviderData);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		LoginProvider.Delete(requestContext, id);
	}

	private static GenericObject2 ConvertToGenericObject2(LoginProviderData loginProvider)
	{
		if (loginProvider == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", loginProvider.Id));
		list.Add(new ObjectProperty2("Name", loginProvider.Name));
		list.Add(new ObjectProperty2("ClientId", loginProvider.ClientId));
		list.Add(new ObjectProperty2("ClientSecret", (string)null));
		list.Add(new ObjectProperty2("ClientSecretType", loginProvider.ClientSecretType));
		list.Add(new ObjectProperty2("ClientSecretHasValue", !string.IsNullOrEmpty(loginProvider.ClientSecret)));
		list.Add(new ObjectProperty2("CallbackPath", loginProvider.CallbackPath));
		list.Add(new ObjectProperty2("Authority", loginProvider.Authority));
		list.Add(new ObjectProperty2("UserNameClaimType", loginProvider.UserNameClaimType));
		list.Add(new ObjectProperty2("Scopes", loginProvider.Scopes.Select((string s, int i) => new ObjectProperty2(i.ToString(CultureInfo.InvariantCulture), s)).ToArray()));
		list.Add(new ObjectProperty2("PromptForLogin", loginProvider.PromptForLogin.Value));
		list.Add(new ObjectProperty2("Enabled", loginProvider.Enabled.Value));
		return new GenericObject2(loginProvider.Id, "LoginProvider", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertToGenericObject2s(IEnumerable<LoginProviderData> loginProviders)
	{
		if (loginProviders != null)
		{
			return loginProviders.Select((LoginProviderData p) => ConvertToGenericObject2(p)).ToArray();
		}
		return Array.Empty<GenericObject2>();
	}

	private static LoginProviderData ConvertToLoginProvider(GenericObject2 genericObject)
	{
		return ConvertToLoginProvider(genericObject.Properties);
	}

	private static LoginProviderData ConvertToLoginProvider(ObjectProperty2[] properties)
	{
		LoginProviderData loginProviderData = new LoginProviderData();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			loginProviderData.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			loginProviderData.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "ClientId"))
		{
			loginProviderData.ClientId = GenericObjectHelper2.GetProperty<string>(properties, "ClientId");
		}
		if (GenericObjectHelper2.Contains(properties, "ClientSecret"))
		{
			loginProviderData.ClientSecret = GenericObjectHelper2.GetProperty<string>(properties, "ClientSecret");
		}
		if (GenericObjectHelper2.Contains(properties, "ClientSecretType"))
		{
			loginProviderData.ClientSecretType = GenericObjectHelper2.GetProperty<string>(properties, "ClientSecretType");
		}
		if (GenericObjectHelper2.Contains(properties, "CallbackPath"))
		{
			loginProviderData.CallbackPath = GenericObjectHelper2.GetProperty<string>(properties, "CallbackPath");
		}
		if (GenericObjectHelper2.Contains(properties, "Authority"))
		{
			loginProviderData.Authority = GenericObjectHelper2.GetProperty<string>(properties, "Authority");
		}
		if (GenericObjectHelper2.Contains(properties, "UserNameClaimType"))
		{
			loginProviderData.UserNameClaimType = GenericObjectHelper2.GetProperty<string>(properties, "UserNameClaimType");
		}
		ObjectProperty2[] nestedProperties = GenericObjectHelper2.GetNestedProperties(properties, "Scopes");
		if (nestedProperties != null)
		{
			loginProviderData.Scopes = new Collection<string>(nestedProperties.Select((ObjectProperty2 p) => p.Value as string).ToList());
		}
		if (GenericObjectHelper2.Contains(properties, "PromptForLogin"))
		{
			loginProviderData.PromptForLogin = GenericObjectHelper2.GetProperty<bool>(properties, "PromptForLogin");
		}
		if (GenericObjectHelper2.Contains(properties, "Enabled"))
		{
			loginProviderData.Enabled = GenericObjectHelper2.GetProperty<bool>(properties, "Enabled");
		}
		return loginProviderData;
	}
}
