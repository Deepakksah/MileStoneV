using System;
using System.Diagnostics;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.IIS7;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Sequencer.Properties;

namespace VideoOS.Installer.Sequencer.Prerequisite;

public class IisPrerequisiteInstallerTask : PrerequisiteInstallerTask
{
	private int _webSitePort = 80;

	public IisPrerequisiteInstallerTask(string targetDir, ProductInstallInfo productInfo, int webSitePort)
		: base(targetDir, productInfo)
	{
		_webSitePort = webSitePort;
	}

	protected override void DoRun()
	{
		Init();
		if (_productInfo.Command != null && _productInfo.Command.Arguments == "Assembly")
		{
			InstallIISPrerequisite();
		}
		else
		{
			InstallPrerequisites();
		}
		NotifyObservers();
	}

	private void InstallIISPrerequisite()
	{
		InstallIIS7 installIIS = new InstallIIS7();
		if (installIIS.Install())
		{
			LogManager.Write("IIS was successfully installed", TraceEventType.Information);
			base.CurrentProgress = base.MaximumProgress;
			NotifyObservers();
			Status = TaskStatus.Completed;
			return;
		}
		string message = ((installIIS.ErrorType == ErrorType.NeedAddRole) ? Resources.Sequencer_IisIsntallFailedWorkstation : Resources.Sequencer_IisIsntallFailed);
		if (installIIS.ErrorType == ErrorType.NeedReboot)
		{
			base.NeedsReboot = true;
			message = Resources.Message_IISInstRebootRequired;
		}
		throw new Exception(message);
	}

	protected override void PreRun()
	{
		base.PreRun();
	}

	protected override void PostRun()
	{
	}
}
