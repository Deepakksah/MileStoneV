using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

public class ConfigurationApiSubController : ISubController
{
	private readonly HashSet<string> _mustHaveMIPItemProperties = new HashSet<string> { "Name", "Id", "GisPoint", "__Encrypt", "Enabled", "LastModified" };

	private static readonly Dictionary<string, HashSet<string>> NoRestoreProperties = new Dictionary<string, HashSet<string>> { 
	{
		"ViewGroup",
		new HashSet<string> { "ViewGroupDataXml" }
	} };

	private static readonly HashSet<string> ChildItemTypesWithoutRootSupport = new HashSet<string> { "PtzPreset", "HardwareDriver" };

	public INameConvert NameConvert { get; set; } = new NameConvert();

	public IConfigurationApiManager ConfigurationApiManager { get; set; } = new ConfigurationApiManager();

	public ResourceEntry ResourceEntry { get; set; }

	public IRestSerializerFactory SerializerFactory { get; set; } = new RestSerializerFactory();

	public ConfigurationApiSubController()
	{
	}

	public ConfigurationApiSubController(ResourceEntry resource)
	{
		ResourceEntry = resource;
	}

	public string GetArrayOfChildItems(IRestApiRequestContext context, string resourceNamePlural, string id, string childResourceNamePlural)
	{
		ResourceEntry resourcePlural = NameConvert.GetResourcePlural(childResourceNamePlural);
		ResourceEntry resourcePlural2 = NameConvert.GetResourcePlural(resourceNamePlural);
		string itemTypeFolder = resourcePlural.ItemTypeFolder;
		ConfigurationItem[] array = ConfigurationApiManager.LookupChildArray(context, resourcePlural2.ItemType, id, itemTypeFolder);
		context.RestApiRequestParameters.SessionData = array;
		JsonBuilder jsonBuilder = new JsonBuilder(array: true);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].EnableProperty == null || array[i].EnableProperty.Enabled || context.RestApiRequestParameters.Disabled)
			{
				IConfigurationItemSerializer restSerializer = SerializerFactory.GetRestSerializer(array[i]);
				jsonBuilder.AppendElement(restSerializer.Serialize(context.RestApiRequestParameters));
			}
		}
		return jsonBuilder.ToJson();
	}

	public int GetCountOfChildItems(IRestApiRequestContext context, string resourceNamePlural, string id, string childResourceNamePlural)
	{
		ResourceEntry resourcePlural = NameConvert.GetResourcePlural(childResourceNamePlural);
		ResourceEntry resourcePlural2 = NameConvert.GetResourcePlural(resourceNamePlural);
		string itemTypeFolder = resourcePlural.ItemTypeFolder;
		return ConfigurationApiManager.LookupChildArray(context, resourcePlural2.ItemType, id, itemTypeFolder).Length;
	}

	public string GetArrayOfItems(IRestApiRequestContext context)
	{
		string itemType = ResourceEntry.ItemType;
		if (!NameConvert.SupportRootFolder(itemType))
		{
			throw new PathInvalidException(string.Format(CultureInfo.InvariantCulture, "Incorrect use of listing objects, please use specific {0} path as prefix like: {0}/specific-id/{1}", NameConvert.SupportRootFolderSuggest(itemType), ResourceEntry.ResourceNamePlural));
		}
		ConfigurationItem[] array = ConfigurationApiManager.LookupArray(context, itemType);
		JsonBuilder jsonBuilder = new JsonBuilder(array: true);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].EnableProperty == null || array[i].EnableProperty.Enabled || context.RestApiRequestParameters.Disabled)
			{
				IConfigurationItemSerializer restSerializer = SerializerFactory.GetRestSerializer(array[i]);
				jsonBuilder.AppendElement(restSerializer.Serialize(context.RestApiRequestParameters));
			}
		}
		return jsonBuilder.ToJson();
	}

	public int GetCountOfItems(IRestApiRequestContext context)
	{
		string itemType = ResourceEntry.ItemType;
		if (!NameConvert.SupportRootFolder(itemType))
		{
			throw new PathInvalidException(string.Format(CultureInfo.InvariantCulture, "Incorrect use of listing objects, please use specific {0} path as prefix like: {0}/specific-id/{1}", NameConvert.SupportRootFolderSuggest(itemType), ResourceEntry.ResourceNamePlural));
		}
		return ConfigurationApiManager.LookupItemCount(context, itemType);
	}

	public string GetChildResources(IRestApiRequestContext context)
	{
		ConfigurationItemSerializer configurationItemSerializer = context.RestApiRequestParameters.SessionData as ConfigurationItemSerializer;
		try
		{
			ConfigurationItem[] items = ConfigurationApiManager.LookupChildItemTypes(context, configurationItemSerializer.Path);
			return configurationItemSerializer.Resources(context, items);
		}
		catch (NotSupportedException)
		{
			return configurationItemSerializer.Resources(context, new ConfigurationItem[0]);
		}
	}

	public string GetDefinitions(IRestApiRequestContext context)
	{
		return (context.RestApiRequestParameters.SessionData as ConfigurationItemSerializer).Definitions(context, null);
	}

	public string GetChildItemsDefinitions(IRestApiRequestContext context)
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: true);
		ConfigurationItem[] array = context.RestApiRequestParameters.SessionData as ConfigurationItem[];
		for (int i = 0; i < array.Length; i++)
		{
			IConfigurationItemSerializer restSerializer = SerializerFactory.GetRestSerializer(array[i]);
			jsonBuilder.AppendElement(restSerializer.Definitions(context, null));
		}
		return jsonBuilder.ToJson();
	}

	public string Delete(IRestApiRequestContext context, string pathId)
	{
		string text = NameConvert.ResourcePluralToItemType(ResourceEntry.ResourceNamePlural);
		ConfigurationItemPath configurationItemPath = (string.IsNullOrEmpty(text) ? ConfigurationItemPath.CreateSystemPath() : ConfigurationItemPath.CreateItemPath(text, pathId));
		ConfigurationItem item = ConfigurationApiManager.GetItem(context, configurationItemPath);
		ConfigurationItem configurationItem;
		if (NameConvert.GetRemoveMethodId(item.MethodIds, configurationItemPath.ItemType) != null)
		{
			configurationItem = ConfigurationApiManager.Delete(context, item, configurationItemPath);
		}
		else
		{
			ConfigurationItemPath pathInstance = (string.IsNullOrEmpty(item.ParentPath) ? null : ConfigurationItemPath.CreatePath(item.ParentPath));
			ConfigurationItem item2 = ConfigurationApiManager.GetItem(context, pathInstance);
			configurationItem = ConfigurationApiManager.Delete(context, item2, configurationItemPath);
		}
		IConfigurationItemSerializer restSerializer = SerializerFactory.GetRestSerializer(configurationItem);
		if (configurationItem.ItemType == "InvokeInfo")
		{
			if (configurationItem.Properties.Any((Property p) => p.Key == "UserAccept"))
			{
				configurationItem.Properties = configurationItem.Properties.Where((Property p) => p.Key != "UserAccept").ToArray();
				string text2 = restSerializer.Serialize(context.RestApiRequestParameters);
				throw new PropertyInvalidException(NameConvert.PropertyKeyToField(configurationItemPath.ItemType, "UserAccept") + " property must be provided in the query." + Environment.NewLine + text2);
			}
			throw new PropertyInvalidException(restSerializer.Serialize(context.RestApiRequestParameters));
		}
		return restSerializer.Serialize(context.RestApiRequestParameters);
	}

	public virtual string Delete(IRestApiRequestContext context, string parentResourceNamePlural, string id, string childId)
	{
		ConfigurationItemPath pathInstance = ConfigurationItemPath.CreateFolderPath(NameConvert.GetResourcePlural(parentResourceNamePlural).ItemType, id, ResourceEntry.ItemTypeFolder);
		ConfigurationItem item = ConfigurationApiManager.GetItem(context, pathInstance);
		if (item == null)
		{
			throw new PathInvalidException();
		}
		ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath(ResourceEntry.ItemType, childId);
		ConfigurationItem item2 = ConfigurationApiManager.Delete(context, item, path);
		return SerializerFactory.GetRestSerializer(item2).Serialize(context.RestApiRequestParameters);
	}

	public HttpStatusCode Patch(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb, string id)
	{
		string text = NameConvert.ResourcePluralToItemType(ResourceEntry.ResourceNamePlural);
		ConfigurationItemPath pathInstance = (string.IsNullOrEmpty(text) ? ConfigurationItemPath.CreateSystemPath() : ConfigurationItemPath.CreateItemPath(text, id));
		ConfigurationItem item = ConfigurationApiManager.GetItem(context, pathInstance);
		if (NoRestoreProperties.TryGetValue(item.ItemType, out var noRestoreProperties))
		{
			item.Properties = item.Properties.Where((Property p) => !noRestoreProperties.Contains(p.Key)).ToArray();
		}
		PropertyHelper.UpdateProperties(item, elementList, listMustMatch: true, optionalProperty: false);
		return JsonUtilities.ConvertValidateResult(ConfigurationApiManager.Update(context, item), text, jb, context.RestApiRequestParameters);
	}

	public virtual HttpStatusCode Patch(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb, string parentResourceNamePlural, string id, string childId)
	{
		ResourceEntry resourcePlural = NameConvert.GetResourcePlural(parentResourceNamePlural);
		string itemTypeFolder = ResourceEntry.ItemTypeFolder;
		ConfigurationItem[] array = ConfigurationApiManager.LookupChildArray(context, resourcePlural.ItemType, id, itemTypeFolder);
		ConfigurationItem configurationItem = null;
		ConfigurationItem[] array2 = array;
		foreach (ConfigurationItem configurationItem2 in array2)
		{
			if (string.Equals(ConfigurationItemPath.CreatePath(configurationItem2.Path).Id, childId, StringComparison.OrdinalIgnoreCase))
			{
				configurationItem = configurationItem2;
				break;
			}
		}
		if (configurationItem == null)
		{
			throw new PathNotFoundException(string.Empty);
		}
		if (NoRestoreProperties.TryGetValue(configurationItem.ItemType, out var noRestoreProperties))
		{
			configurationItem.Properties = configurationItem.Properties.Where((Property p) => !noRestoreProperties.Contains(p.Key)).ToArray();
		}
		PropertyHelper.UpdateProperties(configurationItem, elementList, listMustMatch: true, optionalProperty: false);
		return JsonUtilities.ConvertValidateResult(ConfigurationApiManager.Update(context, configurationItem), ResourceEntry.ItemType, jb, context.RestApiRequestParameters);
	}

	public virtual HttpStatusCode Put(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb, string parentResourceNamePlural, string id, string childId)
	{
		ResourceEntry resourcePlural = NameConvert.GetResourcePlural(parentResourceNamePlural);
		string itemTypeFolder = ResourceEntry.ItemTypeFolder;
		ConfigurationItem[] array = ConfigurationApiManager.LookupChildArray(context, resourcePlural.ItemType, id, itemTypeFolder);
		ConfigurationItem configurationItem = null;
		ConfigurationItem[] array2 = array;
		foreach (ConfigurationItem configurationItem2 in array2)
		{
			if (string.Equals(ConfigurationItemPath.CreatePath(configurationItem2.Path).Id, childId, StringComparison.OrdinalIgnoreCase))
			{
				configurationItem = configurationItem2;
				break;
			}
		}
		if (configurationItem == null)
		{
			throw new PathNotFoundException(string.Empty);
		}
		Collection<string> collection = PropertyHelper.UpdateProperties(configurationItem, elementList, listMustMatch: true, optionalProperty: false);
		Property[] properties = configurationItem.Properties;
		foreach (Property property in properties)
		{
			if (property.IsSettable && !collection.Contains(property.Key))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "All fields need to be filled, missing: {0}", NameConvert.PropertyKeyToField(configurationItem.ItemType, property.Key)));
			}
		}
		return JsonUtilities.ConvertValidateResult(ConfigurationApiManager.Update(context, configurationItem), ResourceEntry.ItemType, jb, context.RestApiRequestParameters);
	}

	public HttpStatusCode Put(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb, string id)
	{
		ConfigurationItemPath pathInstance = (string.IsNullOrEmpty(ResourceEntry.ItemType) ? ConfigurationItemPath.CreateSystemPath() : ConfigurationItemPath.CreateItemPath(ResourceEntry.ItemType, id));
		ConfigurationItem item = ConfigurationApiManager.GetItem(context, pathInstance);
		Collection<string> collection = PropertyHelper.UpdateProperties(item, elementList, listMustMatch: true, optionalProperty: false);
		Collection<string> collection2 = new Collection<string>();
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (property.IsSettable && !collection.Contains(property.Key))
			{
				if (!(item.ItemType == "MIPItem") || _mustHaveMIPItemProperties.Contains(property.Key))
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "All fields need to be filled, missing: {0}", NameConvert.PropertyKeyToField(item.ItemType, property.Key)));
				}
				if (!property.Key.StartsWith("FQID", StringComparison.Ordinal))
				{
					collection2.Add(property.Key);
				}
			}
		}
		foreach (string item2 in collection2)
		{
			RemoveProperty(item, item2);
		}
		return JsonUtilities.ConvertValidateResult(ConfigurationApiManager.Update(context, item), item.ItemType, jb, context.RestApiRequestParameters);
	}

	public HttpStatusCode Post(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb)
	{
		string itemTypeFolder = ResourceEntry.ItemTypeFolder;
		string itemType = ResourceEntry.ItemType;
		ConfigurationItemPath path = (string.IsNullOrEmpty(itemTypeFolder) ? ConfigurationItemPath.CreateSystemPath() : ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), itemTypeFolder));
		if (itemType == "Hardware")
		{
			ElementList obj = (elementList.Element("recordingServerPath") as ElementList) ?? throw new ArgumentException("Parameter: recordingServerPath not formatted correctly");
			StringElement stringElement = obj.Element("type") as StringElement;
			StringElement stringElement2 = obj.Element("id") as StringElement;
			if (stringElement == null || stringElement2 == null || stringElement.Value != "recordingServers")
			{
				throw new ArgumentException("Parameter: recordingServerPath not formatted correctly");
			}
			path = ConfigurationItemPath.CreateItemPath("RecordingServer", stringElement2.Value);
		}
		ConfigurationItem configurationItem = ConfigurationApiManager.LookupFolder(context, path);
		if (configurationItem == null)
		{
			throw new ResourceNotFoundException("resource not found", null);
		}
		bool flag = string.IsNullOrEmpty(context.RestApiRequestParameters.Task);
		string text = (flag ? NameConvert.GetAddMethodId(configurationItem.MethodIds, itemType) : NameConvert.TaskToMethodId(context.RestApiRequestParameters.Task));
		if (!string.IsNullOrEmpty(text) && configurationItem.MethodIds != null)
		{
			string[] methodIds = configurationItem.MethodIds;
			for (int i = 0; i < methodIds.Length; i++)
			{
				if (string.Equals(methodIds[i], text, StringComparison.OrdinalIgnoreCase))
				{
					return ExecuteFolderMethod(context, configurationItem, text, elementList, jb, flag);
				}
			}
		}
		throw new NotSupportedException("Method not found");
	}

	public HttpStatusCode Post(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb, string id)
	{
		string itemType = ResourceEntry.ItemType;
		ConfigurationItemPath pathInstance = (string.IsNullOrEmpty(itemType) ? ConfigurationItemPath.CreateSystemPath() : ConfigurationItemPath.CreateItemPath(itemType, id));
		ConfigurationItem item = ConfigurationApiManager.GetItem(context, pathInstance);
		bool flag = string.IsNullOrEmpty(context.RestApiRequestParameters.Task);
		string text = (flag ? NameConvert.GetAddMethodId(item.MethodIds, itemType) : context.RestApiRequestParameters.Task);
		if (!string.IsNullOrEmpty(text) && item.MethodIds != null)
		{
			string[] methodIds = item.MethodIds;
			for (int i = 0; i < methodIds.Length; i++)
			{
				if (string.Equals(methodIds[i], text, StringComparison.OrdinalIgnoreCase))
				{
					return ExecuteFolderMethod(context, item, text, elementList, jb, flag);
				}
			}
		}
		throw new NotSupportedException("Task not found");
	}

	private HttpStatusCode ExecuteFolderMethod(IRestApiRequestContext context, ConfigurationItem folderItem, string taskToCall, ElementList elementList, JsonBuilder jb, bool anAddTask)
	{
		HttpStatusCode result = HttpStatusCode.OK;
		ConfigurationItem configurationItem = ConfigurationApiManager.InvokeMethod(context, folderItem, taskToCall);
		if (configurationItem.ItemType == "InvokeResult" || configurationItem.ItemType == "Task")
		{
			IConfigurationItemSerializer restSerializer = SerializerFactory.GetRestSerializer(configurationItem);
			jb.AppendElement("result", restSerializer.SerializeInvokeResultWithFilter(context.RestApiRequestParameters, taskToCall, includeError: true, includeProgress: false, out var badRequest));
			if (badRequest)
			{
				result = HttpStatusCode.BadRequest;
			}
		}
		else
		{
			int num = PropertyHelper.CountRequiredSettableProperties(configurationItem);
			try
			{
				bool flag = true;
				int num2 = 0;
				while (flag)
				{
					flag = false;
					bool optionalProperty = (configurationItem.MethodIds == null || configurationItem.MethodIds.Length == 0 || configurationItem.MethodIds[0] != "AddMIPItem") && configurationItem.ItemType != "MIPItem";
					bool noCasing = configurationItem.ItemType == "InvokeInfo" && taskToCall == "AddMIPItem";
					Collection<string> collection = PropertyHelper.UpdateProperties(configurationItem, elementList, listMustMatch: false, optionalProperty, noCasing);
					if (collection.Count == 0 && num > 0)
					{
						throw new ArgumentException("parameters missing");
					}
					if (configurationItem.MethodIds == null || configurationItem.MethodIds.Length < 1)
					{
						throw new NotSupportedException("Method flow missing next MethodId");
					}
					string methodToCall = configurationItem.MethodIds[0];
					ConfigurationItem invokeInfo;
					HttpStatusCode httpStatusCode = ConfigurationApiManager.InvokeMethodAndSerialize(context, jb, configurationItem, methodToCall, out invokeInfo);
					if (httpStatusCode == HttpStatusCode.BadRequest && invokeInfo.ItemType != "InvokeResult" && collection.Count < elementList.Elements.Count && collection.Count > num2)
					{
						jb.Clear();
						configurationItem = invokeInfo;
						num2 = collection.Count;
						flag = true;
						continue;
					}
					if (httpStatusCode == HttpStatusCode.OK && anAddTask)
					{
						return HttpStatusCode.Created;
					}
					return httpStatusCode;
				}
			}
			catch (Exception ex)
			{
				return ExecuteExceptionHandling(context, ex, jb, configurationItem, taskToCall);
			}
		}
		return result;
	}

	private HttpStatusCode ExecuteExceptionHandling(IRestApiRequestContext context, Exception ex, JsonBuilder jb, ConfigurationItem invokeInfo, string methodId)
	{
		ErrorModel errorModel = ExceptionHandling.ConvertToErrorModel(ex);
		JsonUtilities.ConvertErrorModel(errorModel, jb);
		IConfigurationItemSerializer restSerializer = SerializerFactory.GetRestSerializer(invokeInfo);
		jb.AppendElement("params", restSerializer.SerializeParameters());
		if (context.RestApiRequestParameters.Definition)
		{
			jb.AppendElement("definitions", restSerializer.Definitions(context, methodId));
		}
		return errorModel.HttpCode;
	}

	public HttpStatusCode Post(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb, string id, string childResourceName)
	{
		string itemType = ResourceEntry.ItemType;
		string folderType = NameConvert.ResourcePluralToFolderItemType(childResourceName);
		string itemType2 = NameConvert.ResourcePluralToItemType(childResourceName);
		ConfigurationItemPath pathInstance = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateItemPath(itemType, id), folderType);
		ConfigurationItem item = ConfigurationApiManager.GetItem(context, pathInstance);
		bool flag = string.IsNullOrEmpty(context.RestApiRequestParameters.Task);
		string text = (flag ? NameConvert.GetAddMethodId(item.MethodIds, itemType2) : context.RestApiRequestParameters.Task);
		if (!string.IsNullOrEmpty(text) && item.MethodIds != null)
		{
			string[] methodIds = item.MethodIds;
			for (int i = 0; i < methodIds.Length; i++)
			{
				if (string.Equals(methodIds[i], text, StringComparison.OrdinalIgnoreCase))
				{
					return ExecuteFolderMethod(context, item, text, elementList, jb, flag);
				}
			}
		}
		throw new NotSupportedException("Task not found");
	}

	public HttpStatusCode TaskParams(IRestApiRequestContext context, string pathId, JsonBuilder jb)
	{
		ConfigurationItemSerializer configurationItemSerializer = context.RestApiRequestParameters.SessionData as ConfigurationItemSerializer;
		ConfigurationItem invokeInfo;
		return ConfigurationApiManager.InvokeMethodAndSerialize(context, jb, configurationItemSerializer.Item, NameConvert.TaskToMethodId(context.RestApiRequestParameters.Task), out invokeInfo);
	}

	public void LoadSpecificItem(IRestApiRequestContext context, string pathId)
	{
		string itemType = ResourceEntry.ItemType;
		ConfigurationItemPath pathInstance = (string.IsNullOrEmpty(itemType) ? ConfigurationItemPath.CreateSystemPath() : ConfigurationItemPath.CreateItemPath(itemType, pathId));
		ConfigurationItem configurationItem = (context.RestApiRequestParameters.FilterHandler.None() ? ConfigurationApiManager.GetItem(context, pathInstance) : ConfigurationApiManager.GetItemWithChildren(context, pathInstance));
		if (string.IsNullOrEmpty(itemType) && !string.Equals(pathId, configurationItem.Properties.FirstOrDefault((Property p) => p.Key == "Id").Value, StringComparison.OrdinalIgnoreCase))
		{
			throw new ArgumentException("Id not found", "pathId");
		}
		context.RestApiRequestParameters.SessionData = SerializerFactory.GetRestSerializer(configurationItem);
	}

	public virtual void LoadSpecificChildItem(IRestApiRequestContext context, string parentResourceNamePlural, string id, string childId)
	{
		if (!ChildItemTypesWithoutRootSupport.Contains(ResourceEntry.ItemType))
		{
			LoadSpecificItem(context, childId);
			if ((context.RestApiRequestParameters.SessionData as ConfigurationItemSerializer)?.Item == null)
			{
				throw new PathNotFoundException(string.Empty);
			}
			return;
		}
		ResourceEntry resourcePlural = NameConvert.GetResourcePlural(parentResourceNamePlural);
		string itemTypeFolder = ResourceEntry.ItemTypeFolder;
		ConfigurationItem[] array = ConfigurationApiManager.LookupChildArray(context, resourcePlural.ItemType, id, itemTypeFolder);
		foreach (ConfigurationItem configurationItem in array)
		{
			if (string.Equals(ConfigurationItemPath.CreatePath(configurationItem.Path).Id, childId, StringComparison.OrdinalIgnoreCase))
			{
				context.RestApiRequestParameters.SessionData = SerializerFactory.GetRestSerializer(configurationItem);
				return;
			}
		}
		throw new PathNotFoundException(string.Empty);
	}

	public string SerializeLoadedItem(IRestApiRequestContext context)
	{
		return (context.RestApiRequestParameters.SessionData as ConfigurationItemSerializer).Serialize(context.RestApiRequestParameters);
	}

	public string GetTaskListInJson(IRestApiRequestContext context)
	{
		if (context.RestApiRequestParameters.SessionData is ConfigurationItemSerializer configurationItemSerializer)
		{
			return configurationItemSerializer.TaskListInJson();
		}
		if (SpecialCases.UseQueryForMethodSupportedForArray(ResourceEntry.ItemType))
		{
			return " [ ]";
		}
		if (string.IsNullOrEmpty(ResourceEntry.ItemTypeFolder))
		{
			return " [ ]";
		}
		string folderType = NameConvert.ResourcePluralToFolderItemType(ResourceEntry.ResourceNamePlural);
		ConfigurationItemPath path = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), folderType);
		ConfigurationItem item = ConfigurationApiManager.LookupFolder(context, path);
		return SerializerFactory.GetRestSerializer(item).TaskListInJson();
	}

	public string GetTaskListInJson(IRestApiRequestContext context, ResourceEntry parentResource, string parentId)
	{
		string itemTypeFolder = ResourceEntry.ItemTypeFolder;
		string text = parentResource.ItemType;
		string parentId2 = parentId;
		if (string.IsNullOrEmpty(text))
		{
			text = "System";
			parentId2 = string.Empty;
		}
		ConfigurationItemPath path = ConfigurationItemPath.CreateFolderPath(text, parentId2, itemTypeFolder);
		ConfigurationItem item = ConfigurationApiManager.LookupFolder(context, path);
		return SerializerFactory.GetRestSerializer(item).TaskListInJson();
	}

	private static void RemoveProperty(ConfigurationItem item, string keyToRemove)
	{
		List<Property> list = new List<Property>();
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (property.Key != keyToRemove)
			{
				list.Add(property);
			}
		}
		item.Properties = list.ToArray();
	}
}
