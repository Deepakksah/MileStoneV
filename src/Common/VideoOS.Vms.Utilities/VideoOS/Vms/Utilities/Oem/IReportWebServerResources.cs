namespace VideoOS.Vms.Utilities.Oem;

public interface IReportWebServerResources
{
	string ApplicationName { get; }

	string LoggingDir { get; }

	string ProgramFilesDataFolder { get; }

	string ConfigFileName { get; }

	string ConfigFilePath { get; }

	string CommonApplicationDataFolder { get; }
}
