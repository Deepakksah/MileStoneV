using System;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class LoggerWrapper : MarshalByRefObject, VideoOS.ServerConfigurator.CertificateRegistration.ILogger
{
	private VideoOS.ServerConfigurator.Interfaces.ILogger _log;

	public override object InitializeLifetimeService()
	{
		return null;
	}

	public LoggerWrapper(VideoOS.ServerConfigurator.Interfaces.ILogger log)
	{
		_log = log;
	}

	public void LogDebug(string entry)
	{
		_log.LogDebug(entry);
	}

	public void LogError(string entry)
	{
		_log.LogError(entry);
	}

	public void LogInfo(string entry)
	{
		_log.LogInfo(entry);
	}

	public void LogWarning(string entry)
	{
		_log.LogWarning(entry);
	}
}
