using System.Collections.Generic;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Prerequisites.Properties;

namespace VideoOS.Installer.Prerequisites.PrepareTasks;

public class CollectInstallerPrerequisitesTask : CollectPrerequisitesTask
{
	public CollectInstallerPrerequisitesTask(InstallerConfig installerConfig, PrerequisiteInfo prerequisiteInfo)
		: base("", null, installerConfig, prerequisiteInfo)
	{
	}

	public CollectInstallerPrerequisitesTask(string targetDir, Dictionary<string, List<IPackageParameter>> parameters, InstallerConfig installerConfig, PrerequisiteInfo prerequisiteInfo)
		: base(targetDir, parameters, installerConfig, prerequisiteInfo)
	{
	}

	protected override void DoRun()
	{
		Status = TaskStatus.Running;
		int num = 0;
		base.Message = string.Format(Resources.Message_CollectPrerequisites);
		LogManager.Write(base.Message);
		base.MaximumProgress = 100;
		base.CurrentProgress = 0;
		NotifyObservers();
		base.CurrentProgress++;
		new PrerequisitesValidator().Check(_installerConfig.InstallerPrerequisites.InstallerPrerequisite);
		if (_installerConfig.InstallerPrerequisites?.InstallerPrerequisite != null && _installerConfig.InstallerPrerequisites.InstallerPrerequisite.Length != 0)
		{
			num = base.MaximumProgress / _installerConfig.InstallerPrerequisites.InstallerPrerequisite.Length;
			InstallerPrerequisiteCollectionType[] installerPrerequisite = _installerConfig.InstallerPrerequisites.InstallerPrerequisite;
			foreach (InstallerPrerequisiteCollectionType installerPrerequisiteCollectionType in installerPrerequisite)
			{
				if (installerPrerequisiteCollectionType.PrerequisiteInfo.ProductManifest != null && installerPrerequisiteCollectionType.PrerequisiteInfo.ProductManifest.PrerequisitData != null)
				{
					_prerequisiteInfo.InstallerPrerequisites[installerPrerequisiteCollectionType.PrerequisiteInfo.ProductCode] = installerPrerequisiteCollectionType;
				}
				base.CurrentProgress += num;
				NotifyObservers();
			}
		}
		base.MaximumProgress = 0;
		NotifyObservers();
		Status = TaskStatus.Completed;
		_result = TaskResult.Success;
	}

	protected override void PostRun()
	{
		Status = TaskStatus.NotStarted;
		_result = TaskResult.Success;
	}
}
