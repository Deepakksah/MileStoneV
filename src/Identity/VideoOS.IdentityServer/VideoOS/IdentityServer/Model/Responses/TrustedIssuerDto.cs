using System;

namespace VideoOS.IdentityServer.Model.Responses;

public class TrustedIssuerDto
{
	public int Id { get; set; }

	public Uri Address { get; set; }

	public string Issuer { get; set; }
}
