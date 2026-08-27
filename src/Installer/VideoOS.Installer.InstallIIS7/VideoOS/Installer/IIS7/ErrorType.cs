namespace VideoOS.Installer.IIS7;

public enum ErrorType
{
	HasNotStarted = -1,
	Success = 0,
	FailedToInstall = 2,
	NeedAddRole = 3,
	NeedReboot = 3010
}
