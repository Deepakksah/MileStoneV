using VideoOS.Installer.Logging;

namespace VideoOS.Installer.RecorderPlugins;

public class LogManagerWrapper : ILogManager
{
	public void Write(string logMessage)
	{
		LogManager.Write(logMessage);
	}
}
