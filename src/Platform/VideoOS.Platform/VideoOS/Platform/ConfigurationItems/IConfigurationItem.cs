using System;
using System.Collections.ObjectModel;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public class IConfigurationItem : IConfigurationChildItem
{
	public Collection<string> Methods
	{
		get
		{
			if (base.Wrapper.MethodIds == null)
			{
				return new Collection<string>();
			}
			return new Collection<string>(base.Wrapper.MethodIds);
		}
	}

	public Guid Guid => base.Wrapper.Id;

	public IConfigurationItem()
	{
	}

	public IConfigurationItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public IConfigurationItem(ServerId serverId, ConfigurationItem configurationItem)
		: base(serverId, configurationItem, null)
	{
	}

	public IConfigurationItem(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask ExecuteMethod(string methodId)
	{
		return new ServerTask(base.Wrapper.ExecuteMethod(methodId));
	}

	public void ClearChildrenCache()
	{
		base.Wrapper.ClearChildrenCache();
	}

	public ValidateResult ValidateItem()
	{
		try
		{
			return base.Wrapper.ValidateItem();
		}
		catch (Exception ex)
		{
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	public void Save()
	{
		try
		{
			base.Wrapper.Save();
		}
		catch (Exception ex)
		{
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	public void FillChildren(string[] itemTypes)
	{
		if (itemTypes == null)
		{
			throw new ArgumentNullException("itemTypes");
		}
		base.Wrapper.FillChildren(itemTypes, new ItemFilter[0]);
	}

	public void FillChildren(string[] itemTypes, ItemFilter[] itemFilters)
	{
		if (itemTypes == null)
		{
			throw new ArgumentNullException("itemTypes");
		}
		if (itemFilters == null)
		{
			throw new ArgumentNullException("itemFilters");
		}
		base.Wrapper.FillChildren(itemTypes, itemFilters);
	}
}
