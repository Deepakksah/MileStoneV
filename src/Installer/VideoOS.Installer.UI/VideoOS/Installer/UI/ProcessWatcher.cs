using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Logging;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class ProcessWatcher
{
	private readonly IEnumerable<string> _watchedProcesses;

	public ProcessWatcher(IEnumerable<InstallerPackageType> allPackages, IEnumerable<PackageRefType> uninstallSequence, IEnumerable<PackageRefType> installSequence)
	{
		Dictionary<string, InstallerPackageType> packageMap = allPackages.ToDictionary((InstallerPackageType p) => p.ID);
		IEnumerable<InstallerPackageType> uninstallPackages = from pRef in uninstallSequence
			where pRef.Condition != ConditionType.Skip && pRef.Condition != ConditionType.Disabled && pRef.Condition != ConditionType.Finalize
			where packageMap.ContainsKey(pRef.ID)
			select packageMap[pRef.ID];
		IEnumerable<InstallerPackageType> installPackages = from pRef in installSequence
			where pRef.Condition != ConditionType.Skip && pRef.Condition != ConditionType.Disabled
			where packageMap.ContainsKey(pRef.ID)
			select packageMap[pRef.ID];
		_watchedProcesses = GetWatchedProcessNames(uninstallPackages, installPackages);
	}

	public ProcessWatcher(IEnumerable<InstallerPackageType> allPackages, IEnumerable<PackageRefType> uninstallSequence, IEnumerable<PackageRefType> installSequence, IEnumerable<string> packagesToUninstall, IEnumerable<string> packagesToInstall)
	{
		Dictionary<string, InstallerPackageType> packageMap = allPackages.ToDictionary((InstallerPackageType p) => p.ID);
		IEnumerable<InstallerPackageType> uninstallPackages = from pRef in uninstallSequence
			where pRef.AllowUnInstall && packagesToUninstall.Contains(pRef.ID)
			where packageMap.ContainsKey(pRef.ID)
			select packageMap[pRef.ID];
		IEnumerable<InstallerPackageType> installPackages = from pRef in installSequence
			where packagesToInstall.Contains(pRef.ID)
			where packageMap.ContainsKey(pRef.ID)
			select packageMap[pRef.ID];
		_watchedProcesses = GetWatchedProcessNames(uninstallPackages, installPackages);
	}

	public ProcessWatcher(IEnumerable<InstallerPackageType> allPackages, IEnumerable<PackageRefType> uninstallSequence, IEnumerable<string> packagesToUninstall)
		: this(allPackages, uninstallSequence, Enumerable.Empty<PackageRefType>(), packagesToUninstall, Enumerable.Empty<string>())
	{
	}

	public bool CheckWatchedProcesses(IWin32Window dialogParent)
	{
		LogManager.Write("Watching processes: " + string.Join(", ", _watchedProcesses));
		while (true)
		{
			IEnumerable<string> runningProcessTitles = GetRunningProcessTitles(_watchedProcesses);
			if (!runningProcessTitles.Any())
			{
				break;
			}
			LogManager.Write("The following processes are still running: " + string.Join(", ", runningProcessTitles));
			string message = string.Format(CultureInfo.CurrentCulture, Resources.ErrorMessage_ProcessRunningText, Environment.NewLine + string.Join(Environment.NewLine, runningProcessTitles));
			if (MessageHelper.ShowInstallerMessageBox(dialogParent, Resources.ErrorMessage_GeneralErrorLabelHeadLine, message, InstallerMessageBoxIcon.Warning, InstallerMessageBoxButtons.OKCancel) == DialogResult.Cancel)
			{
				LogManager.Write("Installer aborted by user.");
				return false;
			}
		}
		return true;
	}

	private static IEnumerable<string> GetWatchedProcessNames(IEnumerable<InstallerPackageType> uninstallPackages, IEnumerable<InstallerPackageType> installPackages)
	{
		return uninstallPackages.SelectMany(delegate(InstallerPackageType package)
		{
			IEnumerable<string> watchedProcesses = package.ProductManifest.PackageData.Uninstall.WatchedProcesses;
			return watchedProcesses ?? Enumerable.Empty<string>();
		}).Concat(installPackages.SelectMany(delegate(InstallerPackageType package)
		{
			IEnumerable<string> watchedProcesses = package.ProductManifest.PackageData.Install.WatchedProcesses;
			return watchedProcesses ?? Enumerable.Empty<string>();
		})).Distinct()
			.ToArray();
	}

	private static IEnumerable<string> GetRunningProcessTitles(IEnumerable<string> watchedProcesses)
	{
		return (from title in watchedProcesses.SelectMany((string name) => HandleInvalidOperation(() => Process.GetProcessesByName(name), Enumerable.Empty<Process>())).Select(delegate(Process process)
			{
				string text = HandleInvalidOperation(() => process.MainWindowTitle, null);
				string text2 = HandleInvalidOperation(() => process.ProcessName + ".exe", null);
				return string.IsNullOrWhiteSpace(text) ? text2 : text;
			})
			where title != null
			orderby title
			select title).ToArray();
	}

	private static T HandleInvalidOperation<T>(Func<T> call, T defaultValue)
	{
		try
		{
			return call();
		}
		catch (InvalidOperationException)
		{
			return defaultValue;
		}
	}
}
