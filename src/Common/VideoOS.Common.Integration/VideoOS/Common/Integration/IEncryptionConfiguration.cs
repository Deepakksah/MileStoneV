namespace VideoOS.Common.Integration;

public interface IEncryptionConfiguration
{
	bool Enabled { get; }

	string CertificateHash { get; }
}
