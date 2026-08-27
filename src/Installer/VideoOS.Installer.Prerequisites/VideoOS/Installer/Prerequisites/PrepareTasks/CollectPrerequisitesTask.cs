using System;
using System.Collections.Generic;
using System.IO;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Configuration.Package;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Prerequisites.Properties;

namespace VideoOS.Installer.Prerequisites.PrepareTasks;

public class CollectPrerequisitesTask : InstallerTask
{
	protected Dictionary<string, List<IPackageParameter>> _packageParameters;

	protected InstallerConfig _installerConfig;

	protected PrerequisiteInfo _prerequisiteInfo;

	private List<string> _packagesToInstall;

	private bool _isModifyMode;

	public override bool RunInHardwareManufacturerMode => true;

	public override bool RunInFinalizeMode => false;

	public CollectPrerequisitesTask(string targetDir, Dictionary<string, List<IPackageParameter>> parameters, InstallerConfig installerConfig, PrerequisiteInfo prerequisiteInfo)
	{
		base.TargetDir = targetDir;
		_packageParameters = parameters;
		_installerConfig = installerConfig;
		_prerequisiteInfo = prerequisiteInfo;
		_packagesToInstall = null;
		base.MaximumProgress = 0;
	}

	public CollectPrerequisitesTask()
	{
	}

	public CollectPrerequisitesTask(string targetDir, InstallerConfig installerConfig, List<string> packagesToInstall, Dictionary<string, List<IPackageParameter>> parameters, PrerequisiteInfo prerequisiteInfo)
	{
		base.TargetDir = targetDir;
		_packageParameters = parameters;
		_installerConfig = installerConfig;
		_packagesToInstall = packagesToInstall;
		_prerequisiteInfo = prerequisiteInfo;
		_isModifyMode = true;
	}

	protected override void DoRun()
	{
		Dictionary<string, ConditionType> dictionary = new Dictionary<string, ConditionType>();
		Status = TaskStatus.Running;
		int num = 0;
		base.Message = string.Format(Resources.Message_CollectPrerequisites);
		LogManager.Write(base.Message);
		base.MaximumProgress = 100;
		base.CurrentProgress = 0;
		NotifyObservers();
		base.CurrentProgress++;
		PackageRefType[] installExecuteSequence = _installerConfig.InstallExecuteSequence;
		foreach (PackageRefType packageRefType in installExecuteSequence)
		{
			dictionary[packageRefType.ID] = packageRefType.Condition;
		}
		num = base.MaximumProgress / _installerConfig.InstallerPackages.Length;
		PrerequisitesValidator prerequisitesValidator = new PrerequisitesValidator();
		InstallerPackageType[] installerPackages = _installerConfig.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			if (_isModifyMode)
			{
				if (!_packagesToInstall.Contains(installerPackageType.ID))
				{
					continue;
				}
				LoadPrerequisiteXml(installerPackageType);
			}
			else
			{
				LoadPrerequisiteXml(installerPackageType);
			}
			prerequisitesValidator.Check(installerPackageType.ProductManifest?.PackageData?.Prerequisites, installerPackageType.Name);
			bool flag = false;
			if (dictionary.ContainsKey(installerPackageType.ID) && dictionary[installerPackageType.ID] != ConditionType.Skip)
			{
				if (installerPackageType.InstallConditions != null)
				{
					InstallConditionTypeByPassIf[] installConditions = installerPackageType.InstallConditions;
					foreach (InstallConditionTypeByPassIf installConditionTypeByPassIf in installConditions)
					{
						if (installConditionTypeByPassIf.Condition == InstallStateType.NotInstalled)
						{
							if (dictionary.ContainsKey(installConditionTypeByPassIf.ID) && dictionary[installConditionTypeByPassIf.ID] != ConditionType.Skip)
							{
								flag = true;
								break;
							}
						}
						else if (dictionary.ContainsKey(installConditionTypeByPassIf.ID) && dictionary[installConditionTypeByPassIf.ID] == ConditionType.Skip)
						{
							flag = true;
							break;
						}
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					if (installerPackageType.ProductManifest.PackageData.Prerequisites != null)
					{
						PackageCollectionType[] prerequisites = installerPackageType.ProductManifest.PackageData.Prerequisites;
						foreach (PackageCollectionType packageCollectionType in prerequisites)
						{
							try
							{
								if (packageCollectionType != null && !_prerequisiteInfo.Prerequisites.ContainsKey(packageCollectionType.ProductInfo.ProductCode))
								{
									_prerequisiteInfo.Prerequisites[packageCollectionType.ProductInfo.ProductCode] = packageCollectionType;
								}
							}
							catch (Exception exception)
							{
								LogManager.Write(exception);
							}
						}
					}
					if (installerPackageType.InstallationTaskPlugin != null)
					{
						installerPackageType.InstallationTaskPlugin.IsInstalled = false;
						InstallerTaskPluginProperties installerTaskPluginProperties = new InstallerTaskPluginProperties();
						installerTaskPluginProperties.Filename = installerPackageType.ProductManifest.Filename;
						installerTaskPluginProperties.InstallationTaskPlugin = installerPackageType.InstallationTaskPlugin;
						if (_packageParameters.ContainsKey(installerPackageType.ID))
						{
							installerTaskPluginProperties.PackageParameters = _packageParameters[installerPackageType.ID];
						}
						_prerequisiteInfo.InstallationTaskAdapter[installerPackageType.ID] = installerTaskPluginProperties;
					}
				}
				else
				{
					ConditionType condition = ConditionType.Skip;
					SetConditionForThisPackage(installerPackageType, condition);
				}
			}
			base.CurrentProgress += num;
			NotifyObservers();
		}
		base.MaximumProgress = 0;
		NotifyObservers();
		Status = TaskStatus.Completed;
		_result = TaskResult.Success;
	}

	private void SetConditionForThisPackage(InstallerPackageType installerPackage, ConditionType condition)
	{
		PackageRefType[] installExecuteSequence = _installerConfig.InstallExecuteSequence;
		foreach (PackageRefType packageRefType in installExecuteSequence)
		{
			if (installerPackage.ID == packageRefType.ID)
			{
				packageRefType.Condition = condition;
				break;
			}
		}
		installExecuteSequence = _installerConfig.UninstallerInfo.UninstallerSequencer;
		foreach (PackageRefType packageRefType2 in installExecuteSequence)
		{
			if (installerPackage.ID == packageRefType2.ID)
			{
				packageRefType2.Condition = condition;
				break;
			}
		}
	}

	private void LoadPrerequisiteXml(InstallerPackageType package)
	{
		string text = null;
		if (package.ProductManifest.PackageData.Prerequisites == null)
		{
			return;
		}
		PackageCollectionType[] prerequisites = package.ProductManifest.PackageData.Prerequisites;
		foreach (PackageCollectionType packageCollectionType in prerequisites)
		{
			if (packageCollectionType.ProductInfo.ProductManifest == null)
			{
				continue;
			}
			try
			{
				text = Path.Combine(base.TargetDir, Path.Combine(packageCollectionType.ProductInfo.InstallationFolder, packageCollectionType.ProductInfo.ProductManifest.Filename));
				if (packageCollectionType.ProductInfo.ProductManifest.Format == VideoOS.Installer.Configuration.Package.FormatType.Milestone)
				{
					packageCollectionType.ProductInfo.ProductManifest.ManifestData = InstallerConfigManager.LoadPackage(text);
				}
				if (packageCollectionType.ProductInfo.ProductManifest.Format == VideoOS.Installer.Configuration.Package.FormatType.Microsoft)
				{
					packageCollectionType.ProductInfo.ProductManifest.ManifestData = InstallerConfigManager.LoadProduct(text);
				}
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
			}
		}
	}

	protected override void PostRun()
	{
		Status = TaskStatus.NotStarted;
		_result = TaskResult.Success;
	}
}
