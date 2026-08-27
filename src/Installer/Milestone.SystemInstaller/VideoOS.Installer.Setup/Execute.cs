using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VideoOS.Installer.CommandLine;
using VideoOS.Installer.Setup.InitialData;
using VideoOS.Installer.Setup.Properties;

namespace VideoOS.Installer.Setup;

public class Execute
{
	private readonly InstallerOptions _options;

	private readonly IProgress<int> _progressBar;

	private readonly IProgress<string> _statusProgress;

	private readonly string _extractFolder;

	public bool ExtractOnly => _options.ExtractPath != null;

	public bool ExtractToDefaultPath => string.IsNullOrEmpty(_options.ExtractPath);

	public Execute(InstallerOptions options, IProgress<int> progressBar, IProgress<string> statusProgress)
	{
		_options = options ?? throw new ArgumentNullException("options");
		_progressBar = progressBar;
		_statusProgress = statusProgress;
		_extractFolder = (ExtractToDefaultPath ? Extract.GetExtractFolder() : _options.ExtractPath);
	}

	public string ValidateAndExtractExternalZipFile()
	{
		string text = null;
		string zipFileName = ExternalZipSpecification.ZipFileName;
		if (!string.IsNullOrEmpty(zipFileName))
		{
			string zipFileHash = ExternalZipSpecification.ZipFileHash;
			using ExternalArchiveValidator externalArchiveValidator = new ExternalArchiveValidator(zipFileName, zipFileHash);
			SetStatus(Resources.ExternalArchiveValidate);
			text = externalArchiveValidator.Validate(_progressBar);
			if (text == null)
			{
				Stream validatedStream = externalArchiveValidator.ValidatedStream;
				SetStatus(Resources.Unpacking);
				text = Extract.ExtractExternalZip(validatedStream, _extractFolder, _progressBar);
			}
		}
		else
		{
			SetStatus(Resources.Unpacking);
			text = Extract.ExtractEmbeddedZip(_extractFolder, _progressBar);
		}
		if (text != null && Directory.Exists(_extractFolder))
		{
			Directory.Delete(_extractFolder, recursive: true);
		}
		return text;
	}

	public async Task<Process> StartInstaller(string[] args, bool withUi)
	{
		if (!ExtractOnly)
		{
			SetStatus(Resources.Launching);
			string installerFileName = InstallerStartupSpecification.InstallerFileName;
			string text = string.Join(" ", args.Select((string a) => (!a.Contains(' ')) ? a : ("\"" + a + "\"")));
			if (_options.ExecutableLocation == null && _options.SS_Registration)
			{
				string text2 = "--exeDir=\"" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\"";
				text = text + " " + text2;
			}
			ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(_extractFolder, installerFileName), text)
			{
				UseShellExecute = false
			};
			Process process = Process.Start(startInfo);
			if (withUi)
			{
				await ProcessLaunched(process);
			}
			return process;
		}
		return null;
	}

	public int Installation(Process process)
	{
		if (process != null)
		{
			process.WaitForExit();
			int exitCode = process.ExitCode;
			if (exitCode == 100 || exitCode == 50)
			{
				Directory.Delete(_extractFolder, recursive: true);
			}
			if (exitCode != 50)
			{
				return exitCode;
			}
			return 0;
		}
		return 0;
	}

	private async Task ProcessLaunched(Process process)
	{
		int fakeProgress = 0;
		int progressReportStep = 1;
		_progressBar.Report(fakeProgress++);
		while (!(from x in Process.GetProcessesByName(process.ProcessName)
			where x.Id == process.Id
			select x).Any())
		{
			if (process.HasExited)
			{
				return;
			}
			await Task.Delay(100).ConfigureAwait(continueOnCapturedContext: false);
			_progressBar.Report(Math.Min(fakeProgress++ / progressReportStep * progressReportStep, 98));
		}
		try
		{
			process.WaitForInputIdle();
			while (process.MainWindowHandle == IntPtr.Zero && !process.HasExited)
			{
				await Task.Delay(100).ConfigureAwait(continueOnCapturedContext: false);
				_progressBar.Report(Math.Min(fakeProgress++ / progressReportStep * progressReportStep, 98));
			}
		}
		catch (InvalidOperationException)
		{
		}
	}

	private void SetStatus(string statusString)
	{
		_statusProgress.Report(statusString);
	}
}
