using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.Proxy.RestApi.ConfigApiRelated;

public class ConfigApiRelatedJsonBuilder : JsonBuilder
{
	public static INameConvert NameConvert { get; set; } = new NameConvert();

	public ConfigApiRelatedJsonBuilder(bool array)
		: base(array)
	{
	}

	public void AppendPropertyKeyValue(string itemType, string key, string value)
	{
		AppendKeyValue(NameConvert.PropertyKeyToField(itemType, key), value);
	}

	public void AppendPropertyKeyStringValue(string itemType, string key, string value)
	{
		AppendKeyString(NameConvert.PropertyKeyToField(itemType, key), (value == null) ? null : JsonUtilities.JsonEscape(value));
	}

	public void AppendPropertyInt(string itemType, string key, string intValue)
	{
		int result = 0;
		if (string.IsNullOrEmpty(intValue))
		{
			AppendPropertyKeyValue(itemType, key, null);
		}
		else if (!string.IsNullOrEmpty(intValue) && !int.TryParse(intValue, out result))
		{
			AppendPropertyKeyValue(itemType, key, intValue);
		}
		else
		{
			AppendPropertyKeyValue(itemType, key, result.ToString(CultureInfo.InvariantCulture));
		}
	}

	public void AppendPropertyTick(string itemType, string key, string boolValue)
	{
		if (!bool.TryParse(boolValue, out var _) || boolValue == null)
		{
			AppendElement(NameConvert.PropertyKeyToField(itemType, key), "false");
		}
		else
		{
			AppendElement(NameConvert.PropertyKeyToField(itemType, key), boolValue.ToLowerInvariant());
		}
	}

	public static void AppendPropertyPath(JsonBuilder jsonBuilder, Property property)
	{
		if (jsonBuilder == null)
		{
			throw new ArgumentNullException("jsonBuilder");
		}
		if (property == null)
		{
			throw new ArgumentNullException("property");
		}
		string itemType = "";
		string id = Guid.Empty.ToString();
		bool flag = false;
		if (!string.IsNullOrWhiteSpace(property.Value))
		{
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(property.Value);
			if (configurationItemPath.IsFolder)
			{
				flag = false;
			}
			else
			{
				flag = true;
				itemType = configurationItemPath.ItemType;
				id = configurationItemPath.Id;
			}
		}
		else if (property.ValueTypeInfos != null)
		{
			ValueTypeInfo[] valueTypeInfos = property.ValueTypeInfos;
			foreach (ValueTypeInfo valueTypeInfo in valueTypeInfos)
			{
				if (valueTypeInfo.Name == "PathItemType")
				{
					itemType = valueTypeInfo.Value;
					flag = true;
					break;
				}
				if (valueTypeInfo.Name == "Path")
				{
					if (valueTypeInfo.Value.Contains('['))
					{
						ConfigurationItemPath configurationItemPath2 = new ConfigurationItemPath(valueTypeInfo.Value);
						itemType = configurationItemPath2.ItemType;
						id = configurationItemPath2.Id;
					}
					else
					{
						itemType = valueTypeInfo.Value;
					}
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			LinkValue linkValue = new LinkValue(itemType, id);
			jsonBuilder.AppendElement(NameConvert.PropertyKeyToField(itemType, property.Key), linkValue.Serialize(NoData: true));
		}
		else
		{
			jsonBuilder.AppendKeyString(NameConvert.PropertyKeyToField(itemType, property.Key), property.Value);
		}
	}

	public static void AppendPropertyPathList(ConfigApiRelatedJsonBuilder jsonBuilder, string itemType, Property property)
	{
		if (jsonBuilder == null)
		{
			throw new ArgumentNullException("jsonBuilder");
		}
		if (property == null)
		{
			throw new ArgumentNullException("property");
		}
		ConfigApiRelatedJsonBuilder configApiRelatedJsonBuilder = new ConfigApiRelatedJsonBuilder(array: true);
		if (!string.IsNullOrWhiteSpace(property.Value))
		{
			string[] array = property.Value.Split(',', ';');
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					ConfigApiRelatedJsonBuilder configApiRelatedJsonBuilder2 = new ConfigApiRelatedJsonBuilder(array: false);
					ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(text);
					string value;
					try
					{
						value = NameConvert.FolderItemTypeToResourcePlural(configurationItemPath.ItemType);
					}
					catch (ResourceNotFoundException)
					{
						value = "Unknown";
					}
					configApiRelatedJsonBuilder2.AppendKeyString("type", value);
					configApiRelatedJsonBuilder2.AppendKeyString("id", IdToLower(configurationItemPath.Id));
					configApiRelatedJsonBuilder.AppendElement(configApiRelatedJsonBuilder2.ToJson());
				}
			}
		}
		jsonBuilder.AppendPropertyKeyValue(itemType, property.Key, configApiRelatedJsonBuilder.ToJson());
	}

	public static void AppendPropertyArray(ConfigApiRelatedJsonBuilder jsonBuilder, string itemType, Property property)
	{
		if (!property.IsSettable)
		{
			return;
		}
		if (jsonBuilder == null)
		{
			throw new ArgumentNullException("jsonBuilder");
		}
		if (property == null)
		{
			throw new ArgumentNullException("property");
		}
		ConfigApiRelatedJsonBuilder configApiRelatedJsonBuilder = new ConfigApiRelatedJsonBuilder(array: true);
		if (property.ValueArray == null)
		{
			throw new ArgumentException(FormattableString.Invariant($"Value array is null."), "property");
		}
		if (!string.IsNullOrEmpty(property.Value) && (property.ValueArray == null || property.ValueArray.Length == 0))
		{
			property.ValueArray = property.Value.Split(',', ';');
		}
		string text = property.ValueTypeInfos?.FirstOrDefault((ValueTypeInfo vti) => vti.Name == "ArrayValueType")?.Value;
		if (text == "String" || text == null)
		{
			string[] valueArray = property.ValueArray;
			foreach (string text2 in valueArray)
			{
				if (text2 == null)
				{
					throw new ArgumentException(FormattableString.Invariant($"Invalid value for string arrary. Value: <NULL>"), "property");
				}
				configApiRelatedJsonBuilder.AppendElementString(text2);
			}
		}
		else
		{
			if (!(text == "Int"))
			{
				throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Not supported value type info name. Value: {0}", text ?? "<NULL>")), "property");
			}
			string[] valueArray = property.ValueArray;
			foreach (string text3 in valueArray)
			{
				if (!int.TryParse(text3, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
				{
					throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Invalid value for int arrary. Value: {0}", text3 ?? "<NULL>")), "property");
				}
				configApiRelatedJsonBuilder.AppendElementInt(result);
			}
		}
		jsonBuilder.AppendPropertyKeyValue(itemType, property.Key, configApiRelatedJsonBuilder.ToJson());
	}

	public static string IdToLower(string id)
	{
		if (!string.IsNullOrEmpty(id) && Guid.TryParse(id, out var result))
		{
			return result.ToString();
		}
		return id;
	}

	public static void AppendOptionKeyString(JsonBuilder jsonBuilder, string key, string value)
	{
		if (jsonBuilder == null)
		{
			throw new ArgumentNullException("jsonBuilder");
		}
		jsonBuilder.AppendKeyString(key, JsonUtilities.JsonEscape(value));
	}

	public static void AppendPropertyKeyString(JsonBuilder jsonBuilder, string itemType, string key, string value, string methodId)
	{
		if (jsonBuilder == null)
		{
			throw new ArgumentNullException("jsonBuilder");
		}
		if (itemType == "GenericEvent" && key == "DataSource")
		{
			value = JsonConvertUtilities.CheckConvertPath(value);
		}
		jsonBuilder.AppendKeyString(NameConvert.PropertyKeyToField(itemType, key, methodId), JsonUtilities.JsonEscape(value));
	}
}
