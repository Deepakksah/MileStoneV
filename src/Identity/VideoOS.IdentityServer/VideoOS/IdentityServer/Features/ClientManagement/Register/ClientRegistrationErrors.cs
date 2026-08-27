using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.ClientManagement.Register;

public class ClientRegistrationErrors
{
	public static Error ClientIdReserved()
	{
		return Error.Validation("ApiException", "Client id cannot be winauthclient.");
	}

	public static Error GrantTypesUnsupported()
	{
		return Error.Validation("ApiException", "A client can only have the following grant types authorization_code, client_credentials or refresh_token.");
	}

	public static Error RedirectUriRequired()
	{
		return Error.Validation("ApiException", "RedirectUris should be set if one of the grant types is authorization_code ");
	}

	public static Error RedirectUriNotAllowed()
	{
		return Error.Validation("ApiException", "RedirectUris should not be set when authorization_code is not used as a grant type");
	}

	public static Error RedirectUriMalformed(string uri)
	{
		return Error.Validation("ApiException", "Invalid client redirect uri: " + uri + ".");
	}

	public static Error PostLogoutUriMalformed(string uri)
	{
		return Error.Validation("ApiException", "Invalid post logout redirect uri: " + uri + ".");
	}

	public static Error ClientNameLengthValidation()
	{
		return Error.LegacyModelStateValidation("ClientName", "The field ClientName must be a string with a minimum length of 1 and a maximum length of 40.");
	}

	public static Error ClientNameRequired()
	{
		return Error.LegacyModelStateValidation("ClientName", "The ClientName field is required.");
	}

	public static Error ClientScopesRequired()
	{
		return Error.LegacyModelStateValidation("ClientScopes", "The ClientScopes field is required.");
	}

	public static Error ClientGrantTypesRequired()
	{
		return Error.LegacyModelStateValidation("ClientGrantTypes", "The ClientGrantTypes field is required.");
	}
}
