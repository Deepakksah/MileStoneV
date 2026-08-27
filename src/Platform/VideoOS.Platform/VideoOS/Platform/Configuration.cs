using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Util;

namespace VideoOS.Platform;

public abstract class Configuration
{
	public struct ServiceURIInfo
	{
		public Guid Type;

		public Guid Instance;

		public List<string> UriArray;

		public string Name;

		public string Description;

		public bool Trusted;

		public string ServiceData;

		public List<RegistrationServiceEndpoint> Endpoints;
	}

	public class RegistrationServiceEndpoint
	{
		public string Authentication { get; set; }

		public string Description { get; set; }

		public string Name { get; set; }

		public string Uri { get; set; }
	}

	private static Configuration _instance;

	private Dictionary<Guid, ConfigurationCache> _configurationCacheServers = new Dictionary<Guid, ConfigurationCache>();

	private object _configurationCacheServersLock = new object();

	private const string EnabledPropertySpecialFormat = "EnabledPropertySpecialFormat";

	private const string BooleanProperty = "Boolean";

	private bool _isMIPItemSupported = true;

	public bool _hasMIPItemSupportBeenChecked;

	public static bool TraceItemConfigurationCache;

	public static readonly Guid MAPServiceType = new Guid("08AB8F23-9AEF-4298-9CAA-F39259FE7CDA");

	public static readonly Guid MAPServiceId = new Guid("08AB8F23-9AEF-4298-9CAA-F39259FE7CDA");

	public static readonly Guid LogServiceType = new Guid("3D6F1153-92AD-43F1-B467-9482FFD291B2");

	public static readonly Guid LogServiceId = new Guid("3D6F1153-92AD-43F1-B467-9482FFD291B2");

	public static Configuration Instance
	{
		[DebuggerStepThrough]
		get
		{
			return _instance;
		}
		set
		{
			_instance = value;
		}
	}

	public bool IsMIPItemSupported
	{
		get
		{
			if (!_hasMIPItemSupportBeenChecked)
			{
				if (!Factory.IsLoginEstablished(ServerFQID.ServerId))
				{
					return true;
				}
				_isMIPItemSupported = Factory.IsItemTypeSupported(ServerFQID.ServerId, "MIPKind");
				_hasMIPItemSupportBeenChecked = _isMIPItemSupported;
			}
			return _isMIPItemSupported;
		}
		set
		{
			_isMIPItemSupported = value;
		}
	}

	public abstract FQID ServerFQID { get; }

	public abstract string ServerName { get; }

	public ConfigurationApiManager ConfigurationApiManager { get; } = new ConfigurationApiManager();

	public abstract List<Item> GetItems();

	public List<Item> GetItemsSorted()
	{
		List<Item> list = GetItems();
		if (list == null)
		{
			list = new List<Item>();
		}
		list.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
		return list;
	}

	public virtual List<Item> GetItems(ItemHierarchy itemHierarchy)
	{
		List<Item> items = GetItems();
		List<Item> list = new List<Item>();
		foreach (Item item in items)
		{
			if ((item.FQID.FolderType == FolderType.SystemDefined && itemHierarchy != ItemHierarchy.UserDefined) || (item.FQID.FolderType == FolderType.UserDefined && itemHierarchy != ItemHierarchy.SystemDefined) || item.FQID.FolderType == FolderType.No || item.FQID.Kind == Kind.Server)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public List<Item> GetItemsSorted(ItemHierarchy itemHierarchy)
	{
		List<Item> list = GetItems(itemHierarchy);
		if (list == null)
		{
			list = new List<Item>();
		}
		list.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
		return list;
	}

	public virtual void GetItemsAsync(AsyncItemsHandler asyncItemsHandler, Control control, object callerReference)
	{
		throw new NotImplementedException();
	}

	public abstract Item GetItem(FQID fqid);

	public abstract Item GetItem(Guid objectId, Guid kind);

	public abstract Item GetItem(ServerId serverId, Guid objectId, Guid kind);

	public virtual void GetItemAsync(FQID fqid, AsyncItemHandler asyncItemHandler, Control control, object callerReference)
	{
		throw new NotImplementedException();
	}

	public abstract List<Item> GetItemsByKind(Guid kind);

	public List<Item> GetItemsByKindSorted(Guid kind)
	{
		List<Item> list = GetItemsByKind(kind);
		if (list == null)
		{
			list = new List<Item>();
		}
		list.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
		return list;
	}

	public virtual List<Item> GetItemsByKind(Guid kind, ItemHierarchy itemHierarchy)
	{
		return GetItemsByKind(kind);
	}

	public List<Item> GetItemsByKindSorted(Guid kind, ItemHierarchy itemHierarchy)
	{
		List<Item> list = GetItemsByKind(kind, itemHierarchy);
		if (list == null)
		{
			list = new List<Item>();
		}
		list.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
		return list;
	}

	public virtual List<Item> GetItemsBySearch(string searchText, int maxCount, int maxSeconds, out SearchResult searchResult)
	{
		searchResult = ItemSearch.Search(searchText, maxCount, maxSeconds, out var found, GetItems(ItemHierarchy.SystemDefined));
		return found;
	}

	public virtual void GetItemsBySearchAsync(string searchText, int maxCount, int maxSeconds, AsyncItemsSearchHandler asyncItemsHandler, Control control, object callerReference)
	{
		ItemSearch.SearchAsync(searchText, maxCount, maxSeconds, asyncItemsHandler, control, GetItems(ItemHierarchy.SystemDefined), callerReference);
	}

	public virtual List<Item> GetItemConfigurations(Guid pluginDefinitionId, Item parentItem, Guid itemKind)
	{
		return GetItemConfigurationsBase(EnvironmentManager.Instance.MasterSite.ServerId, parentItem, itemKind);
	}

	public virtual List<Item> GetItemConfigurations(ServerId serverId, Guid pluginDefinitionId, Item parentItem, Guid itemKind)
	{
		return GetItemConfigurationsBase(serverId, parentItem, itemKind);
	}

	private List<Item> GetItemConfigurationsBase(ServerId serverId, Item parentItem, Guid itemKind)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		if (itemKind == Guid.Empty)
		{
			throw new MIPException("Kind cannot be Empty");
		}
		ThrowIfNoEnvironment();
		if (!Factory.IsLoginEstablished(serverId))
		{
			return new List<Item>();
		}
		ConfigurationCache configurationCache = PrepareCache(serverId);
		if (configurationCache != null)
		{
			if (parentItem == null)
			{
				List<Item> items = configurationCache.GetItems(itemKind);
				if (items != null)
				{
					return items;
				}
			}
			else
			{
				List<Item> items = configurationCache.GetChildItems(itemKind, parentItem.FQID.ObjectId);
				if (items != null)
				{
					return items;
				}
			}
		}
		try
		{
			MIPItemFolder mIPItemFolder = ((parentItem == null) ? new MIPItemFolder(serverId, Factory.CreateFolderPath("MIPKind", itemKind, "MIPItemFolder")) : new MIPItemFolder(serverId, Factory.CreateFolderPath("MIPItem", parentItem.FQID.ObjectId, "MIPItemFolder")));
			List<Item> items = new List<Item>();
			foreach (MIPItem mIPItem in mIPItemFolder.MIPItems)
			{
				Item item = new Item(mIPItem);
				if (item.Properties.ContainsKey("EnabledPropertySpecialFormat") && item.Properties["EnabledPropertySpecialFormat"] == "Boolean")
				{
					item.Properties["Enabled"] = item.Enabled.ToString(CultureInfo.InvariantCulture);
					item.Properties.Remove("EnabledPropertySpecialFormat");
				}
				if (item.FQID.Kind == itemKind)
				{
					items.Add(item);
				}
			}
			if (configurationCache != null)
			{
				if (parentItem == null)
				{
					configurationCache.StoreList(itemKind, items, Guid.Empty, allItems: true);
				}
				else
				{
					configurationCache.StoreList(itemKind, items, parentItem.FQID.ObjectId, allItems: false);
				}
			}
			return items;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("GetItemConfigurations", exception);
			return new List<Item>();
		}
	}

	public virtual Item GetItemConfiguration(Guid pluginDefinitionId, Guid itemKind, Guid objectId)
	{
		return GetItemConfigurationBase(EnvironmentManager.Instance.MasterSite.ServerId, itemKind, objectId);
	}

	public virtual Item GetItemConfiguration(ServerId serverId, Guid pluginDefinitionId, Guid itemKind, Guid objectId)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		return GetItemConfigurationBase(serverId, itemKind, objectId);
	}

	private Item GetItemConfigurationBase(ServerId serverId, Guid itemKind, Guid objectId)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		if (itemKind == Guid.Empty || objectId == Guid.Empty)
		{
			return null;
		}
		ThrowIfNoEnvironment();
		if (!Factory.IsLoginEstablished(serverId))
		{
			return null;
		}
		ConfigurationCache configurationCache = PrepareCache(serverId);
		if (configurationCache != null)
		{
			Item item = configurationCache.GetItem(itemKind, objectId);
			if (item != null)
			{
				return item;
			}
		}
		MIPItemFolder mIPItemFolder = new MIPItemFolder(serverId, Factory.CreateFolderPath("MIPKind", itemKind, "MIPItemFolder"));
		try
		{
			Item item2 = mIPItemFolder.GetItem(objectId);
			if (item2.Properties.ContainsKey("EnabledPropertySpecialFormat") && item2.Properties["EnabledPropertySpecialFormat"] == "Boolean")
			{
				item2.Properties["Enabled"] = item2.Enabled.ToString(CultureInfo.InvariantCulture);
				item2.Properties.Remove("EnabledPropertySpecialFormat");
			}
			configurationCache?.Store(item2);
			return item2;
		}
		catch (PathNotFoundMIPException)
		{
			return null;
		}
	}

	public virtual long GetItemConfigurationDataVersion(Guid itemKind)
	{
		ThrowIfNoEnvironment();
		return GetItemConfigurationDataVersion(EnvironmentManager.Instance.MasterSite.ServerId, itemKind);
	}

	public virtual long GetItemConfigurationDataVersion(ServerId serverId, Guid itemKind)
	{
		return 0L;
	}

	public virtual DateTime GetItemConfigurationLastModified(ServerId serverId, Guid kind)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		if (kind == Guid.Empty)
		{
			return DateTime.MinValue;
		}
		ThrowIfNoEnvironment();
		return PrepareCache(serverId)?.GetLastModified(kind) ?? DateTime.MinValue;
	}

	private void UpdateLastModifiedFromCache(Item item)
	{
		ConfigurationCache configurationCache = PrepareCache(ServerFQID.ServerId);
		if (configurationCache != null)
		{
			Item item2 = configurationCache.GetItem(item.FQID.Kind, item.FQID.ObjectId);
			if (item2 != null && item2.Properties.ContainsKey("LastModified") && item.Properties.ContainsKey("LastModified") && DateTime.TryParse(item2.Properties["LastModified"], out var result) && DateTime.TryParse(item.Properties["LastModified"], out var result2) && result > result2)
			{
				item.Properties["LastModified"] = item2.Properties["LastModified"];
			}
		}
	}

	private void UpdateCache(Item item)
	{
		PrepareCache(ServerFQID.ServerId)?.Store(item);
	}

	public virtual void SaveItemConfiguration(Guid pluginDefinitionId, Item item)
	{
		SaveItemConfiguration(EnvironmentManager.Instance.MasterSite.ServerId, pluginDefinitionId, item);
	}

	public virtual void SaveItemConfiguration(ServerId serverId, Guid pluginDefinitionId, Item item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		ThrowIfNoEnvironment();
		if (item.Properties.Keys.Any((string k) => k.ToUpperInvariant().StartsWith("FQID")))
		{
			throw new InvalidParameterMIPException("item.Properties must not contain any property named or starting with 'FQID'");
		}
		UpdateLastModifiedFromCache(item);
		if (item.Properties.ContainsKey("Enabled") && bool.TryParse(item.Properties["Enabled"], out var _))
		{
			item.Properties["EnabledPropertySpecialFormat"] = "Boolean";
		}
		MIPItemFolder mIPItemFolder;
		if (item.FQID.ParentId != Guid.Empty)
		{
			try
			{
				mIPItemFolder = new MIPItem(serverId, item.FQID.ParentId).MIPItemFolder;
			}
			catch (MIPException)
			{
				mIPItemFolder = new MIPItemFolder(serverId, Factory.CreateFolderPath("MIPKind", item.FQID.Kind, "MIPItemFolder"));
			}
		}
		else
		{
			mIPItemFolder = new MIPItemFolder(serverId, Factory.CreateFolderPath("MIPKind", item.FQID.Kind, "MIPItemFolder"));
		}
		try
		{
			mIPItemFolder.UpdateItem(item);
		}
		catch (PathNotFoundMIPException)
		{
			try
			{
				mIPItemFolder.AddItem(item);
			}
			catch (ArgumentMIPException ex3)
			{
				if (ex3.Message == null || !ex3.Message.ToUpperInvariant().Contains(item.FQID.Kind.ToString().ToUpperInvariant()))
				{
					throw;
				}
				new MIPKindFolder(serverId).AddMIPKind(item.FQID.Kind.ToString(), "Auto-generated unknown kind", pluginDefinitionId.ToString(), "Unknown plug-in", "ITEM", displayOnGisMap: false, Guid.Empty, string.Empty);
				mIPItemFolder.AddItem(item);
			}
		}
		UpdateCache(item);
	}

	public virtual void DeleteItemConfiguration(Guid platformDefinitionId, Item item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		ThrowIfNoEnvironment();
		PrepareCache(ServerFQID.ServerId)?.Flush(item.FQID.Kind);
		new MIPItemFolder(EnvironmentManager.Instance.MasterSite.ServerId, Factory.CreateFolderPath("MIPKind", item.FQID.Kind, "MIPItemFolder")).RemoveMIPItem(item);
	}

	public virtual void DeleteItemConfiguration(ServerId serverId, Guid platformDefinitionId, Item item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		ThrowIfNoEnvironment();
		PrepareCache(serverId)?.Flush(item.FQID.Kind);
		new MIPItemFolder(serverId, Factory.CreateFolderPath("MIPKind", item.FQID.Kind, "MIPItemFolder")).RemoveMIPItem(item);
	}

	public virtual void RefreshConfiguration(Guid kind)
	{
		if (_configurationCacheServers.ContainsKey(ServerFQID.ServerId.Id))
		{
			_configurationCacheServers[ServerFQID.ServerId.Id].Flush(kind);
			MIPItemFolder.ClearCaches(kind);
			return;
		}
		lock (_configurationCacheServersLock)
		{
			_configurationCacheServers.Clear();
		}
		MIPItemFolder.ClearAllCaches();
	}

	public virtual void RefreshConfiguration(ServerId serverId, Guid kind)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		if (_configurationCacheServers.ContainsKey(serverId.Id))
		{
			_configurationCacheServers[serverId.Id].Flush(kind);
			MIPItemFolder.ClearCaches(kind);
		}
	}

	public virtual void BeginMassUpdate()
	{
	}

	public virtual void EndMassUpdate()
	{
	}

	public virtual XmlNode GetOptionsConfiguration(Guid optionsDialogId, bool userPrivate)
	{
		return null;
	}

	public virtual void SaveOptionsConfiguration(Guid optionsDialogId, bool userPrivate, XmlNode xmlNode)
	{
	}

	public virtual AssociatedProperties GetAssociatedProperties(Item item, Guid pluginId)
	{
		return new AssociatedProperties(item, pluginId);
	}

	public virtual void SaveAssociatedProperties(AssociatedProperties associatedProperties)
	{
		if (associatedProperties == null)
		{
			throw new ArgumentNullException("associatedProperties");
		}
		associatedProperties.Save();
	}

	public virtual List<ServiceURIInfo> GetRegisteredServiceUriInfo(ServerId serverId)
	{
		return ServiceUtil.GetRegisteredServiceUriInfo(serverId) ?? new List<ServiceURIInfo>();
	}

	public virtual List<ServiceURIInfo> GetRegisteredServiceUriInfo(Guid serviceType, ServerId serverId)
	{
		return ServiceUtil.GetRegisteredServiceUriInfo(serviceType, serverId) ?? new List<ServiceURIInfo>();
	}

	public virtual void RegisterServiceUri(Guid serviceType, ServerId serverId, Guid instance, Uri uri, string name, string description, string customData)
	{
		ServiceUtil.RegisterServiceUri(serviceType, serverId, instance, uri, name, description, customData);
	}

	public virtual void RegisterServiceUri(Guid serviceType, ServerId serverId, Guid instance, Uri uri, string name, string description, string customData, List<RegistrationServiceEndpoint> endpoints)
	{
		ServiceUtil.RegisterServiceUri(serviceType, serverId, instance, uri, name, description, customData, endpoints);
	}

	public virtual void UnRegisterServiceUri(ServerId serverId, Guid instance)
	{
		ServiceUtil.UnRegisterServiceUri(serverId, instance);
	}

	public virtual ReadOnlyCollection<ViewGroupInfo> GetViewGroups()
	{
		return new ReadOnlyCollection<ViewGroupInfo>(new Collection<ViewGroupInfo>());
	}

	public virtual VideoOS.Platform.Client.ViewGroup GetViewGroup(Guid groupId)
	{
		return null;
	}

	private ConfigurationCache PrepareCache(ServerId serverId)
	{
		if (serverId.Id == Guid.Empty)
		{
			return null;
		}
		if (!_configurationCacheServers.ContainsKey(serverId.Id))
		{
			lock (_configurationCacheServersLock)
			{
				_configurationCacheServers[serverId.Id] = new ConfigurationCache(serverId);
			}
		}
		return _configurationCacheServers[serverId.Id];
	}

	private static void ThrowIfNoEnvironment()
	{
		if (EnvironmentManager.Instance == null)
		{
			throw new MIPException("MIP Environment not yet initialized");
		}
	}

	public void Logoff()
	{
		ConfigurationApiManager.Close();
	}
}
