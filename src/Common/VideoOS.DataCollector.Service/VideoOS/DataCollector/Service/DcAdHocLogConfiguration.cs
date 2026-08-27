using System.IO;
using VideoOS.Common.Logging;
using VideoOS.DataCollector.Oem;

namespace VideoOS.DataCollector.Service;

public class DcAdHocLogConfiguration : IAdHocLogConfiguration
{
	private readonly IDataCollectorResources _resources;

	private const string LogFileName = "DataCollectorEngine.log";

	private const string LogBase = "Logs";

	public string LogBaseFolder => Path.Combine(_resources.CommonApplicationDataFolder, "Logs");

	public string FileLogName => Path.Combine(LogBaseFolder, "DataCollectorEngine.log");

	public string? AdditionalFileLogName => null;

	public bool LogToFileLog => true;

	public bool LogToConsoleLog => false;

	public bool LogToAdditionalFileLog => false;

	public string? AdditionalFileTargetLoggerName => null;

	public DcAdHocLogConfiguration(IDataCollectorResources resources)
	{
		_resources = resources;
	}
}
