using System;

namespace VideoOS.Utilities.Windows.Http;

public interface ISslConfiguration : ISslConfigurationKey
{
	Guid ApplicationId { get; }

	string CertificateStoreName { get; }

	ClientCertificateCheckMode DefaultCertCheckMode { get; }

	uint DefaultRevocationFreshnessTime { get; }

	uint DefaultRevocationUrlRetrievalTimeout { get; }

	string DefaultSslCtlIdentifier { get; }

	string DefaultSslCtlStoreName { get; }

	SslConfigOptions DefaultFlags { get; }

	byte[] GetCertificateHash();
}
