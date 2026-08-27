using System.Collections.Generic;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces.Msi;
using VideoOS.Installer.Sequencer.Prerequisite;

namespace VideoOS.Installer.Prerequisites.PrepareTasks;

public class CheckInstallerPrerequisitesTask : CheckPrerequisistesTask
{
	public CheckInstallerPrerequisitesTask(IClMsiHandler handler, InstallerConfig installerConfig, PrerequisiteInfo prerequisiteInfo)
		: base(handler, installerConfig, prerequisiteInfo)
	{
	}

	protected override void DoRun()
	{
		base.MaximumProgress = 100;
		base.CurrentProgress = 0;
		NotifyObservers();
		base.CurrentProgress++;
		if (_prerequisiteInfo.InstallerPrerequisites == null || _prerequisiteInfo.InstallerPrerequisites.Count == 0)
		{
			return;
		}
		Status = TaskStatus.Running;
		int delta = base.MaximumProgress / _prerequisiteInfo.InstallerPrerequisites.Count;
		foreach (KeyValuePair<string, InstallerPrerequisiteCollectionType> installerPrerequisite in _prerequisiteInfo.InstallerPrerequisites)
		{
			ProductInstallInfo productInstallInfo = null;
			if (installerPrerequisite.Value.PrerequisiteInfo.ProductManifest.Format == FormatType.Microsoft)
			{
				productInstallInfo = DoProductCheckPrerequisite(installerPrerequisite.Value.PrerequisiteInfo.DisplayName.Value, installerPrerequisite.Value.PrerequisiteInfo.ProductManifest.PrerequisitData, installerPrerequisite.Value.PrerequisiteInfo.ProductManifest.Filename);
			}
			else if (installerPrerequisite.Value.PrerequisiteInfo.ProductManifest.Format == FormatType.Milestone)
			{
				productInstallInfo = DoPackageCheckPrerequisite();
			}
			if (productInstallInfo == null || (productInstallInfo.ErrorString == null && productInstallInfo.Command != null))
			{
				ProceedPrerequisite(installerPrerequisite.Value.PrerequisiteInfo.DisplayName.Value, installerPrerequisite.Value.Type.ToString(), productInstallInfo, installerPrerequisite.Value.PrerequisiteInfo.InstallationFolder, installerPrerequisite.Value.PrerequisiteInfo.ManufactureLink);
			}
			UpdateCurrentProgress(delta);
		}
	}

	protected override void PreRun()
	{
		base.PreRun();
	}

	protected override void PostRun()
	{
		base.PostRun();
	}
}
