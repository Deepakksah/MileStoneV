using System.Security.Cryptography.X509Certificates;

namespace VideoOS.IdentityServer.Services.Interfaces;

public interface ICertificateStore
{
	X509Certificate2 FindByThumbprint(string thumbprint);
}
