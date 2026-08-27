using System;
using System.Security.Cryptography.X509Certificates;

namespace VideoOS.ServerConfigurator.Interfaces;

public interface ICertificate
{
	string FriendlyName { get; }

	string IssuerName { get; }

	string Subject { get; }

	bool HasPrivateKey { get; }

	string SubjectName { get; }

	DateTime ValidFrom { get; }

	DateTime ValidUntil { get; }

	string Thumbprint { get; }

	bool CertificateAuthority { get; }

	bool ServerAuthentication { get; }

	bool ClientAuthentication { get; }

	X509Certificate2 X509Certificate { get; }
}
