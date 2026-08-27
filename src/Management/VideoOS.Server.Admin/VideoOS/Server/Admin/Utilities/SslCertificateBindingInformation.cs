namespace VideoOS.Server.Admin.Utilities;

public class SslCertificateBindingInformation
{
	public bool HasCertificateBinding { get; set; }

	public bool CertificateExistsInStore { get; set; }

	public string FoundHttpsPortString { get; set; }

	public string CertificateHash { get; set; }

	public string CertificateName { get; set; }

	public SslCertificateBindingInformation()
	{
		HasCertificateBinding = false;
		CertificateExistsInStore = false;
	}
}
