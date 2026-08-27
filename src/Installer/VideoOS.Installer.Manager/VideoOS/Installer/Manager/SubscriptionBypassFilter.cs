using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Manager;

public class SubscriptionBypassFilter
{
	private Dictionary<Guid, List<SubscriptionPluginTypeByPassSubscriptionIf>> _subscriptionByPassConditions { get; }

	private InstallerConfig _installerConfig { get; }

	protected virtual IEnumerable<string> PackageIdsToBeInstalled
	{
		get
		{
			List<string> list = new List<string>();
			PackageRefType[] installExecuteSequence = _installerConfig.InstallExecuteSequence;
			foreach (PackageRefType packageRefType in installExecuteSequence)
			{
				if (packageRefType.Condition != ConditionType.Skip)
				{
					list.Add(packageRefType.ID);
				}
			}
			return list;
		}
	}

	public SubscriptionBypassFilter(InstallerConfig installerConfig)
	{
		Dictionary<Guid, List<SubscriptionPluginTypeByPassSubscriptionIf>> dictionary = new Dictionary<Guid, List<SubscriptionPluginTypeByPassSubscriptionIf>>();
		if (installerConfig != null && installerConfig.InstallerSubscriptions != null)
		{
			SubscriptionPluginType[] installerSubscriptions = installerConfig.InstallerSubscriptions;
			foreach (SubscriptionPluginType subscriptionPluginType in installerSubscriptions)
			{
				if (subscriptionPluginType.BypassConditions == null)
				{
					continue;
				}
				SubscriptionPluginTypeByPassSubscriptionIf[] bypassConditions = subscriptionPluginType.BypassConditions;
				foreach (SubscriptionPluginTypeByPassSubscriptionIf subscriptionPluginTypeByPassSubscriptionIf in bypassConditions)
				{
					if (subscriptionPluginTypeByPassSubscriptionIf.Condition != ByPassConditionType.PackageNotInstalled && subscriptionPluginTypeByPassSubscriptionIf.Condition != ByPassConditionType.PackageInstalled)
					{
						throw new UnsupportedSubscriptionBypassConditionException(string.Format(CultureInfo.InvariantCulture, "Unsupported condition: {0} used with Subscription ID: {1}", subscriptionPluginTypeByPassSubscriptionIf.Condition.ToString(), subscriptionPluginTypeByPassSubscriptionIf.SubscriptionID.ToString()));
					}
					Guid key = new Guid(subscriptionPluginTypeByPassSubscriptionIf.SubscriptionID);
					if (!dictionary.ContainsKey(key))
					{
						dictionary.Add(key, new List<SubscriptionPluginTypeByPassSubscriptionIf>());
					}
					dictionary[key].Add(subscriptionPluginTypeByPassSubscriptionIf);
				}
			}
		}
		_subscriptionByPassConditions = dictionary;
		_installerConfig = installerConfig;
	}

	public ReadOnlyCollection<IWizardCommonPageSubscription<TContent>> GetFilteredSubscriptions<TContent>(IEnumerable<IWizardCommonPageSubscription<TContent>> subscriptions)
	{
		List<IWizardCommonPageSubscription<TContent>> list = new List<IWizardCommonPageSubscription<TContent>>();
		if (subscriptions != null)
		{
			foreach (IWizardCommonPageSubscription<TContent> subscription in subscriptions)
			{
				if (!ValidateByPassSubscription(subscription.Id, PackageIdsToBeInstalled))
				{
					list.Add(subscription);
				}
			}
		}
		return new ReadOnlyCollection<IWizardCommonPageSubscription<TContent>>(list);
	}

	private bool ValidateByPassSubscription(Guid subscriptionId, IEnumerable<string> packagesToInstallIds)
	{
		if (!_subscriptionByPassConditions.ContainsKey(subscriptionId))
		{
			return false;
		}
		foreach (SubscriptionPluginTypeByPassSubscriptionIf item in _subscriptionByPassConditions[subscriptionId])
		{
			bool flag = packagesToInstallIds.Contains(item.PackageID, StringComparer.OrdinalIgnoreCase);
			if (!flag && item.Condition == ByPassConditionType.PackageNotInstalled)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Bypassing subscription with Id: {0}. Reason: associated PackageId: {1} will not be installed.", item.SubscriptionID, item.PackageID), TraceEventType.Information);
				return true;
			}
			if (flag && item.Condition == ByPassConditionType.PackageInstalled)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Bypassing subscription with Id: {0}. Reason: associated PackageId: {1} will be installed.", item.SubscriptionID, item.PackageID), TraceEventType.Information);
				return true;
			}
		}
		return false;
	}
}
