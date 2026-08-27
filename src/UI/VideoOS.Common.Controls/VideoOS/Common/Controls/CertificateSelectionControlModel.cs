using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using VideoOS.Common.Integration;

namespace VideoOS.Common.Controls;

public class CertificateSelectionControlModel : ICertificateSelectionControlModel
{
	public IEnumerable<CertificateData> GetCertificates(StoreName storeName, StoreLocation storeLocation)
	{
		using X509Store x509Store = new X509Store(storeName, storeLocation);
		x509Store.Open(OpenFlags.OpenExistingOnly);
		return (from X509Certificate2 i in x509Store.Certificates
			select new CertificateData(i)).ToArray();
	}
}
