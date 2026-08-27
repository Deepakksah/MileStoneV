using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration;

public static class CertificateSelector
{
	public static IEnumerable<CertificateData> FilterServerValidCertificates(this IEnumerable<CertificateData> certificates)
	{
		foreach (CertificateData certificate in certificates)
		{
			if (certificate.NotBefore.ToUniversalTime() <= DateTime.UtcNow && certificate.NotAfter.ToUniversalTime() > DateTime.UtcNow && certificate.HasPrivateKey && certificate.EnhancedKeyUsages.Contains(EnhancedKeyUsage.ServerAuthentication))
			{
				yield return certificate;
			}
		}
	}
}
