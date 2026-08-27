using System.Diagnostics;

namespace VideoOS.Mobile.InstallerWizard;

public interface ILogManager
{
	void Write(string logMessage);

	void Write(string logMessage, TraceEventType severity);
}
