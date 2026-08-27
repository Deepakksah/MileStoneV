using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.IdentityServer.Entities;

public class LoginProvider
{
	[Key]
	[Required]
	public Guid Id { get; set; }

	[MaxLength(100)]
	[Required]
	public string ClientId { get; set; }

	[Required]
	public string ClientSecret { get; set; }

	[MaxLength(250)]
	[Required]
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

	public string Scopes { get; set; }

	[MaxLength(100)]
	[Required]
	public string UserNameClaimType { get; set; }

	public bool PromptForLogin { get; set; }

	public bool Enabled { get; set; }

	[MaxLength(100)]
	[Required]
	public string MappingClaimType { get; set; }

	public virtual ICollection<LoginProviderClient> ScimClients { get; set; }

	public bool EnableInactiveUserDeletion { get; set; }

	public int InactiveUserDeletionAfterDays { get; set; }
}
