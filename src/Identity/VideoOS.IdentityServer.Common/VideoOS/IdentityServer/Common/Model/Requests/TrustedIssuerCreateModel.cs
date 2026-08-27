using System;

namespace VideoOS.IdentityServer.Common.Model.Requests;

public class TrustedIssuerCreateModel
{
	public string Issuer { get; set; }

	public Uri Address { get; set; }
}
