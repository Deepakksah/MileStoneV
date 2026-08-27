namespace VideoOS.IdentityServer.Configuration;

public class ProtectKeysWithCertificate
{
	public string Thumbprint { get; set; } = string.Empty;

	public bool CleanUpNonCertificateKeys { get; set; }
}
