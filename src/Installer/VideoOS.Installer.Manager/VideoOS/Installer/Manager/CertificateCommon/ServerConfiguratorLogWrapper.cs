using System;
using System.Diagnostics;
using VideoOS.Installer.Logging;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.Installer.Manager.CertificateCommon;

public class ServerConfiguratorLogWrapper : MarshalByRefObject, ILogger
{
	public string LogFilePath => LogManager.GetLogFilename();

	public void LogDebug(string entry)
	{
		LogManager.Write(entry, TraceEventType.Verbose);
	}

	public void LogError(string entry)
	{
		LogManager.Write(entry, TraceEventType.Error);
	}

	public void LogInfo(string entry)
	{
		LogManager.Write(entry, TraceEventType.Information);
	}

	public void LogWarning(string entry)
	{
		LogManager.Write(entry, TraceEventType.Warning);
	}
}
