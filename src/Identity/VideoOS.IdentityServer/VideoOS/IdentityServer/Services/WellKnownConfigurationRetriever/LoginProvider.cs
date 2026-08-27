using System;
using System.Text.Json.Serialization;

namespace VideoOS.IdentityServer.Services.WellKnownConfigurationRetriever;

public record LoginProvider
{
	[JsonPropertyName("loginProviderId")]
	public Guid Id { get; set; }

	[JsonPropertyName("displayName")]
	public string Name { get; set; } = string.Empty;
}
