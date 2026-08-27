using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace VideoOS.Mobile.Helpers.Certificates;

public interface ICertificateSelectionControlModel
{
	IEnumerable<CertificateData> GetCertificates(StoreName storeName, StoreLocation storeLocation);
}
