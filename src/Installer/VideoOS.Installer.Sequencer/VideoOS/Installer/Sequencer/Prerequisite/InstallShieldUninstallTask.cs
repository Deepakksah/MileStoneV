using System;
using System.Diagnostics;
using Microsoft.Win32;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Sequencer.ErrorHandling;
using VideoOS.Installer.Sequencer.Properties;

namespace VideoOS.Installer.Sequencer.Prerequisite;

public class InstallShieldUninstallTask : InstallerTask
{
	private string _regUninstallString = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\InstallShield_{0}";

	private const string _regUninstallStringKey = "UninstallString";

	private const string _uninstallSilentArgs = "/s /t1\"{0}\"";

	private const string _issFileName = "iss.iss";

	private bool _silent;

	private string _uninstallProcess;

	private string _uninstallArgs;

	public override bool RunInHardwareManufacturerMode => true;

	public override bool RunInFinalizeMode => false;

	public InstallShieldUninstallTask(string productCode, string displayName, string id, bool silentSupported)
	{
		base.Id = id;
		base.DisplayName = displayName;
		_silent = silentSupported;
		LoadValuesFromReg(productCode);
	}

	private void LoadValuesFromReg(string productCode)
	{
		try
		{
			_regUninstallString = string.Format(_regUninstallString, productCode);
			RegistryKey registryKey = RegistryKeyHelper.OpenLocalMachineKey(_regUninstallString);
			if (registryKey == null)
			{
				return;
			}
			using (registryKey)
			{
				if (registryKey.GetValue("DisplayName") is string displayName)
				{
					base.DisplayName = displayName;
				}
				string localMachineValue = RegistryKeyHelper.GetLocalMachineValue(_regUninstallString, "UninstallString");
				if (localMachineValue != null)
				{
					int num = localMachineValue.IndexOf(".exe");
					_uninstallProcess = localMachineValue.Substring(0, num + 4);
					_uninstallArgs = localMachineValue.Substring(num + 4, localMachineValue.Length - num - 4);
				}
				else
				{
					LogManager.Write($"Uninstallation command for {base.DisplayName} is empty.", TraceEventType.Information);
				}
			}
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
		}
	}

	protected override void DoRun()
	{
		base.MaximumProgress = 5;
		base.CurrentProgress = 0;
		if (_uninstallProcess != null)
		{
			base.Message = string.Format(Resources.Sequencer_Uninstalling, base.DisplayName);
			NotifyObservers();
			LogManager.Write($"Uninstalling '{base.DisplayName} ({base.Id})'...");
			if (_silent)
			{
				_uninstallArgs = _uninstallArgs + " " + string.Format("/s /t1\"{0}\"", "iss.iss");
			}
			StartProcess();
			if (VerifyUninstallation() != 0)
			{
				string message = string.Format(Resources.Sequencer_UninstallingFailed, base.DisplayName);
				LogManager.Write(string.Format("Uninstallation of {0} {1} failed with exit code {0}", _uninstallProcess, _uninstallArgs, base.DisplayName), TraceEventType.Error);
				throw new InstallerSequencerException(message);
			}
		}
		_result = TaskResult.Success;
		base.CurrentProgress = base.MaximumProgress;
		NotifyObservers();
	}

	private int VerifyUninstallation()
	{
		if (RegistryKeyHelper.GetLocalMachineValue(_regUninstallString, "DisplayName") != null)
		{
			return 1;
		}
		return 0;
	}

	private int StartProcess()
	{
		LogManager.Write($"RunProcess: '{_uninstallProcess}' {_uninstallArgs}", TraceEventType.Information);
		new ProcessStartInfo(_uninstallProcess, _uninstallArgs);
		Process process = new Process();
		process.StartInfo.FileName = _uninstallProcess;
		process.StartInfo.Arguments = _uninstallArgs;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.CreateNoWindow = true;
		process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		process.Start();
		process.WaitForExit();
		return process.ExitCode;
	}

	protected override void PreRun()
	{
	}

	protected override void PostRun()
	{
	}
}
