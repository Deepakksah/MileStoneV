namespace VideoOS.GatewayService.InstallerPlugin;

public interface IServerConfigurator
{
	int MaxProgress { get; }

	bool RebootRequiered { get; }

	event ProgressChangedEventHandler OnProgressChanged;

	void RunInstall();

	void RunUninstall();

	void RemoveCertificateRegistration();
}
