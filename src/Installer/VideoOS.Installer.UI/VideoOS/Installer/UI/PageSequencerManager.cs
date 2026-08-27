using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Manager;

namespace VideoOS.Installer.UI;

public class PageSequencerManager
{
	private Dictionary<string, Dictionary<string, List<ShowPageTypeByPassIf>>> _conditions = new Dictionary<string, Dictionary<string, List<ShowPageTypeByPassIf>>>();

	private List<string> _skippedPages = new List<string>();

	private List<string> _uninstallPluginPages = new List<string>();

	public void AddPageCondition(string treckId, string pageName, ShowPageTypeByPassIf byPass, bool isUninstall)
	{
		if (!_conditions.ContainsKey(treckId))
		{
			_conditions[treckId] = new Dictionary<string, List<ShowPageTypeByPassIf>>();
		}
		if (!_conditions[treckId].ContainsKey(pageName))
		{
			_conditions[treckId][pageName] = new List<ShowPageTypeByPassIf>();
		}
		if (isUninstall && !_uninstallPluginPages.Contains(pageName))
		{
			_uninstallPluginPages.Add(pageName);
		}
		_conditions[treckId][pageName].Add(byPass);
	}

	public bool CheckBeforeActivate(string treckId, string pageName)
	{
		bool flag = true;
		if (_conditions.ContainsKey(treckId) && _conditions[treckId].ContainsKey(pageName))
		{
			foreach (ShowPageTypeByPassIf item2 in _conditions[treckId][pageName])
			{
				switch (item2.Condition)
				{
				case ByPassConditionType.PackageNotInstalled:
				case ByPassConditionType.PackageInstalled:
					flag = CheckPackageInstalled(item2.ID, item2.Condition, treckId == "modify", treckId == "uninstall", _uninstallPluginPages.Contains(pageName));
					break;
				case ByPassConditionType.ParameterNotSet:
					flag = InstallerManager.Instance.Parameters.ContainsKey(item2.ID);
					break;
				case ByPassConditionType.ParameterSet:
					flag = (string.IsNullOrEmpty(item2.Name) ? InstallerManager.Instance.Parameters.ContainsKey(item2.ID) : (InstallerManager.Instance.Parameters.ContainsKey(item2.ID) && InstallerManager.Instance.Parameters[item2.ID].Equals(item2.Name, StringComparison.OrdinalIgnoreCase)));
					break;
				}
			}
		}
		string item = FormatPageId(treckId, pageName);
		if (flag)
		{
			if (_skippedPages.Contains(item))
			{
				_skippedPages.Remove(item);
			}
		}
		else if (!_skippedPages.Contains(item))
		{
			_skippedPages.Add(item);
		}
		return flag;
	}

	private string FormatPageId(string treckId, string pageName)
	{
		return $"{treckId}:{pageName}";
	}

	private bool CheckPackageInstalled(string id, ByPassConditionType byPassConditionType, bool isModify, bool isUninstall, bool isUninstallPage)
	{
		if (isModify)
		{
			if (!isUninstallPage)
			{
				if (byPassConditionType != ByPassConditionType.PackageInstalled)
				{
					return InstallerManager.Instance.SequenceManager.PackagesToUninstall.Contains(id);
				}
				return InstallerManager.Instance.SequenceManager.PackagesToInstall.Contains(id);
			}
			if (byPassConditionType != ByPassConditionType.PackageInstalled)
			{
				return InstallerManager.Instance.SequenceManager.PackagesToInstall.Contains(id);
			}
			return InstallerManager.Instance.SequenceManager.PackagesToUninstall.Contains(id);
		}
		if (isUninstall)
		{
			IEnumerable<string> source = InstallerManager.Instance.InstallInfo.InstalledPackages.Select((PackageRefType i) => i.ID);
			if (byPassConditionType != ByPassConditionType.PackageInstalled)
			{
				return false;
			}
			return source.Contains(id);
		}
		IEnumerable<string> source2 = from i in InstallerManager.Instance.InstallerConfig.InstallExecuteSequence
			where i.Condition == ConditionType.Skip || i.Condition == ConditionType.Disabled
			select i.ID;
		if (byPassConditionType != ByPassConditionType.PackageInstalled)
		{
			return !source2.Contains(id);
		}
		return source2.Contains(id);
	}

	public bool IsPageShown(string treckKey, string pageName)
	{
		return !_skippedPages.Contains(FormatPageId(treckKey, pageName));
	}

	public void AddSkipPage(string treckId, string pageName)
	{
		string item = FormatPageId(treckId, pageName);
		_skippedPages.Add(item);
	}
}
