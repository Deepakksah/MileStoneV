#define TRACE
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

public class CorpJsonBuilder : JsonBuilder
{
	public static INameConvert NameConvert { get; set; } = new NameConvert();

	public CorpJsonBuilder(bool array)
		: base(array)
	{
	}

	public void AppendPropertyKeyValue(string itemType, string key, string value)
	{
		AppendKeyValue(NameConvert.PropertyKeyToField(itemType, key), value);
	}

	public void AppendPropertyKeyStringValue(string itemType, string key, string value)
	{
		AppendKeyString(NameConvert.PropertyKeyToField(itemType, key), JsonUtilities.JsonEscape(value));
	}

	public void AppendPropertyInt(string itemType, string key, string intValue)
	{
		if (string.IsNullOrEmpty(intValue))
		{
			AppendPropertyKeyValue(itemType, key, null);
			return;
		}
		int result = 0;
		if (!int.TryParse(intValue, out result))
		{
			Trace.WriteLine("Invalid INT content from ConfigAPI: " + key + ", has value:" + intValue);
			AppendPropertyKeyValue(itemType, key, intValue);
		}
		else
		{
			AppendPropertyKeyValue(itemType, key, result.ToString(CultureInfo.InvariantCulture));
		}
	}

	public void AppendPropertyDouble(string itemType, string key, string doubleValue)
	{
		if (string.IsNullOrEmpty(doubleValue))
		{
			AppendPropertyKeyValue(itemType, key, null);
			return;
		}
		double result = 0.0;
		if (!double.TryParse(doubleValue, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
		{
			Trace.WriteLine("Invalid double content from ConfigAPI: " + key + ", has value:" + doubleValue);
			AppendPropertyKeyValue(itemType, key, doubleValue);
		}
		else if (double.IsNaN(result))
		{
			AppendPropertyKeyValue(itemType, key, null);
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
			Trace.WriteLine("Invalid TICK content from ConfigAPI: " + key + ", has value:" + boolValue);
			AppendElement(NameConvert.PropertyKeyToField(itemType, key), "false");
		}
		else
		{
			AppendElement(NameConvert.PropertyKeyToField(itemType, key), boolValue.ToLowerInvariant());
		}
	}

	public void AppendPropertyPath(Property property)
	{
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
				if (valueTypeInfo.Name == "PathItemType" && valueTypeInfo.Value != "TimeProfile")
				{
					itemType = valueTypeInfo.Value;
					flag = true;
					break;
				}
				if (valueTypeInfo.Name == "PathAllowAllItemType")
				{
					itemType = "any";
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			LinkValue linkValue = new LinkValue(itemType, id);
			AppendElement(NameConvert.PropertyKeyToField(itemType, property.Key), linkValue.Serialize());
		}
		else
		{
			AppendKeyString(NameConvert.PropertyKeyToField(itemType, property.Key), property.Value);
		}
	}

	public void AppendPropertyPathList(string itemType, Property property)
	{
		if (property == null)
		{
			throw new ArgumentNullException("property");
		}
		CorpJsonBuilder corpJsonBuilder = new CorpJsonBuilder(array: true);
		if (!string.IsNullOrWhiteSpace(property.Value))
		{
			string[] array = property.Value.Split(',', ';');
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					CorpJsonBuilder corpJsonBuilder2 = new CorpJsonBuilder(array: false);
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
					corpJsonBuilder2.AppendKeyString("type", value);
					if (!string.IsNullOrEmpty(configurationItemPath.Id))
					{
						corpJsonBuilder2.AppendKeyString("id", IdToLower(configurationItemPath.Id));
					}
					corpJsonBuilder.AppendElement(corpJsonBuilder2.ToJson());
				}
			}
		}
		AppendPropertyKeyValue(itemType, property.Key, corpJsonBuilder.ToJson());
	}

	public void AppendPropertyArray(string itemType, Property property)
	{
		if (property == null)
		{
			throw new ArgumentNullException("property");
		}
		CorpJsonBuilder corpJsonBuilder = new CorpJsonBuilder(array: true);
		if (property.ValueArray == null)
		{
			throw new ArgumentException(FormattableString.Invariant($"Value array is null."), "property");
		}
		string text = property.ValueTypeInfos?.FirstOrDefault((ValueTypeInfo vti) => vti.Name == "ArrayValueType")?.Value;
		if (text == "String")
		{
			string[] valueArray = property.ValueArray;
			foreach (string text2 in valueArray)
			{
				if (text2 == null)
				{
					throw new ArgumentException(FormattableString.Invariant($"Invalid value for string arrary. Value: <NULL>"), "property");
				}
				corpJsonBuilder.AppendElementString(text2);
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
				corpJsonBuilder.AppendElementInt(result);
			}
		}
		AppendPropertyKeyValue(itemType, property.Key, corpJsonBuilder.ToJson());
	}

	public void AppendPropertyDictionary(string itemType, Property property)
	{
		if (property == null)
		{
			throw new ArgumentNullException("property");
		}
		CorpJsonBuilder corpJsonBuilder = new CorpJsonBuilder(array: true);
		if (property.ValueArray == null)
		{
			throw new ArgumentException(FormattableString.Invariant($"Value array is null."), "property");
		}
		string text = property.ValueTypeInfos?.FirstOrDefault((ValueTypeInfo vti) => vti.Name == "ArrayValueType")?.Value;
		if (text == "String")
		{
			string[] valueArray = property.ValueArray;
			for (int num = 0; num < valueArray.Length; num++)
			{
				string[] array = (valueArray[num] ?? throw new ArgumentException(FormattableString.Invariant($"Invalid value for string array. Value: <NULL>"), "property")).Split(';');
				if (array.Length != 2)
				{
					throw new ArgumentException(FormattableString.Invariant($"Invalid value for string array. Value must be in format like 'key;value'"), "property");
				}
				CorpJsonBuilder corpJsonBuilder2 = new CorpJsonBuilder(array: false);
				corpJsonBuilder2.AppendKeyString("key", array[0]);
				corpJsonBuilder2.AppendKeyString("value", array[1]);
				corpJsonBuilder.AppendElement(corpJsonBuilder2.ToJson());
			}
		}
		else
		{
			if (!(text == "Int"))
			{
				throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Not supported value type info name. Value: {0}", text ?? "<NULL>")), "property");
			}
			string[] valueArray = property.ValueArray;
			foreach (string text2 in valueArray)
			{
				if (!int.TryParse(text2, NumberStyles.Integer, CultureInfo.InvariantCulture, out var _))
				{
					throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Invalid value for int arrary. Value: {0}", text2 ?? "<NULL>")), "property");
				}
				string[] array2 = text2.Split(';');
				if (array2.Length != 2)
				{
					throw new ArgumentException(FormattableString.Invariant($"Invalid value for string array. Value must be in format like 'key;value'"), "property");
				}
				CorpJsonBuilder corpJsonBuilder3 = new CorpJsonBuilder(array: false);
				corpJsonBuilder3.AppendKeyString("key", array2[0]);
				corpJsonBuilder3.AppendKeyValue("value", array2[1]);
				corpJsonBuilder.AppendElement(corpJsonBuilder3.ToJson());
			}
		}
		AppendPropertyKeyValue(itemType, property.Key, corpJsonBuilder.ToJson());
	}

	public void AppendProperty(string itemType, Property property, string methodId)
	{
		if (property == null)
		{
			throw new ArgumentNullException("property");
		}
		if (property.Key == "Id")
		{
			AppendPropertyKeyString(itemType, property.Key, property.Value.ToLowerInvariant(), methodId);
		}
		else
		{
			AppendPropertyKeyString(itemType, property.Key, property.Value, methodId);
		}
	}

	public void AppendPropertyKeyString(string itemType, string key, string value, string methodId)
	{
		value = JsonUtilities.CheckConvertPath(value);
		AppendKeyString(NameConvert.PropertyKeyToField(itemType, key, methodId), JsonUtilities.JsonEscape(value));
	}

	public void AppendOptionKeyString(string key, string value)
	{
		AppendKeyString(key, JsonUtilities.JsonEscape(value));
	}

	public static string IdToLower(string id)
	{
		if (!string.IsNullOrEmpty(id) && Guid.TryParse(id, out var result))
		{
			return result.ToString();
		}
		return id;
	}
}
