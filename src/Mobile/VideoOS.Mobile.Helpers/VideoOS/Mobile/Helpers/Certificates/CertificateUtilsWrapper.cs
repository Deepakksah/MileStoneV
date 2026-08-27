using System.Security.Cryptography.X509Certificates;

namespace VideoOS.Mobile.Helpers.Certificates;

public class CertificateUtilsWrapper : ICertificateUtils
{
	public void AddToStore(StoreName storeName, StoreLocation storeLocation, X509Certificate2 certificate)
	{
		Certificate.AddToStore(storeName, storeLocation, certificate);
	}

	public X509Certificate2Collection GetCertificateCollection(StoreName storeName, StoreLocation storeLocation)
	{
		return Certificate.GetCertificateCollection(storeName, storeLocation);
	}

	public string GetSslCertificateHashBindedOnPort(string ip, string port)
	{
		return Certificate.GetSslCertificateHashBindedOnPort(ip, port);
	}

	public bool IsCeritificateValid(X509Certificate2 certificate)
	{
		return Certificate.IsCeritificateValid(certificate);
	}

	public void RemoveFromStoreByConcreteSubjectName(StoreName storeName, StoreLocation storeLocation, string subjName)
	{
		Certificate.RemoveFromStoreByConcreteSubjectName(storeName, storeLocation, subjName);
	}

	public bool TryBindCertificateToPort(string ip, string port, string certHash, string appId, out string errorMsg)
	{
		return Certificate.TryBindCertificateToPort(ip, port, certHash, appId, out errorMsg);
	}

	public X509Certificate2 TryFindCertificateByHash(StoreName storeName, StoreLocation storeLocation, string certHash)
	{
		return Certificate.TryFindCertificateByHash(storeName, storeLocation, certHash);
	}

	public X509Certificate2 TryFindCertificateByPort(int port)
	{
		return Certificate.TryFindCertificateByPort(port);
	}

	public bool TryUnbindCertificateFromPort(string ip, string port, string appId, out string errorMsg)
	{
		return Certificate.TryUnbindCertificateFromPort(ip, port, appId, out errorMsg);
	}
}
