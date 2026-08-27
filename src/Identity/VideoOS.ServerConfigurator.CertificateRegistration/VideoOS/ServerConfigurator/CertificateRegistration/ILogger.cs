namespace VideoOS.ServerConfigurator.CertificateRegistration;

public interface ILogger
{
	void LogDebug(string entry);

	void LogError(string entry);

	void LogInfo(string entry);

	void LogWarning(string entry);
}
