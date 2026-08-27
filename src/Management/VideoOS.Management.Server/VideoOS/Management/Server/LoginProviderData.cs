using System.Collections.ObjectModel;

namespace VideoOS.Management.Server;

public class LoginProviderData
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string ClientId { get; set; }

	public string ClientSecret { get; set; }

	public string ClientSecretType { get; set; }

	public string CallbackPath { get; set; }

	public string Authority { get; set; }

	public Collection<string> Scopes { get; set; }

	public string UserNameClaimType { get; set; }

	public bool? PromptForLogin { get; set; }

	public bool? Enabled { get; set; }
}
