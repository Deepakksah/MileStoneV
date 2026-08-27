namespace VideoOS.ServerConfigurator.RegistrationContainer;

public interface IApplyCertificate : IServerCleanup, IRegistrationContainer
{
	StatusContainer PreApplyCertificate();

	StatusContainer ApplyCertificate(string thumbprint);

	StatusContainer PostApplyCertificate();
}
