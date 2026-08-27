using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Resources;
using VideoOS.Common.DriverResources;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Management.Server.Properties;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

public class ConfigurationApiManager : IConfigurationApiManager
{
	private static string maxResultsString = Config.Instance.GetAppSetting("RestAPI.MaxReturnedItems", "9999");

	public static Dictionary<string, Operator> _opreratorToFilter = new Dictionary<string, Operator>
	{
		{
			"lt",
			Operator.LessThan
		},
		{
			"gt",
			Operator.GreaterThan
		},
		{
			"contains",
			Operator.Contains
		},
		{
			"startsWith",
			Operator.BeginsWith
		},
		{
			"notEquals",
			Operator.NotEquals
		}
	};

	public INameConvert NameConvert { get; set; } = new NameConvert();

	private static int maxResults
	{
		get
		{
			if (!int.TryParse(maxResultsString, out var result))
			{
				return 9999;
			}
			return result;
		}
	}

	public static IConfigurationApiWrapper ConfigurationApi { get; set; } = new ConfigurationApiWrapper();

	private static IRestSerializerFactory RestSerializerFactory { get; } = new RestSerializerFactory();

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath pathInstance)
	{
		return ConfigurationApi.GetObject(pathInstance.ItemType).GetItem(requestContext, pathInstance);
	}

	public ConfigurationItem GetItemWithChildren(IRestApiRequestContext requestContext, ConfigurationItemPath pathInstance)
	{
		IConfigurationApiSupport configurationApiSupport = ConfigurationApi.GetObject(pathInstance.ItemType);
		ConfigurationItem item = configurationApiSupport.GetItem(requestContext, pathInstance);
		List<ItemFilter> list = new List<ItemFilter>();
		foreach (string resourceName in requestContext.RestApiRequestParameters.FilterHandler.ResourceNames)
		{
			string itemType = NameConvert.ResourcePluralToItemType(resourceName);
			list.Add(new ItemFilter(itemType, new PropertyFilter[0], (!requestContext.RestApiRequestParameters.Disabled) ? EnableFilter.Enabled : EnableFilter.All));
		}
		AddChildren(requestContext, configurationApiSupport, item, pathInstance, list);
		return item;
	}

	private void AddChildren(IRestApiRequestContext requestContext, IConfigurationApiSupport obj, ConfigurationItem item, ConfigurationItemPath pathInstance, List<ItemFilter> itemFilters)
	{
		List<string> list = new List<string>();
		foreach (string resourceName in requestContext.RestApiRequestParameters.FilterHandler.ResourceNames)
		{
			list.Add(NameConvert.ResourcePluralToItemType(resourceName));
		}
		ConfigurationItem[] childItems = obj.GetChildItems(requestContext, pathInstance, list.ToArray(), itemFilters.ToArray(), 100, 0, list.ToArray(), itemFilters.ToArray());
		if (item.Children == null)
		{
			item.Children = childItems;
		}
		else
		{
			List<ConfigurationItem> list2 = new List<ConfigurationItem>(item.Children);
			list2.AddRange(childItems);
			item.Children = list2.ToArray();
		}
		item.ChildrenFilled = true;
	}

	public ConfigurationItem[] LookupArray(IRestApiRequestContext context, string itemType)
	{
		int num = ((context.RestApiRequestParameters.PageSize != 0) ? context.RestApiRequestParameters.PageSize : maxResults);
		int startResult = context.RestApiRequestParameters.PageStart * context.RestApiRequestParameters.PageSize;
		ItemFilter itemFilter = ConstructItemFilter(context, itemType);
		if (string.IsNullOrEmpty(itemType))
		{
			ConfigurationItem item = ConfigurationApi.GetObject("System").GetItem(context, ConfigurationItemPath.CreateSystemPath());
			return new ConfigurationItem[1] { item };
		}
		ConfigurationItemPath path;
		if (itemType == "Task")
		{
			IConfigurationApiSupport configurationApiSupport = ConfigurationApi.GetObject("System");
			ConfigurationItem item2 = configurationApiSupport.GetItem(context, ConfigurationItemPath.CreateSystemPath());
			path = ConfigurationItemPath.CreateSystemPath();
			ConfigurationItem configurationItem = configurationApiSupport.InvokeMethod(context, item2, path, null, "LoadTasks");
			if (configurationItem.ItemType == "InvokeInfo")
			{
				List<Property> list = new List<Property>(configurationItem.Properties.Where((Property p) => !string.Equals(p.Key, "IncludeRecordingServerTasks", StringComparison.OrdinalIgnoreCase))) { ManagementServerPropertyDefinition.IncludeRecordingServerTasksProperty(value: true) };
				configurationItem.Properties = list.ToArray();
				configurationItem = configurationApiSupport.InvokeMethod(context, configurationItem, path, null, "LoadTasks");
			}
			return FilterItems(context, configurationItem.Children, itemType);
		}
		if (SpecialCases.UseQueryForMethodSupportedForArray(itemType))
		{
			path = SpecialCases.GetConfigurationItemPath(itemType);
		}
		else
		{
			itemType += "Folder";
			path = ConfigurationItemPath.CreateSystemFolderPath(itemType);
		}
		List<ItemFilter> list2 = new List<ItemFilter>();
		if (context.RestApiRequestParameters.FilterHandler != null && context.RestApiRequestParameters.FilterHandler.Any())
		{
			foreach (string resourceName in context.RestApiRequestParameters.FilterHandler.ResourceNames)
			{
				list2.Add(new ItemFilter(NameConvert.ResourcePluralToItemType(resourceName), itemFilter.PropertyFilters, EnableFilter.Enabled));
			}
		}
		IConfigurationApiSupport configurationApiSupport2 = ConfigurationApi.GetObject(itemType);
		if (itemType == "FailoverGroupFolder" || itemType == "FailoverRecorderFolder")
		{
			return configurationApiSupport2.GetChildItems(context, path);
		}
		return configurationApiSupport2.QueryItems(context, path, itemFilter, num, startResult, list2);
	}

	public int LookupItemCount(IRestApiRequestContext context, string itemType)
	{
		ItemFilter itemFilter = ConstructItemFilter(context, itemType);
		if (string.IsNullOrEmpty(itemType))
		{
			return 1;
		}
		ConfigurationItemPath path;
		if (itemType == "Task")
		{
			IConfigurationApiSupport configurationApiSupport = ConfigurationApi.GetObject("System");
			ConfigurationItem item = configurationApiSupport.GetItem(context, ConfigurationItemPath.CreateSystemPath());
			path = ConfigurationItemPath.CreateSystemPath();
			ConfigurationItem configurationItem = configurationApiSupport.InvokeMethod(context, item, path, null, "LoadTasks");
			if (configurationItem.ItemType == "InvokeInfo")
			{
				List<Property> list = new List<Property>(configurationItem.Properties.Where((Property p) => !string.Equals(p.Key, "IncludeRecordingServerTasks", StringComparison.OrdinalIgnoreCase))) { ManagementServerPropertyDefinition.IncludeRecordingServerTasksProperty(value: true) };
				configurationItem.Properties = list.ToArray();
				configurationItem = configurationApiSupport.InvokeMethod(context, configurationItem, path, null, "LoadTasks");
			}
			return configurationItem.Children.Length;
		}
		if (SpecialCases.UseQueryForMethodSupportedForArray(itemType))
		{
			path = SpecialCases.GetConfigurationItemPath(itemType);
		}
		else
		{
			itemType += "Folder";
			path = ConfigurationItemPath.CreateSystemFolderPath(itemType);
		}
		new List<ItemFilter>();
		return ConfigurationApi.GetObject(itemType).QueryItemCount(context, path, itemFilter);
	}

	public ConfigurationItem LookupFolder(IRestApiRequestContext context, ConfigurationItemPath path)
	{
		return ConfigurationApi.GetObject(path.ItemType).GetItem(context, path);
	}

	public ConfigurationItem[] LookupChildItemTypes(IRestApiRequestContext context, ConfigurationItemPath path)
	{
		return ConfigurationApi.GetObject(path.ItemType).GetChildItems(context, path);
	}

	public ConfigurationItem[] LookupChildArray(IRestApiRequestContext context, string itemType, string id, string childItemType)
	{
		if (childItemType == "TaskFolder")
		{
			if (itemType == "RecordingServer")
			{
				IConfigurationApiSupport configurationApiSupport = ConfigurationApi.GetObject(itemType);
				ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath(itemType, id);
				ConfigurationItem item = configurationApiSupport.GetItem(context, path);
				ConfigurationItemPath parentPath = ConfigurationItemPath.CreatePath(item.ParentPath);
				ConfigurationItem item2 = configurationApiSupport.InvokeMethod(context, item, path, parentPath, "LoadTasks");
				ConfigurationItem configurationItem = configurationApiSupport.InvokeMethod(context, item2, path, parentPath, "LoadTasks");
				return FilterItems(context, configurationItem.Children, childItemType);
			}
			if (string.IsNullOrEmpty(itemType))
			{
				IConfigurationApiSupport configurationApiSupport2 = ConfigurationApi.GetObject("System");
				ConfigurationItem item3 = configurationApiSupport2.GetItem(context, ConfigurationItemPath.CreateSystemPath());
				ConfigurationItemPath path2 = ConfigurationItemPath.CreateSystemPath();
				List<Property> list = new List<Property>(item3.Properties) { ManagementServerPropertyDefinition.IncludeRecordingServerTasksProperty(value: true) };
				item3.Properties = list.ToArray();
				ConfigurationItem configurationItem2 = configurationApiSupport2.InvokeMethod(context, item3, path2, null, "LoadTasks");
				return FilterItems(context, configurationItem2.Children, childItemType);
			}
		}
		int num = ((context.RestApiRequestParameters.PageSize != 0) ? context.RestApiRequestParameters.PageSize : maxResults);
		int startResult = context.RestApiRequestParameters.PageStart * context.RestApiRequestParameters.PageSize;
		string itemType2 = (childItemType.EndsWith("Folder", StringComparison.Ordinal) ? childItemType.Substring(0, childItemType.Length - 6) : childItemType);
		ItemFilter itemFilter = ConstructItemFilter(context, itemType2);
		ConfigurationItemPath configurationItemPath = (string.IsNullOrEmpty(itemType) ? ConfigurationItemPath.CreateSystemFolderPath(childItemType) : ConfigurationItemPath.CreateFolderPath(itemType, id.ToString(), childItemType));
		List<ItemFilter> list2 = new List<ItemFilter>();
		if (context.RestApiRequestParameters.FilterHandler != null && context.RestApiRequestParameters.FilterHandler.Any())
		{
			foreach (string resourceName in context.RestApiRequestParameters.FilterHandler.ResourceNames)
			{
				list2.Add(new ItemFilter(NameConvert.ResourcePluralToItemType(resourceName), new PropertyFilter[0], EnableFilter.Enabled));
			}
		}
		IConfigurationApiSupport configurationApiSupport3 = ConfigurationApi.GetObject(childItemType);
		if (!context.RestApiRequestParameters.Parameters.Any())
		{
			ConfigurationItem[] childItems = configurationApiSupport3.GetChildItems(context, configurationItemPath);
			return Pagination(context, childItems);
		}
		return configurationApiSupport3.QueryItems(context, configurationItemPath, itemFilter, num, startResult, list2);
	}

	private ConfigurationItem[] FilterItems(IRestApiRequestContext context, ConfigurationItem[] items, string itemType)
	{
		ItemFilter itemFilter = ConstructItemFilter(context, itemType);
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		foreach (ConfigurationItem item in items)
		{
			if (ConfigurationApiBase.Match(item, itemFilter))
			{
				list.Add(item);
			}
		}
		return list.ToArray();
	}

	public ValidateResult Update(IRestApiRequestContext context, ConfigurationItem item)
	{
		IConfigurationApiSupport configurationApiSupport = ConfigurationApi.GetObject(item.ItemType);
		ConfigurationItemPath path = ConfigurationItemPath.CreatePath(item.Path);
		ConfigurationItemPath parentPath = (string.IsNullOrEmpty(item.ParentPath) ? null : ConfigurationItemPath.CreatePath(item.ParentPath));
		return configurationApiSupport.SetItem(context, item, path, parentPath);
	}

	public ConfigurationItem Delete(IRestApiRequestContext context, ConfigurationItem parent, ConfigurationItemPath path)
	{
		string removeMethodId = NameConvert.GetRemoveMethodId(parent.MethodIds, path.ItemType);
		if (string.IsNullOrEmpty(removeMethodId))
		{
			throw new NotSupportedException("cannot use HTTP DELETE for this resource: " + path.ItemType);
		}
		IConfigurationApiSupport configurationApiSupport = ConfigurationApi.GetObject(parent.ItemType);
		ConfigurationItemPath path2 = ConfigurationItemPath.CreatePath(parent.Path);
		ConfigurationItem item = configurationApiSupport.GetItem(context, path2);
		ConfigurationItemPath parentPath = ConfigurationItemPath.CreatePath(item.ParentPath);
		ConfigurationItem configurationItem = configurationApiSupport.InvokeMethod(context, item, path2, parentPath, removeMethodId);
		if (configurationItem.ItemType == "InvokeInfo")
		{
			bool flag = false;
			HashSet<string> usedParameters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			Property[] properties = configurationItem.Properties;
			foreach (Property property in properties)
			{
				if (removeMethodId == "Remove" + path.ItemType && property.Key == "Id")
				{
					property.Value = path.Id;
					flag = true;
					continue;
				}
				if (removeMethodId == "MethodIdRemoveLprMatchList" && property.Key == "Id")
				{
					property.Value = path.Id;
					flag = true;
					continue;
				}
				if (property.Key == "ItemSelection" || (removeMethodId == "RemoveRule" && property.Key == "RemoveRulePath"))
				{
					property.Value = path.Text;
					flag = true;
					continue;
				}
				KeyValuePair<string, string> keyValuePair = context.RestApiRequestParameters.Parameters.FirstOrDefault((KeyValuePair<string, string> p) => property.Key.Equals(p.Key, StringComparison.OrdinalIgnoreCase));
				if (!keyValuePair.Equals(default(KeyValuePair<string, string>)))
				{
					property.Value = keyValuePair.Value;
					usedParameters.Add(keyValuePair.Key);
				}
			}
			IEnumerable<Property> second = from p in context.RestApiRequestParameters.Parameters
				where !usedParameters.Contains(p.Key)
				select new Property(NameConvert.PropertyFieldToKey(path.ItemType, p.Key), p.Value);
			configurationItem.Properties = configurationItem.Properties.Concat(second).ToArray();
			if (flag && configurationItem.MethodIds != null && configurationItem.MethodIds.Length == 1)
			{
				return InvokeMethod(context, configurationItem, configurationItem.MethodIds[0]);
			}
			throw new NotSupportedException("Cannot use HTTP DELETE for this resource, ItemSelection property missing");
		}
		throw new ServerException("Unexpected Invoke flow to server", null);
	}

	public HttpStatusCode InvokeMethodAndSerialize(IRestApiRequestContext context, JsonBuilder jb, ConfigurationItem item, string methodToCall, out ConfigurationItem invokeInfo)
	{
		string text = item.MethodIds.FirstOrDefault((string m) => string.Compare(m, methodToCall, StringComparison.OrdinalIgnoreCase) == 0);
		if (text == null)
		{
			throw new NotSupportedException("Task not found");
		}
		HttpStatusCode result = HttpStatusCode.OK;
		invokeInfo = InvokeMethod(context, item, text);
		IConfigurationItemSerializer restSerializer = RestSerializerFactory.GetRestSerializer(invokeInfo);
		if (invokeInfo.ItemType == "InvokeInfo")
		{
			jb.AppendElement("params", restSerializer.SerializeParameters());
			result = HttpStatusCode.BadRequest;
		}
		else
		{
			jb.AppendElement("result", restSerializer.SerializeInvokeResultWithFilter(context.RestApiRequestParameters, text, includeError: true, includeProgress: false, out var badRequest));
			if (badRequest)
			{
				result = HttpStatusCode.BadRequest;
			}
		}
		if (context.RestApiRequestParameters.Definition)
		{
			jb.AppendElement("definitions", restSerializer.Definitions(context, methodToCall));
		}
		return result;
	}

	public ConfigurationItem InvokeMethod(IRestApiRequestContext context, ConfigurationItem item, string methodId)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(item.Path);
		IConfigurationApiSupport configurationApiSupport = ConfigurationApi.GetObject(configurationItemPath.ItemType);
		ConfigurationItemPath parentPath = (string.IsNullOrEmpty(item.ParentPath) ? null : ConfigurationItemPath.CreatePath(item.ParentPath));
		return configurationApiSupport.InvokeMethod(context, item, configurationItemPath, parentPath, methodId);
	}

	public MethodInfo[] GetMethodDefinitions()
	{
		return MethodRepository.GetAllMethods();
	}

	public Dictionary<string, string> GetTranslations(string languageId)
	{
		try
		{
			CultureInfo cultureInfo = null;
			try
			{
				if (languageId == null)
				{
					throw new ArgumentNullException("languageId");
				}
				if (string.IsNullOrEmpty(languageId))
				{
					throw new ArgumentException("The parameter: 'languageId' can't contain an empty string", "languageId");
				}
				cultureInfo = new CultureInfo(languageId);
			}
			catch (ArgumentNullException ex)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex);
			}
			catch (ArgumentException ex2)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex2);
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			ResourceSet resourceSet = ConfigurationApiResources.ResourceManager.GetResourceSet(cultureInfo, createIfNotExists: true, tryParents: true);
			if (resourceSet != null)
			{
				foreach (DictionaryEntry item in from DictionaryEntry f in resourceSet
					where f.Value is string && f.Key is string
					select f)
				{
					string key = (string)item.Key;
					string value = (string)item.Value;
					value = InsertOemNames(value);
					dictionary.Add(key, value);
				}
			}
			ResourceSet resourceSet2 = DriverResourcesLookup.getResourceManager().GetResourceSet(cultureInfo, createIfNotExists: true, tryParents: true);
			if (resourceSet2 != null)
			{
				foreach (DictionaryEntry item2 in from DictionaryEntry f in resourceSet2
					where f.Value is string && f.Key is string
					select f)
				{
					string key2 = (string)item2.Key;
					string value2 = (string)item2.Value;
					dictionary.Add(key2, value2);
				}
			}
			return dictionary;
		}
		catch (ArgumentException ex3)
		{
			throw ConfigurationApiExceptionUtilities.CreateServerException(ex3);
		}
		catch (Exception ex4)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex4);
		}
	}

	public string InsertOemNames(string value)
	{
		if (value != null)
		{
			value = value.Replace("{OemClientName}", OemResources.Instance.SmartClient.ClientName);
			value = value.Replace("{MatrixApplicationName}", OemResources.Instance.Manager.MatrixApplicationName);
			value = value.Replace("{SmartWall}", OemResources.Instance.VideoWall.ApplicationNameShort);
			value = value.Replace("{MobileClientNameShort}", OemResources.Instance.MobileServer.MobileClientNameShort);
			value = value.Replace("{WebClientNameShort}", OemResources.Instance.MobileServer.WebClientNameShort);
		}
		return value;
	}

	public PropertyFilter ConvertParamToFilter(KeyValuePair<string, string> parameter)
	{
		Operator opr = Operator.Equals;
		string text = parameter.Value;
		int num = parameter.Value.IndexOf(":", StringComparison.Ordinal);
		if (num > -1)
		{
			string key = parameter.Value.Substring(0, num);
			text = parameter.Value.Substring(num + 1);
			if (!_opreratorToFilter.ContainsKey(key))
			{
				throw new ArgumentException("Unknown filter prefix");
			}
			opr = _opreratorToFilter[key];
		}
		if (text.Length < 1)
		{
			throw new ArgumentException("Invalid filter value");
		}
		if (text[0] == '\'')
		{
			if (text.Length < 3)
			{
				throw new ArgumentException("Invalid filter value");
			}
			if (text[text.Length - 1] != '\'')
			{
				throw new ArgumentException("Invalid filter value");
			}
			text = text.Substring(1, text.Length - 2);
		}
		string key2 = ((!(parameter.Key == "displayName")) ? ((parameter.Key[0].ToString() ?? "").ToUpper(CultureInfo.InvariantCulture) + parameter.Key.Substring(1)) : "Name");
		return new PropertyFilter(key2, opr, text);
	}

	public ConfigurationItem[] Pagination(IRestApiRequestContext context, ConfigurationItem[] items)
	{
		if (context.RestApiRequestParameters.PageSize == 0)
		{
			return items;
		}
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		for (int i = context.RestApiRequestParameters.PageStart * context.RestApiRequestParameters.PageSize; i < (context.RestApiRequestParameters.PageStart + 1) * context.RestApiRequestParameters.PageSize; i++)
		{
			if (i < items.Length)
			{
				list.Add(items[i]);
			}
		}
		return list.ToArray();
	}

	public ItemFilter ConstructItemFilter(IRestApiRequestContext context, string itemType)
	{
		List<PropertyFilter> list = new List<PropertyFilter>();
		EnableFilter enableFilter = ((!context.RestApiRequestParameters.Disabled) ? EnableFilter.Enabled : EnableFilter.All);
		foreach (KeyValuePair<string, string> parameter in context.RestApiRequestParameters.Parameters)
		{
			if (parameter.Key.Equals("enabled", StringComparison.OrdinalIgnoreCase))
			{
				if (parameter.Value.Equals("'false'", StringComparison.OrdinalIgnoreCase))
				{
					enableFilter = EnableFilter.Disabled;
					context.RestApiRequestParameters.Disabled = true;
				}
				else if (!parameter.Value.Equals("'true'", StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException("Invalid value for parameter: enabled");
				}
			}
			else
			{
				list.Add(ConvertParamToFilter(parameter));
			}
		}
		return new ItemFilter(itemType, list.ToArray(), enableFilter);
	}
}
