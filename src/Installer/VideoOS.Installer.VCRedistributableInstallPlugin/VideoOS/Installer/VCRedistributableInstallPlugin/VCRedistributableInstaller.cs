using System.Diagnostics;
using System.Globalization;
using Microsoft.Win32;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.VCRedistributableInstallPlugin;

public class VCRedistributableInstaller
{
	private readonly string _target;

	private readonly string _arguments = "/QUIET /NORESTART";

	private string _exeToRun;

	private const string _registryBaseVCRuntime = "SOFTWARE\\WOW6432Node\\Microsoft\\VisualStudio\\{0}.0\\VC\\Runtimes\\{1}";

	public string ParameterName { get; }

	public string DisplayName
	{
		get
		{
			if (_exeToRun == null)
			{
				return string.Empty;
			}
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(_exeToRun);
			return versionInfo.ProductName ?? versionInfo.FileName;
		}
	}

	public VCRedistributableInstaller(string parameterName, string target)
	{
		ParameterName = parameterName;
		_target = target;
	}

	public void SetInstallerFile(string path)
	{
		_exeToRun = path;
	}

	public bool IsDefined()
	{
		return _exeToRun != null;
	}

	public int RunProcess()
	{
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "RunProcess: '{0}' {1}", _exeToRun, _arguments));
		using Process process = new Process();
		process.StartInfo.FileName = _exeToRun;
		process.StartInfo.Arguments = _arguments;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.CreateNoWindow = true;
		process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		process.Start();
		process.WaitForExit();
		return process.ExitCode;
	}

	public bool IsInstalled()
	{
		bool result = false;
		FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(_exeToRun);
		string name = string.Format(CultureInfo.InvariantCulture, "SOFTWARE\\WOW6432Node\\Microsoft\\VisualStudio\\{0}.0\\VC\\Runtimes\\{1}", versionInfo.FileMajorPart, _target);
		using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name))
		{
			if (registryKey != null)
			{
				int? num = registryKey.GetValue("Installed") as int?;
				int? num2 = registryKey.GetValue("Major") as int?;
				int? num3 = registryKey.GetValue("Minor") as int?;
				result = num.HasValue && num.Value == 1 && num2.HasValue && num2.Value == versionInfo.FileMajorPart && num3.HasValue && num3.Value >= versionInfo.FileMinorPart;
			}
		}
		return result;
	}
}
