using System;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public class ViewItem : IConfigurationItem
{
	private ConfigurationApiItemWrapper _wrapper;

	public Guid ViewItemType
	{
		get
		{
			return new Guid(_wrapper.GetPropertyValue("ViewItemType"));
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public ViewItem(ServerId serverId, ConfigurationItem viewItem)
		: base(serverId, viewItem)
	{
		try
		{
			_wrapper = new ConfigurationApiItemWrapper(serverId, viewItem);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, GetType().Name, ex.Message);
		}
	}
}
