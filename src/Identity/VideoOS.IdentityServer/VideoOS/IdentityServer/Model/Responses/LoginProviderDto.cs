using System;

namespace VideoOS.IdentityServer.Model.Responses;

public class LoginProviderDto
{
	public Guid Id { get; set; }

	public string ClientId { get; set; }

	public string ClientSecret { get; set; }

	public string ClientSecretType { get; set; }

	public string CallBackPath { get; set; }

	public string Authority { get; set; }

	public string DisplayName { get; set; }

	public string[] Scopes { get; set; }

	public string UserNameClaimType { get; set; }

	public bool PromptForLogin { get; set; }

	public bool Enabled { get; set; }

	public string[] ScimClientIds { get; set; }

	public string MappingClaimType { get; set; }

	public bool EnableInactiveUserDeletion { get; set; }

	public int InactiveUserDeletionAfterDays { get; set; }
}
