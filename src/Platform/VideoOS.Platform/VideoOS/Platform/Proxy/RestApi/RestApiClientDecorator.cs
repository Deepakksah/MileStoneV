using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.Proxy.RestApi;

public class RestApiClientDecorator : IConfigurationClientDecorator, IDisposable
{
	private LoginSettings _loginSettings;

	private IRestApiClient _client;

	private object _lockOpenCall = new object();

	private bool _debugLog;

	private const string DataEntry = "data";

	private const string ErrorEntry = "error";

	private const string ParamsEntry = "params";

	private const string ResultEntry = "result";

	private const string ResourcesEntry = "resources";

	private const string TasksEntry = "tasks";

	private const string ArrayEntry = "array";

	private const string DefinitionsEntry = "definitions";

	private const string ItemTypeInvokeResult = "InvokeResult";

	private const string ItemTypeInvokeInfo = "InvokeInfo";

	public const string InternalLastModifiedOnlyItemType = "_internalLastModifiedOnlyItem";

	public ApiCounters ApiCounters = new ApiCounters();

	public static Func<IRestApiClient> ConstructRestApiClient = () => new RestApiClient();

	private static List<string> _eventGroupsOnRootLevel = new List<string> { "UserDefinedEvent", "AnalyticsEvent", "GenericEvent" };

	private static List<string> _folderOnRootElement = new List<string> { "userDefinedEvents", "analyticsEvents", "genericEvents" };

	private const string PropertyNameRuleGroup = "RuleGroup";

	private const string AccessControlRuleGroupItemType = "AccessControlRuleGroup";

	private const string AllSystemsGroupId = "C3BAD46C-BBA8-0FB0-1580-244BCFE26ADD";

	public bool CacheEnabled { get; set; } = true;

	public static INameConvert NameConvert { get; set; } = new NameConvert();

	public RestApiClientDecorator(LoginSettings loginSettings, bool debugLog)
	{
		if (loginSettings == null)
		{
			throw new ArgumentNullException("loginSettings");
		}
		_loginSettings = loginSettings;
		_debugLog = debugLog;
		if (_loginSettings.IdentityTokenCache == null)
		{
			EnvironmentManager.Instance?.Log(error: true, "RestApiClientDecorator.ctor", "IdentityCache is null" + Environment.NewLine + Environment.StackTrace);
		}
		else
		{
			EnvironmentManager.Instance?.Log(error: false, "RestApiClientDecorator.ctor", "IdentityCache has value");
		}
	}

	public void DeleteItem(string path)
	{
		throw new NotImplementedException("Method not implemented as it is not used or implemented server side");
	}

	public void ClearCache()
	{
		if (_client != null)
		{
			_client.ClearCache();
		}
	}

	public void ClearCache(string resourceType)
	{
		if (_client != null)
		{
			_client.ClearCache(resourceType);
		}
	}

	public ConfigurationItem[] GetChildItems(string path)
	{
		ConfigurationItemPath configPath = CheckSpecificPath(new ConfigurationItemPath(path));
		if (configPath.IsFolder)
		{
			string parentResourceName = RestApiConvert.ConvertItemTypeToResource(configPath.ParentItemType);
			string resourceName = RestApiConvert.ConvertItemTypeToResourceSkipFolder(configPath.ItemType);
			string callerItemType = configPath.ItemType.Substring(0, configPath.ItemType.Length - 6);
			ApiCounters.GetItems++;
			JsonObject jsonObject = ((!(configPath.ParentItemType == "System") && !FolderOnRootElement(resourceName)) ? CallWithRetry((IRestApiClient client) => client.GetResources(parentResourceName, configPath.Id, resourceName)) : CallWithRetry((IRestApiClient client) => client.GetResources(resourceName)));
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			JsonObject child = jsonObject.GetChild("array");
			if (child != null)
			{
				JsonObject child2 = jsonObject.GetChild("tasks");
				JsonObject child3 = jsonObject.GetChild("definitions");
				int num = 0;
				foreach (JsonObject child6 in child.GetChildren())
				{
					JsonObject definitions = child3?.GetChildAt(num++);
					ConfigurationItem configurationItem = RestApiConvert.ConvertToConfigurationItem(child6, callerItemType, child2, definitions, fromInvoke: false);
					if (configurationItem.ChildrenFilled && configurationItem.ItemType == "LicenseInformation")
					{
						configurationItem.Children = configurationItem.Children.Concat(GetChildItems(configurationItem.Path)).ToArray();
					}
					list.Add(configurationItem);
				}
			}
			return list.ToArray();
		}
		if (!string.IsNullOrEmpty(configPath.ParentItemType))
		{
			throw new NotImplementedException("RestAPI to ConfigAPI: 4 parameters should not happen");
		}
		string resourceName2 = RestApiConvert.ConvertItemTypeToResource(configPath.ItemType);
		string id = ((resourceName2 == "sites") ? _loginSettings.Guid.ToString() : configPath.Id);
		JsonObject jsonObject2 = CallWithRetry((IRestApiClient client) => client.GetSpecificResource(resourceName2, id, askResources: true));
		ApiCounters.GetItem++;
		JsonObject child4 = jsonObject2.GetChild("resources");
		JsonObject child5 = jsonObject2.GetChild("tasks");
		List<ConfigurationItem> list2 = new List<ConfigurationItem>();
		if (child4 != null)
		{
			foreach (JsonObject child7 in child4.GetChildren())
			{
				string text = child7.GetString("type");
				string name = child7.GetString("displayName");
				try
				{
					list2.Add(ConstructFolder(configPath, RestApiConvert.ConvertResourceToItemType(text), name, child5));
				}
				catch (ResourceNotFoundException)
				{
					EnvironmentManager.Instance?.Log(error: false, "RestAPIClientDecorator", "Resource type coming from server is not recognized: " + text);
				}
			}
		}
		return list2.ToArray();
	}

	public ConfigurationItem[] GetChildItemsHierarchy(string path, string[] itemTypes, ItemFilter[] filters)
	{
		throw new NotImplementedException("Should be implemented via GraphQL support server side.");
	}

	public ConfigurationItem GetItem(string path)
	{
		if (Factory.TraceCommunication)
		{
			Factory.LogBefore("GetItem", path);
		}
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPath(path);
		JsonObject jsonObject = null;
		if (configurationItemPath.IsFolder)
		{
			ConfigurationItemPath configurationItemPath2 = new ConfigurationItemPath(path);
			string path2 = ((!(configurationItemPath2.ParentItemType == "System")) ? new ConfigurationItemPath(configurationItemPath2.ParentItemType, configurationItemPath2.Id).Path : "/");
			ConfigurationItem configurationItem = ConstructFolder(new ConfigurationItemPath(path2), configurationItemPath2.ItemType, GetFolderName(configurationItemPath2.ItemType), null);
			if (configurationItemPath2.ItemType == "AccessControlSystemFolder")
			{
				configurationItem.Properties = new Property[1] { ConstructAccessControlRuleGroupProperty() };
			}
			return configurationItem;
		}
		if (!string.IsNullOrEmpty(configurationItemPath.ParentItemType))
		{
			throw new PathNotFoundMIPException(configurationItemPath.Text);
		}
		string resourceName = RestApiConvert.ConvertItemTypeToResource(configurationItemPath.ItemType);
		string id = configurationItemPath.Id;
		if (string.IsNullOrEmpty(configurationItemPath.Id) && resourceName == "sites")
		{
			id = _loginSettings.Guid.ToString();
		}
		jsonObject = CallWithRetry((IRestApiClient client) => client.GetSpecificResource(resourceName, id, askResources: false));
		ApiCounters.GetItem++;
		ConfigurationItem configurationItem2 = RestApiConvert.ConvertToConfigurationItem(jsonObject.GetChild("data"), configurationItemPath.ItemType, jsonObject.GetChild("tasks"), jsonObject.GetChild("definitions"), fromInvoke: false);
		if (configurationItem2.ChildrenFilled && configurationItem2.ItemType == "LicenseInformation")
		{
			configurationItem2.Children = configurationItem2.Children.Concat(GetChildItems(path)).ToArray();
		}
		if (Factory.TraceCommunication)
		{
			Factory.LogAfter("GetItem", configurationItem2);
		}
		return configurationItem2;
	}

	public ConfigurationItem[] GetItems(string itemType)
	{
		throw new NotImplementedException();
	}

	public MethodInfo GetMethodInfo(string methodId)
	{
		throw new NotImplementedException();
	}

	public MethodInfo[] GetMethodInfos()
	{
		throw new NotImplementedException();
	}

	public Dictionary<string, string> GetTranslations(string languageId)
	{
		string url = "translations?language=" + languageId;
		JsonObject jsonObject = CallWithRetry((IRestApiClient client) => client.GetResourcesUrl(url));
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		JsonObject child = jsonObject.GetChild("array");
		if (child != null)
		{
			foreach (JsonObject child2 in child.GetChildren())
			{
				string key = child2.GetString("id");
				string value = child2.GetString("text");
				dictionary.Add(key, value);
			}
		}
		return dictionary;
	}

	public bool HasChildItems(string path)
	{
		throw new NotImplementedException("Not implemented server side");
	}

	public ConfigurationItem InvokeMethod(ConfigurationItem item, string methodId)
	{
		bool flag = RestApiConvert.IsRemoveMethod(methodId) && item.Properties?.FirstOrDefault((Property p) => p.Key == "ItemSelection") == null;
		ConfigurationItemPath path = new ConfigurationItemPath(item.Path);
		if (RestApiConvert.IsRemoveMethod(methodId) && !flag && !path.IsFolder)
		{
			Property property = item.Properties?.FirstOrDefault((Property p) => p.Key == "ItemSelection");
			if (property == null)
			{
				return ConstructRemoveItemSelection(item, methodId);
			}
			string value = property.Value;
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentException("ItemSelection is not filled - unable to remove item for ItemType:" + item.ItemType);
			}
			ConvertPathToResource(value, out var resourceName, out var id);
			ApiCounters.Delete++;
			JsonObject jsonObject = CallWithRetry((IRestApiClient client) => client.Delete(resourceName, id));
			if (jsonObject != null)
			{
				JsonObject child = jsonObject.GetChild("error");
				JsonObject child2 = jsonObject.GetChild("State");
				if (child != null)
				{
					throw new PathNotFoundMIPException();
				}
				if (child2 != null)
				{
					return CreateInvokeResult(item.Path, item.ParentPath, null, "Result", child2.Value);
				}
			}
			return CreateInvokeResult(item.Path, item.ParentPath, null, "Result", "Success");
		}
		string itemType = RestApiConvert.TrimFolder(path.ItemType);
		bool provideMethodId = !RestApiConvert.IsAddMethod(itemType, methodId);
		ApiCounters.Post++;
		if (item.ItemType == "InvokeResult")
		{
			return item;
		}
		bool isMIPItem = item.ItemType == "MIPItem" || (item.ItemType == "InvokeInfo" && methodId == "AddMIPItem");
		string json = RestApiConvert.ConvertConfigurationItemToJson(item, noData: false, item.ItemType == "InvokeInfo", isMIPItem);
		ConvertPathToResource(item.Path, out var resourceName2, out var id2);
		string idInUrl = id2;
		int httpStatus = 0;
		if (_debugLog)
		{
			EnvironmentManager.Instance?.Log(error: false, "RestAPI:InvokeMethod", "ParentPath:" + item.ParentPath + ", Path:" + item.Path);
		}
		JsonObject jsonObject2;
		if (path.ParentItemType != "System" && path.IsFolder)
		{
			string parentResourceName = RestApiConvert.ConvertItemTypeToResource(path.ParentItemType);
			jsonObject2 = CallWithRetry((IRestApiClient client) => client.Post(parentResourceName, path.Id, resourceName2, null, provideMethodId ? methodId : "", json, out httpStatus));
		}
		else if (NameConvert.OnlyFourthLevelPostSupported(item.ItemType))
		{
			ConvertPathToResource(item.ParentPath, out var parentResourceName2, out var _, out var parentId);
			jsonObject2 = CallWithRetry((IRestApiClient client) => client.Post(parentResourceName2, parentId, resourceName2, path.Id, provideMethodId ? methodId : "", json, out httpStatus));
		}
		else
		{
			jsonObject2 = CallWithRetry((IRestApiClient client) => client.Post(resourceName2, idInUrl, provideMethodId ? methodId : "", json, out httpStatus));
		}
		JsonObject child3 = jsonObject2.GetChild("params");
		JsonObject child4 = jsonObject2.GetChild("result");
		JsonObject child5 = jsonObject2.GetChild("error");
		JsonObject child6 = jsonObject2.GetChild("definitions");
		ConfigurationItem configurationItem = null;
		List<Property> list = new List<Property>();
		if (child5 != null)
		{
			JsonObject childAt = child5.GetChild("details").GetChildAt(0);
			string text = childAt.GetString("errorText");
			string text2 = childAt.GetString("errorTextId");
			string text3 = childAt.GetString("property");
			string value2 = (string.IsNullOrEmpty(text3) ? text : (text3 + ": " + text));
			if (text != "Bad request: parameters missing" && text2 != "99999")
			{
				list.Add(new Property
				{
					Key = "State",
					Value = "Error"
				});
				list.Add(new Property
				{
					Key = "ErrorText",
					Value = value2
				});
				if (text2 != null)
				{
					list.Add(new Property
					{
						Key = "ErrorCode",
						Value = text2
					});
				}
				if (httpStatus == 403)
				{
					throw new NotAuthorizedMIPException(text);
				}
				if (httpStatus == 400 && !string.IsNullOrEmpty(text3))
				{
					throw new ArgumentMIPException(text3);
				}
			}
		}
		if (child4 != null)
		{
			configurationItem = RestApiConvert.ConvertToConfigurationItem(child4, item.ItemType, null, child6, fromInvoke: true);
			Property property2 = configurationItem.Properties.FirstOrDefault((Property p) => p.Key == "Path");
			configurationItem.Path = ((property2 != null) ? property2.Value : item.Path);
			configurationItem.ParentPath = item.ParentPath;
			configurationItem.ItemType = (IsLprMatchList(item.ItemType) ? "InvokeInfo" : "InvokeResult");
			return configurationItem;
		}
		if (child3 != null)
		{
			configurationItem = RestApiConvert.ConvertToConfigurationItem(child3, item.ItemType, null, child6, fromInvoke: true);
			configurationItem.Path = item.Path;
			configurationItem.ParentPath = item.ParentPath;
			configurationItem.ItemType = "InvokeInfo";
			List<Property> list2 = new List<Property>(configurationItem.Properties);
			list2.AddRange(list);
			configurationItem.Properties = list2.ToArray();
			configurationItem.MethodIds = new string[1] { methodId };
		}
		if (configurationItem == null)
		{
			configurationItem = CreateInvokeResult(item.Path, item.ParentPath, null, "Result", "Error", list.ToArray());
		}
		return configurationItem;
	}

	public ConfigurationItem[] QueryItems(ItemFilter filter, int maxResults)
	{
		throw new NotImplementedException("Should be implemented with GraphQL server side");
	}

	public ValidateResult SetItem(ConfigurationItem item)
	{
		if (item.ItemType.EndsWith("Folder", StringComparison.Ordinal))
		{
			throw new ArgumentException("Folders cannot be updated", "item");
		}
		string json = RestApiConvert.ConvertConfigurationItemToJson(item, noData: false);
		ConfigurationItemPath parentConfigApiPath = new ConfigurationItemPath(item.ParentPath);
		ConvertPathToResource(item, out var resourceName, out var id);
		if (!string.IsNullOrEmpty(item.ParentPath))
		{
			ConfigurationItemPath configurationItemPath = new ConfigurationItemPath(item.ParentPath);
			if (!configurationItemPath.ParentItemType.Equals("System", StringComparison.OrdinalIgnoreCase))
			{
				_client.ClearCache(RestApiConvert.ConvertItemTypeToResource(configurationItemPath.ParentItemType));
			}
		}
		ApiCounters.Patch++;
		JsonObject jsonObject;
		if (item.ItemType == "MIPItem")
		{
			jsonObject = CallWithRetry((IRestApiClient client) => client.Put(resourceName, id, json));
		}
		else if (RequireParentPath(resourceName))
		{
			string parentResourceName = RestApiConvert.ConvertItemTypeToResource(parentConfigApiPath.ParentItemType);
			jsonObject = CallWithRetry((IRestApiClient client) => client.Patch(parentResourceName, parentConfigApiPath.Id, resourceName, id, json));
		}
		else
		{
			jsonObject = CallWithRetry((IRestApiClient client) => client.Patch(resourceName, id, json));
		}
		if (jsonObject != null)
		{
			JsonObject child = jsonObject.GetChild("data");
			JsonObject child2 = jsonObject.GetChild("error");
			ConfigurationItem configurationItem = null;
			string callerItemType = item.ItemType;
			if (child2 == null && child == null)
			{
				try
				{
					child = CallWithRetry((IRestApiClient client) => client.GetSpecificResourceLastModifiedOnly(resourceName, id)).GetChild("data");
					callerItemType = "_internalLastModifiedOnlyItem";
				}
				catch
				{
				}
			}
			configurationItem = ((child != null) ? RestApiConvert.ConvertToConfigurationItem(child, callerItemType, null, null, fromInvoke: false) : null);
			return CreateValidateResult(child2 == null, configurationItem, child2?.GetChild("details"));
		}
		throw new NotImplementedException();
	}

	public ValidateResult ValidateItem(ConfigurationItem item)
	{
		return CreateValidateResult(isOk: true, item);
	}

	private static ValidateResult CreateValidateResult(bool isOk, ConfigurationItem item = null, JsonObject detailArray = null)
	{
		List<ErrorResult> list = new List<ErrorResult>();
		if (detailArray != null)
		{
			JsonObject childAt = detailArray.GetChildAt(0);
			if (childAt != null)
			{
				list.Add(new ErrorResult
				{
					ErrorText = childAt.GetString("errorText"),
					ErrorProperty = childAt.GetString("property"),
					ErrorTextId = childAt.GetString("errorTextId")
				});
			}
		}
		return new ValidateResult
		{
			ValidatedOk = isOk,
			ResultItem = item,
			ErrorResults = list.ToArray()
		};
	}

	private static void ConvertPathToResource(ConfigurationItem item, out string resourceName, out string id)
	{
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPath(item.Path);
		id = item.Properties.FirstOrDefault((Property p) => p.Key == "Id")?.Value;
		if (id == null)
		{
			id = configurationItemPath.Id;
		}
		string itemType = (string.IsNullOrEmpty(configurationItemPath.ItemType) ? "System" : configurationItemPath.ItemType);
		resourceName = RestApiConvert.ConvertItemTypeToResource(itemType);
	}

	private static void ConvertPathToResource(string path, out string resourceName, out string id)
	{
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPath(path);
		id = configurationItemPath.Id;
		string itemType = (string.IsNullOrEmpty(configurationItemPath.ItemType) ? "System" : configurationItemPath.ItemType);
		resourceName = RestApiConvert.ConvertItemTypeToResource(itemType);
	}

	private static void ConvertPathToResource(string path, out string parentResourceName, out string resourceName, out string id)
	{
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPath(path);
		id = configurationItemPath.Id;
		string itemType = (string.IsNullOrEmpty(configurationItemPath.ItemType) ? "System" : configurationItemPath.ItemType);
		resourceName = RestApiConvert.ConvertItemTypeToResource(itemType);
		string itemType2 = (string.IsNullOrEmpty(configurationItemPath.ParentItemType) ? "System" : configurationItemPath.ParentItemType);
		parentResourceName = RestApiConvert.ConvertItemTypeToResource(itemType2);
	}

	private static ConfigurationItemPath CheckSpecificPath(ConfigurationItemPath path)
	{
		if (!path.IsFolder && path.ItemType == "FindSaveSearches")
		{
			return new ConfigurationItemPath(path, "SaveSearchesFolder");
		}
		return path;
	}

	private TResult CallWithRetry<TResult>(Func<IRestApiClient, TResult> call)
	{
		try
		{
			if (_client == null)
			{
				Connect();
			}
			return call(_client);
		}
		catch (FaultException)
		{
			throw;
		}
		catch (CommunicationMIPException)
		{
			lock (_lockOpenCall)
			{
				CloseConnection();
				Connect();
				return call(_client);
			}
		}
		catch (CommunicationException ex3)
		{
			lock (_lockOpenCall)
			{
				if (IsUnauthorizedLogin(ex3))
				{
					CloseConnection();
					throw;
				}
				CloseConnection();
				Connect();
				return call(_client);
			}
		}
		catch (ObjectDisposedException)
		{
			lock (_lockOpenCall)
			{
				CloseConnection();
				Connect();
				return call(_client);
			}
		}
	}

	public void UpdateLoginSettingsIfNoToken(LoginSettings loginSettings)
	{
		if (_loginSettings.IdentityTokenCache == null && _loginSettings != loginSettings)
		{
			EnvironmentManager.Instance?.Log(error: false, "UpdateLoginSettingsIfNoToken", "Updating LoginSettings with new version");
			if (loginSettings.IdentityTokenCache == null)
			{
				EnvironmentManager.Instance?.Log(error: true, "UpdateLoginSettingsIfNoToken", "LoginSettings.IdentityCache is null" + Environment.NewLine + Environment.StackTrace);
			}
			_loginSettings = loginSettings;
			_client.Initialize(_loginSettings, _debugLog);
		}
	}

	private void Connect()
	{
		lock (_lockOpenCall)
		{
			if (_client == null)
			{
				IRestApiClient restApiClient = ConstructRestApiClient();
				restApiClient.EnableCache = CacheEnabled;
				restApiClient.Initialize(_loginSettings, _debugLog);
				_client = restApiClient;
			}
		}
	}

	public void CloseConnection()
	{
		lock (_lockOpenCall)
		{
			if (_client != null)
			{
				_client.Dispose();
				_client = null;
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing && _client != null)
		{
			_client.Dispose();
			_client = null;
		}
	}

	private static ConfigurationItem ConstructRemoveItemSelection(ConfigurationItem resourceOwner, string methodId)
	{
		List<Property> list = new List<Property>();
		list.Add(new Property
		{
			Key = "ItemSelection",
			Value = "",
			ValueType = "Path",
			ValueTypeInfos = new ValueTypeInfo[0]
		});
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.DisplayName = "Remove ";
		configurationItem.SortKey = 1;
		configurationItem.UIHint = "None";
		configurationItem.ChildrenFilled = true;
		configurationItem.EnableProperty = null;
		configurationItem.ItemCategory = "Item";
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Path = resourceOwner.Path;
		configurationItem.ParentPath = resourceOwner.ParentPath;
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static bool IsUnauthorizedLogin(CommunicationException ex)
	{
		if (ex?.InnerException?.InnerException is WebException ex2)
		{
			HttpWebResponse obj = ex2.Response as HttpWebResponse;
			if (obj == null)
			{
				return false;
			}
			return obj.StatusCode == HttpStatusCode.Unauthorized;
		}
		return false;
	}

	private static ConfigurationItem ConstructFolder(ConfigurationItemPath parentPath, string itemType, string name, JsonObject tasks)
	{
		string itemType2 = (itemType.EndsWith("Folder", StringComparison.Ordinal) ? itemType : (itemType + "Folder"));
		parentPath = AdjustParentPath(parentPath, itemType);
		string[] methodIds = RestApiConvert.TasksToMethodIds(tasks);
		return new ConfigurationItem
		{
			Path = new ConfigurationItemPath(parentPath, itemType).Path,
			ParentPath = parentPath.Path,
			ItemType = itemType2,
			DisplayName = name,
			ChildrenFilled = false,
			EnableProperty = null,
			ItemCategory = "Group",
			Properties = new Property[0],
			MethodIds = methodIds
		};
	}

	private static ConfigurationItemPath AdjustParentPath(ConfigurationItemPath parentPath, string itemType)
	{
		if (_eventGroupsOnRootLevel.Contains(itemType))
		{
			return new ConfigurationItemPath("/");
		}
		return parentPath;
	}

	private static bool RequireParentPath(string resourceName)
	{
		return new List<string> { "ptzPresets" }.Contains(resourceName);
	}

	private static bool FolderOnRootElement(string resourceName)
	{
		return _folderOnRootElement.Contains(resourceName);
	}

	private static bool IsLprMatchList(string itemType)
	{
		return itemType == "LprMatchList";
	}

	private static string GetFolderName(string itemType)
	{
		string text = RestApiConvert.TrimFolder(itemType);
		if (text == "Hardware")
		{
			return text;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(text[0]);
		for (int i = 1; i < text.Length; i++)
		{
			char c = text[i];
			if (c >= 'A' && c <= 'Z')
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(char.ToLowerInvariant(c));
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		string text2 = stringBuilder.ToString();
		if (!text2.EndsWith("s", StringComparison.Ordinal))
		{
			text2 += "s";
		}
		return text2;
	}

	private static ConfigurationItem CreateInvokeResult(string path, string parentPath, string resultPath = null, string displayName = "Result", string invokeInfoState = "Success", params Property[] resultProperties)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "InvokeResult",
			DisplayName = displayName,
			UIHint = "None",
			ItemCategory = "Item",
			ParentPath = parentPath,
			Path = path,
			MethodIds = Array.Empty<string>()
		};
		List<Property> list = new List<Property>();
		if (resultProperties != null)
		{
			list.AddRange(resultProperties);
		}
		list.Add(new Property
		{
			Key = "State",
			Value = invokeInfoState
		});
		if (resultPath != null)
		{
			list.Add(new Property
			{
				Key = "Path",
				Value = resultPath
			});
		}
		obj.Properties = list.ToArray();
		return obj;
	}

	private static Property ConstructAccessControlRuleGroupProperty()
	{
		string value = string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", "AccessControlRuleGroup", "C3BAD46C-BBA8-0FB0-1580-244BCFE26ADD");
		return new Property
		{
			Key = "RuleGroup",
			Value = value,
			IsSettable = false
		};
	}
}
