using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Timers;
using Microsoft.Installer.ProductConfig;
using VideoOS.Installer.Common;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Sequencer.Properties;

namespace VideoOS.Installer.Sequencer.Prerequisite;

public class PrerequisiteInstallerTask : InstallerTask
{
	private const string _currentDirReplaceStr = "%currentdir%";

	private const string _systemRootReplaceStr = "%systemroot%";

	private const string _winDirStr = "%windir%";

	protected ProductInstallInfo _productInfo;

	protected int _summEstimatedInstallSecounds = 100;

	protected int _delta = 2;

	private string _dashes = "----------------------------------";

	public override bool RunInHardwareManufacturerMode => true;

	public override bool RunInFinalizeMode => false;

	public PrerequisiteInstallerTask()
	{
	}

	public PrerequisiteInstallerTask(string targetDir, ProductInstallInfo productInfo)
	{
		if (productInfo == null)
		{
			throw new ArgumentException("product");
		}
		_productInfo = productInfo;
		base.DisplayName = productInfo.DisplayName;
		base.Id = productInfo.DisplayName;
		base.TargetDir = targetDir;
		if (_productInfo.SummEstimatedInstallSecounds != 0)
		{
			_summEstimatedInstallSecounds = _productInfo.SummEstimatedInstallSecounds;
		}
	}

	protected override void DoRun()
	{
		Init();
		InstallPrerequisites();
		NotifyObservers();
	}

	protected void Init()
	{
		base.MaximumProgress = _summEstimatedInstallSecounds;
		base.CurrentProgress = 0;
		base.Action = base.Id;
		LogManager.Write(_dashes);
		LogManager.Write(string.Format("{0}   {1}   {2}", _dashes, "Installing of the prerequisites started.", _dashes));
		base.Message = string.Format(Resources.Sequencer_Installing, base.Id);
		LogManager.Write(base.Message);
		NotifyObservers();
		System.Timers.Timer timer = new System.Timers.Timer();
		timer.Elapsed += timer_Elapsed;
		timer.Interval = _delta * 1000;
		timer.Enabled = true;
	}

	protected void InstallPrerequisites()
	{
		if (_productInfo.Command != null)
		{
			if (_productInfo.Command.Arguments.Contains("%currentdir%"))
			{
				_productInfo.Command.Arguments = _productInfo.Command.Arguments.Replace("%currentdir%", base.TargetDir + "\\");
			}
			if (_productInfo.Command.Arguments.Contains("%systemroot%"))
			{
				_productInfo.Command.Arguments = _productInfo.Command.Arguments.Replace("%systemroot%", Environment.GetFolderPath(Environment.SpecialFolder.Windows));
			}
			string text = _productInfo.Command.PackageFile;
			if (_productInfo.Command.PackageFile.Contains("%windir%"))
			{
				_productInfo.Command.PackageFile = _productInfo.Command.PackageFile.Replace("%windir%", Environment.GetFolderPath(Environment.SpecialFolder.Windows));
			}
			else
			{
				text = Path.Combine(base.TargetDir, _productInfo.Command.PackageFile);
			}
			if (!File.Exists(text))
			{
				LogManager.Write($"Prerequisite {text} not found.", TraceEventType.Warning);
				text = _productInfo.Command.PackageFile;
			}
			LogManager.Write($"Running {text} with arguments: {_productInfo.Command.Arguments}", TraceEventType.Information);
			Process process = Process.Start(new ProcessStartInfo(text, _productInfo.Command.Arguments)
			{
				CreateNoWindow = true,
				ErrorDialog = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				RedirectStandardInput = true,
				UseShellExecute = false,
				WindowStyle = ProcessWindowStyle.Hidden
			});
			process.WaitForExit();
			LogManager.Write(process.StandardOutput.ReadToEnd());
			int exitCode = process.ExitCode;
			string text2 = null;
			CommandsTypeCommandExitCodesExitCode[] exitCode2 = _productInfo.Command.ExitCodes.ExitCode;
			foreach (CommandsTypeCommandExitCodesExitCode commandsTypeCommandExitCodesExitCode in exitCode2)
			{
				if (commandsTypeCommandExitCodesExitCode.Value == exitCode)
				{
					text2 = commandsTypeCommandExitCodesExitCode.Result.ToString();
					LogManager.Write($"Prerequisite {base.Id} installed. Exit code: {exitCode.ToString()}", TraceEventType.Information);
					_result = TaskResult.Success;
					base.CurrentProgress = base.MaximumProgress;
					base.Action = base.Id;
					NotifyObservers();
					if (commandsTypeCommandExitCodesExitCode.Result == ExitCodeResultType.SuccessReboot)
					{
						base.NeedsReboot = true;
						LastError = new Exception(string.Format(Resources.Message_RestartAfterPrereqInstalled, base.DisplayName));
					}
					break;
				}
			}
			if (text2 == null)
			{
				try
				{
					ProcessResult(exitCode, _productInfo.Command);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			Status = TaskStatus.Completed;
		}
		else
		{
			LogManager.Write($"Commands for {base.Id} prerequisite are empty.", TraceEventType.Error);
		}
	}

	protected void ProcessResult(int processExitCode, CommandsTypeCommand command)
	{
		string text = null;
		if (command.ExitCodes.DefaultExitCode != null)
		{
			if (command.ExitCodes.DefaultExitCode.FormatMessageFromSystemSpecified && command.ExitCodes.DefaultExitCode.FormatMessageFromSystem)
			{
				if (string.Compare(_productInfo.ProductCode, "Microsoft.Net.Framework.3.5", StringComparison.OrdinalIgnoreCase) == 0)
				{
					text = string.Format(CultureInfo.CurrentCulture, "{0} is not installed and there is no Internet access to automatically install it.", base.DisplayName);
					LogManager.Write(text);
				}
				else
				{
					text = $"Last error: {MessageHelper.GetSystemErrorMessage(processExitCode, Thread.CurrentThread.CurrentUICulture)}";
					LogManager.Write(text);
				}
			}
			else
			{
				text = $"Installation of prerequisite {base.Id} failed. Reason: {command.ExitCodes.DefaultExitCode.String}.";
				LogManager.Write(text, TraceEventType.Error);
			}
		}
		else
		{
			text = "Failed to install package ";
			LogManager.Write(text);
		}
		_result = TaskResult.Error;
		throw new SystemException(text);
	}

	protected void timer_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (Status != TaskStatus.Completed)
		{
			UpdateProgress();
		}
	}

	private void UpdateProgress()
	{
		if (base.CurrentProgress + _delta < base.MaximumProgress)
		{
			base.CurrentProgress += _delta;
		}
		else
		{
			base.CurrentProgress = _delta;
		}
		base.Message = string.Format(Resources.Sequencer_Installing, base.Id);
		NotifyObservers();
	}

	protected override void PostRun()
	{
	}

	protected override void PreRun()
	{
		if (_productInfo.Command == null)
		{
			string text = $"Installation of prerequisite {base.Id} failed. Reason: {_productInfo.ErrorString}.";
			LogManager.Write(text);
			throw new InvalidOperationException(text);
		}
	}
}
