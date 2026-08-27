using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using VideoOS.Common.Integration.Internal;

namespace VideoOS.Common.Integration;

[DebuggerDisplay("CertificateData, Subject: {Subject}, FriendlyName: {FriendlyName}")]
public class CertificateData
{
	public DateTime NotBefore { get; }

	public DateTime NotAfter { get; }

	public string FriendlyName { get; }

	public string SimpleSubjectName { get; }

	public string Subject { get; }

	public string Thumbprint { get; }

	public byte[] CertificateHash { get; }

	public bool HasPrivateKey { get; }

	public Collection<EnhancedKeyUsage> EnhancedKeyUsages { get; }

	public X509Certificate2 Certificate { get; }

	public CertificateData(X509Certificate2 certificate)
		: this(certificate, certificate?.FriendlyName, certificate?.GetNameInfo(X509NameType.SimpleName, forIssuer: false), certificate?.Subject, certificate?.HasPrivateKey ?? false, certificate?.Thumbprint, certificate?.GetCertHash(), certificate?.NotBefore ?? DateTime.MinValue, certificate?.NotAfter ?? DateTime.MaxValue, certificate?.GetEnhancedKeyUsages())
	{
	}

	public CertificateData(X509Certificate2 certificate, string friendlyName, string simpleSubjectName, string subject, bool hasPrivateKey, string thumbprint, byte[] certificateHash, DateTime notBefore, DateTime notAfter, IEnumerable<EnhancedKeyUsage> enhancedKeyUsages)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException("certificate");
		}
		Certificate = certificate;
		FriendlyName = friendlyName;
		Subject = subject;
		HasPrivateKey = hasPrivateKey;
		SimpleSubjectName = simpleSubjectName;
		NotBefore = notBefore;
		NotAfter = notAfter;
		Thumbprint = thumbprint;
		CertificateHash = certificateHash;
		EnhancedKeyUsages = new Collection<EnhancedKeyUsage>(enhancedKeyUsages.ToList());
	}
}
