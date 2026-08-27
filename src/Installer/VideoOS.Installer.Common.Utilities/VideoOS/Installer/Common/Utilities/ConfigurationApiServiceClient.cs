using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Installer.Common.Utilities;

[CLSCompliant(false)]
public class ConfigurationApiServiceClient : ClientBase<IConfigurationService>, IConfigurationService
{
	public ConfigurationApiServiceClient()
	{
	}

	public ConfigurationApiServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ConfigurationApiServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ConfigurationApiServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ConfigurationApiServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public ConfigurationItem GetItem(string path)
	{
		return base.Channel.GetItem(path);
	}

	public ConfigurationItem[] GetItems(string itemType)
	{
		return base.Channel.GetItems(itemType);
	}

	public ValidateResult SetItem(ConfigurationItem item)
	{
		return base.Channel.SetItem(item);
	}

	public bool HasChildItems(string path)
	{
		return base.Channel.HasChildItems(path);
	}

	public ConfigurationItem[] GetChildItems(string path)
	{
		return base.Channel.GetChildItems(path);
	}

	public ConfigurationItem ConstructItem(string itemType, Property[] properties)
	{
		return base.Channel.ConstructItem(itemType, properties);
	}

	public ConfigurationItem ConstructChildItem(string itemType, string parentPath)
	{
		return base.Channel.ConstructChildItem(itemType, parentPath);
	}

	public ValidateResult ValidateItem(ConfigurationItem item)
	{
		return base.Channel.ValidateItem(item);
	}

	public void DeleteItem(string path)
	{
		base.Channel.DeleteItem(path);
	}

	public ConfigurationItem[] GetDependantItems(string path)
	{
		return base.Channel.GetDependantItems(path);
	}

	public MethodInfo[] GetMethodInfos()
	{
		return base.Channel.GetMethodInfos();
	}

	public MethodInfo GetMethodInfo(string methodId)
	{
		return base.Channel.GetMethodInfo(methodId);
	}

	public ConfigurationItem InvokeMethod(ConfigurationItem item, string method)
	{
		return base.Channel.InvokeMethod(item, method);
	}

	public Dictionary<string, string> GetTranslations(string languageId)
	{
		return base.Channel.GetTranslations(languageId);
	}

	public ConfigurationItem[] GetChildItemsHierarchy(string path, string[] itemTypes, ItemFilter[] itemFilters)
	{
		return base.Channel.GetChildItemsHierarchy(path, itemTypes, itemFilters);
	}

	public ConfigurationItem[] QueryItems(ItemFilter itemFilter, int maxResult)
	{
		return base.Channel.QueryItems(itemFilter, maxResult);
	}
}
