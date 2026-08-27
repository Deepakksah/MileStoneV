namespace VideoOS.Server.Admin.Utilities;

public enum SslStatusStep
{
	CreateSelfSignedCertificate,
	CreateSslCertificateConfiguration,
	DeleteSslCertificateConfiguration,
	CreateSslBinding,
	DeleteSslBinding
}
