using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Manager;

public class PluginLoader
{
	private readonly InstallerConfig _installerConfig;

	private readonly IPluginTypeLoader _typeLoader;

	public PluginLoader(InstallerConfig installerConfig, IPluginTypeLoader typeLoader)
	{
		_installerConfig = installerConfig;
		_typeLoader = typeLoader;
	}

	public IEnumerable<Plugin> LoadPlugins()
	{
		List<Plugin> list = new List<Plugin>();
		InstallerPluginType[] installerPlugins = _installerConfig.InstallerPlugins;
		foreach (InstallerPluginType installerPluginType in installerPlugins)
		{
			if (installerPluginType.PluginAssembly == null)
			{
				continue;
			}
			string filename = installerPluginType.PluginAssembly.Filename;
			foreach (Type item in CollectSpecificTypes<IWizardPageProvider>(filename))
			{
				if (Activator.CreateInstance(item) is IWizardPageProvider wizardProvider)
				{
					Plugin plugin = new Plugin();
					plugin.WizardProvider = wizardProvider;
					plugin.PageFlow = installerPluginType.PageFlow;
					plugin.WizardPages = installerPluginType.WizardPages;
					list.Add(plugin);
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Plug-in {0} with type {1} was loaded.", filename, item.ToString()), TraceEventType.Information);
				}
			}
		}
		return list;
	}

	public Dictionary<string, IPageSetupActions> LoadCustomPageSetup()
	{
		Dictionary<string, IPageSetupActions> dictionary = new Dictionary<string, IPageSetupActions>();
		if (_installerConfig.InstallerSettings?.CustomPagesSetup != null)
		{
			IEnumerable<string> assemblyFilenames = from i in _installerConfig.InstallerSettings.CustomPagesSetup
				where i.PluginAssembly != null
				select i.PluginAssembly.Filename;
			foreach (Type item in CollectSpecificTypes<IPageSetupActions>(assemblyFilenames))
			{
				if (Activator.CreateInstance(item) is IPageSetupActions pageSetupActions)
				{
					dictionary[pageSetupActions.PageName] = pageSetupActions;
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Plug-in {0} with type {1} was loaded.", item.Module.Name, pageSetupActions.PageName), TraceEventType.Information);
				}
			}
		}
		return dictionary;
	}

	public SubscriptionPlugins LoadSubscriptionPlugins()
	{
		if (_installerConfig.InstallerSubscriptions != null)
		{
			IEnumerable<string> enumerable = from i in _installerConfig.InstallerSubscriptions
				where i.PluginAssembly != null
				select i.PluginAssembly.Filename;
			string text = (from n in enumerable
				group n by n.ToUpperInvariant() into g
				where g.Count() > 1
				select g).FirstOrDefault()?.First();
			if (text != null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Subscription plugin assembly defined more than once: {0}", text));
			}
			List<IWizardCommonPageSubscription<CertificatePageContent>> certificateSubscriptions = LoadPageSubscriptions<CertificatePageContent>(enumerable);
			List<IWizardCommonPageSubscription<ServiceUserPageContent>> serviceUserSubscriptions = LoadPageSubscriptions<ServiceUserPageContent>(enumerable);
			List<IWizardCommonPageSubscription<WarningPageContent>> warningSubscriptions = LoadPageSubscriptions<WarningPageContent>(enumerable);
			List<IWizardCommonPageSubscription<SamplePageContent>> samplePageSubscriptions = LoadPageSubscriptions<SamplePageContent>(enumerable);
			return new SubscriptionPlugins(certificateSubscriptions, serviceUserSubscriptions, warningSubscriptions, samplePageSubscriptions);
		}
		return new SubscriptionPlugins();
	}

	private List<IWizardCommonPageSubscription<TContent>> LoadPageSubscriptions<TContent>(IEnumerable<string> assemblyFilenames)
	{
		List<Type> list = CollectSpecificTypes<IWizardCommonPageSubscription<TContent>>(assemblyFilenames);
		List<IWizardCommonPageSubscription<TContent>> list2 = new List<IWizardCommonPageSubscription<TContent>>();
		foreach (Type item2 in list)
		{
			IWizardCommonPageSubscription<TContent> item = (IWizardCommonPageSubscription<TContent>)Activator.CreateInstance(item2);
			list2.Add(item);
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Subscription {0} with type {1} was loaded.", item2.Assembly.FullName, item2.ToString()), TraceEventType.Information);
		}
		IGrouping<Guid, IWizardCommonPageSubscription<TContent>> grouping = (from s in list2
			group s by s.Id into g
			where g.Count() > 1
			select g).FirstOrDefault();
		if (grouping != null)
		{
			Guid id = grouping.First().Id;
			IEnumerable<string> values = grouping.Select((IWizardCommonPageSubscription<TContent> s) => s.GetType().Name);
			throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Multiple subscriptions are using the same id ({0}): {1}", id, string.Join(", ", values)));
		}
		return list2;
	}

	private List<Type> CollectSpecificTypes<TInterface>(string assemblyFilename)
	{
		return CollectSpecificTypes<TInterface>(new string[1] { assemblyFilename });
	}

	private List<Type> CollectSpecificTypes<TInterface>(IEnumerable<string> assemblyFilenames)
	{
		List<Type> list = new List<Type>();
		foreach (string assemblyFilename in assemblyFilenames)
		{
			IEnumerable<Type> specificTypes = _typeLoader.GetSpecificTypes(assemblyFilename, typeof(TInterface));
			list.AddRange(specificTypes);
		}
		return list;
	}
}
