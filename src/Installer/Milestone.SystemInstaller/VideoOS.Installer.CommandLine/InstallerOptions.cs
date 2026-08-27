using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace VideoOS.Installer.CommandLine;

public class InstallerOptions
{
	[Option(null, "arguments", "filename", HelpText = "- Sets the argument file in quiet mode")]
	public string ArgumentFile;

	[Option(null, "language", "lang", HelpText = "- Sets the language for the installer and product. e.g. \"en-US\" ")]
	public string LanguageId;

	[Option(null, "partner_id", "id", HelpText = "- Sets the partner ID. Used mostly for the Download Manager")]
	public string PartnerId;

	[Option(null, "idm_id", "id", HelpText = "- Sets the ID for the Internet Download Manager")]
	public string IDMId;

	[Option(null, "quiet", null, HelpText = "- Installs the software in quiet mode")]
	public bool IsQuiet;

	[Option(null, "help", null, HelpText = "- Shows this message")]
	public bool IsHelp;

	[Option(null, "debugmode", null, HelpText = "- Runs installer in debugmode without installing anything, just UI")]
	public bool IsDebugMode;

	[Option(null, "msilog", null, HelpText = "- Enables extended msi logging")]
	public bool IsMSILog;

	[Option(null, "logpath", "filepath", HelpText = "- Sets the path to the log file")]
	public string LogFile;

	[Option(null, "acceptstatistics", "0/1", HelpText = "- Enables the Customer Experience Improvement Program")]
	public int Acceptstatistics;

	[Option(null, "generateargsfile", "path", HelpText = "- Generates a file with the default arguments in the folder")]
	public string ArgumentsPath;

	[Option(null, "showconsole", null, HelpText = "- Enables console output in quiet mode.")]
	public bool ShowConsole;

	[Option(null, "license", "licensefile", HelpText = "- Sets the path to the license file")]
	public string LicensePath;

	[Option(null, "licensetype", "type", HelpText = "- Sets the license type")]
	public string LicenseType;

	[Option(null, "ss_registration", null, HelpText = "- Registers this installer on the download page")]
	public bool SS_Registration;

	[Option(null, "modify", null, HelpText = null)]
	public bool IsModify;

	[Option(null, "uninstall", null, HelpText = null)]
	public bool IsUninstall;

	[Option(null, "mode", "name", HelpText = null)]
	public string Mode;

	[Option(null, "continue", null, HelpText = null)]
	public bool DoContinue;

	[Option(null, "extract", "path", HelpText = "- Extracts the installer files in the specified path or to the default directory if no path is specified")]
	public string ExtractPath;

	[Option(null, "hardwaremanufacturermode", null, HelpText = "- Runs the installer in hardware manufacturer mode for preparing Windows images")]
	public bool HardwareManufacturerMode;

	[Option(null, "manufacturerinfopath", "path", HelpText = "- Sets the path to the hardware manufacturer information XML file")]
	public string ManufacturerInfoPath;

	[Option(null, "finalizemode", null, HelpText = "- Runs the installer in finalize mode by end users")]
	public bool FinalizeMode;

	[Option(null, "extractMSIs", "path", HelpText = null)]
	public string ExtractMSIPath;

	[Option(null, "showlangpage", null, HelpText = null)]
	public bool ShowLangPage;

	[Option(null, "exeDir", null, HelpText = null)]
	public string ExecutableLocation;

	[Option(null, "donotpreparefinalize", null, HelpText = "- In quiet mode, do not initiate finalize mode when hardware manufacture mode ends")]
	public bool DoNotPrepareFinalize;

	[Option(null, "donotremoveinstalllog", null, HelpText = "- In quiet mode, do not delete installation log files when hardware manufacture mode ends")]
	public bool DoNotRemoveInstallLog;

	[Option(null, "skipMsFailoverInstall", null, HelpText = "- Skips the installation of MS Failover")]
	public bool SkipMsFailoverInstall;

	[Option(null, "MANAGEMENTSERVERURI", null, HelpText = null)]
	public string ManagementServerUri;

	[OptionList(null, "parameters", null, ':', HelpText = "- Sets list of parameters for silent mode separated by ':'. Use '[' and ']' to enclose values that contains ':'. To ex. [http://localhost]")]
	public IList<string> Parameters;

	[Option(null, "rebootOnPackageId", "packageId", HelpText = null)]
	public string RebootOnPackageId;

	[Option(null, "password", "value", HelpText = "- Sets the system configuration password")]
	public string SystemPassword;

	[Option(null, "en-US", null, HelpText = null)]
	public bool ignored;

	public void GetUsage()
	{
		IEnumerable<OptionAttribute> optionList = new CommandLineParser().GetOptionList(this);
		Console.WriteLine();
		Console.WriteLine("Usage: " + Path.GetFileName(Assembly.GetEntryAssembly().Location) + " [options]");
		Console.WriteLine();
		Console.WriteLine("Options:");
		foreach (OptionAttribute item in optionList)
		{
			if (item.HelpText != null)
			{
				string arg = ((item.ShortExample != null) ? ("--" + item.LongName + "=<" + item.ShortExample + ">") : ("--" + item.LongName));
				Console.WriteLine($"  {arg,-30} {item.HelpText}");
			}
		}
	}
}
