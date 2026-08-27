namespace VideoOS.Common.Integration;

public class EncryptionConfiguration : IEncryptionConfiguration
{
	public bool Enabled { get; private set; }

	public string CertificateHash { get; private set; }

	public EncryptionConfiguration(bool enabled, string certificateHash)
	{
		Enabled = enabled;
		CertificateHash = certificateHash;
	}
}
