using System;

namespace VideoOS.IdentityServer.Common.Model.Responses;

public class LoginProviderCreateModel
{
	public Guid Id { get; set; }

	public string ClientId { get; set; }

	public string ClientSecret { get; set; }

	public string ClientSecretType { get; set; }

	public string CallbackPath { get; set; }

	public string Authority { get; set; }

	public string DisplayName { get; set; }

	public string[] Scopes { get; set; }

	public string UserNameClaimType { get; set; }

	public bool PromptForLogin { get; set; }

	public bool Enabled { get; set; }
}
