using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using VideoOS.Common.Integration;

namespace VideoOS.Common.Controls;

public interface ICertificateSelectionControlModel
{
	IEnumerable<CertificateData> GetCertificates(StoreName storeName, StoreLocation storeLocation);
}
