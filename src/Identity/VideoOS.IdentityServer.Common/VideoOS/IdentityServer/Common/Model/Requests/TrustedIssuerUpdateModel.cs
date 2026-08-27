using System;

namespace VideoOS.IdentityServer.Common.Model.Requests;

public class TrustedIssuerUpdateModel
{
	public string Issuer { get; set; }

	public Uri Address { get; set; }
}
