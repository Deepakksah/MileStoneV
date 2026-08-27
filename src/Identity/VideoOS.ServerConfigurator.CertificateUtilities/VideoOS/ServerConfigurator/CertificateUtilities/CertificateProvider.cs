using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateUtilities;

public class CertificateProvider : ICertificateProvider
{
	public ICertificate GetCertificate(string thumbprint)
	{
		using X509Store x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
		x509Store.Open(OpenFlags.ReadOnly);
		return (from X509Certificate2 x in x509Store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, validOnly: false)?
			select new Certificate(x)).FirstOrDefault();
	}

	public IEnumerable<ICertificate> GetValidCertificates(IEnumerable<Func<ICertificate, bool>> validatorFunctions)
	{
		return GetValidCertificates(StoreName.My, StoreLocation.LocalMachine, validatorFunctions);
	}

	public IEnumerable<ICertificate> GetValidCertificates(StoreName storeName, StoreLocation storeLocation, IEnumerable<Func<ICertificate, bool>> validatorFunctions)
	{
		if (validatorFunctions == null)
		{
			validatorFunctions = new List<Func<ICertificate, bool>>();
		}
		using X509Store x509Store = new X509Store(storeName, storeLocation);
		x509Store.Open(OpenFlags.OpenExistingOnly);
		IEnumerable<ICertificate> enumerable = from X509Certificate2 x in x509Store.Certificates
			select new Certificate(x);
		foreach (Func<ICertificate, bool> validatorFunction in validatorFunctions)
		{
			enumerable = enumerable.Where(validatorFunction);
		}
		return enumerable;
	}
}
