namespace VideoOS.Installer.Manager.Log;

public static class LogStrings
{
	public const string DefaultParameters = "Default target directory {0} is used for {1} package.";

	public const string EntryRegistryDeleted = "Entry {0} deleted from registry.";

	public const string ErrorAddingRunOnceKEy = "Error: Could not add value to the registry.";

	public const string ErrorGeneratingArgumentFile = "Could not generate file {0}.";

	public const string ErrorCopyFile = "Could not copy file {0}.";

	public const string ErrorNotEnoughSpace = "Not enough free space on the drive. {0} needed.";

	public const string ArgumentFileCreated = "File {0} has been created in {1}.";

	public const string FileNotFound = "File not found {0}.";

	public const string InstallerInfoSaved = "Installer info was saved {0}.\t";

	public const string InstallerStatus = "{0} is already installed. Changing to uninstall mode...";

	public const string LicenseLoaded = "License loaded.";

	public const string LoadingXml = "Loading XML file...";

	public const string LoadingFromReg = "Loading from registry...";

	public const string PackagesFailedToInstall = "Packages were not installed. Error message: {0} :";

	public const string Parameters = "Parameters";

	public const string Platform = "Platform: {0}";

	public const string PluginLoaded = "Plug-in {0} with type {1} was loaded.";

	public const string SubscriptionLoaded = "Subscription {0} with type {1} was loaded.";

	public const string SubscriptionBypassPackageWillNotBeInstalled = "Bypassing subscription with Id: {0}. Reason: associated PackageId: {1} will not be installed.";

	public const string SubscriptionBypassPackageWillBeInstalled = "Bypassing subscription with Id: {0}. Reason: associated PackageId: {1} will be installed.";

	public const string PrerequisitesNotInstalled = "Prerequisites need to be installed by user: {0}.";

	public const string RegistryDeleteError = "An attempt to delete an entry from the registry {0} failed. Details: {1}.";

	public const string SecurityIssue = "Security error detected {0}.";

	public const string XmlWriteFailed = "Failed to write to XML file. Details: {0}";

	public const string LicenseCheck_NotFound = "The selected license is not found.";

	public const string LicenseCheck_Expired = "The selected license is expired.";

	public const string LicenseCheck_Invalid = "The selected license is invalid.";

	public const string LicenseCheck_ProductInvalidLicense = "The specified license is not valid for this product.";

	public const string CouldNotLoadInstallerConfig = "Could not generate argument xml file, error while loading installer config";

	public const string ManagmentServerUri_Invalid = "Management server uri was locked with value \"{0}\", that is different than one specified in arguments \"{1}\"";
}
