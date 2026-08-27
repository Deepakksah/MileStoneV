using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using VideoOS.Platform.Admin;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.Util;

public class ConfigurationChangedService
{
	private const string MipKindChangedIndication = "Server.MipKindChangedIndication";

	private static readonly Guid ConfigServiceId = new Guid("7BA35BDA-87DA-4D9B-9403-A1EDAE42DAA8");

	private static readonly Guid ConfigServiceKind = new Guid("E05DDF5B-EB39-41F3-A0EF-420277AB6C39");

	private static readonly Guid LicenseId = new Guid("E145597A-ECAF-40A1-99E6-14BB9D45A09E");

	private bool _stopNow;

	private Thread _thread;

	private long _checkIntervalInTicks = 600000000L;

	private long _seconds = 60L;

	private long _tick;

	private object _msgRef;

	private MIPKindFolder mipKindFolder;

	private int _dataVersionSum;

	private bool _hasCheckedMipConfigs;

	public long CheckInterval
	{
		get
		{
			return _seconds;
		}
		set
		{
			if (value > 3)
			{
				_seconds = value;
				_checkIntervalInTicks = 10000000 * _seconds;
				_tick = DateTime.Now.Ticks + _checkIntervalInTicks;
			}
		}
	}

	public void Init()
	{
		_stopNow = false;
		_hasCheckedMipConfigs = false;
		if (_thread == null)
		{
			_thread = new Thread(Run);
			_thread.Name = "MIP:ConfigurationChangedService";
			_thread.IsBackground = true;
			_thread.Start();
		}
		if (EnvironmentManager.Instance.EnvironmentOptions.ContainsKey("ConfigurationChangeCheckInterval"))
		{
			string s = EnvironmentManager.Instance.EnvironmentOptions["ConfigurationChangeCheckInterval"];
			long result = 60L;
			if (long.TryParse(s, out result))
			{
				CheckInterval = result;
			}
		}
		_msgRef = EnvironmentManager.Instance.RegisterReceiver(MipKindChangedHandler, new MessageIdFilter("Server.MipKindChangedIndication"));
	}

	public void Close()
	{
		_stopNow = true;
		if (_msgRef != null)
		{
			if (EnvironmentManager.Instance != null)
			{
				EnvironmentManager.Instance.UnRegisterReceiver(_msgRef);
			}
			_msgRef = null;
		}
		if (_thread != null)
		{
			_thread.Join();
			_thread = null;
		}
	}

	private object MipKindChangedHandler(Message message, FQID dest, FQID sender)
	{
		_tick = DateTime.Now.Ticks + 50000000;
		return null;
	}

	private void Run()
	{
		try
		{
			Dictionary<Guid, long> currentConfig = new Dictionary<Guid, long>();
			Dictionary<Guid, DateTime> currentConfigDateTime = new Dictionary<Guid, DateTime>();
			_tick = DateTime.Now.Ticks + _checkIntervalInTicks;
			while (!_stopNow)
			{
				ServerId serverId = EnvironmentManager.Instance.MasterSite.ServerId;
				if (DateTime.Now.Ticks > _tick && Factory.IsLoginEstablished(serverId))
				{
					try
					{
						if (Configuration.Instance.IsMIPItemSupported)
						{
							Dictionary<Guid, DateTime> dictionary = CheckForUpdatesViaMIPKind(serverId, currentConfigDateTime);
							if (dictionary != null)
							{
								currentConfigDateTime = dictionary;
							}
						}
						else
						{
							currentConfig = CheckForUpdatesViaXmlIndex(serverId, currentConfig);
						}
					}
					catch (Exception exception)
					{
						EnvironmentManager.Instance.ExceptionHandler("ConfigurationChangedServiceInner", exception);
					}
					_tick = DateTime.Now.Ticks + _checkIntervalInTicks;
				}
				Thread.Sleep(100);
			}
		}
		catch (Exception exception2)
		{
			EnvironmentManager.Instance.ExceptionHandler("ConfigurationChangedService", exception2);
		}
		_thread = null;
	}

	private Dictionary<Guid, DateTime> CheckForUpdatesViaMIPKind(ServerId serverId, Dictionary<Guid, DateTime> currentConfigDateTime)
	{
		Dictionary<Guid, DateTime> dictionary = BuildIndexViaMIP(serverId);
		HashSet<FQID> hashSet = CompareConfigsDateTime(currentConfigDateTime, dictionary);
		if (!hashSet.Any() || !dictionary.Any())
		{
			return null;
		}
		RefreshConfigurationForMIPKind(hashSet);
		SendConfigurationChanged(hashSet);
		return dictionary;
	}

	private Dictionary<Guid, long> CheckForUpdatesViaXmlIndex(ServerId serverId, Dictionary<Guid, long> currentConfig)
	{
		Dictionary<Guid, long> dictionary = LoadConfigIndex(serverId);
		HashSet<FQID> hashSet = CompareConfigs(serverId, currentConfig, dictionary);
		if (!hashSet.Any() || !dictionary.Any())
		{
			return dictionary;
		}
		RefreshConfigurationForXmlIndex(hashSet);
		SendConfigurationChanged(hashSet);
		return dictionary;
	}

	private void SendConfigurationChanged(HashSet<FQID> changedConfigs)
	{
		if (!ShouldSendConfigurationChanged())
		{
			return;
		}
		while (changedConfigs.Count > 0)
		{
			HashSet<FQID> hashSet = new HashSet<FQID>();
			EnvironmentManager.Instance.Log(error: false, "ConfigurationChangedCheck", changedConfigs.Count.ToString(CultureInfo.InvariantCulture) + " Changes found", null);
			foreach (FQID changedConfig in changedConfigs)
			{
				ItemNode itemNode = PluginManager.GetItemNode(changedConfig.Kind);
				if (itemNode != null)
				{
					SendConfigurationChangedForItem(changedConfigs, hashSet, changedConfig, itemNode);
				}
				else
				{
					SendConfigurationChangedForBuiltInId(changedConfig);
				}
			}
			changedConfigs = hashSet;
		}
	}

	private bool ShouldSendConfigurationChanged()
	{
		if (_hasCheckedMipConfigs)
		{
			return true;
		}
		_hasCheckedMipConfigs = true;
		return ShouldNotifyConfigurationChangeFirstTime();
	}

	private static bool ShouldNotifyConfigurationChangeFirstTime()
	{
		return EnvironmentManager.Instance.EnvironmentType != EnvironmentType.SmartClient;
	}

	private static void SendConfigurationChangedForItem(HashSet<FQID> changedConfigs, HashSet<FQID> redoHash, FQID fqid, ItemNode itemNode)
	{
		if (IdsContainParents(changedConfigs, itemNode))
		{
			redoHash.Add(fqid);
			return;
		}
		EnvironmentManager.Instance.Log(error: false, "ConfigurationChangedCheck", "Sending ConfigurationChangedIndication for Kind:" + itemNode.Name, null);
		EnvironmentManager.Instance.PostMessage(new Message("Server.ConfigurationChangedIndication", fqid));
		Thread.Sleep(1000);
	}

	private static void SendConfigurationChangedForBuiltInId(FQID fqid)
	{
		if (fqid.Kind == LicenseId)
		{
			EnvironmentManager.Instance.LicenseManager.ReloadActivatedLicense();
			EnvironmentManager.Instance.PostMessage(new Message(MessageId.System.LicenseChangedIndication));
		}
		else
		{
			EnvironmentManager.Instance.PostMessage(new Message("Server.ConfigurationChangedIndication", fqid));
		}
	}

	private static void RefreshConfigurationForMIPKind(IEnumerable<FQID> changedConfigs)
	{
		foreach (FQID changedConfig in changedConfigs)
		{
			EnvironmentManager.Instance.Log(error: false, "ConfigChg", "Check Kind=" + changedConfig.Kind.ToString() + ", ObjectId=" + changedConfig.ObjectId.ToString(), null);
			Configuration.Instance.RefreshConfiguration(changedConfig.Kind);
		}
	}

	private static void RefreshConfigurationForXmlIndex(IEnumerable<FQID> changedConfigs)
	{
		foreach (FQID changedConfig in changedConfigs)
		{
			EnvironmentManager.Instance.Log(error: false, "ConfigChg", "Check Kind=" + changedConfig.Kind.ToString() + ", ObjectId=" + changedConfig.ObjectId.ToString(), null);
			PluginDefinition pluginDefinitionFromKind = PluginManager.GetPluginDefinitionFromKind(changedConfig.Kind);
			if (pluginDefinitionFromKind != null)
			{
				Configuration.Instance.RefreshConfiguration(pluginDefinitionFromKind.Id);
			}
			else
			{
				Configuration.Instance.RefreshConfiguration(changedConfig.Kind);
			}
		}
	}

	private static bool IdsContainParents(IEnumerable<FQID> changedConfigs, ItemNode itemNode)
	{
		if (itemNode == null)
		{
			return false;
		}
		if (itemNode.ParentKind == Guid.Empty)
		{
			return false;
		}
		foreach (FQID changedConfig in changedConfigs)
		{
			if (itemNode.ParentKind == changedConfig.Kind)
			{
				return true;
			}
			if (itemNode.ParentKind != Guid.Empty && IdsContainParents(changedConfigs, PluginManager.GetItemNode(itemNode.ParentKind)))
			{
				return true;
			}
		}
		return false;
	}

	private static Dictionary<Guid, long> LoadConfigIndex(ServerId serverId)
	{
		if (Factory.IsLoginEstablished(serverId))
		{
			return BuildIndexViaXml(serverId);
		}
		return new Dictionary<Guid, long>();
	}

	private Dictionary<Guid, DateTime> BuildIndexViaMIP(ServerId serverId)
	{
		Dictionary<Guid, DateTime> dictionary = new Dictionary<Guid, DateTime>();
		try
		{
			if (mipKindFolder == null)
			{
				mipKindFolder = new MIPKindFolder(serverId);
			}
			int dataVersionSum = mipKindFolder.GetDataVersionSum();
			if (dataVersionSum == _dataVersionSum)
			{
				return dictionary;
			}
			mipKindFolder.ClearChildrenCache();
			List<MIPKind> list = new List<MIPKind>();
			list.Add(new MIPKind(serverId, LicenseId));
			list.AddRange(mipKindFolder.MIPKinds);
			foreach (MIPKind item in list)
			{
				if (item.KindType == "ITEM")
				{
					dictionary.Add(item.Guid, item.LastModified);
					continue;
				}
				Guid key = new Guid(item.PluginId);
				if (!dictionary.ContainsKey(key) || dictionary[key] < item.LastModified)
				{
					dictionary[key] = item.LastModified;
				}
				if (!dictionary.ContainsKey(item.Guid))
				{
					dictionary.Add(item.Guid, item.LastModified);
				}
			}
			_dataVersionSum = dataVersionSum;
			return dictionary;
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "ConfigurationChangedService", ex.Message);
			return dictionary;
		}
	}

	private static Dictionary<Guid, long> BuildIndexViaXml(ServerId serverId)
	{
		Configuration.Instance.RefreshConfiguration(ConfigServiceId);
		List<Item> itemConfigurations = Configuration.Instance.GetItemConfigurations(serverId, ConfigServiceId, null, ConfigServiceKind);
		Dictionary<Guid, long> dictionary = new Dictionary<Guid, long>();
		if (itemConfigurations == null)
		{
			return dictionary;
		}
		foreach (Item item in itemConfigurations)
		{
			if (!dictionary.ContainsKey(item.FQID.ObjectId))
			{
				dictionary.Add(item.FQID.ObjectId, GetDataVersion(item.Properties));
			}
		}
		return dictionary;
	}

	private static long GetDataVersion(IDictionary<string, string> properties)
	{
		if (!properties.TryGetValue("DataVersion", out var value) || !long.TryParse(value, out var result))
		{
			return 0L;
		}
		return result;
	}

	private static HashSet<FQID> CompareConfigsDateTime(Dictionary<Guid, DateTime> currentConfig, Dictionary<Guid, DateTime> newConfig)
	{
		HashSet<FQID> hashSet = new HashSet<FQID>();
		foreach (Guid key in newConfig.Keys)
		{
			if (currentConfig.ContainsKey(key))
			{
				if (currentConfig[key] != newConfig[key])
				{
					hashSet.Add(BuildFQID(key));
				}
			}
			else
			{
				hashSet.Add(BuildFQID(key));
			}
		}
		return hashSet;
	}

	private static HashSet<FQID> CompareConfigs(ServerId serverId, Dictionary<Guid, long> currentConfig, Dictionary<Guid, long> newConfig)
	{
		HashSet<FQID> hashSet = new HashSet<FQID>();
		foreach (Guid key in newConfig.Keys)
		{
			if (currentConfig.ContainsKey(key))
			{
				long itemConfigurationDataVersion = Configuration.Instance.GetItemConfigurationDataVersion(serverId, key);
				if (currentConfig[key] != newConfig[key] || (itemConfigurationDataVersion != 0L && itemConfigurationDataVersion < newConfig[key]))
				{
					hashSet.Add(BuildFQID(key));
				}
			}
			else
			{
				hashSet.Add(BuildFQID(key));
			}
		}
		foreach (Guid key2 in currentConfig.Keys)
		{
			if (!newConfig.ContainsKey(key2))
			{
				hashSet.Add(BuildFQID(key2));
			}
		}
		return hashSet;
	}

	private static FQID BuildFQID(Guid kind)
	{
		return new FQID(new ServerId("N/A", "unknown", 80, Guid.Empty), FolderType.UserDefined, kind);
	}
}
