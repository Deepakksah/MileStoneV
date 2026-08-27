namespace VideoOS.Server.CertificateRegistration;

public enum IisProgressStep
{
	Unknown,
	CreateSelfSignedCertificate,
	CreateSslCertificateConfiguration,
	DeleteSslCertificateConfiguration,
	CreateSslBinding,
	DeleteSslBinding,
	DisableSsl
}
