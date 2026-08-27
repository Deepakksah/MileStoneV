using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.IdentityServer.Common.Model.Responses;
using VideoOS.Management.Common;
using VideoOS.Management.Server.DataComponents.Idp;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class LoginProviderIdpComponent : IdpComponentBase, ILoginProviderIdpComponent, IDisposable
{
	public LoginProviderIdpComponent(IdpClientService idpClientService, ITokenCacheService tokenCacheService)
		: base(idpClientService, tokenCacheService)
	{
	}

	public LoginProviderData CreateLoginProvider(Guid id, string name, string clientId, string clientSecret, string clientSecretType, string callbackPath, string authority, IEnumerable<string> scopes, string userNameClaimType, bool promptForLogin, bool enabled)
	{
		LoginProviderCreateModel model = new LoginProviderCreateModel
		{
			Id = id,
			DisplayName = name,
			ClientId = clientId,
			ClientSecret = clientSecret,
			ClientSecretType = clientSecretType,
			Authority = authority,
			CallbackPath = callbackPath,
			Enabled = enabled,
			PromptForLogin = promptForLogin,
			Scopes = scopes.ToArray(),
			UserNameClaimType = userNameClaimType
		};
		return ConvertToLoginProviderData(CallIdp((IIdpAdministrationClient client, string token) => client.CreateLoginProvider(token, model), (IdpException IdpException) => HandleCreateException(IdpException)));
	}

	public LoginProviderData[] QueryLoginProviders()
	{
		return CallIdp((IIdpAdministrationClient client, string token) => client.GetLoginProviders(token), (IdpException IdpException) => HandleGetException(IdpException))?.Select((LoginProviderGetModel m) => ConvertToLoginProviderData(m))?.ToArray() ?? Array.Empty<LoginProviderData>();
	}

	public LoginProviderData QueryLoginProviderById(Guid id)
	{
		return ConvertToLoginProviderData(CallIdp((IIdpAdministrationClient client, string token) => client.GetLoginProvider(token, id), (IdpException IdpException) => HandleGetException(IdpException)));
	}

	public LoginProviderData[] QueryLoginProvidersByIds(IEnumerable<Guid> ids)
	{
		return CallIdp((IIdpAdministrationClient client, string token) => client.GetLoginProviders(token), (IdpException IdpException) => HandleGetException(IdpException))?.Where((LoginProviderGetModel m) => ids.Contains(m.Id))?.Select((LoginProviderGetModel m) => ConvertToLoginProviderData(m))?.ToArray() ?? Array.Empty<LoginProviderData>();
	}

	public LoginProviderData UpdateLoginProvider(Guid id, string name, string clientId, string clientSecret, string clientSecretType, string callbackPath, string authority, IEnumerable<string> scopes, string userNameClaimType, bool? promptForLogin, bool? enabled)
	{
		LoginProviderUpdateModel updateModel = new LoginProviderUpdateModel
		{
			DisplayName = name,
			ClientId = clientId,
			ClientSecret = clientSecret,
			ClientSecretType = clientSecretType,
			Authority = authority,
			CallbackPath = callbackPath,
			Enabled = enabled,
			PromptForLogin = promptForLogin,
			Scopes = scopes.ToArray(),
			UserNameClaimType = userNameClaimType
		};
		return ConvertToLoginProviderData(CallIdp((IIdpAdministrationClient client, string token) => client.UpdateLoginProvider(token, id, updateModel), (IdpException ex) => HandleUpdateException(ex)));
	}

	public void DeleteLoginProvider(Guid id)
	{
		CallIdp((IIdpAdministrationClient client, string token) => client.DeleteLoginProvider(token, id), (IdpException ex) => HandleDeleteException(ex));
	}

	public void Dispose()
	{
	}

	private static LoginProviderData ConvertToLoginProviderData(LoginProviderGetModel loginProviderGetModel)
	{
		return new LoginProviderData
		{
			Id = loginProviderGetModel.Id.ToString(),
			Name = loginProviderGetModel.DisplayName,
			ClientId = loginProviderGetModel.ClientId,
			ClientSecret = loginProviderGetModel.ClientSecret,
			ClientSecretType = loginProviderGetModel.ClientSecretType,
			Authority = loginProviderGetModel.Authority,
			CallbackPath = loginProviderGetModel.CallbackPath,
			Enabled = loginProviderGetModel.Enabled,
			PromptForLogin = loginProviderGetModel.PromptForLogin,
			Scopes = new Collection<string>(loginProviderGetModel.Scopes.ToList()),
			UserNameClaimType = loginProviderGetModel.UserNameClaimType
		};
	}

	private static Exception HandleGetException(Exception ex)
	{
		if (ex is NotFoundException)
		{
			return VmoExceptionFactory.VMO60357_ObjectNotFoundException();
		}
		return new GenericIdpException(VmoResources.VMO68000, ex);
	}

	private static Exception HandleCreateException(IdpException ex)
	{
		if (ex is InvalidArgumentsException ex2)
		{
			return new ArgumentException(VmoResources.VMO62001, ex2.Arguments.First());
		}
		if (ex is DefaultErrorException ex3)
		{
			switch (ex3.Errors.Keys?.FirstOrDefault())
			{
			case "ThereCanBeOnlyOne":
				return VmoExceptionFactory.VMO60358_VmoServerServiceException();
			case "DuplicateLoginProvider":
				return VmoExceptionFactory.VMO60359_VmoServerServiceException();
			case "DuplicateDisplayName":
				return VmoExceptionFactory.VMO60360_VmoServerServiceException();
			case "DuplicateAuthority":
				return VmoExceptionFactory.VMO60361_VmoServerServiceException();
			case "InvalidScopes":
				return new GenericIdpException(VmoResources.VMO68000, ex);
			}
		}
		return new GenericIdpException(VmoResources.VMO68000, ex);
	}

	private static Exception HandleUpdateException(IdpException ex)
	{
		if (ex is NotFoundException)
		{
			return VmoExceptionFactory.VMO60362_ObjectNotFoundException();
		}
		if (ex is InvalidArgumentsException ex2)
		{
			return new ArgumentException(VmoResources.VMO62001, ex2.Arguments.First());
		}
		if (ex is DefaultErrorException ex3)
		{
			switch (ex3.Errors.Keys?.FirstOrDefault())
			{
			case "DuplicateDisplayName":
				return VmoExceptionFactory.VMO60363_VmoServerServiceException();
			case "DuplicateAuthority":
				return VmoExceptionFactory.VMO60364_VmoServerServiceException();
			case "InvalidScopes":
				return new GenericIdpException(VmoResources.VMO68000, ex);
			}
		}
		return new GenericIdpException(VmoResources.VMO68000, ex);
	}

	private static Exception HandleDeleteException(IdpException ex)
	{
		if (ex is NotFoundException)
		{
			return VmoExceptionFactory.VMO60365_ObjectNotFoundException();
		}
		return new GenericIdpException(VmoResources.VMO68000, ex);
	}
}
