using System;
using System.Collections.Generic;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public class IConfigurationChildItem
{
	private ConfigurationApiItemWrapper _wrapper;

	private ServerId _serverId;

	public ConfigurationApiItemWrapper Wrapper
	{
		get
		{
			return _wrapper;
		}
		set
		{
			_wrapper = value;
		}
	}

	public ServerId ServerId
	{
		get
		{
			return _serverId;
		}
		public set
		{
			_serverId = value;
		}
	}

	public string Name
	{
		get
		{
			return Wrapper.GetPropertyValue("Name");
		}
		set
		{
			Wrapper.SetPropertyValue("Name", value);
			Wrapper.IsDirty = true;
		}
	}

	public virtual string DisplayName => Wrapper.ConfigurationItem.DisplayName;

	public string Path => Wrapper.ConfigurationItem.Path;

	public string ParentPath => Wrapper.ConfigurationItem.ParentPath;

	public string ItemCategory => Wrapper.ConfigurationItem.ItemCategory;

	public string Description
	{
		get
		{
			return Wrapper.GetPropertyValue("Description");
		}
		set
		{
			Wrapper.SetPropertyValue("Description", value);
			Wrapper.IsDirty = true;
		}
	}

	public string ParentItemPath
	{
		get
		{
			ConfigurationItemPath configurationItemPath = new ConfigurationItemPath(ParentPath);
			if (configurationItemPath.ItemType.EndsWith("Folder"))
			{
				return new ConfigurationItemPath(configurationItemPath.ParentItemType, configurationItemPath.Id).Path;
			}
			return configurationItemPath.Path;
		}
	}

	public IConfigurationChildItem()
	{
	}

	public IConfigurationChildItem(ServerId serverId, string path)
	{
		try
		{
			_serverId = EnsureManagementServerId(serverId);
			_wrapper = new ConfigurationApiItemWrapper(_serverId, path);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, GetType().Name, ex.Message);
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	public IConfigurationChildItem(ServerId serverId, ConfigurationItem configurationItem, ConfigurationApiItemWrapper parentWrapper)
	{
		try
		{
			_serverId = EnsureManagementServerId(serverId);
			_wrapper = new ConfigurationApiItemWrapper(_serverId, configurationItem, parentWrapper);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, GetType().Name, ex.Message);
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	public IConfigurationChildItem(FQID fqid)
	{
		try
		{
			ServerId serverId = ((!(fqid.ServerId.ServerType != "XPCO")) ? fqid.ServerId : EnsureManagementServerId(fqid.ServerId));
			string itemTypeFromKind = Factory.GetItemTypeFromKind(fqid.Kind);
			if ((itemTypeFromKind == null || (itemTypeFromKind != "MIPItem" && itemTypeFromKind != GetType().Name)) && (fqid.Kind != Kind.Server || ((!(fqid.ServerId.ServerType == "XPCO") || !(GetType() == typeof(ManagementServer))) && (!(fqid.ServerId.ServerType == "XPCORS") || !(GetType() == typeof(RecordingServer))))))
			{
				throw new ArgumentException("Invalid kind for this constructor", "fqid");
			}
			string path = Factory.CreatePath(fqid);
			_serverId = serverId;
			_wrapper = new ConfigurationApiItemWrapper(serverId, path);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, GetType().Name, ex.Message);
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	public string GetProperty(string key)
	{
		return Wrapper.GetProperty(key);
	}

	public void SetProperty(string key, string content)
	{
		Wrapper.SetPropertyValue(key, content);
	}

	public ICollection<string> GetPropertyKeys()
	{
		return Wrapper.GetPropertyKeys();
	}

	public List<ValueTypeInfo> GetValueTypeInfoList(string propertyKey)
	{
		return Wrapper.GetValueTypeInfoList(propertyKey);
	}

	public override string ToString()
	{
		return DisplayName;
	}

	public static ServerId EnsureManagementServerId(ServerId serverId)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		if (serverId.ServerType == "XPCO")
		{
			return serverId;
		}
		if (serverId.ServerType == "XPCORS")
		{
			Item item = ((serverId.UserContext == null) ? Configuration.Instance.GetItem(serverId.Id, Kind.Server) : serverId.UserContext.Configuration.GetItem(serverId.Id, Kind.Server));
			if (item != null)
			{
				return item.FQID.GetParent().ServerId;
			}
		}
		throw new ArgumentMIPException("Server with serverId not found");
	}
}
