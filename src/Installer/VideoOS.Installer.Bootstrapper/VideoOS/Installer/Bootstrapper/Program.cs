using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using VideoOS.Installer.Bootstrapper.Properties;
using VideoOS.Installer.CommandLine;
using VideoOS.Installer.Common;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.Sequencer.ErrorHandling;
using VideoOS.Installer.UI;

namespace VideoOS.Installer.Bootstrapper;

public static class Program
{
	[Flags]
	public enum ErrorModes : uint
	{
		SYSTEM_DEFAULT = 0u,
		SEM_FAILCRITICALERRORS = 1u,
		SEM_NOALIGNMENTFAULTEXCEPT = 4u,
		SEM_NOGPFAULTERRORBOX = 2u,
		SEM_NOOPENFILEERRORBOX = 0x8000u
	}

	private static BindingRedirector _bindRedirector = new BindingRedirector();

	[DllImport("kernel32.dll")]
	private static extern bool AttachConsole(int dwProcessId);

	[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
	private static extern bool FreeConsole();

	[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
	private static extern bool AllocConsole();

	[DllImport("kernel32.dll")]
	private static extern ErrorModes SetErrorMode(ErrorModes uMode);

	[STAThread]
	private static void Main(string[] args)
	{
		bool flag = false;
		try
		{
			InstallerOptions installerOptions = new InstallerOptions();
			ICommandLineParser commandLineParser = new CommandLineParser();
			for (int i = 0; i < args.Length; i++)
			{
				args[i] = Regex.Replace(args[i], "^([^-].*)", "--${1}");
				args[i] = args[i].Replace("\"", "");
			}
			bool flag2 = commandLineParser.ParseArguments(args, installerOptions);
			if (string.IsNullOrEmpty(installerOptions.LanguageId) && !installerOptions.IsQuiet)
			{
				installerOptions.ShowLangPage = true;
				installerOptions.LanguageId = CultureInfo.CurrentUICulture.Name;
			}
			Process process = null;
			GlobalData.Settings = new List<IPackageParameter>();
			Process process2 = Parent(Process.GetCurrentProcess());
			string text = string.Empty;
			if (process2 != null)
			{
				text = process2.ProcessName;
			}
			if (installerOptions.IsQuiet && installerOptions.ShowConsole)
			{
				if (process2 != null)
				{
					process = Parent(process2);
					if (process != null && process.ProcessName == "cmd")
					{
						flag = AttachConsole(process.Id);
					}
					else
					{
						FreeConsole();
						flag = AllocConsole();
					}
				}
			}
			else
			{
				VideoOS.Installer.Logging.ConsoleTraceListener.DisableConsoleLogging = true;
			}
			if (installerOptions.IsUninstall)
			{
				GlobalData.Settings.Add(new PackageParameter("Installation Context", "uninstall"));
			}
			else if (installerOptions.IsModify)
			{
				GlobalData.Settings.Add(new PackageParameter("Installation Context", "modify"));
			}
			else
			{
				GlobalData.Settings.Add(new PackageParameter("Installation Context", "install"));
			}
			if (GlobalData.OemHelper.CompanyName == null)
			{
				Environment.ExitCode = 1;
				return;
			}
			flag2 &= SetLogFileSettings(installerOptions);
			LogManager.Init();
			_bindRedirector.SetLogger(LogManager.InnerLog);
			for (int j = 0; j < args.Length; j++)
			{
				LogManager.Write(Regex.Replace(args[j], "(.*?password.*?=).*", "$1*", RegexOptions.IgnoreCase));
			}
			if (installerOptions.ArgumentsPath != null)
			{
				if (GenerateArgumentsFile(installerOptions))
				{
					Environment.ExitCode = 50;
					return;
				}
				flag2 = false;
			}
			if (!string.IsNullOrEmpty(text))
			{
				LogManager.Write("Parent Process Name: " + text);
			}
			SetModeSettings(installerOptions);
			if (Environment.ExitCode != 0)
			{
				LogManager.Write($"Exit code: {Environment.ExitCode}");
				return;
			}
			if (installerOptions.IsHelp || !flag2)
			{
				installerOptions.GetUsage();
				if (flag2)
				{
					Environment.ExitCode = 50;
				}
				else
				{
					Environment.ExitCode = 100;
				}
				return;
			}
			if ((!installerOptions.DoContinue && installerOptions.HardwareManufacturerMode && string.IsNullOrEmpty(installerOptions.ManufacturerInfoPath)) || (!installerOptions.DoContinue && !installerOptions.HardwareManufacturerMode && !string.IsNullOrEmpty(installerOptions.ManufacturerInfoPath)))
			{
				MessageBox.Show("The hardware manufacturer mode option should be used with the manufacturer info path option.\nSee help for more information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
				Environment.ExitCode = 1;
				return;
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			if (InstallerManager.Instance.PrepareInstallation(installerOptions.IsModify, installerOptions.IsUninstall, installerOptions.LanguageId, installerOptions.Acceptstatistics, installerOptions.PartnerId, installerOptions.IsMSILog, installerOptions.IsDebugMode, installerOptions.IDMId, installerOptions.DoContinue, installerOptions.ShowLangPage, installerOptions.ExecutableLocation, installerOptions.ManufacturerInfoPath, text))
			{
				if (installerOptions.ExtractMSIPath != null)
				{
					if (!ExtractMSIFiles(installerOptions.ExtractMSIPath))
					{
						Environment.ExitCode = 1;
					}
				}
				else if (installerOptions.IsQuiet)
				{
					if (!InstallerManager.Instance.AnotherInstanceRunning())
					{
						if (installerOptions.HardwareManufacturerMode)
						{
							if (installerOptions.DoNotPrepareFinalize)
							{
								GlobalData.SetValue("PREPAREFINALIZE", bool.FalseString, "Main()");
							}
							if (installerOptions.DoNotRemoveInstallLog)
							{
								GlobalData.SetValue("REMOVEINSTALLLOG", bool.FalseString, "Main()");
							}
						}
						Environment.ExitCode = (int)(installerOptions.IsUninstall ? InstallerManager.Instance.RunUnInstallationInQuietMode() : InstallerManager.Instance.RunInstallationInQuietMode(installerOptions.ArgumentFile, installerOptions.LicensePath, installerOptions.LicenseType, installerOptions.DoContinue, CreateArgsFromCmd(installerOptions)));
					}
					else
					{
						LogManager.Write("Another installation is already running. Wait until it is finished and run this installer again.");
						Environment.ExitCode = 1;
					}
				}
				else
				{
					Application.Run(new InstallerForm(installerOptions.Acceptstatistics, installerOptions.SS_Registration, CreateArgsFromCmd(installerOptions)));
				}
			}
			else
			{
				Environment.ExitCode = 1;
				LogManager.Write("Preparation phase failed.");
			}
		}
		catch (NotImplementedException)
		{
			MessageHelper.ShowInstallerMessageBox(Resources.MessageBox_NotImplementedCaption, Resources.MessageBox_NotImplementedText);
		}
		catch (ObjectDisposedException exception)
		{
			LogManager.Write(exception);
			Environment.ExitCode = 1;
		}
		catch (SecuritySchemeChangeDisallowedException ex2)
		{
			LogManager.Write(ex2.Message, TraceEventType.Error);
			Environment.ExitCode = 1;
		}
		catch (Exception ex3)
		{
			try
			{
				LogManager.Write(ex3);
			}
			catch
			{
				MessageHelper.ShowInstallerMessageBox("", ex3.Message, InstallerMessageBoxIcon.Error, InstallerMessageBoxButtons.OK);
			}
			Environment.ExitCode = 1;
		}
		SetErrorMode(ErrorModes.SEM_NOGPFAULTERRORBOX);
		LogManager.Write($"Exit code: {Environment.ExitCode.ToString()}");
		if (flag)
		{
			FreeConsole();
		}
	}

	private static Dictionary<string, string> CreateArgsFromCmd(InstallerOptions options)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrEmpty(options.ManagementServerUri) && !string.IsNullOrEmpty(options.ManagementServerUri))
		{
			dictionary["MANAGEMENTSERVERURI"] = options.ManagementServerUri;
		}
		if (!string.IsNullOrEmpty(options.SystemPassword))
		{
			dictionary["SYSTEMPASSWORD"] = options.SystemPassword;
		}
		if (options.Parameters != null && options.Parameters.Count != 0)
		{
			IList<string> list = HandleTaggedParameterWithDelimiterCharacters(options.Parameters, "[", ':', "]");
			for (int i = 0; i < list.Count; i += 2)
			{
				dictionary[list[i]] = list[i + 1];
			}
		}
		foreach (KeyValuePair<string, string> item in dictionary)
		{
			if (item.Key.Equals("SYSTEMPASSWORD", StringComparison.OrdinalIgnoreCase))
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Overwrite {0} from command line ******", item.Key));
			}
			else
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Overwrite {0} from command line {1}", item.Key, item.Value));
			}
		}
		return dictionary;
	}

	public static IList<string> HandleTaggedParameterWithDelimiterCharacters(IList<string> parameters, string startTag, char delimiter, string endTag)
	{
		List<string> list = new List<string>();
		int i;
		for (i = 0; i < parameters.Count - 1; i++)
		{
			int num;
			if (IsStartTag(parameters[i], startTag, endTag) && (num = FindEndTag(parameters, i + 1, startTag, endTag)) > 0)
			{
				int num2 = i;
				string text = string.Empty;
				for (int j = num2; j < num; j++)
				{
					text = text + parameters[j] + delimiter;
				}
				text += parameters[num];
				list.Add(RemoveTags(text));
				i = num;
			}
			else
			{
				list.Add(parameters[i]);
			}
		}
		if (i < parameters.Count)
		{
			list.Add(parameters[i]);
		}
		return list;
	}

	private static string RemoveTags(string taggedParameter)
	{
		return taggedParameter.Remove(taggedParameter.Length - 1, 1).Remove(0, 1);
	}

	private static int FindEndTag(IList<string> parameters, int startIx, string startTag, string endTag)
	{
		for (int i = startIx; i < parameters.Count; i++)
		{
			if (IsEndTag(parameters[i], startTag, endTag))
			{
				return i;
			}
		}
		return -1;
	}

	private static bool IsStartTag(string parameter, string startTag, string endTag)
	{
		if (parameter.StartsWith(startTag, StringComparison.OrdinalIgnoreCase))
		{
			return !parameter.EndsWith(endTag, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private static bool IsEndTag(string parameter, string startTag, string endTag)
	{
		if (!parameter.StartsWith(startTag, StringComparison.OrdinalIgnoreCase))
		{
			return parameter.EndsWith(endTag, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private static string FindIndexedProcessName(int pid)
	{
		string processName = Process.GetProcessById(pid).ProcessName;
		Process[] processesByName = Process.GetProcessesByName(processName);
		string text = null;
		for (int i = 0; i < processesByName.Length; i++)
		{
			try
			{
				text = ((i == 0) ? processName : (processName + "#" + i));
				if ((int)new PerformanceCounter("Process", "ID Process", text).NextValue() == pid)
				{
					return text;
				}
			}
			catch
			{
			}
		}
		return text;
	}

	private static Process FindPidFromIndexedProcessName(string indexedProcessName)
	{
		PerformanceCounter performanceCounter = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
		Process result = null;
		try
		{
			result = Process.GetProcessById((int)performanceCounter.NextValue());
		}
		catch
		{
		}
		return result;
	}

	public static Process Parent(Process process)
	{
		return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
	}

	private static bool ExtractMSIFiles(string targetDir)
	{
		if (Regex.IsMatch(targetDir, "^[a-zA-Z]\\:$"))
		{
			targetDir += "\\";
		}
		if (new Regex("^(([a-zA-Z]\\:)|(\\\\))(\\\\{1}|((\\\\{1})[^\\\\]([^/:*?<>\"|]*))+)$").IsMatch(targetDir))
		{
			InstallerManager.Instance.ExtractMSIFiles(targetDir);
			return true;
		}
		return false;
	}

	private static bool GenerateArgumentsFile(InstallerOptions options)
	{
		if (Regex.IsMatch(options.ArgumentsPath, "^[a-zA-Z]\\:$"))
		{
			options.ArgumentsPath += "\\";
		}
		if (new Regex("^(([a-zA-Z]\\:)|(\\\\))(\\\\{1}|((\\\\{1})[^\\\\]([^/:*?<>\"|]*))+)$").IsMatch(options.ArgumentsPath))
		{
			InstallerManager.Instance.ExtractArgumentsFile(options.ArgumentsPath);
			return true;
		}
		return false;
	}

	private static void SetModeSettings(InstallerOptions options)
	{
		if (options.Mode == "SILENT")
		{
			options.IsQuiet = true;
		}
		if (GlobalData.Settings == null)
		{
			GlobalData.Settings = new List<IPackageParameter>();
		}
		if (options.IsUninstall)
		{
			GlobalData.Settings.Add(new PackageParameter("Installation Context", "uninstall"));
		}
		else
		{
			GlobalData.Settings.Add(new PackageParameter("Installation Context", "install"));
		}
		if (options.IsDebugMode)
		{
			GlobalData.Settings.Add(new PackageParameter("DEBUG_MODE", "true"));
		}
		if (options.IsQuiet)
		{
			GlobalData.Settings.Add(new PackageParameter("QUIET-MODE", "true"));
		}
		if (options.HardwareManufacturerMode)
		{
			GlobalData.Settings.Add(new PackageParameter("HARDWARE-MANUFACTURER-MODE", "true"));
		}
		if (options.FinalizeMode)
		{
			GlobalData.Settings.Add(new PackageParameter("FINALIZE-MODE", "true"));
			options.DoContinue = true;
		}
		if (options.SkipMsFailoverInstall)
		{
			GlobalData.Settings.Add(new PackageParameter("SKIP_MS_FAILOVER_INSTALL", "true"));
		}
		if (!string.IsNullOrEmpty(options.RebootOnPackageId))
		{
			GlobalData.Settings.Add(new PackageParameter("REBOOTONPACKAGEID", options.RebootOnPackageId));
		}
	}

	private static bool SetLogFileSettings(InstallerOptions options)
	{
		LogManager.ApplicationPath = Path.Combine(Environment.CurrentDirectory, Environment.GetCommandLineArgs()[0]);
		LogManager.SetLogInstance((ILogger)(object)new InstallerLog());
		string text = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Path.Combine(GlobalData.OemHelper.CompanyName, "Installer")), Path.GetFileName(LogManager.GetLogFilename()));
		CleanupLogFiles(text);
		LogManager.SetLogFilename(text);
		if (!string.IsNullOrEmpty(options.LogFile))
		{
			if (!new Regex("^(([a-zA-Z]\\:)|(\\\\))(\\\\{1}|((\\\\{1})[^\\\\]([^/:*?<>\"|]*))+)$").IsMatch(options.LogFile))
			{
				return false;
			}
			LogManager.SetLogFilename(Path.Combine(options.LogFile, Path.GetFileName(LogManager.GetLogFilename())));
		}
		return true;
	}

	private static void CleanupLogFiles(string installerLogPath)
	{
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(installerLogPath);
		string extension = Path.GetExtension(installerLogPath);
		string arg = "\\d{4}-\\d{2}-\\d{2}";
		Regex logFileRegex = new Regex(string.Format(CultureInfo.InvariantCulture, "^{0}(\\.{1})?{2}$", Regex.Escape(fileNameWithoutExtension), arg, Regex.Escape(extension)));
		Regex regex = new Regex("(?<before>Parameter '.*?PASSWORD.*?' was (added with value|changed to) ')(?<pass>.*[^*\\n].*|[*]{0,3}|[*]{5,})(?<after>' from)");
		try
		{
			foreach (FileInfo item in from f in new DirectoryInfo(Path.GetDirectoryName(installerLogPath)).GetFiles()
				where logFileRegex.IsMatch(f.Name)
				select f)
			{
				try
				{
					string input = File.ReadAllText(item.FullName);
					if (regex.IsMatch(input))
					{
						string contents = regex.Replace(input, (Match m) => string.Format(CultureInfo.InvariantCulture, "{0}****{1}", m.Groups["before"].Value, m.Groups["after"].Value));
						File.WriteAllText(item.FullName, contents, Encoding.Default);
					}
				}
				catch (SecurityException)
				{
				}
				catch (IOException)
				{
				}
			}
		}
		catch (SecurityException)
		{
		}
		catch (IOException)
		{
		}
	}
}
