namespace VideoOS.Server.Admin.Utilities;

public interface IIisInternal
{
	void Iis7DeleteSslCertificateBindings(int httpsPort);

	void DeleteSecureBindings(string websiteId);

	void SetSecureBindings(string websiteId, int httpsPort);

	HttpsBindingInformation CheckSecureBindingsAndGetHttpsPort(string websiteId);

	void IisControlledReset();

	SslCertificateBindingInformation GetCertificateBinding(int httpsPort);

	void AddSslCertificate(int httpsPort, string certificateHash);

	string CreateSelfSignedCertificateIfNeeded();

	string GetPhysicalPath(string websiteId, string path);

	HttpBindingInformation GetHttpPort(string websiteId);

	AppPoolState RecycleAppPool(string appPoolName);

	AppPoolState GetAppPoolState(string appPoolName);

	void Control(IisAction action);
}
