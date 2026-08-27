using System;

namespace VideoOS.IdentityServer.Entities;

public class TrustedIssuer
{
	public int Id { get; set; }

	public Uri Address { get; set; }

	public string Issuer { get; set; }
}
