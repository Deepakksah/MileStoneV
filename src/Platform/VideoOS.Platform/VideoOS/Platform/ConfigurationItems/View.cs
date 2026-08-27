using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class View : IConfigurationItem
{
	public ICollection<ViewItemChildItem> ViewItemChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ViewItemChildItem> list = new List<ViewItemChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ViewItem")
				{
					list.Add(new ViewItemChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public string Shortcut
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Shortcut");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Shortcut", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ViewLayoutType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ViewLayoutType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ViewLayoutType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string LayoutCustomId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("LayoutCustomId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("LayoutCustomId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string LayoutIcon
	{
		get
		{
			return base.Wrapper.GetPropertyValue("LayoutIcon");
		}
		set
		{
			base.Wrapper.SetPropertyValue("LayoutIcon", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string LayoutViewItems
	{
		get
		{
			return base.Wrapper.GetPropertyValue("LayoutViewItems");
		}
		set
		{
			base.Wrapper.SetPropertyValue("LayoutViewItems", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ClientId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ClientId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ClientId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public View(ServerId serverId, ConfigurationItem view)
		: base(serverId, view)
	{
	}

	public View(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public View(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask ApplyViewToView(string viewToApply, string clientId)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ApplyViewToView");
			configurationApiItemWrapper.SetProperty("ViewToApply", viewToApply);
			configurationApiItemWrapper.SetProperty("ClientId", clientId);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ApplyViewToView"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "View", "Unable to call method 'ApplyViewToView':" + ex.Message);
		}
		return null;
	}

	public ApplyViewToViewServerTask ApplyViewToView()
	{
		try
		{
			return new ApplyViewToViewServerTask(base.Wrapper.ExecuteMethod("ApplyViewToView"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "View", "Unable to call method 'ApplyViewToView':" + ex.Message);
		}
		return null;
	}

	public ServerTask ClearView(string clientId)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ClearView");
			configurationApiItemWrapper.SetProperty("ClientId", clientId);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ClearView"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "View", "Unable to call method 'ClearView':" + ex.Message);
		}
		return null;
	}

	public ClearViewServerTask ClearView()
	{
		try
		{
			return new ClearViewServerTask(base.Wrapper.ExecuteMethod("ClearView"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "View", "Unable to call method 'ClearView':" + ex.Message);
		}
		return null;
	}

	public ServerTask ClearViewItem(string viewItemId, string clientId)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ClearViewItem");
			configurationApiItemWrapper.SetProperty("ViewItemId", viewItemId);
			configurationApiItemWrapper.SetProperty("ClientId", clientId);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ClearViewItem"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "View", "Unable to call method 'ClearViewItem':" + ex.Message);
		}
		return null;
	}

	public ClearViewItemServerTask ClearViewItem()
	{
		try
		{
			return new ClearViewItemServerTask(base.Wrapper.ExecuteMethod("ClearViewItem"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "View", "Unable to call method 'ClearViewItem':" + ex.Message);
		}
		return null;
	}
}
