namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public interface IWrapperLogger
{
	string LogFilePath { get; }

	void LogDebug(string entry);

	void LogError(string entry);

	void LogInfo(string entry);

	void LogWarning(string entry);
}
