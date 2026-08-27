using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.IdentityServer.Entities;

public class LoginProviderClient
{
	[Key]
	[Required]
	public string ClientId { get; set; }

	[Required]
	public Guid LoginProviderId { get; set; }
}
