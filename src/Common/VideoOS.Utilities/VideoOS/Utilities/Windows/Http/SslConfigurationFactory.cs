using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using VideoOS.Utilities.Windows.Http.Internal;

namespace VideoOS.Utilities.Windows.Http;

public sealed class SslConfigurationFactory : ISslConfigurationFactory
{
	public static ISslConfigurationFactory Instance { get; } = new SslConfigurationFactory();

	private SslConfigurationFactory()
	{
	}

	ISslConfiguration ISslConfigurationFactory.GetDefaultConfigurationFromCertificate(IPAddress address, ushort port, Guid applicationId, StoreName certificateStore, string thumbprint)
	{
		if (string.IsNullOrEmpty(thumbprint))
		{
			throw new ArgumentException("Thumbprint value must be specified.");
		}
		X509Certificate2 x509Certificate = null;
		X509Store x509Store = new X509Store(certificateStore, StoreLocation.LocalMachine);
		try
		{
			x509Store.Open(OpenFlags.ReadOnly);
			x509Certificate = x509Store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, validOnly: false).Cast<X509Certificate2>().FirstOrDefault();
		}
		finally
		{
			x509Store.Close();
		}
		if (x509Certificate != null)
		{
			return GetDefaultConfigurationFromCertificate(address, port, applicationId, certificateStore, x509Certificate.GetCertHash());
		}
		return null;
	}

	ISslConfiguration ISslConfigurationFactory.GetDefaultConfigurationFromCertificate(IPAddress address, ushort port, Guid applicationId, StoreName certificateStore, X509Certificate2 certificate)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException("certificate");
		}
		return GetDefaultConfigurationFromCertificate(address, port, applicationId, certificateStore, certificate.GetCertHash());
	}

	ISslConfiguration ISslConfigurationFactory.GetDefaultConfigurationFromCertificate(IPAddress address, ushort port, Guid applicationId, StoreName certificateStore, byte[] certificateHash)
	{
		return GetDefaultConfigurationFromCertificate(address, port, applicationId, certificateStore, certificateHash);
	}

	private static ISslConfiguration GetDefaultConfigurationFromCertificate(IPAddress address, ushort port, Guid applicationId, StoreName certificateStore, byte[] certificateHash)
	{
		if (address == null)
		{
			throw new ArgumentNullException("address");
		}
		if (certificateHash == null)
		{
			throw new ArgumentNullException("certificateHash");
		}
		if (port == 0)
		{
			throw new ArgumentOutOfRangeException("port", "Port value cannot be 0");
		}
		return new SslConfigurationWrapper(new SslConfiguration(port, address, applicationId, ConvertCertStoreName(certificateStore), certificateHash, 0u, 0u, null, null, ClientCertCheckMode.UseRevocationFreshnessTime, SslConfigFlag.None));
	}

	private static string ConvertCertStoreName(StoreName storeName)
	{
		using X509Store x509Store = new X509Store(storeName);
		return x509Store.Name;
	}
}
