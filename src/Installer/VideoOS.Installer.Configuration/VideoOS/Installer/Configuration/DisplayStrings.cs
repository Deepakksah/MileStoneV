namespace VideoOS.Installer.Configuration;

public static class DisplayStrings
{
	public const string ErrorMessageBoxHeader = "Error";

	public const string InvalidChars = "The specified manufacturer information contains unsupported characters.";

	public const string EmptyInformationItem = "The specified manufacturer information should not have empty content fields.";

	public const string ErrorReadingManufacturerInfo = "There is an error reading the manufacturer information XML.";

	public const string InvalidXmlFile = "The manufacturer information XML is invalid.";

	public const string ErrorStoringRegistry = "There is an error storing manufacturer information in the registry.";

	public const string ErrorModeNotSupported = "This installer cannot run in 'Hardware Manufacturer Mode' or in 'Finalize Mode'";

	public const string ErrorReadingManufacturerInfoFraReg = "There is an error reading the manufacturer information from the registry.";

	public const string ErrorMissingHardwareManufacturerOption = "The hardware manufacturer mode option should be used with the manufacturer info path option.\nSee help for more information.";

	public const string ErrorPreviousInstallation = "A previous installation has been detected. Hardware manufacturer mode must be run on a fresh image.";

	public const string FinalizeShortcutName = "Setup of {0}.lnk";
}
