using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateUtilities;

public interface ICertificateProvider
{
	ICertificate GetCertificate(string thumbprint);

	IEnumerable<ICertificate> GetValidCertificates(IEnumerable<Func<ICertificate, bool>> validatorFunctions);

	IEnumerable<ICertificate> GetValidCertificates(StoreName storeName, StoreLocation storeLocation, IEnumerable<Func<ICertificate, bool>> validatorFunctions);
}
