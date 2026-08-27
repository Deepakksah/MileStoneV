using System;

namespace VideoOS.IdentityServer.Common.Model.Responses;

public class TrustedIssuerResponseModel
{
	public int Id { get; set; }

	public string Issuer { get; set; }

	public Uri Address { get; set; }
}
