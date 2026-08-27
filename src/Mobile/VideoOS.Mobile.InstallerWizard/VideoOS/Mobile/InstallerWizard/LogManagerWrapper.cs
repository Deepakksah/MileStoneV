using System.Diagnostics;
using VideoOS.Installer.Logging;

namespace VideoOS.Mobile.InstallerWizard;

public class LogManagerWrapper : ILogManager
{
	public void Write(string logMessage)
	{
		LogManager.Write(logMessage);
	}

	public void Write(string logMessage, TraceEventType severity)
	{
		LogManager.Write(logMessage, severity);
	}
}
