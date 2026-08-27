namespace VideoOS.IdentityServer.Services.Interfaces;

public record SubjectTokenClaims
{
	public string Issuer { get; set; }

	public string Idp { get; set; }
}
