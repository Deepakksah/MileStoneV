using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using VideoOS.Installer.DotNetHostingPlugin.Properties;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.DotNetHostingPlugin;

public class DotNetHostingBundleTask : InstallerTask, IInstallationAdapter
{
	private enum InstallerType
	{
		DotNetHosting,
		DotNetRuntime,
		WindowsDesktopRuntime,
		AspNetRuntime
	}

	private const string DotNetHostingFolder = "DotNetHosting";

	private const string DotNetHostingInstallerNamePattern = "dotnet-hosting-*-win.exe";

	private const string AspNetRuntimeInstallerNamePattern = "aspnetcore-runtime-*-win-x64.exe";

	private const string DotNetRuntimeInstallerNamePattern = "dotnet-runtime-*-win-x64.exe";

	private const string WindowsDesktopRuntimeInstallerNamePattern = "windowsdesktop-runtime-*-win-x64.exe";

	private const string AspNetCoreModule = "AspNetCoreModule*_x64.msi";

	private readonly string DefaultInstallerNamePattern = "dotnet-hosting-*-win.exe";

	private readonly Dictionary<InstallerType, string> InstallerTypeToNamePattern = new Dictionary<InstallerType, string>
	{
		{
			InstallerType.DotNetHosting,
			"dotnet-hosting-*-win.exe"
		},
		{
			InstallerType.DotNetRuntime,
			"dotnet-runtime-*-win-x64.exe"
		},
		{
			InstallerType.WindowsDesktopRuntime,
			"windowsdesktop-runtime-*-win-x64.exe"
		},
		{
			InstallerType.AspNetRuntime,
			"aspnetcore-runtime-*-win-x64.exe"
		}
	};

	private readonly int _interval = 4;

	public override bool RunInHardwareManufacturerMode => true;

	public override bool RunInFinalizeMode => false;

	protected override void DoRun()
	{
		base.Message = string.Format(CultureInfo.CurrentCulture, Resources.DotNetHosting_StartInstallation, "DotNet Hosting");
		base.CurrentProgress = 0;
		base.MaximumProgress = _interval * 15;
		NotifyObservers();
		using Timer timer = new Timer();
		timer.Elapsed += OnTimedProgressUpdate;
		timer.Interval = 1000.0;
		timer.Enabled = true;
		LogManager.Write("DoRun(): DotNetHostingBundle task stopping W3SVC before installation");
		IisServiceStop();
		LogManager.Write("DoRun(): DotNetHostingBundle task installation started");
		try
		{
			base.NeedsReboot |= InstallDotNetHosting();
			base.NeedsReboot |= InstallRuntime(InstallerType.DotNetRuntime);
			base.NeedsReboot |= InstallRuntime(InstallerType.AspNetRuntime);
			base.NeedsReboot |= InstallRuntime(InstallerType.WindowsDesktopRuntime);
		}
		catch (Exception ex)
		{
			throw new InstallationAdapterException(string.Format(CultureInfo.InvariantCulture, Resources.DotNetHosting_InstallationException, ex.Message), ex);
		}
		finally
		{
			timer.Enabled = false;
			base.CurrentProgress = base.MaximumProgress;
			NotifyObservers();
			base.MaximumProgress = 0;
		}
	}

	private bool InstallRuntime(InstallerType type)
	{
		IEnumerable<string> installerName = GetInstallerName(type);
		bool flag = false;
		foreach (string item in installerName)
		{
			flag |= IsRebootCode(RunInstaller(type, item));
		}
		return flag;
	}

	private static bool IsRebootCode(int exitCode)
	{
		if (exitCode != 1641)
		{
			return exitCode == 3010;
		}
		return true;
	}

	private int RunInstaller(InstallerType type, string installerName)
	{
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "DoRun(): Installation file is {0}", installerName));
		string fileName = Path.Combine(base.TargetDir, Path.Combine("DotNetHosting", installerName));
		string arguments = "/INSTALL /QUIET /NORESTART";
		int num = RunProcess(fileName, arguments);
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "{0} installation exited with code {1}.", type, num));
		if (type == InstallerType.DotNetRuntime)
		{
			CheckDotnetEnvironmentVariable(installerName);
		}
		return num;
	}

	private bool InstallDotNetHosting()
	{
		IEnumerable<string> installerName = GetInstallerName(InstallerType.DotNetHosting);
		bool flag = false;
		foreach (string item in installerName)
		{
			flag |= IsRebootCode(RunDotNetHostingInstaller(item));
		}
		return flag;
	}

	private int RunDotNetHostingInstaller(string dotNetHostingInstallerName)
	{
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "DoRun(): Installation file is {0}", dotNetHostingInstallerName));
		string fileName = Path.Combine(base.TargetDir, Path.Combine("DotNetHosting", dotNetHostingInstallerName));
		string arguments = "OPT_NO_SHAREDFX=1 OPT_NO_RUNTIME=1 /REPAIR /QUIET /NORESTART";
		int num = RunProcess(fileName, arguments);
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "DotNetHostingBundle installation exited with code {0}", num));
		if (num == 1638)
		{
			try
			{
				FileInfo aspNetCoreModule = GetAspNetCoreModule();
				if (aspNetCoreModule != null)
				{
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Installing {0} in case hosting bundle was installed before IIS", aspNetCoreModule.Name));
					num = RunProcess("msiexec", string.Format(CultureInfo.InvariantCulture, "/i \"{0}\" /QUIET /NORESTART", aspNetCoreModule.FullName));
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "AspNetCoreModule installation exited with code {0}", num));
				}
				else
				{
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "AspNetCoreModule was not found in {0} ", "DotNetHosting"));
				}
			}
			catch (Exception ex)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Installing AspNetCoreModule failed: {0}.", ex.Message));
			}
		}
		else
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "DotNetHostingBundle installation exited with code {0}.", num));
		}
		return num;
	}

	private FileInfo GetAspNetCoreModule()
	{
		return (from path in Directory.GetFiles(Path.Combine(base.TargetDir, "DotNetHosting"), "AspNetCoreModule*_x64.msi")
			select new FileInfo(path)).FirstOrDefault();
	}

	private IEnumerable<string> GetInstallerName(InstallerType type)
	{
		string searchPattern = (InstallerTypeToNamePattern.ContainsKey(type) ? InstallerTypeToNamePattern[type] : DefaultInstallerNamePattern);
		string text = Path.Combine(base.TargetDir, "DotNetHosting");
		string[] files = Directory.GetFiles(text, searchPattern);
		if (files.Length == 0)
		{
			throw new InstallationAdapterException("No .NET Windows hosting bundle installer found in " + text + ".");
		}
		return files;
	}

	private static void CheckDotnetEnvironmentVariable(string dotNetHostingInstallerName)
	{
		List<string> list = (from s in Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine).Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries)
			select s.TrimEnd('\\', '/')).ToList();
		string programFilesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet");
		if (list.FindIndex((string s) => string.Compare(s, programFilesPath, StringComparison.OrdinalIgnoreCase) == 0) == -1)
		{
			throw new InstallationAdapterException("Could not find the required 'dotnet' path in system variables after installation of " + dotNetHostingInstallerName + ".");
		}
	}

	private static int RunProcess(string fileName, string arguments)
	{
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "RunProcess: '{0}' {1}", fileName, arguments));
		using Process process = new Process();
		process.StartInfo.FileName = fileName;
		process.StartInfo.Arguments = arguments;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.CreateNoWindow = true;
		process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		process.Start();
		process.WaitForExit();
		return process.ExitCode;
	}

	protected override void PostRun()
	{
		IisServiceStart();
	}

	public bool CheckPrequisite(string fileName, List<IPackageParameter> installParameters)
	{
		return false;
	}

	public void Finalize(List<IPackageParameter> installParameters)
	{
	}

	public void Initialize(List<IPackageParameter> installParameters)
	{
	}

	public void Install(string fileName, List<IPackageParameter> installParameters)
	{
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}

	private void OnTimedProgressUpdate(object source, ElapsedEventArgs e)
	{
		if (base.CurrentProgress + _interval < base.MaximumProgress)
		{
			base.CurrentProgress += _interval;
			NotifyObservers();
		}
	}

	private static void IisServiceStop()
	{
		if (ServiceExists("W3SVC"))
		{
			using (ServiceController serviceController = new ServiceController("W3SVC"))
			{
				if (serviceController != null && serviceController.Status == ServiceControllerStatus.Running)
				{
					try
					{
						LogManager.Write("IisServiceStop(): DotNetHostingBundle task stopping W3SVC service...");
						serviceController.Stop();
						serviceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 3, 0));
						LogManager.Write("IisServiceStop(): DotNetHostingBundle task W3SVC service stopped.");
						return;
					}
					catch (System.ServiceProcess.TimeoutException)
					{
						LogManager.Write("IisServiceStop(): DotNetHostingBundle task timeout waiting for service to stop, trying to continue anyway...");
						return;
					}
				}
				return;
			}
		}
		LogManager.Write("IisServiceStop(): DotNetHostingBundle W3SVC service does not exist, nothing to do.");
	}

	private static void IisServiceStart()
	{
		if (ServiceExists("W3SVC"))
		{
			using (ServiceController serviceController = new ServiceController("W3SVC"))
			{
				if (serviceController != null && serviceController.Status != ServiceControllerStatus.Running)
				{
					try
					{
						LogManager.Write("IisServiceStart(): DotNetHostingBundle task starting W3SVC service...");
						serviceController.Start();
						serviceController.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 3, 0));
						LogManager.Write("IisServiceStart(): DotNetHostingBundle task W3SVC service started.");
						return;
					}
					catch (System.ServiceProcess.TimeoutException)
					{
						LogManager.Write("IisServiceStart(): DotNetHostingBundle task timeout waiting for service to start, trying to continue anyway...");
						return;
					}
				}
				return;
			}
		}
		LogManager.Write("IisServiceStop(): DotNetHostingBundle W3SVC service does not exist, nothing to do.");
	}

	private static bool ServiceExists(string ServiceName)
	{
		return ServiceController.GetServices().Any((ServiceController serviceController) => serviceController.ServiceName.Equals(ServiceName, StringComparison.OrdinalIgnoreCase));
	}
}
