namespace VideoOS.ServerConfigurator.CertificateRegistration;

public interface IApplyCertificate : IServerCleanup
{
	Status PreApplyCertificate();

	Status ApplyCertificate(string thumbprint);

	Status PostApplyCertificate();
}
