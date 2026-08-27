using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.ServiceModel;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MIPItemFolder : IConfigurationItem
{
	private static Dictionary<Guid, List<WeakReference<MIPItemFolder>>> _cachedFolders = new Dictionary<Guid, List<WeakReference<MIPItemFolder>>>();

	private static object _cachedFoldersLock = new object();

	private Guid _kind = Guid.Empty;

	public ICollection<MIPItem> MIPItems
	{
		get
		{
			RegisterThisFolderCached();
			ConfigurationItem[] array = base.Wrapper.Children();
			List<MIPItem> list = new List<MIPItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "MIPItem")
				{
					list.Add(new MIPItem(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public MIPItemFolder(ServerId serverId, ConfigurationItem mipItemFolder)
		: base(serverId, mipItemFolder)
	{
		if (mipItemFolder != null)
		{
			string idFromPath = Factory.GetIdFromPath(mipItemFolder.Path);
			if (idFromPath != null && !Guid.TryParse(idFromPath, out _kind))
			{
				_kind = Guid.Empty;
			}
		}
	}

	public MIPItemFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
		string idFromPath = Factory.GetIdFromPath(path);
		if (idFromPath != null && !Guid.TryParse(idFromPath, out _kind))
		{
			_kind = Guid.Empty;
		}
	}

	public MIPItem GetMIPItem(Guid objectId)
	{
		if (objectId == Guid.Empty)
		{
			throw new ArgumentException("GUID cannot be Empty");
		}
		ConfigurationItem item = base.Wrapper.GetItem(string.Format(CultureInfo.InvariantCulture, "MIPItem[{0}]", objectId));
		return new MIPItem(base.ServerId, item);
	}

	private void RegisterThisFolderCached()
	{
		if (!(_kind != Guid.Empty))
		{
			return;
		}
		lock (_cachedFoldersLock)
		{
			List<WeakReference<MIPItemFolder>> list;
			if (_cachedFolders.ContainsKey(_kind))
			{
				list = _cachedFolders[_kind];
			}
			else
			{
				list = new List<WeakReference<MIPItemFolder>>();
				_cachedFolders.Add(_kind, list);
			}
			List<WeakReference<MIPItemFolder>> list2 = new List<WeakReference<MIPItemFolder>>();
			MIPItemFolder target = null;
			foreach (WeakReference<MIPItemFolder> item in list)
			{
				if (item.TryGetTarget(out target))
				{
					if (target == this)
					{
						break;
					}
					target = null;
				}
				else
				{
					list2.Add(item);
				}
			}
			foreach (WeakReference<MIPItemFolder> item2 in list2)
			{
				list.Remove(item2);
			}
			if (target == null)
			{
				list.Add(new WeakReference<MIPItemFolder>(this));
			}
		}
	}

	public static void ClearAllCaches()
	{
		lock (_cachedFoldersLock)
		{
			foreach (List<WeakReference<MIPItemFolder>> value in _cachedFolders.Values)
			{
				foreach (WeakReference<MIPItemFolder> item in value)
				{
					MIPItemFolder target = null;
					if (item.TryGetTarget(out target))
					{
						target.ClearChildrenCache();
					}
				}
			}
			_cachedFolders.Clear();
		}
	}

	public static void ClearCaches(Guid itemKind)
	{
		if (itemKind == Guid.Empty)
		{
			ClearAllCaches();
			return;
		}
		lock (_cachedFoldersLock)
		{
			if (!_cachedFolders.ContainsKey(itemKind))
			{
				return;
			}
			foreach (WeakReference<MIPItemFolder> item in _cachedFolders[itemKind])
			{
				MIPItemFolder target = null;
				if (item.TryGetTarget(out target))
				{
					target.ClearChildrenCache();
				}
			}
			_cachedFolders.Remove(itemKind);
		}
	}

	public ServerTask AddMIPItem(string name)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddMIPItem");
			configurationApiItemWrapper.SetProperty("Name", name);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddMIPItem"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MIPItemFolder", "Unable to call method 'AddMIPItem':" + ex.Message);
		}
		return null;
	}

	public AddMIPItemServerTask AddMIPItem()
	{
		try
		{
			return new AddMIPItemServerTask(base.Wrapper.ExecuteMethod("AddMIPItem"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MIPItemFolder", "Unable to call method 'AddMIPItem':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveMIPItem(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveMIPItem");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveMIPItem"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MIPItemFolder", "Unable to call method 'RemoveMIPItem':" + ex.Message);
		}
		return null;
	}

	public RemoveMIPItemServerTask RemoveMIPItem()
	{
		try
		{
			return new RemoveMIPItemServerTask(base.Wrapper.ExecuteMethod("RemoveMIPItem"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MIPItemFolder", "Unable to call method 'RemoveMIPItem':" + ex.Message);
		}
		return null;
	}

	public Item GetItem(Guid objectId)
	{
		if (objectId == Guid.Empty)
		{
			throw new ArgumentException("GUID cannot be Empty");
		}
		ConfigurationItem item = base.Wrapper.GetItem(string.Format(CultureInfo.InvariantCulture, "MIPItem[{0}]", objectId));
		Item item2 = new Item();
		Factory.FillMIPItemInItem(new MIPItem(base.ServerId, item), item2);
		return item2;
	}

	public void UpdateItem(Item item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		ConfigurationItem item2;
		try
		{
			item2 = base.Wrapper.GetItem(string.Format(CultureInfo.InvariantCulture, "MIPItem[{0}]", item.FQID.ObjectId));
		}
		catch (FaultException<PathNotFoundExceptionFault>)
		{
			throw new PathNotFoundMIPException("Item not found on server");
		}
		ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(base.Wrapper.ServerId, item2);
		Item item3 = Factory.ConvertMIPItemToItem(item2);
		if (item3.FQID.ObjectId != item.FQID.ObjectId)
		{
			throw new ArgumentMIPException("Cannot update the ObjectId in the FQID");
		}
		if (item3.FQID.Kind != item.FQID.Kind)
		{
			throw new ArgumentMIPException("Cannot update the Kind in the FQID");
		}
		configurationApiItemWrapper.SetProperty("Name", item.Name);
		if (configurationApiItemWrapper.GetProperty("Enabled") != null)
		{
			configurationApiItemWrapper.SetProperty("Enabled", item.Enabled.ToString());
		}
		if (item.PositioningInformation != null)
		{
			configurationApiItemWrapper.SetProperty("GisPoint", item.PositioningInformation.ToString());
		}
		if (configurationApiItemWrapper.ConfigurationItem.EnableProperty != null && configurationApiItemWrapper.ConfigurationItem.EnableProperty.Enabled != item.Enabled)
		{
			configurationApiItemWrapper.ConfigurationItem.EnableProperty.Enabled = item.Enabled;
			configurationApiItemWrapper.IsDirty = true;
		}
		foreach (string propertyKey in configurationApiItemWrapper.GetPropertyKeys())
		{
			switch (propertyKey)
			{
			case "Id":
			case "Name":
			case "Enabled":
			case "GisPoint":
			case "__Encrypt":
			case "LastModified":
				continue;
			}
			if (!propertyKey.StartsWith("FQID.", StringComparison.OrdinalIgnoreCase))
			{
				configurationApiItemWrapper.RemoveProperty(propertyKey);
			}
		}
		foreach (KeyValuePair<string, string> property in item.Properties)
		{
			configurationApiItemWrapper.SetProperty(property.Key, property.Value);
		}
		configurationApiItemWrapper.SetProperty("FQID.ParentId", item.FQID.ParentId.ToString());
		configurationApiItemWrapper.SetProperty("FQID.ServerId", item.FQID.ServerId.Id.ToString());
		configurationApiItemWrapper.SetProperty("FQID.Scheme", item.FQID.ServerId.ServerScheme);
		configurationApiItemWrapper.SetProperty("FQID.Host", item.FQID.ServerId.ServerHostname);
		configurationApiItemWrapper.SetProperty("FQID.Port", item.FQID.ServerId.Serverport.ToString(CultureInfo.InvariantCulture));
		configurationApiItemWrapper.SetProperty("FQID.Type", item.FQID.ServerId.ServerType);
		configurationApiItemWrapper.Save();
		item.Properties["LastModified"] = configurationApiItemWrapper.GetProperty("LastModified");
	}

	public void AddItem(Item item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		try
		{
			ConfigurationItemPath configurationItemPath = new ConfigurationItemPath(base.Wrapper.Path);
			ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(base.Wrapper.ServerId, new ConfigurationItem
			{
				MethodIds = new string[1] { "AddMIPItem" },
				ItemType = ConfigurationApiItemWrapper.ItemTypes_InvokeInfo,
				Path = base.Wrapper.Path,
				ParentPath = ConfigurationItemPath.CreateItemPath(configurationItemPath.ParentItemType, configurationItemPath.Id).Path,
				EnableProperty = new EnablePropertyInfo()
			}, base.Wrapper);
			configurationApiItemWrapper.SetProperty("Id", item.FQID.ObjectId.ToString());
			configurationApiItemWrapper.SetProperty("Name", item.Name);
			configurationApiItemWrapper.SetProperty("FQID.Kind", item.FQID.Kind.ToString());
			configurationApiItemWrapper.SetProperty("FQID.ParentId", item.FQID.ParentId.ToString());
			configurationApiItemWrapper.SetProperty("FQID.ServerId", item.FQID.ServerId.Id.ToString());
			configurationApiItemWrapper.SetProperty("FQID.Scheme", item.FQID.ServerId.ServerScheme);
			configurationApiItemWrapper.SetProperty("FQID.Host", item.FQID.ServerId.ServerHostname);
			configurationApiItemWrapper.SetProperty("FQID.Port", item.FQID.ServerId.Serverport.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("FQID.Type", item.FQID.ServerId.ServerType);
			if (configurationApiItemWrapper.ConfigurationItem.EnableProperty != null && configurationApiItemWrapper.ConfigurationItem.EnableProperty.Enabled != item.Enabled)
			{
				configurationApiItemWrapper.ConfigurationItem.EnableProperty.Enabled = item.Enabled;
				configurationApiItemWrapper.IsDirty = true;
			}
			if (item.PositioningInformation != null)
			{
				configurationApiItemWrapper.SetProperty("GisPoint", item.PositioningInformation.ToString());
			}
			foreach (KeyValuePair<string, string> property in item.Properties)
			{
				configurationApiItemWrapper.SetProperty(property.Key, property.Value);
			}
			ServerTask serverTask = new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddMIPItem"));
			if (serverTask.State != StateEnum.Success)
			{
				throw new ArgumentMIPException("Unable to add MIPItem: " + serverTask.ErrorText);
			}
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MIPItemFolder", "Unable to call method 'AddMIPItem':" + ex.Message);
		}
	}

	public void RemoveMIPItem(Item item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveMIPItem");
			configurationApiItemWrapper.SetProperty("ItemSelection", string.Format(CultureInfo.InvariantCulture, "MIPItem[{0}]", item.FQID.ObjectId));
			ServerTask serverTask = new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveMIPItem"));
			if (serverTask.State != StateEnum.Success)
			{
				throw new MIPException("Unable to remove: " + serverTask.ErrorText);
			}
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MIPItemFolder", "Unable to call method 'RemoveMIPItem':" + ex.Message);
		}
	}
}
