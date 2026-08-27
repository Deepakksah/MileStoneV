using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace VideoOS.Mobile.Helpers.Certificates;

public static class CertificateSelector
{
	public static IEnumerable<CertificateData> FilterServerValidCertificates(this IEnumerable<CertificateData> certificates)
	{
		certificates = certificates ?? throw new ArgumentNullException("certificates");
		foreach (CertificateData certificate in certificates)
		{
			if (certificate.NotBefore.ToUniversalTime() <= DateTime.UtcNow && certificate.NotAfter.ToUniversalTime() > DateTime.UtcNow && certificate.HasPrivateKey)
			{
				yield return certificate;
			}
		}
	}

	public static CertificateData FindBestCertificateMatch(this IEnumerable<CertificateData> certificates)
	{
		return certificates.OrderByDescending((CertificateData c) => c.NotBefore).FirstOrDefault();
	}

	public static X509Certificate2Collection ToX509Collection(this IEnumerable<CertificateData> certificateDatas)
	{
		return new X509Certificate2Collection(certificateDatas.Select((CertificateData certificateData) => certificateData.Certificate).ToArray());
	}

	public static X509Certificate2Collection MoveItemToLastPosition(this X509Certificate2Collection collection, string thumbprint)
	{
		if (collection == null || string.IsNullOrEmpty(thumbprint))
		{
			return collection;
		}
		X509Certificate2Collection x509Certificate2Collection = collection.Find(X509FindType.FindByThumbprint, thumbprint, validOnly: false);
		if (x509Certificate2Collection.Count < 1)
		{
			return collection;
		}
		collection.Remove(x509Certificate2Collection[0]);
		collection.Add(x509Certificate2Collection[0]);
		return collection;
	}
}
