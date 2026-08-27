using System;
using System.ComponentModel.DataAnnotations;
using VideoOS.IdentityServer.Attributes;

namespace VideoOS.IdentityServer.Model.Requests;

public class TrustedIssuerForUpdateModel
{
	[Required]
	[HttpOrHttps]
	[MaxLengthUri(450)]
	public Uri Address { get; set; }

	[Required]
	[MaxLength(450)]
	public string Issuer { get; set; }
}
