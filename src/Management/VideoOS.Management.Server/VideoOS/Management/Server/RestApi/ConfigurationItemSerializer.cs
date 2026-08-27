#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Management.Server.RestApi.Utils;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

public class ConfigurationItemSerializer : IConfigurationItemSerializer
{
	private static List<string> _statusKeys = new List<string> { "Progress", "Path", "State", "PathList" };

	public ConfigurationItem Item { get; }

	public INameConvert NameConvert { get; set; } = new NameConvert();

	public IConfigurationApiManager ConfigurationApiManager { get; set; } = new ConfigurationApiManager();

	public string Id => ConfigurationItemPath.CreatePath(Item.Path).Id;

	public string ItemType
	{
		get
		{
			if (!(Item.ItemType == "System"))
			{
				return Item.ItemType;
			}
			return "";
		}
	}

	public ConfigurationItemPath Path => ConfigurationItemPath.CreatePath(Item.Path);

	public ConfigurationItemSerializer(ConfigurationItem item)
	{
		Item = item;
	}

	public string Serialize(IRestApiRequestParameters restApiRequestParameters)
	{
		bool badRequest;
		if (Item.ItemType == "InvokeResult" || Item.ItemType == "InvokeInfo")
		{
			return SerializeInvokeResultWithFilter(restApiRequestParameters, null, Item.ItemType == "InvokeInfo", includeProgress: false, out badRequest);
		}
		CorpJsonBuilder corpJsonBuilder = new CorpJsonBuilder(array: false);
		string displayNameKey = NameConvert.GetDisplayNameKey(Item.ItemType);
		string resourceName = NameConvert.ItemTypeToResourcePlural(Item.ItemType);
		if (restApiRequestParameters.FilterHandler.IncludeField(resourceName, "displayName"))
		{
			corpJsonBuilder.AppendKeyString(displayNameKey, JsonUtilities.JsonEscape(Item.DisplayName));
		}
		if (Item.EnableProperty != null && restApiRequestParameters.FilterHandler.IncludeField(resourceName, "enabled"))
		{
			corpJsonBuilder.AppendElement("enabled", Item.EnableProperty.Enabled.ToString().ToLowerInvariant());
		}
		string id = "";
		if (Item.Properties != null)
		{
			Property[] properties;
			if (Item.ItemType == "MIPItem" && restApiRequestParameters.FilterHandler.IncludeField(resourceName, "FQID"))
			{
				JsonBuilder jsonBuilder = new JsonBuilder(array: false);
				properties = Item.Properties;
				foreach (Property property in properties)
				{
					if (property.Key.StartsWith("FQID", StringComparison.Ordinal))
					{
						jsonBuilder.AppendKeyString(NameConvert.PropertyKeyToField("MIPItem", property.Key.Substring(5)), property.Value);
					}
				}
				corpJsonBuilder.AppendElement("FQID", jsonBuilder.ToJson());
			}
			properties = Item.Properties;
			foreach (Property property2 in properties)
			{
				if (!property2.Key.StartsWith("FQID", StringComparison.Ordinal))
				{
					string fieldName = NameConvert.PropertyKeyToField(Item.ItemType, property2.Key);
					if (restApiRequestParameters.FilterHandler.IncludeField(resourceName, fieldName))
					{
						id = SerializeProperty(corpJsonBuilder, id, property2);
					}
				}
			}
		}
		if (Item.ChildrenFilled && Item.Children != null)
		{
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
			ConfigurationItem[] children = Item.Children;
			foreach (ConfigurationItem configurationItem in children)
			{
				if (configurationItem.ItemType.EndsWith("Folder", StringComparison.Ordinal) && !configurationItem.ItemType.Equals("InfoItemFolder", StringComparison.Ordinal))
				{
					if (configurationItem.Children == null || !configurationItem.ChildrenFilled)
					{
						continue;
					}
					Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
					ConfigurationItem[] children2 = configurationItem.Children;
					foreach (ConfigurationItem child in children2)
					{
						SerializeChildItem(corpJsonBuilder, dictionary2, child, restApiRequestParameters);
					}
					foreach (KeyValuePair<string, List<string>> item in dictionary2)
					{
						JsonBuilder jsonBuilder2 = new JsonBuilder(array: true);
						foreach (string item2 in item.Value)
						{
							jsonBuilder2.AppendElement(item2);
						}
						corpJsonBuilder.AppendElement(NameConvert.ChildItemTypeToFieldName(item.Key), jsonBuilder2.ToJson());
					}
				}
				else
				{
					if (configurationItem.ItemType == "Task")
					{
						configurationItem.ItemCategory = ItemCategories.ChildItem;
					}
					SerializeChildItem(corpJsonBuilder, dictionary, configurationItem, restApiRequestParameters);
				}
			}
			foreach (KeyValuePair<string, List<string>> item3 in dictionary)
			{
				JsonBuilder jsonBuilder3 = new JsonBuilder(array: true);
				foreach (string item4 in item3.Value)
				{
					jsonBuilder3.AppendElement(item4);
				}
				corpJsonBuilder.AppendElement(NameConvert.PropertyKeyToField("", item3.Key), jsonBuilder3.ToJson());
			}
		}
		ConfigurationItemPath configurationItemPath = (string.IsNullOrEmpty(Item.Path) ? null : ConfigurationItemPath.CreatePath(Item.Path));
		ConfigurationItemPath configurationItemPath2 = (string.IsNullOrEmpty(Item.ParentPath) ? null : ConfigurationItemPath.CreatePath(Item.ParentPath));
		LinkList linkList = new LinkList();
		if (Item.ItemCategory != ItemCategories.ChildItem && Item.ItemType != "InvokeInfo" && Item.ItemType != "InvokeResult" && restApiRequestParameters.FilterHandler.IncludeField(resourceName, "relations"))
		{
			if (Item.ItemType == "RecordingServer" || Item.ItemType == "RecordingServerFailover")
			{
				linkList.Links.Add(new LinkValue("parent", "System", RestApiCache.SiteId.ToString()));
			}
			else if (configurationItemPath2 != null && !string.IsNullOrEmpty(configurationItemPath2.Id) && !Item.Path.Equals(Item.ParentPath))
			{
				if (configurationItemPath2.IsFolder)
				{
					linkList.Links.Add(new LinkValue("parent", configurationItemPath2.ParentItemType, configurationItemPath2.Id));
				}
				else
				{
					linkList.Links.Add(new LinkValue("parent", configurationItemPath2.ItemType, configurationItemPath2.Id));
				}
			}
			if (configurationItemPath != null)
			{
				if (!string.IsNullOrEmpty(configurationItemPath.Id))
				{
					linkList.Links.Add(new LinkValue("self", configurationItemPath.ItemType, configurationItemPath.Id));
				}
				else if (configurationItemPath.ItemType == "System")
				{
					linkList.Links.Add(new LinkValue("self", "System", id));
				}
			}
			if (linkList.Links.Count() != 0)
			{
				corpJsonBuilder.AppendElement("relations", linkList.Serialize());
			}
		}
		return corpJsonBuilder.ToJson();
	}

	private void SerializeChildItem(JsonBuilder jb, Dictionary<string, List<string>> childItemArrays, ConfigurationItem child, IRestApiRequestParameters restApiRequestParameters)
	{
		if (child.ItemType == "InfoItem" || child.ItemType == "InfoItemFolder")
		{
			ConfigurationItemSerializer configurationItemSerializer = new ConfigurationItemSerializer(child);
			if (child.ItemType == "InfoItemFolder")
			{
				string key = (string.IsNullOrEmpty(child.DisplayName) ? "infoItemFolder" : CamelCasify(child.DisplayName));
				jb.AppendElement(NameConvert.PropertyKeyToField("", key), configurationItemSerializer.Serialize(restApiRequestParameters));
				return;
			}
			string key2 = "array";
			if (!childItemArrays.ContainsKey(key2))
			{
				childItemArrays.Add(key2, new List<string>());
			}
			childItemArrays[key2].Add(configurationItemSerializer.Serialize(restApiRequestParameters));
			return;
		}
		string key3 = ((child.ItemCategory == ItemCategories.ChildItem) ? NameConvert.ChildItemTypeToFieldName(child.ItemType) : NameConvert.ItemTypeToResourcePlural(child.ItemType));
		if (!restApiRequestParameters.FilterHandler.Any() && !(child.ItemCategory == ItemCategories.ChildItem) && !(child.ItemType == "Task"))
		{
			return;
		}
		ConfigurationItemSerializer configurationItemSerializer2 = new ConfigurationItemSerializer(child);
		string resourceName = NameConvert.ItemTypeToResourcePlural(child.ItemType);
		if (!restApiRequestParameters.FilterHandler.None() && !restApiRequestParameters.FilterHandler.IncludeResourceName(resourceName))
		{
			return;
		}
		if (child.ItemCategory == ItemCategories.ChildItem && !NameConvert.HasMultipleChildItems(child.ItemType))
		{
			jb.AppendElement(NameConvert.PropertyKeyToField("", key3), configurationItemSerializer2.Serialize(restApiRequestParameters));
			return;
		}
		if (!childItemArrays.ContainsKey(key3))
		{
			childItemArrays.Add(key3, new List<string>());
		}
		childItemArrays[key3].Add(configurationItemSerializer2.Serialize(restApiRequestParameters));
	}

	private static string CamelCasify(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}
		string[] value = name?.Split(' ').Select((string w, int i) => ((i == 0) ? w[0].ToString().ToLower(CultureInfo.InvariantCulture) : w[0].ToString().ToUpper(CultureInfo.InvariantCulture)) + w.Substring(1)).ToArray();
		return string.Join("", value);
	}

	private void SerializeInvokeResultChild(JsonBuilder jb, Dictionary<string, List<string>> childItemArrays, ConfigurationItem child, IRestApiRequestParameters iRestApiRequestParameters)
	{
		string itemType = child.ItemType;
		ConfigurationItemSerializer configurationItemSerializer = new ConfigurationItemSerializer(child);
		if (!NameConvert.HasMultipleChildItems(child.ItemType))
		{
			jb.AppendElement(NameConvert.PropertyKeyToField("", itemType), configurationItemSerializer.Serialize(iRestApiRequestParameters));
			return;
		}
		if (!childItemArrays.ContainsKey(itemType))
		{
			childItemArrays.Add(itemType, new List<string>());
		}
		childItemArrays[itemType].Add(configurationItemSerializer.Serialize(iRestApiRequestParameters));
	}

	private string SerializeProperty(CorpJsonBuilder jb, string id, Property property)
	{
		return SerializeProperty(Item.ItemType, jb, id, property);
	}

	public static string SerializeProperty(string itemType, CorpJsonBuilder jb, string id, Property property)
	{
		if (property.Key == "Id")
		{
			id = CorpJsonBuilder.IdToLower(property.Value);
			property.Value = id;
		}
		if (property.ValueType == "Int" || property.ValueType == "Progress" || property.ValueType == "Slider")
		{
			jb.AppendPropertyInt(itemType, property.Key, property.Value);
		}
		else if (property.ValueType == "Double")
		{
			jb.AppendPropertyDouble(itemType, property.Key, property.Value);
		}
		else if (property.ValueType == "Tick")
		{
			jb.AppendPropertyTick(itemType, property.Key, property.Value);
		}
		else if (property.ValueType == "Path")
		{
			jb.AppendPropertyPath(property);
		}
		else if (property.ValueType == "PathList")
		{
			jb.AppendPropertyPathList(itemType, property);
		}
		else if (property.ValueType == "Array")
		{
			jb.AppendPropertyArray(itemType, property);
		}
		else if (property.ValueType == "Dictionary")
		{
			jb.AppendPropertyDictionary(itemType, property);
		}
		else
		{
			jb.AppendProperty(itemType, property, null);
		}
		return id;
	}

	public string SerializeParameters()
	{
		CorpJsonBuilder corpJsonBuilder = new CorpJsonBuilder(array: false);
		string methodId = null;
		if (Item.MethodIds != null && Item.MethodIds.Length != 0 && Item.ItemType == "InvokeInfo")
		{
			methodId = Item.MethodIds[0];
		}
		if (Item.Properties != null)
		{
			Property[] properties = Item.Properties;
			foreach (Property property in properties)
			{
				if (!property.Key.StartsWith("FQID", StringComparison.Ordinal))
				{
					if (property.ValueType == "Int" || property.ValueType == "Progress" || property.ValueType == "Slider")
					{
						corpJsonBuilder.AppendPropertyInt(Item.ItemType, property.Key, property.Value);
					}
					else if (property.ValueType == "Tick")
					{
						corpJsonBuilder.AppendPropertyTick(Item.ItemType, property.Key, property.Value);
					}
					else if (property.ValueType == "Path")
					{
						corpJsonBuilder.AppendPropertyPath(property);
					}
					else if (property.ValueType == "PathList")
					{
						corpJsonBuilder.AppendPropertyPathList(Item.ItemType, property);
					}
					else if (property.ValueType == "Array")
					{
						corpJsonBuilder.AppendPropertyArray(Item.ItemType, property);
					}
					else if (property.ValueType == "Dictionary")
					{
						corpJsonBuilder.AppendPropertyDictionary(Item.ItemType, property);
					}
					else
					{
						corpJsonBuilder.AppendProperty(Item.ItemType, property, methodId);
					}
				}
			}
		}
		return corpJsonBuilder.ToJson();
	}

	public string SerializeInvokeResultWithFilter(IRestApiRequestParameters restApiRequestParameters, string methodId, bool includeError, bool includeProgress, out bool badRequest)
	{
		CorpJsonBuilder corpJsonBuilder = new CorpJsonBuilder(array: false);
		if (Item.EnableProperty != null && Item.Properties.Any((Property p) => p.Key == "Enabled"))
		{
			corpJsonBuilder.AppendElement("enabled", Item.EnableProperty.Enabled.ToString().ToLowerInvariant());
		}
		badRequest = false;
		if (Item.Properties != null)
		{
			Property[] properties = Item.Properties;
			foreach (Property property in properties)
			{
				if (property.Key == "ErrorCode" || property.Key == "ErrorText")
				{
					badRequest = true;
				}
				if ((includeError || (!(property.Key == "ErrorCode") && !(property.Key == "ErrorText"))) && (includeProgress || (!(property.Key == "ItemSelection") && (!restApiRequestParameters.NoData || _statusKeys.Contains(property.Key)))) && !property.Key.StartsWith("FQID", StringComparison.Ordinal))
				{
					if (property.ValueType == "Int" || property.ValueType == "Progress" || property.ValueType == "Slider")
					{
						corpJsonBuilder.AppendPropertyInt(Item.ItemType, property.Key, property.Value);
					}
					else if (property.ValueType == "Double")
					{
						corpJsonBuilder.AppendPropertyDouble(Item.ItemType, property.Key, property.Value);
					}
					else if (property.ValueType == "Tick")
					{
						corpJsonBuilder.AppendPropertyTick(Item.ItemType, property.Key, property.Value);
					}
					else if (property.ValueType == "Path" || property.Key == "Path")
					{
						corpJsonBuilder.AppendPropertyPath(property);
					}
					else if (property.ValueType == "PathList")
					{
						corpJsonBuilder.AppendPropertyPathList(Item.ItemType, property);
					}
					else if (property.ValueType == "Array")
					{
						corpJsonBuilder.AppendPropertyArray(Item.ItemType, property);
					}
					else if (property.ValueType == "Dictionary")
					{
						corpJsonBuilder.AppendPropertyDictionary(Item.ItemType, property);
					}
					else
					{
						corpJsonBuilder.AppendProperty(Item.ItemType, property, methodId);
					}
				}
			}
		}
		if (Item.ChildrenFilled && Item.Children != null && Item.Children.Length != 0)
		{
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
			ConfigurationItem[] children = Item.Children;
			foreach (ConfigurationItem child in children)
			{
				SerializeInvokeResultChild(corpJsonBuilder, dictionary, child, restApiRequestParameters);
			}
			foreach (KeyValuePair<string, List<string>> item in dictionary)
			{
				JsonBuilder jsonBuilder = new JsonBuilder(array: true);
				foreach (string item2 in item.Value)
				{
					jsonBuilder.AppendElement(item2);
				}
				corpJsonBuilder.AppendElement(NameConvert.PropertyKeyToField("", item.Key), jsonBuilder.ToJson());
			}
		}
		return corpJsonBuilder.ToJson();
	}

	public bool SerializeInvokeResultErrors(JsonBuilder jsonBuilder)
	{
		string text = null;
		string text2 = null;
		if (Item.Properties != null)
		{
			Property[] properties = Item.Properties;
			foreach (Property property in properties)
			{
				if (property.Key == "ErrorCode")
				{
					text = property.Value;
				}
				else if (property.Key == "ErrorText")
				{
					text2 = property.Value;
				}
			}
		}
		if (text != null || text2 != null)
		{
			JsonUtilities.ConvertError(text, text2, jsonBuilder);
			return true;
		}
		return false;
	}

	public string TaskListInJson()
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: true);
		if (Item.MethodIds != null && Item.MethodIds.Length != 0)
		{
			string[] methodIds = Item.MethodIds;
			foreach (string text in methodIds)
			{
				if (!SpecialCases.IsMethodInternal(text))
				{
					string text2 = NameConvert.MethodIdToTask(text);
					string text3 = RestApiCache.GetMethodInfo(text)?.DisplayName;
					if (text3 == null)
					{
						text3 = text2;
					}
					JsonBuilder jsonBuilder2 = new JsonBuilder(array: false);
					jsonBuilder2.AppendKeyString("id", text2);
					jsonBuilder2.AppendKeyString("displayName", text3);
					jsonBuilder.AppendElement(jsonBuilder2.ToJson());
				}
			}
		}
		if (Item.ItemType != null && ApiGatewayDefinedTasks.AdditionalTasksForItemTypes.TryGetValue(Item.ItemType, out var value))
		{
			foreach (ApiGatewayDefinedTasks.ActionDefinition item in value)
			{
				if (item.AppliesToItem == null || item.AppliesToItem(Item))
				{
					JsonBuilder jsonBuilder3 = new JsonBuilder(array: false);
					jsonBuilder3.AppendKeyString("id", item.Name);
					jsonBuilder3.AppendKeyString("displayName", item.DisplayName);
					jsonBuilder.AppendElement(jsonBuilder3.ToJson());
				}
			}
		}
		return jsonBuilder.ToJson();
	}

	public string Resources(IRestApiRequestContext context, ConfigurationItem[] items)
	{
		if (!context.RestApiRequestParameters.Resources)
		{
			return "";
		}
		JsonBuilder jsonBuilder = new JsonBuilder(array: true);
		foreach (ConfigurationItem configurationItem in items)
		{
			JsonBuilder jsonBuilder2 = new JsonBuilder(array: false);
			jsonBuilder2.AppendKeyString("type", NameConvert.FolderItemTypeToResourcePlural(configurationItem.ItemType));
			jsonBuilder2.AppendKeyString("displayName", configurationItem.DisplayName);
			jsonBuilder.AppendElement(jsonBuilder2.ToJson());
		}
		return jsonBuilder.ToJson();
	}

	public string Definitions(IRestApiRequestContext context, string methodId)
	{
		CorpJsonBuilder corpJsonBuilder = new CorpJsonBuilder(array: false);
		if (Item.EnableProperty != null && !string.IsNullOrEmpty(Item.EnableProperty.TranslationId))
		{
			CorpJsonBuilder corpJsonBuilder2 = new CorpJsonBuilder(array: false);
			corpJsonBuilder2.AppendPropertyKeyValue(Item.ItemType, "canSet", "true");
			corpJsonBuilder2.AppendPropertyKeyString(Item.ItemType, "translationId", Item.EnableProperty.TranslationId, null);
			corpJsonBuilder2.AppendKeyString("valueType", "bool");
			corpJsonBuilder.AppendPropertyKeyValue(Item.ItemType, "enabled", corpJsonBuilder2.ToJson());
		}
		if (Item.ItemType != "InvokeInfo")
		{
			CorpJsonBuilder corpJsonBuilder3 = new CorpJsonBuilder(array: false);
			corpJsonBuilder3.AppendPropertyKeyValue(Item.ItemType, "canSet", "false");
			corpJsonBuilder3.AppendPropertyKeyString(Item.ItemType, "translationId", "PropertyDisplayName", null);
			corpJsonBuilder.AppendPropertyKeyValue(Item.ItemType, "displayName", corpJsonBuilder3.ToJson());
		}
		if (Item.Properties != null)
		{
			Property[] properties = Item.Properties;
			foreach (Property property in properties)
			{
				if (!property.Key.StartsWith("FQID", StringComparison.Ordinal))
				{
					CorpJsonBuilder corpJsonBuilder4 = PropertyDefinition(context, property);
					if (!string.IsNullOrEmpty(property.TranslationId))
					{
						corpJsonBuilder4.AppendPropertyKeyString(Item.ItemType, "translationId", property.TranslationId, null);
					}
					if (!string.IsNullOrEmpty(property.ToolTipTranslationId))
					{
						corpJsonBuilder4.AppendPropertyKeyString(Item.ItemType, "toolTipTranslationId", property.ToolTipTranslationId, null);
					}
					corpJsonBuilder.AppendKeyValue(NameConvert.PropertyKeyToField(Item.ItemType, property.Key, methodId), corpJsonBuilder4.ToJson());
				}
			}
		}
		if (Item.Children != null)
		{
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
			ConfigurationItem[] children = Item.Children;
			foreach (ConfigurationItem configurationItem in children)
			{
				if (configurationItem.ItemCategory == ItemCategories.ChildItem)
				{
					ConfigurationItemSerializer configurationItemSerializer = new ConfigurationItemSerializer(configurationItem);
					if (!dictionary.ContainsKey(configurationItem.ItemType))
					{
						dictionary.Add(configurationItem.ItemType, new List<string>());
					}
					dictionary[configurationItem.ItemType].Add(configurationItemSerializer.Definitions(context, null));
				}
			}
			foreach (KeyValuePair<string, List<string>> item in dictionary)
			{
				string key = NameConvert.ChildItemTypeToFieldName(item.Key);
				if (!NameConvert.HasMultipleChildItems(item.Key))
				{
					corpJsonBuilder.AppendKeyValue(NameConvert.PropertyKeyToField("", key), item.Value[0]);
					continue;
				}
				JsonBuilder jsonBuilder = new JsonBuilder(array: true);
				foreach (string item2 in item.Value)
				{
					jsonBuilder.AppendElement(item2);
				}
				corpJsonBuilder.AppendElement(NameConvert.PropertyKeyToField("", key), jsonBuilder.ToJson());
			}
		}
		return corpJsonBuilder.ToJson();
	}

	private CorpJsonBuilder PropertyDefinition(IRestApiRequestContext context, Property property)
	{
		CorpJsonBuilder corpJsonBuilder = new CorpJsonBuilder(array: false);
		corpJsonBuilder.AppendPropertyTick(Item.ItemType, "canSet", property.IsSettable.ToString().ToLowerInvariant());
		if (property.UIImportance == UIImportance.Hidden)
		{
			corpJsonBuilder.AppendPropertyTick(Item.ItemType, "hidden", "true");
		}
		string valueType = property.ValueType;
		bool flag = property.ValueTypeInfos != null && !string.IsNullOrEmpty(property.ValueTypeInfos.FirstOrDefault()?.TranslationId);
		if (property.IsSettable || flag)
		{
			if (property.ItemReference)
			{
				corpJsonBuilder.AppendPropertyKeyValue(Item.ItemType, "link", "true");
			}
			if (property.ValueTypeInfos != null && (property.ValueTypeInfos.Any() || property.ValueType == "Enum"))
			{
				switch (property.ValueType)
				{
				case "Slider":
				case "Double":
				case "Int":
					SerializeToCommaSeperatedString(context, property, corpJsonBuilder);
					break;
				case "Separator":
					if (property.ValueTypeInfos != null && property.ValueTypeInfos.Length != 0 && property.ValueTypeInfos[0].Name == "PathItemType")
					{
						valueType = "PathList";
						SerializePathList(property, corpJsonBuilder);
					}
					else
					{
						JsonBuilder jsonBuilder2 = SerializeEnum(property);
						corpJsonBuilder.AppendPropertyKeyValue(Item.ItemType, "options", jsonBuilder2.ToJson());
					}
					break;
				case "Array":
				{
					valueType = "Array";
					string text = property.ValueTypeInfos?.FirstOrDefault((ValueTypeInfo vti) => vti.Name == "ArrayValueType")?.Value ?? "";
					if (!(text == "String"))
					{
						if (text == "Int")
						{
							corpJsonBuilder.AppendPropertyKeyValue(Item.ItemType, "type", "\"int\"");
						}
					}
					else
					{
						corpJsonBuilder.AppendPropertyKeyValue(Item.ItemType, "type", "\"string\"");
					}
					break;
				}
				case "Enum":
				{
					JsonBuilder jsonBuilder = SerializeEnum(property);
					corpJsonBuilder.AppendPropertyKeyValue(Item.ItemType, "options", jsonBuilder.ToJson());
					break;
				}
				case "Path":
					SerializePathList(property, corpJsonBuilder);
					break;
				case "PathList":
					SerializePathList(property, corpJsonBuilder);
					break;
				case "String":
					Trace.WriteLine("ConfigAPI: Property of type 'String' should not have any ValueTypeInfo: " + property.Key + " - May not be handled");
					SerializeToCommaSeperatedString(context, property, corpJsonBuilder);
					break;
				default:
					Trace.WriteLine("Unhandled property valuetypeInfo info: " + property.ValueType);
					break;
				case "Tick":
					break;
				}
			}
		}
		AddPropertyDefinition(corpJsonBuilder, valueType);
		return corpJsonBuilder;
	}

	private static CorpJsonBuilder SerializeEnum(Property property)
	{
		CorpJsonBuilder corpJsonBuilder = new CorpJsonBuilder(array: true);
		ValueTypeInfo[] valueTypeInfos = property.ValueTypeInfos;
		foreach (ValueTypeInfo valueTypeInfo in valueTypeInfos)
		{
			CorpJsonBuilder corpJsonBuilder2 = new CorpJsonBuilder(array: false);
			corpJsonBuilder2.AppendKeyString("value", JsonUtilities.CheckConvertPath(valueTypeInfo.Value));
			corpJsonBuilder2.AppendOptionKeyString("displayName", valueTypeInfo.Name);
			if (!string.IsNullOrEmpty(valueTypeInfo.TranslationId))
			{
				corpJsonBuilder2.AppendKeyString("translationId", valueTypeInfo.TranslationId);
			}
			corpJsonBuilder.AppendElement(corpJsonBuilder2.ToJson());
		}
		return corpJsonBuilder;
	}

	private static void SerializeToCommaSeperatedString(IRestApiRequestContext context, Property property, CorpJsonBuilder propertyBuilder)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		ValueTypeInfo[] valueTypeInfos = property.ValueTypeInfos;
		foreach (ValueTypeInfo valueTypeInfo in valueTypeInfos)
		{
			string text = valueTypeInfo.Value;
			if (!string.IsNullOrEmpty(context.RestApiRequestParameters.Language) && !string.IsNullOrEmpty(valueTypeInfo.TranslationId))
			{
				string translationString = RestApiCache.GetTranslationString(context, context.RestApiRequestParameters.Language, valueTypeInfo.TranslationId);
				if (!string.IsNullOrEmpty(translationString))
				{
					text = translationString;
				}
			}
			if (dictionary.ContainsKey(valueTypeInfo.Name))
			{
				dictionary[valueTypeInfo.Name] = dictionary[valueTypeInfo.Name] + "," + text;
			}
			else
			{
				dictionary[valueTypeInfo.Name] = text;
			}
		}
		foreach (KeyValuePair<string, string> item in dictionary)
		{
			propertyBuilder.AppendOptionKeyString(item.Key, item.Value);
		}
	}

	private void SerializePathList(Property property, CorpJsonBuilder propertyBuilder)
	{
		if (property.ValueTypeInfos == null)
		{
			return;
		}
		string text = "";
		string text2 = "";
		int num = 0;
		ValueTypeInfo[] valueTypeInfos = property.ValueTypeInfos;
		foreach (ValueTypeInfo valueTypeInfo in valueTypeInfos)
		{
			if (valueTypeInfo.Name == "PathAllowAllItemType")
			{
				propertyBuilder.AppendPropertyKeyValue(Item.ItemType, "allowAllItems", "true");
			}
			if (!(valueTypeInfo.Name == "PathItemType"))
			{
				continue;
			}
			string[] array = valueTypeInfo.Value.Split(',');
			foreach (string text3 in array)
			{
				if (!string.IsNullOrEmpty(text3))
				{
					text2 = NameConvert.ItemTypeToResourcePlural(text3);
					text = text + "\"" + text2 + "\",";
					num++;
				}
			}
		}
		if (num == 1)
		{
			propertyBuilder.AppendPropertyKeyString(Item.ItemType, "type", text2, null);
			return;
		}
		propertyBuilder.AppendPropertyKeyValue(Item.ItemType, "type", " [ " + text.TrimEnd(',') + " ] ");
	}

	private static void AddPropertyDefinition(JsonBuilder jb, string valueType)
	{
		jb.AppendKeyString("valueType", ConvertValueTypeToRestType(valueType));
	}

	private static string ConvertValueTypeToRestType(string valueType)
	{
		return valueType switch
		{
			"Tick" => "bool", 
			"DateTime" => "dateTime", 
			"PathList" => "pathList", 
			_ => valueType.ToLowerInvariant(), 
		};
	}
}
