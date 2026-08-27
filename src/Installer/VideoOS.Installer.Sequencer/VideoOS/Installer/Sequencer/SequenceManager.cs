using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Properties;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Configuration.Package;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Interfaces.Msi;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;
using VideoOS.Installer.Sequencer.ErrorHandling;
using VideoOS.Installer.Sequencer.InstallerObserver;
using VideoOS.Installer.Sequencer.Prerequisite;
using VideoOS.Installer.Sequencer.Properties;

namespace VideoOS.Installer.Sequencer;

public sealed class SequenceManager : ISequenceManager
{
	private delegate void BeginInstallDelegate();

	private delegate void BeginUninstallDelegate();

	private delegate void BeginCancelDelegate();

	private Dictionary<string, ProductInstallInfo> _prerequisiteToInstall;

	private bool _isDebugMode;

	private const string Dashes = "----------------------------------";

	private readonly PlatformType _platform;

	private readonly InstallationParams _currentInstallation;

	private readonly InstallationParams _oldInstallation;

	private int _websitePort;

	private IClMsiHandler _clMsiHandler;

	private ITaskSequencer TaskSequencer;

	Dictionary<string, List<IPackageParameter>> ISequenceManager.PackageParameters
	{
		get
		{
			return _currentInstallation.PackageParameters;
		}
		set
		{
			_currentInstallation.PackageParameters = value;
		}
	}

	public List<string> InstalledPackages
	{
		get
		{
			if (TaskSequencer.TaskSuite != null)
			{
				List<string> installedPackages = ((InstallerTaskSuite)TaskSequencer.TaskSuite).InstalledPackages;
				List<string> list = new List<string>();
				{
					foreach (Task task in TaskSequencer.TaskSuite.Tasks)
					{
						if (installedPackages.Contains(task.Id.ToString()) && !(task is MSIUninstallerTask) && !(task is InstallShieldUninstallTask))
						{
							InstallerTask installerTask = (InstallerTask)task;
							list.Add(installerTask.Id);
						}
					}
					return list;
				}
			}
			return null;
		}
	}

	public List<string> UninstalledPackages
	{
		get
		{
			if (TaskSequencer.TaskSuite != null)
			{
				List<string> installedPackages = ((InstallerTaskSuite)TaskSequencer.TaskSuite).InstalledPackages;
				List<string> list = new List<string>();
				{
					foreach (Task task in TaskSequencer.TaskSuite.Tasks)
					{
						if (installedPackages.Contains(task.Id.ToString()) && task is MSIUninstallerTask)
						{
							InstallerTask installerTask = (InstallerTask)task;
							list.Add(installerTask.Id);
						}
					}
					return list;
				}
			}
			return null;
		}
	}

	public List<string> NotExecutedInstallPackages
	{
		get
		{
			if (TaskSequencer.TaskSuite != null)
			{
				List<string> unknownStatePackages = ((InstallerTaskSuite)TaskSequencer.TaskSuite).UnknownStatePackages;
				List<string> list = new List<string>();
				{
					foreach (Task task in TaskSequencer.TaskSuite.Tasks)
					{
						if (unknownStatePackages.Contains(task.Id.ToString()) && !(task is MSIUninstallerTask))
						{
							InstallerTask installerTask = (InstallerTask)task;
							list.Add(installerTask.Id);
						}
					}
					return list;
				}
			}
			return null;
		}
	}

	public List<string> NotExecutedUninstallPackages
	{
		get
		{
			if (TaskSequencer.TaskSuite != null)
			{
				List<string> unknownStatePackages = ((InstallerTaskSuite)TaskSequencer.TaskSuite).UnknownStatePackages;
				List<string> list = new List<string>();
				{
					foreach (Task task in TaskSequencer.TaskSuite.Tasks)
					{
						if (unknownStatePackages.Contains(task.Id.ToString()) && task is MSIUninstallerTask)
						{
							InstallerTask installerTask = (InstallerTask)task;
							list.Add(installerTask.Id);
						}
					}
					return list;
				}
			}
			return null;
		}
	}

	public Dictionary<string, string> FailedInstallPackages
	{
		get
		{
			List<string> list = new List<string>();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			list = ((InstallerTaskSuite)TaskSequencer.TaskSuite).FailedPackages;
			foreach (Task task in TaskSequencer.TaskSuite.Tasks)
			{
				if (list.Contains(task.Id.ToString()) && !(task is MSIUninstallerTask))
				{
					InstallerTask installerTask = (InstallerTask)task;
					if (installerTask.Id != "SSREGISTRATION")
					{
						dictionary[installerTask.Id] = installerTask.LastError.Message;
					}
				}
			}
			return dictionary;
		}
	}

	public Dictionary<string, string> FailedUninstallationPackages
	{
		get
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			_ = ((InstallerTaskSuite)TaskSequencer.TaskSuite).InstalledPackages;
			foreach (Task task in TaskSequencer.TaskSuite.Tasks)
			{
				if (task is MSIUninstallerTask)
				{
					MSIUninstallerTask mSIUninstallerTask = (MSIUninstallerTask)task;
					if (mSIUninstallerTask.UninstallFailed)
					{
						dictionary[mSIUninstallerTask.Id] = mSIUninstallerTask.LastError.Message;
					}
				}
			}
			return dictionary;
		}
	}

	public List<string> PackagesToUninstall { get; set; }

	public List<string> PackagesToInstall { get; set; }

	public bool NeedReboot => ((InstallerTaskSuite)TaskSequencer.TaskSuite).NeedsReboot;

	string ISequenceManager.WinTargetDir => _currentInstallation.InstallerSourceDir;

	public SequenceManager(IClMsiHandler clMsiHandler, InstallationParams currentInstallation, InstallationParams oldInstallation, ILogger log, PlatformType platform, int websitePort, ITaskSequencer taskSequencer, bool isDebugMode)
	{
		_currentInstallation = currentInstallation ?? throw new ArgumentNullException("currentInstallation");
		_oldInstallation = oldInstallation;
		if (log == null)
		{
			throw new ArgumentNullException("log");
		}
		if (taskSequencer == null)
		{
			throw new ArgumentNullException("taskSequencer");
		}
		if (clMsiHandler == null)
		{
			throw new ArgumentNullException("clMsiHandler");
		}
		TaskSequencer = taskSequencer;
		_isDebugMode = isDebugMode;
		_websitePort = websitePort;
		CommonSettings.Log = log;
		_platform = platform;
		_clMsiHandler = clMsiHandler;
		_clMsiHandler.EnableLog(Path.Combine(LogManager.GetMsiLogPath(), "msi"), _currentInstallation.InstallerConfiguration.InstallerSettings.EnableExtendedMSILogging);
		CultureManager.Instance.CultureChanged += CultureChanged;
	}

	public void SetPrerequisitesToInstall(Dictionary<string, ProductInstallInfo> doInstall, int websitePort)
	{
		_prerequisiteToInstall = doInstall;
		_websitePort = websitePort;
	}

	public void Install()
	{
		try
		{
			LogManager.Write("--------------------------------------------------------------------");
			LogManager.Write(string.Format("{0}  {1}  {2}", "----------------------------------", "Installing...", "----------------------------------"));
			InstallerTaskSuiteObserver observer = new InstallerTaskSuiteObserver(TaskSequencer);
			CommonSettings.IsInstallation = true;
			TaskSequencer.TaskSuite = new InstallerTaskSuite(TaskSeqencerDirection.Forward);
			TaskSequencer.TaskSuite.ClearAllTasks();
			Dictionary<string, InstallerPackageType> packagesByPlatform = GetPackagesByPlatform(_currentInstallation.InstallerConfiguration.InstallerPackages);
			PackageRefType[] installExecuteSequence = _currentInstallation.InstallerConfiguration.InstallExecuteSequence;
			foreach (PackageRefType packageRefType in installExecuteSequence)
			{
				if (packagesByPlatform.ContainsKey(packageRefType.ID))
				{
					string target = string.Format("{0}:{1}", "INSTALL", packageRefType.ID);
					string value = ((packageRefType.Condition != ConditionType.Skip) ? "yes" : "no");
					GlobalData.Settings.Add(new PackageParameter(target, value));
				}
			}
			VerifyVersionsOfPackages(packagesByPlatform, _currentInstallation.InstallerSourceDir);
			CreateTasksForPrerequisites(_currentInstallation.InstallerSourceDir);
			installExecuteSequence = _currentInstallation.InstallerConfiguration.UninstallerInfo.UninstallerSequencer;
			foreach (PackageRefType packageRefType2 in installExecuteSequence)
			{
				if (packagesByPlatform.ContainsKey(packageRefType2.ID) && packageRefType2.Condition != ConditionType.Skip && packageRefType2.Condition != ConditionType.Disabled && packageRefType2.Condition != ConditionType.Finalize)
				{
					InstallerPackageType installerPackage = packagesByPlatform[packageRefType2.ID];
					CreateUninstallTasks(installerPackage, _currentInstallation.PackageParameters, _currentInstallation.InstallerSourceDir);
				}
			}
			foreach (InstallerPackageType item in GetObsoletePackagesToUninstall(_currentInstallation.InstallerConfiguration, _oldInstallation?.InstallerConfiguration, _oldInstallation?.InstallInfo))
			{
				CreateUninstallTasks(item, _oldInstallation.PackageParameters, _oldInstallation.InstallerSourceDir);
			}
			bool valueAsBoolean = GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE");
			bool valueAsBoolean2 = GlobalData.GetValueAsBoolean("FINALIZE-MODE");
			List<InstallerPackageType> list = new List<InstallerPackageType>();
			foreach (KeyValuePair<string, InstallerPackageType> item2 in packagesByPlatform)
			{
				if (item2.Value.PostInstallActions != null && item2.Value.PostInstallActions.PerformSSRegistration)
				{
					list.Add(item2.Value);
				}
			}
			bool flag = false;
			installExecuteSequence = _currentInstallation.InstallerConfiguration.InstallExecuteSequence;
			foreach (PackageRefType packageRef in installExecuteSequence)
			{
				if (!packagesByPlatform.ContainsKey(packageRef.ID))
				{
					continue;
				}
				InstallerPackageType installerPackage2 = packagesByPlatform[packageRef.ID];
				bool runInHardwareManufacturerMode = false;
				bool runInFinalizeMode = true;
				GetSystemBuilderSettings(installerPackage2, ref runInHardwareManufacturerMode, ref runInFinalizeMode);
				if ((!(valueAsBoolean && runInHardwareManufacturerMode) || _currentInstallation.InstallInfo.InstalledPackages.Exists((PackageRefType f) => f.ID.ToUpperInvariant() == packageRef.ID.ToUpperInvariant())) && !(valueAsBoolean2 && runInFinalizeMode) && (valueAsBoolean || valueAsBoolean2))
				{
					continue;
				}
				if ((valueAsBoolean2 && packageRef.Condition == ConditionType.Finalize) || (packageRef.Condition != ConditionType.Skip && packageRef.Condition != ConditionType.Disabled))
				{
					if (valueAsBoolean && runInHardwareManufacturerMode && runInFinalizeMode)
					{
						packageRef.Condition = ConditionType.Finalize;
					}
					AddInstallTask(installerPackage2, _currentInstallation.InstallerSourceDir);
				}
				if (packageRef.RunOnlyPlugin)
				{
					AddInstallTask(installerPackage2, runOnlyPlugin: true, _currentInstallation.InstallerSourceDir);
				}
				if (packageRef.SSRegistration && list.Count != 0 && !flag)
				{
					CreateSSRegistrationTask(list);
					flag = true;
				}
			}
			if (list.Count != 0 && !flag && !valueAsBoolean)
			{
				CreateSSRegistrationTask(list);
			}
			TaskSequencer.TaskSuite.AttachObserver(observer);
		}
		catch (InvalidCastException innerException)
		{
			LogManager.Write("Package type not supported.", TraceEventType.Error);
			throw new InstallerSequencerException("Package type not supported.", innerException);
		}
		InstallerSequencerException ex = null;
		try
		{
			TaskSequencer.TaskSuite.Start();
			TaskSequencer.EnsureResults();
			if ((TaskSequencer.TaskSuite.Result == TaskResult.Error && TaskSequencer.TaskSuite.LastError is InvalidOperationException) || ((InstallerTaskSuite)TaskSequencer.TaskSuite).NeedsReboot || TaskSequencer.TaskSuite.LastError is InstallationAdapterException)
			{
				ex = new InstallerSequencerException(TaskSequencer.TaskSuite.LastError.Message, TaskSequencer.TaskSuite.LastError.InnerException);
			}
			else
			{
				string text = ExecutePostCommands();
				if (text != null)
				{
					ex = new InstallerSequencerException(text);
				}
			}
		}
		catch (InvalidOperationException ex2)
		{
			LogManager.Write("Failed to install package " + ex2.TypeAndMessage(), TraceEventType.Critical);
			ex = ((!string.IsNullOrEmpty(ex2.Message)) ? new InstallerSequencerException(ex2.Message) : new InstallerSequencerException("Failed to install package "));
		}
		catch (InstallerSequencerException ex3)
		{
			LogManager.Write("Failed to install package " + ex3.TypeAndMessage(), TraceEventType.Error);
			ex = (string.IsNullOrEmpty(ex3.Message) ? new InstallerSequencerException("Failed to install package ", ex3) : ex3);
		}
		catch (Exception ex4)
		{
			LogManager.Write("Failed to install package " + ex4.TypeAndMessage(), TraceEventType.Error);
			ex = (string.IsNullOrEmpty(ex4.Message) ? new InstallerSequencerException("Failed to install package ", ex4) : new InstallerSequencerException("Failed to install package " + ex4.Message));
		}
		if (ex != null)
		{
			ExecutePostCommands();
			throw ex;
		}
	}

	private IEnumerable<InstallerPackageType> GetObsoletePackagesToUninstall(InstallerConfig installerConfig, InstallerConfig oldInstallerConfig, InstallInfo oldInstallInfo)
	{
		if (oldInstallerConfig == null || oldInstallInfo?.InstalledPackages == null)
		{
			return new List<InstallerPackageType>();
		}
		IEnumerable<InstallerPackageType> installerPackages = from x in oldInstallInfo.InstalledPackages
			where !installerConfig.InstallerPackages.Any((InstallerPackageType y) => x.ID == y.ID)
			where x.AllowUnInstall
			select oldInstallerConfig.InstallerPackages.FirstOrDefault((InstallerPackageType y) => x.ID == y.ID) into y
			where y != null
			select y;
		return GetPackagesByPlatform(installerPackages).Values;
	}

	private static void GetSystemBuilderSettings(InstallerPackageType installerPackage, ref bool runInHardwareManufacturerMode, ref bool runInFinalizeMode)
	{
		if (installerPackage.InstallationTaskPlugin != null && installerPackage.InstallationTaskPlugin.InstallationTask != null && installerPackage.InstallationTaskPlugin.InstallationTask is IInstallationAdapter installationAdapter)
		{
			runInHardwareManufacturerMode = installationAdapter.RunInHardwareManufacturerMode;
			runInFinalizeMode = installationAdapter.RunInFinalizeMode;
		}
	}

	public Dictionary<string, InstallerPackageType> GetPackagesByPlatform(IEnumerable<InstallerPackageType> installerPackages)
	{
		Dictionary<string, InstallerPackageType> dictionary = new Dictionary<string, InstallerPackageType>();
		if (installerPackages != null)
		{
			foreach (InstallerPackageType installerPackage in installerPackages)
			{
				PlatformType platformType = ProcessorArchitectureHelper.ConvertToPlatform(installerPackage.ProductManifest.PackageData.ProductInfo.TargetPlatform);
				if (platformType == _platform || platformType == PlatformType.ANY || platformType == PlatformType.X86 || _platform == PlatformType.ANY)
				{
					dictionary[installerPackage.ID] = installerPackage;
				}
			}
		}
		return dictionary;
	}

	private void VerifyVersionsOfPackages(Dictionary<string, InstallerPackageType> packageMap, string installerSourceDir)
	{
		List<string> list = new List<string>();
		PackageRefType[] installExecuteSequence = _currentInstallation.InstallerConfiguration.InstallExecuteSequence;
		foreach (PackageRefType packageRefType in installExecuteSequence)
		{
			if (!packageMap.ContainsKey(packageRefType.ID) || packageRefType.Condition == ConditionType.Disabled || packageRefType.Condition == ConditionType.Skip || (packageMap[packageRefType.ID].PreInstallActions != null && packageMap[packageRefType.ID].PreInstallActions.AllowDowngrade) || packageMap[packageRefType.ID].PackageType == PackageTypeType.ConfigurationPackage)
			{
				continue;
			}
			string fileName = Path.Combine(installerSourceDir, packageMap[packageRefType.ID].ProductManifest.PackageData.PrimaryFile[0]);
			string previousInstalledProductVersion = _clMsiHandler.GetPreviousInstalledProductVersion(fileName);
			if (string.IsNullOrEmpty(previousInstalledProductVersion))
			{
				continue;
			}
			string productVersion = _clMsiHandler.GetProductVersion(fileName);
			if (string.IsNullOrEmpty(productVersion))
			{
				continue;
			}
			string text = null;
			try
			{
				Version version = new Version(GetMajorMinor(productVersion));
				Version version2 = new Version(GetMajorMinor(previousInstalledProductVersion));
				if (version < version2)
				{
					packageRefType.Condition = ConditionType.Skip;
					list.Add(packageRefType.ID);
					string text2 = string.Format(CultureInfo.InvariantCulture, "Package {0} {1} was not installed because a more recent version({2}) is already installed on the computer.", packageRefType.Name, version, version2);
					LogManager.Write(text2);
					if (packageMap[packageRefType.ID].ProductManifest.MainPackage)
					{
						text = text2;
					}
				}
			}
			catch (Exception)
			{
			}
			if (!string.IsNullOrEmpty(text))
			{
				throw new DowngradeDisallowedException(text);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		installExecuteSequence = _currentInstallation.InstallerConfiguration.UninstallerInfo.UninstallerSequencer;
		foreach (PackageRefType packageRefType2 in installExecuteSequence)
		{
			if (list.Contains(packageRefType2.ID))
			{
				packageRefType2.Condition = ConditionType.Skip;
			}
		}
	}

	private string GetMajorMinor(string versionMsi)
	{
		string result = "1.0";
		if (versionMsi.IndexOf(".") != -1)
		{
			string[] array = versionMsi.Split('.');
			if (array.Length > 1)
			{
				result = $"{array[0]}.{array[1]}";
			}
		}
		else
		{
			result = $"{versionMsi}.0";
		}
		return result;
	}

	private string ExecutePostCommands()
	{
		List<string> installedPackages = InstalledPackages;
		List<string> uninstalledPackages = UninstalledPackages;
		Dictionary<string, string> failedInstallPackages = FailedInstallPackages;
		InstallerPackageType[] installerPackages = _currentInstallation.InstallerConfiguration.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			if (installerPackageType.PostCommands != null && installerPackageType.PostCommands.Commands != null && installerPackageType.PostCommands.Commands.Length != 0)
			{
				CommandType[] commands = installerPackageType.PostCommands.Commands;
				foreach (CommandType commandType in commands)
				{
					if (installedPackages.Contains(installerPackageType.ID) && commandType.Condition == CommandCondition.AfterInstallation)
					{
						CreateProcess(commandType.Arguments, commandType.Executable);
					}
					if (!installedPackages.Contains(installerPackageType.ID) && uninstalledPackages.Contains(installerPackageType.ID) && commandType.Condition == CommandCondition.AfterUninstallation)
					{
						CreateProcess(commandType.Arguments, commandType.Executable);
					}
					if (failedInstallPackages.ContainsKey(installerPackageType.ID) && commandType.Condition == CommandCondition.OnError)
					{
						CreateProcess(commandType.Arguments, commandType.Executable);
					}
				}
			}
			if (installerPackageType.PostCommands == null || installerPackageType.PostCommands.ServiceActionsOn == null || installerPackageType.PostCommands.ServiceActionsOn.Length == 0)
			{
				continue;
			}
			ServiceActionHandler serviceActionHandler = new ServiceActionHandler();
			ServiceActionOnType[] serviceActionsOn = installerPackageType.PostCommands.ServiceActionsOn;
			foreach (ServiceActionOnType serviceActionOnType in serviceActionsOn)
			{
				if (!installedPackages.Contains(installerPackageType.ID) || (serviceActionOnType.OnParameters != null && !CheckOnParameters(serviceActionOnType.OnParameters, installerPackageType.ID)))
				{
					continue;
				}
				if (serviceActionOnType.OnPackage != null)
				{
					if (serviceActionOnType.OnPackage.InstallState == InstallStateType.Installed)
					{
						if (!installedPackages.Contains(serviceActionOnType.OnPackage.ID))
						{
							continue;
						}
						LogManager.Write($"Action for ServiceName '{serviceActionOnType.ServiceName}' will be performed because package '{serviceActionOnType.OnPackage.Name}' was installed.");
					}
					else if (serviceActionOnType.OnPackage.InstallState == InstallStateType.NotInstalled)
					{
						if (installedPackages.Contains(serviceActionOnType.OnPackage.ID))
						{
							continue;
						}
						LogManager.Write($"Action for ServiceName '{serviceActionOnType.ServiceName}' will be performed because package '{serviceActionOnType.OnPackage.Name}' was not installed.");
					}
				}
				if (serviceActionOnType.Condition == CommandCondition.AfterInstallation && serviceActionOnType.ExecuteOn == Timing.AfterAll)
				{
					serviceActionHandler.Perform(serviceActionOnType.ServiceName, serviceActionOnType.Action, serviceActionOnType.SetErrorMessage, serviceActionOnType.SearchOn, serviceActionOnType.Startuptype);
					if (serviceActionHandler.ErrorMessage != null)
					{
						return serviceActionHandler.ErrorMessage;
					}
				}
				if (!installedPackages.Contains(installerPackageType.ID) && uninstalledPackages.Contains(installerPackageType.ID) && serviceActionOnType.Condition == CommandCondition.AfterUninstallation)
				{
					serviceActionHandler.Perform(serviceActionOnType.ServiceName, serviceActionOnType.Action, serviceActionOnType.SetErrorMessage, serviceActionOnType.SearchOn, serviceActionOnType.Startuptype);
					if (serviceActionHandler.ErrorMessage != null)
					{
						return serviceActionHandler.ErrorMessage;
					}
				}
				if (failedInstallPackages.ContainsKey(installerPackageType.ID) && serviceActionOnType.Condition == CommandCondition.OnError)
				{
					serviceActionHandler.Perform(serviceActionOnType.ServiceName, serviceActionOnType.Action, setErrorMessage: false, serviceActionOnType.SearchOn, serviceActionOnType.Startuptype);
				}
			}
		}
		return null;
	}

	private void CreateProcess(string args, string exec)
	{
		try
		{
			LogManager.Write($"Executed command {exec} {args}");
			ProcessStartInfo processStartInfo = new ProcessStartInfo(exec, args);
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;
			Process process = new Process();
			process.StartInfo = processStartInfo;
			process.Start();
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
		}
	}

	private void CreateUninstallTasks(InstallerPackageType installerPackage, Dictionary<string, List<IPackageParameter>> packageParameters, string installerSourceDir)
	{
		if (installerPackage.PreInstallActions != null && installerPackage.PreInstallActions.Uninstall)
		{
			if (installerPackage.PreInstallActions.UninstallType == UninstallTypeType.MSI || installerPackage.PreInstallActions.UninstallType == UninstallTypeType.MSIandInstallShield)
			{
				StringBuilder stringBuilder = CheckIfNeedToUninstall(installerPackage, installerSourceDir);
				if (stringBuilder != null)
				{
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Found product code '{0}' for package '{1}' to uninstall ", stringBuilder, installerPackage.Name), TraceEventType.Information);
					AddUninstallTask(installerPackage, packageParameters, stringBuilder.ToString(), GetRelatedUninstallTaskDisplayName(stringBuilder, installerPackage.ProductManifest.PackageData), installerSourceDir);
				}
			}
			if (installerPackage.PreInstallActions.UninstallType == UninstallTypeType.InstallShield || installerPackage.PreInstallActions.UninstallType == UninstallTypeType.MSIandInstallShield)
			{
				InstallShieldUninstallTask task = new InstallShieldUninstallTask(installerPackage.ProductManifest.PackageData.ProductInfo.ProductCode, installerPackage.ProductManifest.PackageData.ProductInfo.DisplayName.Value, installerPackage.ID, silentSupported: false);
				TaskSequencer.TaskSuite.AddTask(task);
			}
		}
		if (installerPackage.PostUninstallActions != null && installerPackage.PostUninstallActions.UpgradeCode != null)
		{
			AddRelatedProductUninstallTask(installerPackage);
		}
	}

	public void AddImmediateUnistallActions(MSIUninstallerTask msiTask, InstallerPackageType installerPackage)
	{
		if (installerPackage.PostCommands != null && installerPackage.PostCommands.ServiceActionsOn != null)
		{
			ServiceActionOnType[] serviceActionsOn = installerPackage.PostCommands.ServiceActionsOn;
			foreach (ServiceActionOnType serviceActionOnType in serviceActionsOn)
			{
				bool flag = true;
				if (serviceActionOnType.OnParameters != null)
				{
					flag = CheckOnParameters(serviceActionOnType.OnParameters, installerPackage.ID);
				}
				if (flag && serviceActionOnType.ExecuteOn == Timing.Immediate)
				{
					if (serviceActionOnType.Condition == CommandCondition.AfterUninstallation)
					{
						msiTask.AddServiceAction(new ServiceActionHandler(serviceActionOnType.ServiceName, serviceActionOnType.Action, serviceActionOnType.SetErrorMessage, serviceActionOnType.SearchOn, isAfterAction: true, serviceActionOnType.Startuptype));
					}
					else if (serviceActionOnType.Condition == CommandCondition.BeforeUninstallation)
					{
						msiTask.AddServiceAction(new ServiceActionHandler(serviceActionOnType.ServiceName, serviceActionOnType.Action, serviceActionOnType.SetErrorMessage, serviceActionOnType.SearchOn, isAfterAction: false, serviceActionOnType.Startuptype));
					}
				}
			}
		}
		if (installerPackage.ReferenceCounts != null)
		{
			RegistryCountType[] referenceCounts = installerPackage.ReferenceCounts;
			foreach (RegistryCountType registryCountType in referenceCounts)
			{
				if (!string.IsNullOrEmpty(registryCountType.KeyName) && !string.IsNullOrEmpty(registryCountType.Value))
				{
					msiTask.AddRegistryCount(registryCountType.KeyName, registryCountType.Value, registryCountType.OnlyRegistryActions);
				}
			}
		}
		if (installerPackage.ProductManifest != null && installerPackage.ProductManifest.PackageData.Install != null && installerPackage.ProductManifest.PackageData.Install.ServiceDefinition != null)
		{
			ServiceDefinitionType[] serviceDefinition = installerPackage.ProductManifest.PackageData.Install.ServiceDefinition;
			foreach (ServiceDefinitionType serviceDefinitionType in serviceDefinition)
			{
				if (IsParameterMatching(installerPackage.ID, serviceDefinitionType.OnParameter) && !string.IsNullOrEmpty(serviceDefinitionType.ServiceName) && !string.IsNullOrEmpty(serviceDefinitionType.ServiceType))
				{
					msiTask.AddRegistryAction(VideoOS.Installer.Common.Properties.Settings.Default.InstallersOverviewRegEntry, serviceDefinitionType.ServiceType, serviceDefinitionType.ServiceName, allowDeleteByValue: true);
				}
			}
		}
		if (installerPackage.PostUninstallActions != null)
		{
			msiTask.PendingFileNameOperations = installerPackage.PostUninstallActions.PendingFileRenameOperations;
		}
	}

	private bool IsParameterMatching(string packageId, ServiceDefinitionTypeOnParameter serviceDefinitionTypeOnParameter)
	{
		if (serviceDefinitionTypeOnParameter == null || string.IsNullOrEmpty(serviceDefinitionTypeOnParameter.Name))
		{
			return true;
		}
		if (_currentInstallation.PackageParameters.ContainsKey(packageId) && _currentInstallation.PackageParameters[packageId].Any((IPackageParameter p) => p.Target.Equals(serviceDefinitionTypeOnParameter.Name, StringComparison.OrdinalIgnoreCase) && (string.IsNullOrEmpty(serviceDefinitionTypeOnParameter.Value) || p.Value.Equals(serviceDefinitionTypeOnParameter.Value, StringComparison.OrdinalIgnoreCase))))
		{
			return true;
		}
		return false;
	}

	private void CreateTasksForPrerequisites(string installerSourceDir)
	{
		if (_prerequisiteToInstall == null)
		{
			return;
		}
		foreach (KeyValuePair<string, ProductInstallInfo> item in _prerequisiteToInstall)
		{
			ProductInstallInfo value = item.Value;
			if (value != null)
			{
				string key = item.Key;
				LoggerExtensions.LogInformation(CommonSettings.Log, string.Format(CultureInfo.InvariantCulture, "Adding prerequisite to be installed '{0}' with product code '{1}'...", value.DisplayName, value.ProductCode), Array.Empty<object>());
				if (value.ProductCode.ToLower() == "iis")
				{
					TaskSequencer.TaskSuite.AddTask(new IisPrerequisiteInstallerTask(installerSourceDir, item.Value, _websitePort));
				}
				else
				{
					TaskSequencer.TaskSuite.AddTask(new PrerequisiteInstallerTask(Path.Combine(installerSourceDir, key), value));
				}
			}
		}
	}

	private string GetRelatedUninstallTaskDisplayName(StringBuilder productCode, PackageType packageData)
	{
		string text;
		try
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(Path.Combine(VideoOS.Installer.Common.Properties.Settings.Default.SoftwareKey, productCode.ToString()));
			if (registryKey != null)
			{
				text = registryKey.GetValue("DisplayName").ToString();
				LogManager.Write($"Found name '{text}' in registry for prodCode '{productCode}'", TraceEventType.Information);
			}
			else
			{
				registryKey = Registry.LocalMachine.OpenSubKey(Path.Combine("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall", productCode.ToString()));
				if (registryKey != null)
				{
					text = registryKey.GetValue("DisplayName").ToString();
					LogManager.Write($"Found name '{text}' in registry for prodCode '{productCode}'", TraceEventType.Information);
				}
				else
				{
					text = packageData.ProductInfo.DisplayName.Value;
					LogManager.Write("Using name from package " + text);
				}
			}
		}
		catch (Exception exception)
		{
			text = packageData.IdentificationName;
			LogManager.Write(exception);
		}
		return text;
	}

	private static string BuildDisplayNameFromPackageData(PackageType packageData)
	{
		if (string.IsNullOrWhiteSpace(packageData.ProductInfo.Manufacture))
		{
			return packageData.DisplayName;
		}
		return string.Format(CultureInfo.InvariantCulture, "{0} {1}", packageData.ProductInfo.Manufacture, packageData.DisplayName);
	}

	private void AddInstallTask(InstallerPackageType installerPackage, string installerSourceDir)
	{
		AddInstallTask(installerPackage, runOnlyPlugin: false, installerSourceDir);
	}

	private void AddInstallTask(InstallerPackageType installerPackage, bool runOnlyPlugin, string installerSourceDir)
	{
		if (installerPackage.InstallationTaskPlugin != null && installerPackage.InstallationTaskPlugin.IsInstalled)
		{
			return;
		}
		LoggerExtensions.LogInformation(CommonSettings.Log, string.Format(CultureInfo.InvariantCulture, "Adding package to be installed '{0}' with package id '{1}'...", installerPackage.Name, installerPackage.ID), Array.Empty<object>());
		if (installerPackage.InstallationTaskPlugin != null && installerPackage.InstallationTaskPlugin.InstallationTask != null)
		{
			IInstallationAdapter installationAdapter = installerPackage.InstallationTaskPlugin.InstallationTask as IInstallationAdapter;
			List<IPackageParameter> installParameters = _currentInstallation.PackageParameters[installerPackage.ID];
			installationAdapter.DisplayName = BuildDisplayNameFromPackageData(installerPackage.ProductManifest.PackageData);
			InstallerTask installerTask = installationAdapter as InstallerTask;
			installerTask.Id = installerPackage.ID;
			installerTask.TargetDir = installerSourceDir;
			installerTask.RunOnlyPlugin = runOnlyPlugin;
			if (typeof(MSIInstallerTask).IsAssignableFrom(installationAdapter.GetType()))
			{
				MSIInstallerTask mSIInstallerTask = installationAdapter as MSIInstallerTask;
				mSIInstallerTask.ProductCode = installerPackage.ProductManifest.PackageData.ProductInfo.ProductCode;
				mSIInstallerTask.Id = installerPackage.ID;
				mSIInstallerTask.FileName = installerPackage.ProductManifest.PackageData.PrimaryFile[0];
				mSIInstallerTask.RunOnlyPlugin = runOnlyPlugin;
				mSIInstallerTask.ClMsiHandler = _clMsiHandler;
				AddRebootActionForTest(mSIInstallerTask, installerPackage);
				AddImmediateInstallPostActions(mSIInstallerTask, installerPackage);
			}
			installationAdapter.Install(Path.Combine(installerSourceDir, installerPackage.ProductManifest.PackageData.PrimaryFile[0]), installParameters);
			TaskSequencer.TaskSuite.AddTask(installerTask);
		}
		else if (installerPackage.ProductManifest.Format == VideoOS.Installer.Configuration.FormatType.Milestone)
		{
			MSIInstallerTask mSIInstallerTask2 = new MSIInstallerTask(_clMsiHandler, installerSourceDir, _currentInstallation.PackageParameters[installerPackage.ID]);
			mSIInstallerTask2.ProductCode = installerPackage.ProductManifest.PackageData.ProductInfo.ProductCode;
			mSIInstallerTask2.DisplayName = BuildDisplayNameFromPackageData(installerPackage.ProductManifest.PackageData);
			mSIInstallerTask2.Id = installerPackage.ID;
			mSIInstallerTask2.FileName = installerPackage.ProductManifest.PackageData.PrimaryFile[0];
			mSIInstallerTask2.ClMsiHandler = _clMsiHandler;
			AddRebootActionForTest(mSIInstallerTask2, installerPackage);
			AddImmediateInstallPostActions(mSIInstallerTask2, installerPackage);
			TaskSequencer.TaskSuite.AddTask(mSIInstallerTask2);
		}
		else
		{
			LoggerExtensions.LogInformation(CommonSettings.Log, string.Format(CultureInfo.InvariantCulture, Resources.Sequencer_UnknownFormat, installerPackage.Name), Array.Empty<object>());
		}
	}

	private static void AddRebootActionForTest(MSIInstallerTask msiTask, InstallerPackageType installerPackage)
	{
		if (GlobalData.TryGetValue("REBOOTONPACKAGEID", out var value) && !string.IsNullOrEmpty(value) && value.ToUpperInvariant() == installerPackage.ID.ToUpperInvariant())
		{
			msiTask.RebootAfterInstall = true;
		}
	}

	private void AddImmediateInstallPostActions(MSIInstallerTask msiTask, InstallerPackageType installerPackage)
	{
		if (installerPackage.PostCommands != null && installerPackage.PostCommands.ServiceActionsOn != null)
		{
			ServiceActionOnType[] serviceActionsOn = installerPackage.PostCommands.ServiceActionsOn;
			foreach (ServiceActionOnType serviceActionOnType in serviceActionsOn)
			{
				bool flag = true;
				if (serviceActionOnType.OnParameters != null)
				{
					flag = CheckOnParameters(serviceActionOnType.OnParameters, installerPackage.ID);
				}
				if (flag && serviceActionOnType.ExecuteOn == Timing.Immediate)
				{
					if (serviceActionOnType.Condition == CommandCondition.AfterInstallation)
					{
						msiTask.AddServiceAction(new ServiceActionHandler(serviceActionOnType.ServiceName, serviceActionOnType.Action, serviceActionOnType.SetErrorMessage, serviceActionOnType.SearchOn, isAfterAction: true, serviceActionOnType.Startuptype));
					}
					else if (serviceActionOnType.Condition == CommandCondition.BeforeInstallation)
					{
						msiTask.AddServiceAction(new ServiceActionHandler(serviceActionOnType.ServiceName, serviceActionOnType.Action, serviceActionOnType.SetErrorMessage, serviceActionOnType.SearchOn, isAfterAction: false, serviceActionOnType.Startuptype));
					}
				}
			}
		}
		if (installerPackage.ReferenceCounts != null)
		{
			RegistryCountType[] referenceCounts = installerPackage.ReferenceCounts;
			foreach (RegistryCountType registryCountType in referenceCounts)
			{
				if (!string.IsNullOrEmpty(registryCountType.KeyName) && !string.IsNullOrEmpty(registryCountType.Value))
				{
					msiTask.AddRegistryCount(registryCountType.KeyName, registryCountType.Value, registryCountType.OnlyRegistryActions);
				}
			}
		}
		if (installerPackage.ProductManifest != null && installerPackage.ProductManifest.PackageData.Install != null && installerPackage.ProductManifest.PackageData.Install.ServiceDefinition != null)
		{
			ServiceDefinitionType[] serviceDefinition = installerPackage.ProductManifest.PackageData.Install.ServiceDefinition;
			foreach (ServiceDefinitionType serviceDefinitionType in serviceDefinition)
			{
				if (IsParameterMatching(installerPackage.ID, serviceDefinitionType.OnParameter) && !string.IsNullOrEmpty(serviceDefinitionType.ServiceName) && !string.IsNullOrEmpty(serviceDefinitionType.ServiceType))
				{
					msiTask.AddRegistryAction(VideoOS.Installer.Common.Properties.Settings.Default.InstallersOverviewRegEntry, serviceDefinitionType.ServiceType, serviceDefinitionType.ServiceName, allowDeleteByValue: true);
				}
			}
		}
		if (installerPackage.PostInstallActions != null)
		{
			msiTask.PendingFileNameOperations = installerPackage.PostInstallActions.PendingFileRenameOperations;
		}
	}

	private bool CheckOnParameters(IEnumerable<ServiceActionOnTypeOnParameter> serviceActionOnTypeOnParameter, string id)
	{
		return serviceActionOnTypeOnParameter.All((ServiceActionOnTypeOnParameter p) => CheckOnParameter(p, id));
	}

	private bool CheckOnParameter(ServiceActionOnTypeOnParameter serviceActionOnTypeOnParameter, string id)
	{
		if (!string.IsNullOrEmpty(serviceActionOnTypeOnParameter.Name) && serviceActionOnTypeOnParameter.Value != null)
		{
			foreach (IPackageParameter item in _currentInstallation.PackageParameters[id])
			{
				if (string.Equals(item.Target, serviceActionOnTypeOnParameter.Name, StringComparison.OrdinalIgnoreCase))
				{
					return string.Equals(item.Value, serviceActionOnTypeOnParameter.Value, StringComparison.OrdinalIgnoreCase);
				}
			}
			return string.IsNullOrEmpty(serviceActionOnTypeOnParameter.Value);
		}
		return true;
	}

	private void CreateSSRegistrationTask(List<InstallerPackageType> installerPackages)
	{
		SSRegistrationTask task = new SSRegistrationTask(_clMsiHandler, installerPackages);
		TaskSequencer.TaskSuite.AddTask(task);
	}

	public void ServerSideRegistration(string exeLocation, string parentProcessName)
	{
		LogManager.Write("--------------------------------------------------------------------");
		LogManager.Write(string.Format("{0}  {1}  {2}", "----------------------------------", "Registering products...", "----------------------------------"));
		new InstallerTaskSuiteObserver(TaskSequencer);
		CommonSettings.IsInstallation = false;
		TaskSequencer.TaskSuite = new InstallerTaskSuite(TaskSeqencerDirection.Forward);
		TaskSequencer.TaskSuite.ClearAllTasks();
		InstallerPackageType[] installerPackages = _currentInstallation.InstallerConfiguration.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			if (installerPackageType.ProductManifest.MainPackage)
			{
				TaskSequencer.TaskSuite.AddTask(new SSRegistrationTask(_clMsiHandler, new List<InstallerPackageType> { installerPackageType }, exeLocation, parentProcessName, selfRegistration: true));
				break;
			}
		}
		try
		{
			TaskSequencer.TaskSuite.Start();
			if (TaskSequencer.TaskSuite.Result == TaskResult.Error)
			{
				throw TaskSequencer.TaskSuite.LastError;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private StringBuilder CheckIfNeedToUninstall(InstallerPackageType installerPackage, string targetDir)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		StringBuilder pProductId = new StringBuilder(1024);
		string pFileName = Path.Combine(targetDir, installerPackage.ProductManifest.PackageData.PrimaryFile[0]);
		_clMsiHandler.FindProductAndUpgradeCode(pFileName, pProductId, 1024, stringBuilder, 1024);
		return _clMsiHandler.EnumRelatedProducts(stringBuilder, 0u);
	}

	public void Modify()
	{
		try
		{
			InstallerTaskSuiteObserver observer = new InstallerTaskSuiteObserver(TaskSequencer);
			CommonSettings.IsInstallation = false;
			TaskSequencer.TaskSuite = new InstallerTaskSuite(TaskSeqencerDirection.Forward);
			TaskSequencer.TaskSuite.ClearAllTasks();
			LoggerExtensions.LogInformation(CommonSettings.Log, "Uninstalling package...", Array.Empty<object>());
			Dictionary<string, InstallerPackageType> dictionary = new Dictionary<string, InstallerPackageType>();
			InstallerPackageType[] installerPackages = _currentInstallation.InstallerConfiguration.InstallerPackages;
			foreach (InstallerPackageType installerPackageType in installerPackages)
			{
				dictionary[installerPackageType.ID] = installerPackageType;
			}
			ByPassPackages(dictionary);
			CreateTasksForPrerequisites(_currentInstallation.InstallerSourceDir);
			if (PackagesToUninstall != null && PackagesToUninstall.Count != 0)
			{
				PackageRefType[] uninstallerSequencer = _currentInstallation.InstallerConfiguration.UninstallerInfo.UninstallerSequencer;
				foreach (PackageRefType packageRefType in uninstallerSequencer)
				{
					if (PackagesToUninstall.Contains(packageRefType.ID) && dictionary.ContainsKey(packageRefType.ID) && packageRefType.AllowUnInstall)
					{
						InstallerPackageType installerPackageType2 = dictionary[packageRefType.ID];
						if (installerPackageType2 != null)
						{
							LogManager.Write($"Adding package to be uninstalled '{installerPackageType2.Name}' with package id '{installerPackageType2.ID}'...");
							CreateUninstallTasks(installerPackageType2, _currentInstallation.PackageParameters, _currentInstallation.InstallerSourceDir);
						}
					}
				}
			}
			if (PackagesToInstall != null && PackagesToInstall.Count != 0)
			{
				PackageRefType[] uninstallerSequencer = _currentInstallation.InstallerConfiguration.InstallExecuteSequence;
				foreach (PackageRefType packageRefType2 in uninstallerSequencer)
				{
					if (dictionary.ContainsKey(packageRefType2.ID) && PackagesToInstall.Contains(packageRefType2.ID))
					{
						InstallerPackageType installerPackageType3 = dictionary[packageRefType2.ID];
						LogManager.Write($"Adding package to be uninstalled '{installerPackageType3.Name}' with package id '{installerPackageType3.ID}'...");
						CreateUninstallTasks(installerPackageType3, _currentInstallation.PackageParameters, _currentInstallation.InstallerSourceDir);
						if (installerPackageType3 != null)
						{
							LogManager.Write($"Adding package to be installed '{installerPackageType3.Name}' with package id '{installerPackageType3.ID}'...");
							AddInstallTask(installerPackageType3, _currentInstallation.InstallerSourceDir);
						}
					}
				}
			}
			TaskSequencer.TaskSuite.AttachObserver(observer);
		}
		catch (InvalidCastException innerException)
		{
			LogManager.Write("Package type not supported.", TraceEventType.Error);
			throw new InstallerSequencerException("Package type not supported.", innerException);
		}
		try
		{
			TaskSequencer.TaskSuite.Start();
			if (TaskSequencer.TaskSuite.Result == TaskResult.Error)
			{
				throw TaskSequencer.TaskSuite.LastError;
			}
		}
		catch (Exception ex)
		{
			LogManager.Write("Failed to install package " + ex.TypeAndMessage(), TraceEventType.Error);
			if (!string.IsNullOrEmpty(ex.Message))
			{
				throw new InstallerSequencerException("Failed to install package " + ex.Message);
			}
			throw new InstallerSequencerException("Failed to install package ", ex);
		}
	}

	public void Uninstall()
	{
		try
		{
			InstallerTaskSuiteObserver observer = new InstallerTaskSuiteObserver(TaskSequencer);
			CommonSettings.IsInstallation = false;
			TaskSequencer.TaskSuite = new InstallerTaskSuite(TaskSeqencerDirection.Forward);
			TaskSequencer.TaskSuite.ClearAllTasks();
			LoggerExtensions.LogInformation(CommonSettings.Log, "Uninstalling package...", Array.Empty<object>());
			Dictionary<string, InstallerPackageType> dictionary = new Dictionary<string, InstallerPackageType>();
			InstallerPackageType[] installerPackages = _currentInstallation.InstallerConfiguration.InstallerPackages;
			foreach (InstallerPackageType installerPackageType in installerPackages)
			{
				dictionary[installerPackageType.ID] = installerPackageType;
			}
			ByPassPackages(dictionary);
			PackageRefType[] uninstallerSequencer = _currentInstallation.InstallerConfiguration.UninstallerInfo.UninstallerSequencer;
			foreach (PackageRefType packageRefType in uninstallerSequencer)
			{
				if (PackagesToUninstall.Contains(packageRefType.ID) && dictionary.ContainsKey(packageRefType.ID) && packageRefType.AllowUnInstall)
				{
					InstallerPackageType installerPackageType2 = dictionary[packageRefType.ID];
					if (installerPackageType2 != null)
					{
						CreateUninstallTasks(installerPackageType2, _currentInstallation.PackageParameters, _currentInstallation.InstallerSourceDir);
					}
				}
			}
			TaskSequencer.TaskSuite.AttachObserver(observer);
		}
		catch (InvalidCastException innerException)
		{
			LogManager.Write("Package type not supported.", TraceEventType.Error);
			throw new InstallerSequencerException("Package type not supported.", innerException);
		}
		InstallerSequencerException ex = null;
		try
		{
			TaskSequencer.TaskSuite.Start();
			if ((TaskSequencer.TaskSuite.Result == TaskResult.Error && TaskSequencer.TaskSuite.LastError is InvalidOperationException) || TaskSequencer.TaskSuite.LastError is MSIRunException || TaskSequencer.TaskSuite.LastError is InstallationAdapterException)
			{
				ex = new InstallerSequencerException(TaskSequencer.TaskSuite.LastError.Message, TaskSequencer.TaskSuite.LastError.InnerException);
			}
			else
			{
				string text = ExecutePostCommands();
				if (text != null)
				{
					ex = new InstallerSequencerException(text);
				}
			}
		}
		catch (Exception ex2)
		{
			LogManager.Write("Failed to install package " + ex2.TypeAndMessage(), TraceEventType.Error);
			ex = new InstallerSequencerException("Failed to install package ", ex2);
		}
		if (ex != null)
		{
			ExecutePostCommands();
			throw ex;
		}
	}

	private void ByPassPackages(Dictionary<string, InstallerPackageType> packageMap)
	{
		PackageRefType[] uninstallerSequencer = _currentInstallation.InstallerConfiguration.UninstallerInfo.UninstallerSequencer;
		foreach (PackageRefType packageRefType in uninstallerSequencer)
		{
			if (!packageRefType.AllowUnInstall || (packageRefType.Condition == ConditionType.Skip && packageRefType.Condition == ConditionType.Disabled) || !packageMap.ContainsKey(packageRefType.ID))
			{
				continue;
			}
			InstallerPackageType installerPackageType = packageMap[packageRefType.ID];
			if (installerPackageType.UninstallConditions == null || installerPackageType.UninstallConditions.ByPassIf == null)
			{
				continue;
			}
			UninstallConditionTypeByPassIf[] byPassIf = installerPackageType.UninstallConditions.ByPassIf;
			foreach (UninstallConditionTypeByPassIf uninstallConditionTypeByPassIf in byPassIf)
			{
				if (DoByPassUninstall(installerPackageType.ID, uninstallConditionTypeByPassIf))
				{
					packageRefType.Condition = ConditionType.Skip;
					packageRefType.AllowUnInstall = false;
					LogManager.Write($"Uninstalling of {installerPackageType.Name} was bypassed because {uninstallConditionTypeByPassIf.Name} {uninstallConditionTypeByPassIf.Condition.ToString()}", TraceEventType.Information);
				}
			}
		}
	}

	private void AddUninstallTask(InstallerPackageType uninstallPackage, Dictionary<string, List<IPackageParameter>> packageParameters, string productCode, string displayName, string installerSourceDir)
	{
		if (displayName == null)
		{
			displayName = BuildDisplayNameFromPackageData(uninstallPackage.ProductManifest.PackageData);
		}
		if (uninstallPackage.InstallationTaskPlugin != null)
		{
			if (productCode == null)
			{
				productCode = uninstallPackage.ProductManifest.PackageData.ProductInfo.ProductCode;
			}
			if (uninstallPackage.InstallationTaskPlugin.UninstallationTask is IInstallationAdapter installationAdapter)
			{
				List<IPackageParameter> list = packageParameters[uninstallPackage.ID];
				installationAdapter.DisplayName = displayName;
				if (installationAdapter is InstallerTask installerTask)
				{
					installerTask.Id = uninstallPackage.ID;
					installerTask.TargetDir = installerSourceDir;
					if (installationAdapter is MSIUninstallerTask mSIUninstallerTask)
					{
						mSIUninstallerTask.ProductCode = productCode;
						mSIUninstallerTask.Id = uninstallPackage.ID;
						mSIUninstallerTask.FileName = uninstallPackage.ProductManifest.PackageData.PrimaryFile[0];
						mSIUninstallerTask.Parameters = list;
						AddImmediateUnistallActions(mSIUninstallerTask, uninstallPackage);
					}
					installationAdapter.Uninstall(Path.Combine(installerSourceDir, uninstallPackage.ProductManifest.PackageData.PrimaryFile[0]), list);
					TaskSequencer.TaskSuite.AddTask(installerTask);
				}
			}
			else
			{
				MSIUninstallerTask mSIUninstallerTask2 = new MSIUninstallerTask(_clMsiHandler, packageParameters[uninstallPackage.ID]);
				AddImmediateUnistallActions(mSIUninstallerTask2, uninstallPackage);
				AddTask(displayName, productCode, uninstallPackage.ID, uninstallPackage.ProductManifest.PackageData.PrimaryFile[0], mSIUninstallerTask2);
			}
		}
		else
		{
			MSIUninstallerTask mSIUninstallerTask3 = new MSIUninstallerTask(_clMsiHandler, packageParameters[uninstallPackage.ID]);
			AddImmediateUnistallActions(mSIUninstallerTask3, uninstallPackage);
			AddTask(displayName, productCode, uninstallPackage.ID, uninstallPackage.ProductManifest.PackageData.PrimaryFile[0], mSIUninstallerTask3);
		}
	}

	public bool DoByPassUninstall(string id, UninstallConditionTypeByPassIf bypass)
	{
		switch (bypass.Condition)
		{
		case ByPassConditionType.ParameterNotSet:
			return !PackParamConstains(id, bypass);
		case ByPassConditionType.ParameterSet:
			return PackParamConstains(id, bypass);
		case ByPassConditionType.PackageNotInstalled:
		case ByPassConditionType.PackageInstalled:
			throw new NotImplementedException();
		default:
			return true;
		}
	}

	private bool PackParamConstains(string id, UninstallConditionTypeByPassIf bypass)
	{
		foreach (IPackageParameter item in _currentInstallation.PackageParameters[id])
		{
			if (item.Target == bypass.Name)
			{
				if (!string.IsNullOrEmpty(bypass.ConditionValue))
				{
					return item.Value == bypass.ConditionValue;
				}
				return true;
			}
		}
		return false;
	}

	private void AddRelatedProductUninstallTask(InstallerPackageType uninstallPackage)
	{
		StringBuilder stringBuilder = new StringBuilder(uninstallPackage.PostUninstallActions.UpgradeCode);
		StringBuilder stringBuilder2 = _clMsiHandler.EnumRelatedProducts(stringBuilder, 0u);
		if (stringBuilder2 != null && stringBuilder2.Length != 0)
		{
			LogManager.Write($"Found existing product code '{stringBuilder2}' for upgrade code '{stringBuilder}' ({uninstallPackage.Name}) ");
			if (string.IsNullOrEmpty(uninstallPackage.PostUninstallActions.ProductCode))
			{
				string productInfo = _clMsiHandler.GetProductInfo(stringBuilder2.ToString(), "InstallSource");
				if (!string.IsNullOrEmpty(productInfo))
				{
					LogManager.Write($"InstallSource was found {productInfo}");
					string name = Path.Combine(GlobalData.PublisherKey, "Installation");
					try
					{
						using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name);
						if (registryKey != null)
						{
							string[] subKeyNames = registryKey.GetSubKeyNames();
							foreach (string text in subKeyNames)
							{
								RegistryKey registryKey2 = registryKey.OpenSubKey(text);
								if (registryKey2 == null)
								{
									continue;
								}
								object value = registryKey2.GetValue("InstallLocation");
								if (value != null)
								{
									string text2 = (string)value;
									if (!string.IsNullOrEmpty(text2) && string.Compare(Path.GetFullPath(productInfo).TrimEnd('\\'), Path.GetFullPath(text2).TrimEnd('\\'), StringComparison.InvariantCultureIgnoreCase) == 0)
									{
										uninstallPackage.PostUninstallActions.ProductCode = text;
										LogManager.Write($"Product code for post uninstall actions was set to '{text}' ({uninstallPackage.Name})");
										break;
									}
								}
							}
						}
					}
					catch (Exception exception)
					{
						LogManager.Write(exception);
					}
				}
			}
			else
			{
				LogManager.Write("Product code is already specified = " + uninstallPackage.PostUninstallActions.ProductCode);
			}
			MSIUninstallerTask mSIUninstallerTask = new MSIUninstallerTask(_clMsiHandler, _currentInstallation.PackageParameters[uninstallPackage.ID]);
			AddImmediateUnistallActions(mSIUninstallerTask, uninstallPackage);
			AddTask(GetRelatedUninstallTaskDisplayName(stringBuilder2, uninstallPackage.ProductManifest.PackageData), stringBuilder2.ToString(), uninstallPackage.ID, uninstallPackage.ProductManifest.PackageData.PrimaryFile[0], mSIUninstallerTask);
		}
		else
		{
			LogManager.Write($"Product code for '{uninstallPackage.PostUninstallActions.UpgradeCode}' was empty ({uninstallPackage.Name})");
		}
	}

	private void AddTask(string displayName, string productCode, string id, string filename, MSIInstallerTask task)
	{
		task.DisplayName = displayName;
		task.ProductCode = productCode;
		task.Id = id;
		task.FileName = filename;
		LogManager.Write($"displayName= '{displayName}' productCode= '{((productCode != null) ? productCode : string.Empty)}', id= '{id}' filename='{filename}'");
		TaskSequencer.TaskSuite.AddTask(task);
	}

	public IAsyncResult BeginInstall(AsyncCallback callback, object asyncState)
	{
		BeginInstallDelegate beginInstallDelegate = Install;
		return beginInstallDelegate.BeginInvoke(callback, beginInstallDelegate);
	}

	public void EndInstall(IAsyncResult asyncResult)
	{
		((BeginInstallDelegate)asyncResult.AsyncState).EndInvoke(asyncResult);
	}

	public IAsyncResult BeginCancel(AsyncCallback callback, object asyncState)
	{
		BeginCancelDelegate beginCancelDelegate = Cancel;
		return beginCancelDelegate.BeginInvoke(callback, beginCancelDelegate);
	}

	public void EndCancel(IAsyncResult asyncResult)
	{
		((BeginCancelDelegate)asyncResult.AsyncState).EndInvoke(asyncResult);
	}

	public void Rollback()
	{
		throw new NotImplementedException();
	}

	public IAsyncResult BeginRollback(AsyncCallback callback, object asyncState)
	{
		throw new NotImplementedException();
	}

	public IAsyncResult BeginUninstall(AsyncCallback callback, object asyncState)
	{
		BeginUninstallDelegate beginUninstallDelegate = Uninstall;
		return beginUninstallDelegate.BeginInvoke(callback, beginUninstallDelegate);
	}

	public IAsyncResult BeginModify(AsyncCallback callback, object asyncState)
	{
		BeginUninstallDelegate beginUninstallDelegate = Modify;
		return beginUninstallDelegate.BeginInvoke(callback, beginUninstallDelegate);
	}

	public void EndUninstall(IAsyncResult asyncResult)
	{
		((BeginUninstallDelegate)asyncResult.AsyncState).EndInvoke(asyncResult);
	}

	public void EndModify(IAsyncResult asyncResult)
	{
		((BeginUninstallDelegate)asyncResult.AsyncState).EndInvoke(asyncResult);
	}

	public void Cancel()
	{
		TaskSequencer.TaskSuite.Canceled = true;
	}

	public string GetPrerequisiteCode(string prerequisiteDisplayName)
	{
		if (_prerequisiteToInstall != null)
		{
			foreach (KeyValuePair<string, ProductInstallInfo> item in _prerequisiteToInstall)
			{
				if (string.Compare(item.Value.DisplayName, prerequisiteDisplayName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return item.Value.ProductCode;
				}
			}
		}
		return null;
	}

	private void CultureChanged(object sender, CultureChangedEventArgs e)
	{
		Resources.Culture = e.Culture;
		CultureHelper.SetCulture(e.Culture);
	}
}
