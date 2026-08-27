using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using VideoOS.Utilities.Formatting;

namespace VideoOS.Common.Integration;

public static class CertificateLoader
{
	public static X509Certificate2 LoadCertificateByHashString(string certificateHash)
	{
		X509Certificate2 x509Certificate = null;
		try
		{
			if (certificateHash == null)
			{
				throw new ArgumentNullException("certificateHash");
			}
			return LoadCertificateByHash(certificateHash.ParseAsBytes());
		}
		catch (ArgumentException)
		{
			return null;
		}
	}

	public static X509Certificate2 LoadCertificateByHash(byte[] certificateHash)
	{
		if (certificateHash == null)
		{
			throw new ArgumentNullException("certificateHash");
		}
		using X509Store x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
		x509Store.Open(OpenFlags.ReadOnly);
		return x509Store.Certificates.Cast<X509Certificate2>().FirstOrDefault((X509Certificate2 c) => c.GetCertHash().SequenceEqual(certificateHash));
	}
}
