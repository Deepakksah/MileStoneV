using System;
using System.Net;

namespace VideoOS.Utilities.Windows.Http.Internal;

public class SslConfiguration : SslConfigurationKey
{
	public Guid AppId { get; }

	public string CertStoreName { get; }

	public uint RevocationFreshnessTime { get; }

	public uint RevocationUrlRetrievalTimeout { get; }

	public string SslCtlIdentifier { get; }

	public string SslCtlStoreName { get; }

	public byte[] CertificateHash { get; }

	public ClientCertCheckMode CertCheckMode { get; }

	public SslConfigFlag Flags { get; }

	public SslConfiguration(ushort port, IPAddress address, Guid appId, string certStoreName, byte[] certificateHash, uint revocationFreshnessTime, uint revocationUrlRetrievalTimeout, string sslCtlIdentifier, string sslCtlStoreName, ClientCertCheckMode certCheckMode, SslConfigFlag flags)
		: base(address, port)
	{
		AppId = appId;
		CertStoreName = certStoreName;
		CertificateHash = certificateHash;
		RevocationFreshnessTime = revocationFreshnessTime;
		RevocationUrlRetrievalTimeout = revocationUrlRetrievalTimeout;
		SslCtlIdentifier = sslCtlIdentifier;
		SslCtlStoreName = sslCtlStoreName;
		CertCheckMode = certCheckMode;
		Flags = flags;
	}
}
