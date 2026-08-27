namespace VideoOS.Installer.Sequencer.Log;

public static class LogStrings
{
	public const string AddingPackageToBeInstalled = "Adding package to be installed '{0}' with package id '{1}'...";

	public const string AddingPrerequisiteToBeInstalled = "Adding prerequisite to be installed '{0}' with product code '{1}'...";

	public const string AddingPackageToBeUninstalled = "Adding package to be uninstalled '{0}' with package id '{1}'...";

	public const string CastTypeError = "Package type not supported.";

	public const string ErrorCopyFile = "Could not copy file {0}.";

	public const string ExecutedCommand = "Executed command {0} {1}";

	public const string FilesCopied = "Installation files are copied to {0}.";

	public const string GenerateArgsFailed = "Args.xml for {0} failed to generate. Error details: {1}";

	public const string GeneratedArgsSuccessfully = "Args.xml were generated for {0}";

	public const string LastError = "Last error: {0}";

	public const string LastWarning = "Last warning: {0}";

	public const string NeedToReboot = "Restart computer to complete installation.";

	public const string PrerequisiteCommandsAreEmpty = "Commands for {0} prerequisite are empty.";

	public const string PrerequisiteFileNotFound = "Prerequisite {0} not found.";

	public const string PrerequisiteInstalled = "Prerequisite {0} installed. Exit code: {1}";

	public const string PrerequisiteInstallFailed = "Installation of prerequisite {0} failed. Reason: {1}.";

	public const string ProductCode = "Product code: {0}.";

	public const string ProductRegistered = "{0} was registered. Exit code: {1}";

	public const string SkipPackage = "Package {0} {1} was not installed because a more recent version({2}) is already installed on the computer.";

	public const string Uninstalling = "Uninstalling package...";

	public const string UpgradeCode = "Upgrade code: {0}.";

	public const string Sequencer_FailedToInstallePackage = "Failed to install package ";

	public const string Sequencer_Uninstalling = "Uninstalling '{0} ({1})'...";

	public const string Sequencer_InstallingPrerequisitesStarted = "Installing of the prerequisites started.";

	public const string Sequenceser_DotNet35InstallFailed = "{0} is not installed and there is no Internet access to automatically install it.";

	public const string Sequencer_Installing = "Installing...";

	public const string SSRegistrationTask_MessageRegisteringProducts = "Registering products...";
}
