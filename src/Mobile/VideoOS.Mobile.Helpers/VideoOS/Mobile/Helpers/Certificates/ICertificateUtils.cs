using System.Security.Cryptography.X509Certificates;

namespace VideoOS.Mobile.Helpers.Certificates;

public interface ICertificateUtils
{
	void AddToStore(StoreName storeName, StoreLocation storeLocation, X509Certificate2 certificate);

	void RemoveFromStoreByConcreteSubjectName(StoreName storeName, StoreLocation storeLocation, string subjName);

	bool IsCeritificateValid(X509Certificate2 certificate);

	X509Certificate2Collection GetCertificateCollection(StoreName storeName, StoreLocation storeLocation);

	X509Certificate2 TryFindCertificateByPort(int port);

	X509Certificate2 TryFindCertificateByHash(StoreName storeName, StoreLocation storeLocation, string certHash);

	bool TryBindCertificateToPort(string ip, string port, string certHash, string appId, out string errorMsg);

	string GetSslCertificateHashBindedOnPort(string ip, string port);

	bool TryUnbindCertificateFromPort(string ip, string port, string appId, out string errorMsg);
}
