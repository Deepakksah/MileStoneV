namespace VideoOS.Installer.Common;

public enum ExitCode
{
	Success = 0,
	SuccessDoCleanup = 50,
	Error = 1,
	CloseFromErrorPage = 60,
	ConfigPageCancel = 80,
	CommandSyntaxErrorOrCancel = 100,
	SuccessRebootRequiredAllPackagesInstalled = 30,
	SuccessRebootRequired = 3010
}
