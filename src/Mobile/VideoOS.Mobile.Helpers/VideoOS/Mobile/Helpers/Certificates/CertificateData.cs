using System;
using System.Security.Cryptography.X509Certificates;

namespace VideoOS.Mobile.Helpers.Certificates;

public class CertificateData
{
	public DateTime NotBefore { get; }

	public DateTime NotAfter { get; }

	public string FriendlyName { get; }

	public string SimpleSubjectName { get; }

	public string Subject { get; }

	public bool HasPrivateKey { get; }

	public X509Certificate2 Certificate { get; }

	public CertificateData(X509Certificate2 certificate)
		: this(certificate, certificate.FriendlyName, certificate.GetNameInfo(X509NameType.SimpleName, forIssuer: false), certificate.Subject, certificate.HasPrivateKey, certificate.NotBefore, certificate.NotAfter)
	{
	}

	public CertificateData(X509Certificate2 certificate, string friendlyName, string simpleSubjectName, string subject, bool hasPrivateKey, DateTime notBefore, DateTime notAfter)
	{
		FriendlyName = friendlyName;
		Subject = subject;
		HasPrivateKey = hasPrivateKey;
		SimpleSubjectName = simpleSubjectName;
		NotBefore = notBefore;
		NotAfter = notAfter;
		Certificate = certificate;
	}
}
