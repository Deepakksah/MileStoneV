using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace VideoOS.Utilities.Windows.Http;

public interface ISslConfigurationFactory
{
	ISslConfiguration GetDefaultConfigurationFromCertificate(IPAddress address, ushort port, Guid applicationId, StoreName certificateStore, string thumbprint);

	ISslConfiguration GetDefaultConfigurationFromCertificate(IPAddress address, ushort port, Guid applicationId, StoreName certificateStore, X509Certificate2 certificate);

	ISslConfiguration GetDefaultConfigurationFromCertificate(IPAddress address, ushort port, Guid applicationId, StoreName certificateStore, byte[] certificateHash);
}
