namespace VideoOS.Installer.Common.Dism;

public enum PackageFeatureState
{
	NotPresent,
	UninstallPending,
	Staged,
	Removed,
	Installed,
	InstallPending,
	Superseded,
	PartiallyInstalled
}
