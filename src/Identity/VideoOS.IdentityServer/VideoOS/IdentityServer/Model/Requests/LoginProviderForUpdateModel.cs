using System.ComponentModel.DataAnnotations;

namespace VideoOS.IdentityServer.Model.Requests;

public class LoginProviderForUpdateModel
{
	[MaxLength(100)]
	[Required]
	public string ClientId { get; set; }

	[MaxLength(450)]
	[Required]
	public string ClientSecret { get; set; }

	[MaxLength(250)]
	[AllowedValues(new object[] { "SharedSecret", "X509Thumbprint" }, ErrorMessage = "Invalid client secret type. Supported types are 'SharedSecret and 'X509Thumbprint'")]
	public string ClientSecretType { get; set; }

	[MaxLength(450)]
	[Required]
	public string CallBackPath { get; set; }

	[MaxLength(450)]
	[Required]
	public string Authority { get; set; }

	[MaxLength(100)]
	[Required]
	public string DisplayName { get; set; }

	public string[] Scopes { get; set; }

	[MaxLength(100)]
	[Required(AllowEmptyStrings = true)]
	public string UserNameClaimType { get; set; }

	public bool PromptForLogin { get; set; }

	public bool Enabled { get; set; }

	public string[] ScimClientIds { get; set; }

	[MaxLength(100)]
	public string MappingClaimType { get; set; }

	public bool EnableInactiveUserDeletion { get; set; }

	public int InactiveUserDeletionAfterDays { get; set; }
}
