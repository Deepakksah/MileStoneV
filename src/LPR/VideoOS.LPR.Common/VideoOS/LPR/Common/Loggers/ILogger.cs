namespace VideoOS.LPR.Common.Loggers;

public interface ILogger
{
	void LogAddLicensePlate(string listName, string licensePlate);

	void LogDeleteLicensePlate(string listName, string licensePlate);

	void LogClearLicensePlates(string listName);

	void LogSearchLicensePlateList(string listName, string licensePlate);

	void LogSearchLicensePlateEvents(string filter);

	void Initialize();
}
