using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace VideoOS.IdentityServer.Model;

public class ApiCodes
{
	public static List<ApiError> BlackListedCodes { get; } = new List<ApiError>
	{
		new ApiError(new IdentityErrorDescriber().PasswordMismatch())
	};

	public ApiError UserNotFound(string id)
	{
		return new ApiError
		{
			Code = "UserNotFound",
			Description = "Requested user with id '" + id + "' could not be found."
		};
	}

	public ApiError RoleNotFound(string id)
	{
		return new ApiError
		{
			Code = "RoleNotFound",
			Description = "Requested role with id '" + id + "' could not be found."
		};
	}

	public ApiError AdminIdentityNotFound(string identity)
	{
		return new ApiError
		{
			Code = "AdminIdentityNotFound",
			Description = "AdminIdentity '" + identity + "' could not be found."
		};
	}

	public ApiError LoginProviderNotFound(Guid providerId)
	{
		return new ApiError
		{
			Code = "LoginProviderNotFound",
			Description = $"LoginProvider '{providerId}' could not be found."
		};
	}

	public ApiError ClientNotFound(string clientId)
	{
		return new ApiError
		{
			Code = "ClientNotFound",
			Description = "Client '" + clientId + "' could not be found."
		};
	}

	public ApiError ClientNotScim(string clientId)
	{
		return new ApiError
		{
			Code = "ClientNotScim",
			Description = "Client '" + clientId + "' doesn't have the Scim scope."
		};
	}

	public ApiError InvalidScopes()
	{
		return new ApiError
		{
			Code = "InvalidScopes",
			Description = "The scopes format is invalid."
		};
	}

	public ApiError DuplicateAdminIdentity(string identity)
	{
		return new ApiError
		{
			Code = "DuplicateAdminIdentity",
			Description = "AdminIdentity '" + identity + "' already exists."
		};
	}

	public ApiError ThereCanBeOnlyOne()
	{
		return new ApiError
		{
			Code = "ThereCanBeOnlyOne",
			Description = "Only one login provider is supported."
		};
	}

	public ApiError DuplicateLoginProvider(Guid providerId)
	{
		return new ApiError
		{
			Code = "DuplicateLoginProvider",
			Description = $"LoginProvider '{providerId}' already exists."
		};
	}

	public ApiError DuplicateDisplayName(string displayName)
	{
		return new ApiError
		{
			Code = "DuplicateDisplayName",
			Description = "Display name '" + displayName + "' already exists."
		};
	}

	public ApiError DuplicateAuthority(string authority)
	{
		return new ApiError
		{
			Code = "DuplicateAuthority",
			Description = "Authority name '" + authority + "' already exists."
		};
	}

	public ApiError DuplicateClientProviderMapping(string clientId, Guid providerId)
	{
		return new ApiError
		{
			Code = "DuplicateClientProviderMapping",
			Description = $"Client '{clientId}' is already mapped to a loginprovider '{providerId}'."
		};
	}

	public ApiError NotAllowed(string msg)
	{
		return new ApiError
		{
			Code = "NotAllowed",
			Description = "Action not allowed: " + msg
		};
	}

	public ApiError DuplicateIssuer(string issuer)
	{
		return new ApiError
		{
			Code = "DuplicateIssuer",
			Description = "Issuer '" + issuer + "' already exists."
		};
	}

	public ApiError DuplicateAddress(string address)
	{
		return new ApiError
		{
			Code = "DuplicateAddress",
			Description = "Address '" + address + "' already exists."
		};
	}

	public ApiError TrustedIssuerNotFound(int id)
	{
		return new ApiError
		{
			Code = "TrustedIssuerNotFound",
			Description = $"Trusted issuer with id '{id}' not found."
		};
	}
}
