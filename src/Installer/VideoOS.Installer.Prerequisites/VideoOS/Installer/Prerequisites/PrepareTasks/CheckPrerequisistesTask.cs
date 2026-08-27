using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using Microsoft.Installer.ProductConfig;
using Microsoft.Win32;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Configuration.Package;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Interfaces.Msi;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Prerequisites.Properties;
using VideoOS.Installer.Sequencer.Prerequisite;

namespace VideoOS.Installer.Prerequisites.PrepareTasks;

public class CheckPrerequisistesTask : InstallerTask
{
	protected PrerequisiteInfo _prerequisiteInfo;

	protected InstallerConfig _installerConfig;

	public override bool RunInHardwareManufacturerMode => true;

	public override bool RunInFinalizeMode => false;

	public CheckPrerequisistesTask(IClMsiHandler handlesClMsiHandler, InstallerConfig installerConfig, PrerequisiteInfo prerequisiteInfo)
	{
		if (handlesClMsiHandler == null)
		{
			throw new ArgumentNullException("handlesClMsiHandler");
		}
		base.ClMsiHandler = handlesClMsiHandler;
		_prerequisiteInfo = prerequisiteInfo;
		_installerConfig = installerConfig;
		base.MaximumProgress = 0;
	}

	public CheckPrerequisistesTask()
	{
	}

	protected override void DoRun()
	{
		base.MaximumProgress = 100;
		base.CurrentProgress = 0;
		NotifyObservers();
		base.CurrentProgress++;
		if (_prerequisiteInfo.Prerequisites.Count != 0 || _prerequisiteInfo.InstallationTaskAdapter.Count != 0)
		{
			Status = TaskStatus.Running;
			int delta = base.MaximumProgress / (_prerequisiteInfo.Prerequisites.Count + _prerequisiteInfo.InstallationTaskAdapter.Count);
			foreach (KeyValuePair<string, PackageCollectionType> prerequisite in _prerequisiteInfo.Prerequisites)
			{
				ProductInstallInfo productInstallInfo = null;
				if (prerequisite.Value.ProductInfo.ProductManifest != null && prerequisite.Value.ProductInfo.ProductManifest.ProductData != null)
				{
					if (prerequisite.Value.ProductInfo.ProductManifest.Format == VideoOS.Installer.Configuration.Package.FormatType.Microsoft)
					{
						productInstallInfo = DoProductCheckPrerequisite(prerequisite.Value.ProductInfo.DisplayName.Value, prerequisite.Value.ProductInfo.ProductManifest.ProductData, prerequisite.Value.ProductInfo.ProductManifest.Filename);
					}
					else if (prerequisite.Value.ProductInfo.ProductManifest.Format == VideoOS.Installer.Configuration.Package.FormatType.Milestone)
					{
						productInstallInfo = DoPackageCheckPrerequisite();
					}
				}
				if (productInstallInfo == null || (productInstallInfo.ErrorString == null && productInstallInfo.Command != null))
				{
					ProceedPrerequisite(prerequisite.Value.ProductInfo.DisplayName.Value, prerequisite.Value.Type.ToString(), productInstallInfo, prerequisite.Value.ProductInfo.InstallationFolder, prerequisite.Value.ProductInfo.ManufactureLink);
				}
				UpdateCurrentProgress(delta);
			}
			if (_prerequisiteInfo.InstallationTaskAdapter.Count != 0)
			{
				foreach (KeyValuePair<string, InstallerTaskPluginProperties> item in _prerequisiteInfo.InstallationTaskAdapter)
				{
					if (!(item.Value.InstallationTaskPlugin.InstallationTask is IInstallationAdapter installationAdapter) || !installationAdapter.CheckPrequisite(item.Value.Filename, item.Value.PackageParameters))
					{
						continue;
					}
					InstallerPackageType[] installerPackages = _installerConfig.InstallerPackages;
					foreach (InstallerPackageType installerPackageType in installerPackages)
					{
						if (installerPackageType.ID == item.Key)
						{
							installerPackageType.InstallationTaskPlugin.IsInstalled = true;
							break;
						}
					}
				}
				UpdateCurrentProgress(delta);
			}
		}
		base.CurrentProgress = base.MaximumProgress;
		NotifyObservers();
		base.MaximumProgress = 0;
		Status = TaskStatus.Completed;
		_result = TaskResult.Success;
	}

	protected void ProceedPrerequisite(string displayName, string type, ProductInstallInfo prodInfo, string installationFolder, string manufacturerLink)
	{
		if (type == "ByUser")
		{
			_prerequisiteInfo.PackagesCannotInstall[displayName] = manufacturerLink;
			LogManager.Write($"Prerequisite {displayName} is not installed. Type: {type}", TraceEventType.Information);
		}
		else if (prodInfo != null)
		{
			_prerequisiteInfo.PackagesToInstall[installationFolder] = prodInfo;
			LogManager.Write($"Prerequisite {prodInfo.DisplayName} is not installed. Type: {type}", TraceEventType.Information);
		}
		else
		{
			LogManager.Write("Prerequisite not found " + displayName, TraceEventType.Error);
		}
	}

	protected void UpdateCurrentProgress(int delta)
	{
		base.CurrentProgress += delta;
		if (base.CurrentProgress >= 95)
		{
			base.CurrentProgress = 100;
		}
		NotifyObservers();
	}

	protected ProductInstallInfo DoPackageCheckPrerequisite()
	{
		throw new NotImplementedException();
	}

	protected override void PostRun()
	{
		base.MaximumProgress = 0;
		Status = TaskStatus.NotStarted;
		_result = TaskResult.Success;
	}

	protected override void PreRun()
	{
		base.Message = string.Format(Resources.Message_CheckingPrerequisistes);
		base.MaximumProgress = 0;
	}

	protected ProductInstallInfo DoProductCheckPrerequisite(string displayName, Product product, string filename)
	{
		object[] installChecks = product.InstallChecks;
		ProductInstallInfo productInstallInfo = null;
		object[] array = installChecks;
		foreach (object obj in array)
		{
			if (obj is InstallChecksTypeAssemblyCheck)
			{
				InstallChecksTypeAssemblyCheck assemblyCheck = obj as InstallChecksTypeAssemblyCheck;
				productInstallInfo = CheckPrerequisite(assemblyCheck, displayName);
			}
			else if (obj is InstallChecksTypeExternalCheck)
			{
				InstallChecksTypeExternalCheck externalCheck = obj as InstallChecksTypeExternalCheck;
				productInstallInfo = CheckPrerequisite(externalCheck, filename, displayName);
			}
			else if (obj is InstallChecksTypeFileCheck)
			{
				InstallChecksTypeFileCheck fileCheck = obj as InstallChecksTypeFileCheck;
				productInstallInfo = CheckPrerequisite(fileCheck, product, displayName);
			}
			else if (obj is InstallChecksTypeMsiProductCheck)
			{
				InstallChecksTypeMsiProductCheck msiProductCheck = obj as InstallChecksTypeMsiProductCheck;
				productInstallInfo = CheckPrerequisite(msiProductCheck, product, displayName);
			}
			else if (obj is InstallChecksTypeRegistryCheck)
			{
				InstallChecksTypeRegistryCheck registryCheck = obj as InstallChecksTypeRegistryCheck;
				productInstallInfo = CheckPrerequisite(registryCheck, product, displayName);
				if (productInstallInfo == null || (productInstallInfo.ErrorString == null && productInstallInfo.Command != null))
				{
					return productInstallInfo;
				}
			}
			else if (obj is InstallChecksTypeRegistryFileCheck)
			{
				InstallChecksTypeRegistryFileCheck registryFileCheck = obj as InstallChecksTypeRegistryFileCheck;
				productInstallInfo = CheckPrerequisite(registryFileCheck);
			}
		}
		return productInstallInfo;
	}

	public ProductInstallInfo CheckPrerequisite(InstallChecksTypeAssemblyCheck assemblyCheck, string displayName)
	{
		if (assemblyCheck != null)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				string fullName = assemblies[i].FullName;
				if (fullName.Contains(assemblyCheck.Name) && fullName.Contains(assemblyCheck.PublicKeyToken) && fullName.Contains(assemblyCheck.Version))
				{
					return new ProductInstallInfo(displayName);
				}
			}
		}
		return null;
	}

	public ProductInstallInfo CheckPrerequisite(InstallChecksTypeExternalCheck externalCheck, string filename, string displayName)
	{
		if (externalCheck != null)
		{
			Process process = Process.Start(filename.Remove(filename.LastIndexOf("\\")) + "\\" + externalCheck.PackageFile);
			process.WaitForExit();
			if (process.ExitCode == 0)
			{
				return new ProductInstallInfo(displayName);
			}
		}
		return null;
	}

	public ProductInstallInfo CheckPrerequisite(InstallChecksTypeFileCheck fileCheck, Product product, string displayName)
	{
		if (fileCheck != null)
		{
			string text = Path.Combine(Environment.SystemDirectory, fileCheck.FileName);
			if (File.Exists(text))
			{
				string fileVersion = FileVersionInfo.GetVersionInfo(text).FileVersion;
				LogManager.Write($"File Check of {displayName} File='{text}' Version='{fileVersion}'");
				return new ProductInstallInfo(displayName, product, fileCheck.Property, fileVersion);
			}
			LogManager.Write($"File {text} doesn't exist");
		}
		return null;
	}

	public ProductInstallInfo CheckPrerequisite(InstallChecksTypeRegistryFileCheck registryFileCheck)
	{
		throw new NotImplementedException();
	}

	public ProductInstallInfo CheckPrerequisite(InstallChecksTypeMsiProductCheck msiProductCheck, Product product, string displayName)
	{
		if (msiProductCheck != null && product != null)
		{
			string[] obj = (msiProductCheck.Product.Contains(";") ? msiProductCheck.Product.Split(';') : new string[1] { msiProductCheck.Product });
			int num = -1;
			string[] array = obj;
			foreach (string product2 in array)
			{
				num = base.ClMsiHandler.MsiQueryProductState(product2);
				if (num != -1)
				{
					break;
				}
			}
			LogManager.Write("MsiProductCheck =" + num);
			return new ProductInstallInfo(displayName, product, msiProductCheck.Property, num.ToString());
		}
		return null;
	}

	public ProductInstallInfo CheckPrerequisite(InstallChecksTypeRegistryCheck registryCheck, Product product, string displayName)
	{
		if (registryCheck != null && product != null)
		{
			string key = registryCheck.Key.Replace("HKLM\\", "");
			RegistryKey registryKey = OpenRegistryKey(key, isWow6432Node: false);
			if (registryKey != null)
			{
				LogManager.Write("Checking " + registryCheck.Value, TraceEventType.Information);
				object value = registryKey.GetValue(registryCheck.Value);
				if (value != null)
				{
					return new ProductInstallInfo(displayName, product, registryCheck.Property, value.ToString());
				}
				registryKey = OpenRegistryKey(key, isWow6432Node: true);
				if (registryKey != null)
				{
					value = registryKey.GetValue(registryCheck.Value);
				}
				return new ProductInstallInfo(displayName, product, registryCheck.Property, value);
			}
			int num = registryCheck.Key.LastIndexOf("\\");
			if (int.TryParse(registryCheck.Key.Substring(num + 1, registryCheck.Key.Length - num - 1), out var _))
			{
				registryCheck.Key = registryCheck.Key.Substring(0, num);
				return CheckPrerequisite(registryCheck, product, displayName);
			}
			return new ProductInstallInfo(displayName, product, registryCheck.Property, null);
		}
		return null;
	}

	private RegistryKey OpenRegistryKey(string key, bool isWow6432Node)
	{
		RegistryKey result = null;
		if (isWow6432Node)
		{
			int index = 9;
			StringBuilder stringBuilder = new StringBuilder(key);
			stringBuilder.Insert(index, "Wow6432Node\\");
			key = stringBuilder.ToString();
		}
		try
		{
			result = Registry.LocalMachine.OpenSubKey(key);
		}
		catch (SecurityException ex)
		{
			LogManager.Write(ex.TypeAndMessage(), ex.StackTrace, TraceEventType.Error);
		}
		catch (Exception ex2)
		{
			LogManager.Write(ex2.TypeAndMessage(), ex2.StackTrace, TraceEventType.Error);
		}
		return result;
	}
}
