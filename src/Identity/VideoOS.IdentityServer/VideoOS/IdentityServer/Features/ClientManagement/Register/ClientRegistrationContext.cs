using System.Collections.Generic;
using Duende.IdentityServer.EntityFramework.Entities;

namespace VideoOS.IdentityServer.Features.ClientManagement.Register;

public sealed class ClientRegistrationContext
{
	public RegisterClientCommand Command { get; }

	public Client Client { get; }

	public string? GeneratedSecret { get; set; }

	public OperationType Operation { get; }

	public ClientRegistrationContext(RegisterClientCommand command, Client? existingClient)
	{
		Command = command;
		Client = existingClient ?? CreateNewClient(command.ClientId);
		Operation = ((existingClient != null) ? OperationType.Updated : OperationType.Created);
	}

	private Client CreateNewClient(string clientId)
	{
		return new Client
		{
			Enabled = true,
			ClientId = clientId,
			AccessTokenType = 0,
			AlwaysIncludeUserClaimsInIdToken = true,
			IncludeJwtId = true,
			AllowedCorsOrigins = new List<ClientCorsOrigin>(),
			AllowedScopes = new List<ClientScope>(),
			AllowedGrantTypes = new List<ClientGrantType>(),
			Claims = new List<ClientClaim>(),
			ClientSecrets = new List<ClientSecret>(),
			IdentityProviderRestrictions = new List<ClientIdPRestriction>(),
			PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>(),
			Properties = new List<ClientProperty>(),
			RedirectUris = new List<ClientRedirectUri>()
		};
	}
}
