using System.Security.Cryptography.X509Certificates;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.Services;

public class CertificateStore : ICertificateStore
{
	public X509Certificate2 FindByThumbprint(string thumbprint)
	{
		using X509Store x509Store = new X509Store(StoreLocation.LocalMachine);
		x509Store.Open(OpenFlags.ReadOnly);
		X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, validOnly: true);
		return (x509Certificate2Collection.Count > 0) ? x509Certificate2Collection[0] : null;
	}
}
