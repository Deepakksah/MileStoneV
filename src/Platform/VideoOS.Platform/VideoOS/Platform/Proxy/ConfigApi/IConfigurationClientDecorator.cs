using System.Collections.Generic;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.Proxy.ConfigApi;

public interface IConfigurationClientDecorator
{
	void UpdateLoginSettingsIfNoToken(LoginSettings loginSettings);

	ConfigurationItem GetItem(string path);

	ConfigurationItem[] GetItems(string itemType);

	ValidateResult SetItem(ConfigurationItem item);

	bool HasChildItems(string path);

	ConfigurationItem[] GetChildItems(string path);

	ConfigurationItem[] GetChildItemsHierarchy(string path, string[] itemTypes, ItemFilter[] filters);

	ConfigurationItem[] QueryItems(ItemFilter filter, int maxResults);

	ValidateResult ValidateItem(ConfigurationItem item);

	void DeleteItem(string path);

	MethodInfo[] GetMethodInfos();

	MethodInfo GetMethodInfo(string methodId);

	ConfigurationItem InvokeMethod(ConfigurationItem item, string method);

	Dictionary<string, string> GetTranslations(string languageId);

	void CloseConnection();
}
