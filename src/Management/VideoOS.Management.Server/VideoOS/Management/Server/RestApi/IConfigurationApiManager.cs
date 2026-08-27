using System.Collections.Generic;
using System.Net;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

public interface IConfigurationApiManager
{
	INameConvert NameConvert { get; set; }

	ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath pathInstance);

	ConfigurationItem GetItemWithChildren(IRestApiRequestContext requestContext, ConfigurationItemPath pathInstance);

	ConfigurationItem[] LookupArray(IRestApiRequestContext context, string itemType);

	int LookupItemCount(IRestApiRequestContext context, string itemType);

	ConfigurationItem LookupFolder(IRestApiRequestContext context, ConfigurationItemPath path);

	ConfigurationItem[] LookupChildItemTypes(IRestApiRequestContext context, ConfigurationItemPath path);

	ConfigurationItem[] LookupChildArray(IRestApiRequestContext context, string itemType, string id, string childItemType);

	ValidateResult Update(IRestApiRequestContext context, ConfigurationItem item);

	ConfigurationItem Delete(IRestApiRequestContext context, ConfigurationItem parent, ConfigurationItemPath path);

	HttpStatusCode InvokeMethodAndSerialize(IRestApiRequestContext context, JsonBuilder jb, ConfigurationItem item, string methodToCall, out ConfigurationItem invokeInfo);

	ConfigurationItem InvokeMethod(IRestApiRequestContext context, ConfigurationItem item, string methodId);

	MethodInfo[] GetMethodDefinitions();

	Dictionary<string, string> GetTranslations(string languageId);

	string InsertOemNames(string value);

	PropertyFilter ConvertParamToFilter(KeyValuePair<string, string> parameter);

	ConfigurationItem[] Pagination(IRestApiRequestContext context, ConfigurationItem[] items);

	ItemFilter ConstructItemFilter(IRestApiRequestContext context, string itemType);
}
