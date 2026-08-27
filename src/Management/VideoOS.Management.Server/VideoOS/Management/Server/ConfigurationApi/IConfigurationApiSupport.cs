using System.Collections.Generic;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi;

public interface IConfigurationApiSupport
{
	bool SupportGetChildItemsWithItemTypes { get; }

	ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path, string[] itemTypes, ItemFilter[] filters, int maxResult, int startIndex, string[] childItemTypes, ItemFilter[] childFilters);

	ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method);

	ConfigurationItem[] QueryItems(IGeminiRequestContext requestContext, ConfigurationItemPath folderPath, ItemFilter itemFilter, int maxResults, int startResult, List<ItemFilter> childItemFilters);

	int QueryItemCount(IGeminiRequestContext requestContext, ConfigurationItemPath folderPath, ItemFilter itemFilter);
}
