namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public interface IWrapperCleanup
{
	string InterfaceName { get; set; }

	string InitializeName { get; set; }

	string CleanupName { get; set; }

	string StatusCodeName { get; set; }

	string Initialize(IWrapperLogger logger);

	string CleanUp();
}
