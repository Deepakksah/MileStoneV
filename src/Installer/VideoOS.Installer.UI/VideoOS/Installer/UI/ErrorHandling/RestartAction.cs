using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI.ErrorHandling;

public class RestartAction : ErrorAction
{
	public RestartAction()
		: base(Resources.ErrorMessage_RestartButton, "ShutDown", "/r /t 0")
	{
	}
}
