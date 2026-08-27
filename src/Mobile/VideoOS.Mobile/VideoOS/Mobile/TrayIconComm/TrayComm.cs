using System;

namespace VideoOS.Mobile.TrayIconComm;

public static class TrayComm
{
	public enum PasswordChangeReturnCode
	{
		Success,
		IncorrectPassword,
		InternalError
	}

	public enum CommonReturnCode
	{
		Success,
		FeatureNotEnabled,
		ServiceNotInitialized,
		NoConnection,
		InternalError,
		TemporaryUnavailable,
		InvalidPath
	}

	public enum PortChangeReturnCode
	{
		Success,
		InternalError
	}

	public enum MobileServerState
	{
		Connected = -1,
		Initializing,
		InitializedWithWarningsRegister,
		InitializedWithWarningsCommunication,
		InitializedWithWarnings,
		FailedToConnect,
		FailedNoGoForOem
	}

	public static readonly string WEB_CLIENT_URL = "{0}://{1}:{2:d}/index.html";

	public static readonly string WEB_CLIENT_SHORTCUT_NAME = "XProtect Web client.url";

	public static readonly string START_MENU_COMPANY_FOLDER = "Programs\\Milestone";

	public static readonly string PSK = Guid.NewGuid().ToString().Replace("-", "a");
}
