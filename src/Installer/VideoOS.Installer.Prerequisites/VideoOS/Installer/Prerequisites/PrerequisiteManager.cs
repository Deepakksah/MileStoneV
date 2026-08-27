using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Properties;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Configuration.Package;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Interfaces.Msi;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Prerequisites.PrepareTasks;
using VideoOS.Installer.Prerequisites.Properties;
using VideoOS.Installer.Sequencer;
using VideoOS.Installer.Sequencer.InstallerObserver;
using VideoOS.Installer.Sequencer.Prerequisite;

namespace VideoOS.Installer.Prerequisites;

public class PrerequisiteManager : TaskSequencer
{
	private class ExtractMsiHelper
	{
		public string ProductFileName { get; }

		public string DisplayName { get; }

		public List<string> FilesToExtract { get; }

		public ExtractMsiHelper(string productFileName, string displayName)
		{
			ProductFileName = productFileName;
			DisplayName = displayName;
			FilesToExtract = new List<string>();
		}

		public InstallerTask GetInstallerTask(string targetDir)
		{
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			ExtractProductInstallerFiles extractProductInstallerFiles = new ExtractProductInstallerFiles(targetDir, DisplayName, Path.Combine(directoryName, ProductFileName), FilesToExtract);
			if (extractProductInstallerFiles.IsSupportedInstaller)
			{
				return extractProductInstallerFiles;
			}
			return new ExtractProductInstallerFilesLegacy(targetDir, ProductFileName, FilesToExtract, DisplayName);
		}
	}

	private delegate void BeginPrepareInstallationDelegate();

	private delegate void BeginInstallPrereqDelegate(int websitePort);

	private delegate void BeginCancelDelegate();

	private InstallerConfig _installerConfig;

	private bool _needReboot;

	private PrerequisiteInfo _prerequisiteInfo;

	private string _winTargetDir;

	private IClMsiHandler _clMsiHandler;

	public Dictionary<string, ProductInstallInfo> PackagesToInstall => _prerequisiteInfo.PackagesToInstall;

	public Dictionary<string, string> PackagesCannotInstall => _prerequisiteInfo.PackagesCannotInstall;

	public List<string> NotInstalledPrerequisites => _prerequisiteInfo.NotInstalledPrerequisites;

	public string WinTargetDir => _winTargetDir;

	public Dictionary<string, List<IPackageParameter>> PackageParameters { get; set; }

	public bool NeedReboot => _needReboot;

	public PrerequisiteManager(IClMsiHandler handler, InstallerConfig installerConfig, Dictionary<string, List<IPackageParameter>> parameters, string winTargetDir)
	{
		_installerConfig = installerConfig;
		_prerequisiteInfo = new PrerequisiteInfo();
		PackageParameters = parameters;
		_winTargetDir = winTargetDir;
		_clMsiHandler = handler;
		CultureManager.Instance.CultureChanged += Instance_CultureChanged;
	}

	public void PrepareInstallation()
	{
		try
		{
			InstallerTaskSuiteObserver observer = new InstallerTaskSuiteObserver(this);
			base.TaskSuite = new TaskSequencerSuite(TaskSeqencerDirection.Forward);
			base.TaskSuite.ClearAllTasks();
			Dictionary<string, ExtractMsiHelper> dictionary = new Dictionary<string, ExtractMsiHelper>();
			InstallerPackageType[] installerPackages = _installerConfig.InstallerPackages;
			foreach (InstallerPackageType installerPackageType in installerPackages)
			{
				PackageType packageData = installerPackageType.ProductManifest.PackageData;
				if ((packageData.PrimaryFile != null && packageData.PrimaryFile.Length != 0 && File.Exists(Path.Combine(WinTargetDir, packageData.PrimaryFile.First()))) || installerPackageType.ProductManifest.MainPackage || packageData.SecondaryFiles == null || packageData.SecondaryFiles.Length == 0)
				{
					continue;
				}
				string text = packageData.SecondaryFiles[0][0];
				if (!dictionary.TryGetValue(text, out var value))
				{
					value = new ExtractMsiHelper(text, packageData.DisplayName);
					dictionary.Add(text, value);
				}
				string[] primaryFile = packageData.PrimaryFile;
				foreach (string item in primaryFile)
				{
					value.FilesToExtract.Add(item);
				}
				if (packageData.Prerequisites == null)
				{
					continue;
				}
				PackageCollectionType[] prerequisites = packageData.Prerequisites;
				foreach (PackageCollectionType packageCollectionType in prerequisites)
				{
					if (packageCollectionType.ProductInfo.ProductManifest != null)
					{
						string item2 = Path.Combine(packageCollectionType.ProductInfo.InstallationFolder, packageCollectionType.ProductInfo.ProductManifest.Filename);
						value.FilesToExtract.Add(item2);
					}
				}
			}
			foreach (ExtractMsiHelper value2 in dictionary.Values)
			{
				base.TaskSuite.AddTask(value2.GetInstallerTask(WinTargetDir));
			}
			base.TaskSuite.AddTask(new CollectPrerequisitesTask(WinTargetDir, PackageParameters, _installerConfig, _prerequisiteInfo));
			base.TaskSuite.AddTask(new CheckPrerequisistesTask(_clMsiHandler, _installerConfig, _prerequisiteInfo));
			base.TaskSuite.AttachObserver(observer);
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			throw;
		}
		try
		{
			base.TaskSuite.Start();
			EnsureResults();
			if (base.TaskSuite.Result == TaskResult.Error)
			{
				throw base.TaskSuite.LastError;
			}
		}
		catch (Exception ex)
		{
			LogManager.Write($"Preparation phase failed {ex.TypeAndMessage()}.", TraceEventType.Error);
			throw;
		}
	}

	public void Unpack(string targetDir, List<string> filesToExtract)
	{
		try
		{
			InstallerTaskSuiteObserver observer = new InstallerTaskSuiteObserver(this);
			base.TaskSuite = new TaskSequencerSuite(TaskSeqencerDirection.Forward);
			base.TaskSuite.ClearAllTasks();
			base.TaskSuite.AddTask(new UnpackFilesTask(Path.Combine(targetDir, string.Format(Settings.Default.InstallationFolder, _installerConfig.InstallerInfo.ID)), filesToExtract));
			base.TaskSuite.AttachObserver(observer);
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			throw;
		}
		try
		{
			base.TaskSuite.Start();
			EnsureResults();
		}
		catch (Exception ex)
		{
			LogManager.Write($"Preparation phase failed {ex.TypeAndMessage()}.", TraceEventType.Error);
			throw;
		}
	}

	public bool PrepareInstallationPrerequisites()
	{
		try
		{
			base.TaskSuite = new TaskSequencerSuite(TaskSeqencerDirection.Forward);
			base.TaskSuite.AddTask(new CollectInstallerPrerequisitesTask(WinTargetDir, PackageParameters, _installerConfig, _prerequisiteInfo));
			base.TaskSuite.AddTask(new CheckInstallerPrerequisitesTask(_clMsiHandler, _installerConfig, _prerequisiteInfo));
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			throw;
		}
		try
		{
			base.TaskSuite.Start();
		}
		catch (Exception ex)
		{
			LogManager.Write($"Preparation phase failed {ex.TypeAndMessage()}.", TraceEventType.Error);
			throw;
		}
		return _prerequisiteInfo.PackagesToInstall.Count != 0;
	}

	private void CreateTasksForPrerequisites(int webSitePort)
	{
		if (_prerequisiteInfo.PackagesToInstall == null)
		{
			return;
		}
		foreach (KeyValuePair<string, ProductInstallInfo> item in _prerequisiteInfo.PackagesToInstall)
		{
			ProductInstallInfo value = item.Value;
			if (value != null)
			{
				LogManager.Write($"Prerequisite '{value.DisplayName}' was added.");
				if (value.ProductCode.ToLower() == "iis")
				{
					base.TaskSuite.AddTask(new IisPrerequisiteInstallerTask(WinTargetDir, item.Value, webSitePort));
				}
				else
				{
					base.TaskSuite.AddTask(new PrerequisiteInstallerTask(WinTargetDir, item.Value));
				}
			}
		}
	}

	public void InstallPrerequisites(int websitePort)
	{
		InstallerTaskSuiteObserver observer = new InstallerTaskSuiteObserver(this);
		try
		{
			base.TaskSuite = new TaskSequencerSuite(TaskSeqencerDirection.Forward);
			CreateTasksForPrerequisites(websitePort);
			base.TaskSuite.AttachObserver(observer);
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			throw;
		}
		try
		{
			LogManager.Write("Start installing prerequisites...");
			base.TaskSuite.Start();
			EnsureResults();
		}
		catch (Exception ex)
		{
			LogManager.Write($"Preparation phase failed {ex.TypeAndMessage()}.", TraceEventType.Error);
			throw;
		}
		foreach (Task task in base.TaskSuite.Tasks)
		{
			if (task.LastError != null)
			{
				if (((InstallerTask)task).NeedsReboot)
				{
					_needReboot = true;
				}
				throw task.LastError;
			}
		}
		_prerequisiteInfo.PackagesToInstall = new Dictionary<string, ProductInstallInfo>();
	}

	public void PrepareModifyInstallation(List<string> packagesToInstall)
	{
		try
		{
			InstallerTaskSuiteObserver observer = new InstallerTaskSuiteObserver(this);
			base.TaskSuite = new TaskSequencerSuite(TaskSeqencerDirection.Forward);
			base.TaskSuite.ClearAllTasks();
			base.TaskSuite.AddTask(new CollectPrerequisitesTask(WinTargetDir, _installerConfig, packagesToInstall, PackageParameters, _prerequisiteInfo));
			base.TaskSuite.AddTask(new CheckPrerequisistesTask(_clMsiHandler, _installerConfig, _prerequisiteInfo));
			base.TaskSuite.AttachObserver(observer);
		}
		catch (Exception ex)
		{
			LogManager.Write(string.Format("{0} {1}", "Unpacking...", ex.TypeAndMessage()), ex.StackTrace, TraceEventType.Error);
			throw;
		}
		try
		{
			base.TaskSuite.Start();
			EnsureResults();
			if (base.TaskSuite.Result == TaskResult.Error)
			{
				throw base.TaskSuite.LastError;
			}
		}
		catch (Exception ex2)
		{
			LogManager.Write($"Preparation phase failed {ex2.TypeAndMessage()}.", TraceEventType.Error);
			throw;
		}
	}

	public void Cancel()
	{
		throw new NotImplementedException();
	}

	public IAsyncResult BeginPrepareInstallation(AsyncCallback callback, object asyncState)
	{
		BeginPrepareInstallationDelegate beginPrepareInstallationDelegate = PrepareInstallation;
		return beginPrepareInstallationDelegate.BeginInvoke(callback, beginPrepareInstallationDelegate);
	}

	public IAsyncResult BeginPrerequisiteInstallation(AsyncCallback callback, object asyncState, int websitePort)
	{
		BeginInstallPrereqDelegate beginInstallPrereqDelegate = InstallPrerequisites;
		return beginInstallPrereqDelegate.BeginInvoke(websitePort, callback, beginInstallPrereqDelegate);
	}

	public void EndPrerequisiteInstallation(IAsyncResult asyncResult)
	{
		((BeginInstallPrereqDelegate)asyncResult.AsyncState).EndInvoke(asyncResult);
	}

	public void EndPrepareInstallation(IAsyncResult asyncResult)
	{
		((BeginPrepareInstallationDelegate)asyncResult.AsyncState).EndInvoke(asyncResult);
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

	private void Instance_CultureChanged(object sender, CultureChangedEventArgs e)
	{
		Resources.Culture = e.Culture;
	}

	public void CheckInstallerPrerequisites()
	{
		try
		{
			InstallerTaskSuiteObserver observer = new InstallerTaskSuiteObserver(this);
			base.TaskSuite = new TaskSequencerSuite(TaskSeqencerDirection.Forward);
			base.TaskSuite.ClearAllTasks();
			base.TaskSuite.AddTask(new CollectInstallerPrerequisitesTask(_installerConfig, _prerequisiteInfo));
			base.TaskSuite.AddTask(new CheckInstallerPrerequisitesTask(_clMsiHandler, _installerConfig, _prerequisiteInfo));
			base.TaskSuite.AttachObserver(observer);
		}
		catch (Exception ex)
		{
			LogManager.Write(string.Format("{0} {1}", "Unpacking...", ex.TypeAndMessage()), ex.StackTrace, TraceEventType.Error);
			throw;
		}
		try
		{
			base.TaskSuite.Start();
			EnsureResults();
			if (base.TaskSuite.Result == TaskResult.Error)
			{
				throw base.TaskSuite.LastError;
			}
		}
		catch (Exception ex2)
		{
			LogManager.Write($"Preparation phase failed {ex2.TypeAndMessage()}.", TraceEventType.Error);
			throw;
		}
	}
}
