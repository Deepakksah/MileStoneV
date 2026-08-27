using System;
using System.Collections.Generic;
using System.Linq;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace VideoOS.IdentityServer.Features.ClientManagement.Register;

public sealed class ClientConfigurationService
{
	public ClientRegistrationContext ConfigureClient(RegisterClientCommand command, Duende.IdentityServer.EntityFramework.Entities.Client? existingClient)
	{
		ClientRegistrationContext clientRegistrationContext = new ClientRegistrationContext(command, existingClient);
		SetGrantTypes(clientRegistrationContext);
		SetScopes(clientRegistrationContext);
		SetClientName(clientRegistrationContext);
		SetRedirectUris(clientRegistrationContext);
		SetPostLogoutRedirectUris(clientRegistrationContext);
		if (SetClientSecretRequirement(clientRegistrationContext))
		{
			clientRegistrationContext.GeneratedSecret = AddClientSecret(clientRegistrationContext);
		}
		return clientRegistrationContext;
	}

	private void SetPostLogoutRedirectUris(ClientRegistrationContext context)
	{
		Duende.IdentityServer.EntityFramework.Entities.Client client = context.Client;
		if (client.PostLogoutRedirectUris == null)
		{
			List<ClientPostLogoutRedirectUri> list = (client.PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>());
		}
		if (context.Command.PostLogoutRedirectUris != null)
		{
			IEnumerable<ClientPostLogoutRedirectUri> collection = context.Command.PostLogoutRedirectUris.Select((string r) => new ClientPostLogoutRedirectUri
			{
				PostLogoutRedirectUri = r
			});
			context.Client.PostLogoutRedirectUris.Clear();
			context.Client.PostLogoutRedirectUris.AddRange(collection);
		}
	}

	private string AddClientSecret(ClientRegistrationContext context)
	{
		byte[] array = CryptoRandom.CreateRandomKey(64);
		string text = BitConverter.ToString(array).Replace("-", "", StringComparison.InvariantCulture);
		context.Client.ClientSecrets.Clear();
		context.Client.ClientSecrets.Add(new ClientSecret
		{
			Value = text.Sha256()
		});
		return text;
	}

	private bool SetClientSecretRequirement(ClientRegistrationContext context)
	{
		if (Enumerable.Contains(context.Command.ClientGrantTypes, "client_credentials"))
		{
			context.Client.RequireClientSecret = true;
			return true;
		}
		if (Enumerable.Contains(context.Command.ClientGrantTypes, "refresh_token") || Enumerable.Contains(context.Command.ClientGrantTypes, "authorization_code"))
		{
			context.Client.RequireClientSecret = context.Command.RequireSecretForAuthCodeFlow == true;
			return context.Client.RequireClientSecret;
		}
		return false;
	}

	private void SetRedirectUris(ClientRegistrationContext context)
	{
		Duende.IdentityServer.EntityFramework.Entities.Client client = context.Client;
		if (client.RedirectUris == null)
		{
			List<ClientRedirectUri> list = (client.RedirectUris = new List<ClientRedirectUri>());
		}
		if (context.Command.ClientRedirectUris != null)
		{
			IEnumerable<ClientRedirectUri> collection = context.Command.ClientRedirectUris.Select((string r) => new ClientRedirectUri
			{
				RedirectUri = r
			});
			context.Client.RedirectUris.Clear();
			context.Client.RedirectUris.AddRange(collection);
		}
	}

	private void SetClientName(ClientRegistrationContext context)
	{
		context.Client.ClientName = context.Command.ClientName;
	}

	private void SetGrantTypes(ClientRegistrationContext context)
	{
		Duende.IdentityServer.EntityFramework.Entities.Client client = context.Client;
		if (client.AllowedGrantTypes == null)
		{
			List<ClientGrantType> list = (client.AllowedGrantTypes = new List<ClientGrantType>());
		}
		IEnumerable<ClientGrantType> collection = context.Command.ClientGrantTypes.Select((string gt) => new ClientGrantType
		{
			GrantType = gt
		});
		context.Client.AllowedGrantTypes.Clear();
		context.Client.AllowedGrantTypes.AddRange(collection);
		context.Client.AllowOfflineAccess = true;
		if (Enumerable.Contains(context.Command.ClientGrantTypes, "refresh_token"))
		{
			context.Client.AllowOfflineAccess = true;
			context.Client.RefreshTokenUsage = 0;
			context.Client.UpdateAccessTokenClaimsOnRefresh = true;
			context.Client.RefreshTokenExpiration = 0;
			context.Client.AbsoluteRefreshTokenLifetime = 0;
		}
	}

	private void SetScopes(ClientRegistrationContext context)
	{
		Duende.IdentityServer.EntityFramework.Entities.Client client = context.Client;
		if (client.AllowedScopes == null)
		{
			List<ClientScope> list = (client.AllowedScopes = new List<ClientScope>());
		}
		IEnumerable<ClientScope> collection = context.Command.ClientScopes.Select((string s) => new ClientScope
		{
			Scope = s
		});
		context.Client.AllowedScopes.Clear();
		context.Client.AllowedScopes.AddRange(collection);
	}
}
